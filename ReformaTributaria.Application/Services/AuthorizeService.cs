using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Interfaces;
using ReformaTributaria.CrossCutting.Validation;

namespace ReformaTributaria.Application.Services
{
    public class AuthorizeService(IClienteService clienteService, IValidatorHandler validatorHandler, IIdentityService autenticacaoService) : IAuthorizeService
    {

        public async Task<TokenResponseDto> Execute(AuthorizeDto dto, string apiKey)
        {

            var cliente = await clienteService.GetByQuestorIdAsync(dto.QuestorId);

            if (dto.ChavePrivada != cliente.ChavePrivada)
                validatorHandler.AddMsgErrorAndStopExecution("Chave privada inválida.");

            var autenticacao = await autenticacaoService.ObterTokenProductAuthorize(apiKey, dto.ApiSecret, dto.QuestorId, false);

            if (autenticacao == null)
                validatorHandler.AddMsgErrorAndStopExecution("Erro ao obter token de autenticação.");

            return new TokenResponseDto
            {
                AccessToken = autenticacao!.AccessToken,
                ExpiresIn = autenticacao.ExpiresIn
            };
        }

        public async Task<TokenResponseDto> ExecuteProduto(AuthorizeProdutoDto dto, string apiKey)
        {
            var autenticacao = await autenticacaoService.ObterTokenProductAuthorize(apiKey, dto.ApiSecret, string.Empty, true);

            if (autenticacao == null)
                validatorHandler.AddMsgErrorAndStopExecution("Erro ao obter token de autenticação.");

            return new TokenResponseDto
            {
                AccessToken = autenticacao!.AccessToken,
                ExpiresIn = autenticacao.ExpiresIn
            };
        }
    }
}
