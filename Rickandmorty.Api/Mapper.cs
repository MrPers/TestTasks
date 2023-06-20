using AutoMapper;
using Rickandmorty.Api.Models;
using Rickandmorty.DTO;
using Rickandmorty.Entity;

namespace Rickandmorty.Api
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<CheckPersonVM, CheckPersonDto>().ReverseMap();

            CreateMap<LocationDto, LocationVM>().ReverseMap();
            CreateMap<LocationDto, LocationJSON>().ReverseMap();

            CreateMap<PersonFullInformationDto, PersonVM>()
                .ForMember(dst => dst.Origin, opt => opt.MapFrom(src => src.Location))
                .ReverseMap();
            CreateMap<PersonFullInformationDto, PersonJSON>().ReverseMap();
        }
    }
}
