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
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
                .ForMember(dest => dest.RecordsPerPage, opt => opt.MapFrom(src => src.RecordsPerPage))
                .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.SearchTerm))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
                 .ForMember(dest => dest.SortDirection, opt => opt.MapFrom(src => src.SortDirection))
                .ForMember(dest => dest.Videogames, opt => opt.Ignore())   // lo llenamos aparte
                .ForMember(dest => dest.TotalItems, opt => opt.Ignore()); // también
        }



    }
}

