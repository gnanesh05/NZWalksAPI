using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<RegionCreateDTO, Region>().ReverseMap();
            CreateMap<RegionUpdateDTO, Region>().ReverseMap();

            CreateMap<WalkCreateDTO, Walk>().ReverseMap();
            CreateMap<Walk, WalkDTO>().ReverseMap();
        }
    }
}
