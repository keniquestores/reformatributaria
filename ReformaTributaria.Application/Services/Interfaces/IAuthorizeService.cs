using ReformaTributaria.Application.Services.Dtos;

namespace ReformaTributaria.Application.Services.Interfaces
{
    public interface IAuthorizeService
    {
        Task<TokenResponseDto> Execute(AuthorizeDto dto, string apiKey);
        Task<TokenResponseDto> ExecuteProduto(AuthorizeProdutoDto dto, string apiKey);
    }
}
