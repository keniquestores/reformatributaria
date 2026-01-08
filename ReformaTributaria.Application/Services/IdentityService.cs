using System.Text;
using Newtonsoft.Json;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Interfaces;
using ReformaTributaria.CrossCutting.Models;
using ReformaTributaria.CrossCutting.Validation;
using ReformaTributaria.Domain.Common;

namespace ReformaTributaria.Application.Services
{
    public class IdentityService(AppSettings appSettings, IValidatorHandler validatorHandler) : IIdentityService
    {
        public async Task<AuthorizeResponseDto> ObterTokenProductAuthorize(string apikey, string apisecret, string questorId, bool produto)
        {
            HttpClientHandler handler = new()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            using var client = new HttpClient(handler);

            client.BaseAddress = new Uri(appSettings.OAuthConfig.Uri);

            client.DefaultRequestHeaders.Add("x-api-key", appSettings.AppConfig.ApiKey);

            object customClaims;

            if (produto)
                customClaims = new { IsProduct = true };
            else
                customClaims = new { IsProduct = false, QuestorId = questorId };


            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                apikey,
                apisecret,
                customClaims
            }), Encoding.UTF8, "application/json");

            var result = await client.PostAsync(appSettings.OAuthConfig.PathAuthorizeProduct, content);

            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<GenericResponseType<AuthorizeResponseDto>>(resultContent);


                if (!response!.Success)
                {
                    for (int i = 0; i < response.Messages.Count; i++)
                    {
                        var msg = response.Messages[i];

                        if (i == response.Messages.Count - 1)
                            validatorHandler.AddMsgErrorAndStopExecution(msg);
                        else
                            validatorHandler.AddMsgError(msg);
                    }
                }
                else
                {
                    return response.Data!;
                }
            }

            validatorHandler.AddMsgErrorAndStopExecution("Ocorreu um erro ao autenticar o produto.");

            return default!;
        }
    }
}
