using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Dtos.Responses;

namespace ReformaTributaria.Application.Services.Interfaces
{
    public interface IReformaTributariaConsumoService
    {

        Task ExecutarApuracoesCbsAsync();

        Task<TokenCbsResponseDto> ObterTokenAsync(string clientId, string clientSecret,CancellationToken cancellationToken = default);

        Task<ApuracaoCbsResponseDto> EnviarApuracaoCbsAsync(string inscricaoFederal, string urlRetorno, string accessToken, CancellationToken cancellationToken = default);
    }
}
