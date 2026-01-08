using AutoMapper;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Domain.Entities;

namespace ReformaTributaria.Application.Mappings
{
    public class FilaProfile : Profile
    {
        public FilaProfile() {

            CreateMap<FilaDto, Fila>()
                .ForMember(dest => dest.InscricaoFederalContribuinte, opt => opt.MapFrom(src => src.InscricaoFederalContribuinte));

            CreateMap<Fila, FilaClienteDto>()
                .ForMember(dest => dest.InscricaoFederalContribuinte, opt => opt.MapFrom(src => src.InscricaoFederalContribuinte));
        }
    }
}