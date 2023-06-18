using AutoMapper;
using MediatR;
using Pakosti.Application.Cqrs.Products.Queries.GetProduct;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Cqrs.Products.Queries.GetProductList;

public class GetProductListQuery : IRequest<ProductListVm>
{

}