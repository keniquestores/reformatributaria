using ReformaTributaria.Application.Services.Dtos;

namespace ReformaTributaria.Application.Services.Interfaces
{
    public interface IFilaService
    {
        Task<long> InserirFilaAsync(FilaDto dto, string questorId);
        Task<bool> AlterarStatusFila(bool ativo, string inscricaoFederal, string questorId);
        Task<IEnumerable<FilaClienteDto>> BuscarFilasParaExecucaoAsync(int page, int take);
    }
}
