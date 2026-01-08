using ReformaTributaria.Application.Services.Dtos;

namespace ReformaTributaria.Application.Validators.Dtos
{
    public class ClienteDtoValidator(ClienteDto dto)
    {
        public ClienteDto ClienteDto { get; } = dto;
    }
}
