using AutoMapper;
using newzelandWalks.Models.Domain;
using newzelandWalks.Models.DTO;

namespace newzelandWalks.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            // this is invoked for method in C# when you inherit you can call the method in this shape(without making object from this class) but only in constructor because constructor is making instance in runtime so it will invoked it automatically
            CreateMap<Region, RegionDto>().ReverseMap(); // map region to regionDto and versa verce
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkRequestDto,Walk>().ReverseMap();
            CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
