using ReformaTributaria.Application.Services.Dtos;

namespace ReformaTributaria.Application.Services.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteResponseDto> GetByIdAsync(long id);
        Task<IEnumerable<ClienteResponseDto>> GetAllAsync();
        Task<ClienteResponseDto> GetByQuestorIdAsync(string questorId);
        Task<ClienteResponseDto> CriarAsync(ClienteDto dto);
        Task AlterarAsync(ClienteDto dto, long id);
        Task DeletarAsync(long id);
    }
}
