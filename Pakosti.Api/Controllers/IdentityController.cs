using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Domain.Entities;
using Pakosti.Application.Extensions;
using Pakosti.Infrastructure.Persistence;
using Pakosti.Api.Models.Identity;
using Pakosti.Application.Interfaces;

namespace Pakosti.Api.Controllers;

public class IdentityController : BaseController
{
    private readonly PakostiDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public IdentityController(PakostiDbContext context, UserManager<AppUser> userManager, 
        ITokenService tokenService, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _tokenService = tokenService;
        _configuration = configuration;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var managedUser = await _userManager.FindByEmailAsync(request.Email);

        if (managedUser == null) return BadRequest("Bad credentials");

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);

        if (!isPasswordValid) return BadRequest("Bad credentials"); 
        
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user is null) return Unauthorized();
        
        var roleIds = await _context.UserRoles
            .Where(r => r.UserId == user.Id)
            .Select(r => r.RoleId).ToListAsync();
        var roles = _context.Roles
            .Where(x => roleIds.Contains(x.Id)).ToList();

        var accessToken = _tokenService.CreateToken(user, roles);
        user.RefreshToken = _configuration.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow
            .AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            Username = user.UserName!,
            Email = user.Email!,
            Token = accessToken,
            RefreshToken = user.RefreshToken
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(request);

        var user = new AppUser
        {
            Firstname = request.FirstName,
            Lastname = request.LastName,
            Email = request.Email,
            UserName = request.Email
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        if (!result.Succeeded) return BadRequest(request);

        var findUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (findUser is null) throw new NotFoundException(nameof(user), request.Email);

        await _userManager.AddToRoleAsync(findUser, RoleConstants.Consumer);

        return await Authenticate(new AuthRequest
        {
            Email = request.Email,
            Password = request.Password
        });
    }

    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
    {
        if (tokenModel is null) return BadRequest("Invalid client request");

        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;
        var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

        if (principal is null) return BadRequest("Invalid access token or refresh token");

        var username = principal.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username!);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _configuration.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }
    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return BadRequest("Invalid user name");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return Ok();
    }
    
    [Authorize(Policy = "AdministratorOnly")]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        return Ok();
    }
}