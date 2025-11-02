using AutoMapper;
using Endpoint.RequestsAndResponses;
using Entities;

namespace Endpoint.Infastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryRequest, Category>().ReverseMap();
            CreateMap<CategoryResponse, Category>().ReverseMap();

            CreateMap<TeacherRequest, Teacher>().ReverseMap();
            CreateMap<TeacherResponse, Teacher>().ReverseMap();
        }
    }
}
