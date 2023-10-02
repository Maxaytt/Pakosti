namespace Pakosti.Domain.Constants;

public static class SecretKeys
{
    public const string CurrencyApiKey = "CURRENCY_API_KEY";
    public const string PostgresConnectionString = "POSTGRES_CONNECTION_STRING";
    public const string JwtSecret = "JWT_SECRET";
    public static readonly (string Email, string Password, string Username) SuperAdmin = 
        ("SuperTermVibrator1699@gmail.com", "SuperTermVibrator1699!", "Vyzhivalovo");
}