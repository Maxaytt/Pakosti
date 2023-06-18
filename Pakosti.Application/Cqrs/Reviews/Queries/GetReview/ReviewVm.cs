using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Reviews.Queries.GetReview;

public class ReviewVm : IMapWith<Review>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Header { get; set; } = null!;
    public string Body { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public DateTime EditionDate { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Review, ReviewVm>()
            .ForMember(vm => vm.Id,
                opt => opt
                    .MapFrom(r => r.Id))
            .ForMember(vm => vm.ProductId,
                opt => opt
                    .MapFrom(r => r.ProductId))
            .ForMember(vm => vm.Header,
                opt => opt
                    .MapFrom(r => r.Header))
            .ForMember(vm => vm.Body,
                opt => opt
                    .MapFrom(r => r.Body))
            .ForMember(vm => vm.CreationDate,
                opt => opt
                    .MapFrom(r => r.CreationDate))
            .ForMember(vm => vm.EditionDate,
                opt => opt
                    .MapFrom(r => r.EditionDate));
    }
}