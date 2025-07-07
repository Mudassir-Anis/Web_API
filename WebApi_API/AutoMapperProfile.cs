using AutoMapper;
using WebApi_API.Models;
using WebApi_API.Models.DTO;

namespace WebApi_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VilaDto, Vila>();
            CreateMap<Vila, VilaDto>();
        }
    }
}
