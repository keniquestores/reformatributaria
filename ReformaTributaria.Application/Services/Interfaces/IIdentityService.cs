using ReformaTributaria.Application.Services.Dtos;

namespace ReformaTributaria.Application.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthorizeResponseDto> ObterTokenProductAuthorize(string apikey, string apisecret, string questorId, bool produto);
    }
}
