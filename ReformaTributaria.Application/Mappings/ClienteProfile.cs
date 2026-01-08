using AutoMapper;
using ReformaTributaria.Application.Mappings.Extensions;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Domain.Entities;

namespace ReformaTributaria.Application.Mappings
{
    /// <summary>
    /// Perfil de mapeamento AutoMapper para Cliente com criptografia/descriptografia automática.
    /// </summary>
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ChavePrivada, opt => opt.EncryptFrom())
                .ForMember(dest => dest.ClientId, opt => opt.EncryptFrom(src => src.ClientId))
                .ForMember(dest => dest.ClientSecret, opt => opt.EncryptFrom(src => src.ClientSecret));

            CreateMap<Cliente, ClienteResponseDto>()
                .ForMember(dest => dest.ChavePrivada, opt => opt.DecryptFrom(src => src.ChavePrivada))
                .ForMember(dest => dest.ClientId, opt => opt.DecryptFrom(src => src.ClientId))
                .ForMember(dest => dest.ClientSecret, opt => opt.DecryptFrom(src => src.ClientSecret));

            CreateMap<Cliente, ClienteDto>();
        }
    }
}