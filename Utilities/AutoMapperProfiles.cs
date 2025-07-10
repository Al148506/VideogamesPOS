using AutoMapper;
using NetTopologySuite.Geometries;
using VideogamesPOS.DTO;
using VideogamesPOS.Models.ViewModels;
namespace VideogamesPOS.Utilities

{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            ConfigureVideogameMap();

        }

        private void ConfigureVideogameMap()
        {
            CreateMap<VideogamesFilterDTO, VideogameIndexViewModel>()
           .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.Page))
           .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.RecordsPerPage))
           .ForMember(dest => dest.Videogames, opt => opt.Ignore()) // lo llenamos aparte
           .ForMember(dest => dest.TotalItems, opt => opt.Ignore()); // también
        }


    }
}

