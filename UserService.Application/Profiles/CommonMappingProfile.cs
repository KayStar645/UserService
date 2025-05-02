using AutoMapper;
using UserService.Application.Features.Base.Queries;

namespace UserService.Application.Profiles;

public class CommonMappingProfile : Profile
{
    public CommonMappingProfile()
    {
        CreateMap(typeof(PagedListResult<>), typeof(PagedListResult<>));

    }
}
