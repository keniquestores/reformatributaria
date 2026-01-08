using Microsoft.Extensions.Options;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Dtos.Requests;
using ReformaTributaria.Application.Services.Dtos.Responses;
using ReformaTributaria.Application.Services.Interfaces;
using ReformaTributaria.CrossCutting.Security;
using ReformaTributaria.Domain.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ReformaTributaria.Application.Services
{
    public class ReformaTributariaConsumoService : IReformaTributariaConsumoService
    {

        private readonly HttpClient _httpClient;
        private readonly AmbienteReceitaOptions _options;
        private readonly IFilaService _filaService;
        private readonly AppSettings _appSettings;


        public ReformaTributariaConsumoService(
            IFilaService filaService,
            HttpClient httpClient,
            IOptions<AmbienteReceitaOptions> options,
            AppSettings appSettings)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _filaService = filaService;
            _appSettings = appSettings;
        }

        #region Private Methods

        public async Task<TokenCbsResponseDto> ObterTokenAsync(
                string clientId,
                string clientSecret,
                CancellationToken cancellationToken = default)
        {
            var url = $"{_options.Url.TrimEnd('/')}/token";

            var payload = new
            {
                client_id = clientId,
                client_secret = clientSecret,
                grant_type = "client_credentials"
            };

            using var response = await _httpClient.PostAsJsonAsync(
                url,
                payload,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"Erro ao obter token da Receita Federal. Status: {(int)response.StatusCode}. Conteúdo: {error}");
            }

            var token = await response.Content.ReadFromJsonAsync<TokenCbsResponseDto>(cancellationToken: cancellationToken);

            if (token is null)
                throw new InvalidOperationException("Resposta de token vazia ou inválida.");

            return token;
        }

        public async Task<ApuracaoCbsResponseDto> EnviarApuracaoCbsAsync(
        string inscricaoFederal,
        string urlRetorno,
        string accessToken,
        CancellationToken cancellationToken = default)
        {
            var endpoint = $"{_options.Url.TrimEnd('/')}/rtc/apuracao-cbs/v1/{inscricaoFederal}";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new ApuracaoCbsRequestDto
            {
                UrlRetorno = urlRetorno
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            using var content = new StringContent(
                JsonSerializer.Serialize(payload, jsonOptions),
                Encoding.UTF8,
                "application/json");

            using var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);

            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Erro ao enviar Apuração CBS. Status: {(int)response.StatusCode}. Body: {body}");
            }

            var result = JsonSerializer.Deserialize<ApuracaoCbsResponseDto>(body, jsonOptions);

            if (result is null || string.IsNullOrWhiteSpace(result.Tiquete))
                throw new InvalidOperationException($"Resposta inválida da Apuração CBS. Body: {body}");

            return result;
        }

        #endregion


        #region Public Methods


        public async Task ExecutarApuracoesCbsAsync()
        {
            var filas = await _filaService.BuscarFilasParaExecucaoAsync(1, 50);

            foreach (var fila in filas)
            {
                var token = await ObterTokenAsync(FileIntegrityCrypto.Decrypt(fila.Cliente.ClientId, _appSettings.Security.FileIntegrityCrypto.Key), FileIntegrityCrypto.Decrypt(fila.Cliente.ClientSecret, _appSettings.Security.FileIntegrityCrypto.Key));

                await EnviarApuracaoCbsAsync(fila.InscricaoFederalContribuinte, "https://questor.free.beeceptor.com/reformatributaria", "k8QjITf0i0O3ojAXdcVUHcOFPEPugb84" /*token.Access_Token*/);
            }





            

        }


        #endregion



    }
}