using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Interfaces;

namespace ReformaTributaria.Controllers.V1
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OAuthController(IAuthorizeService authorizeService) : QControllerBaseController
    {
        private readonly IAuthorizeService _authorizeService = authorizeService;

        [HttpPost]
        [AllowAnonymous]
        [Route("authorize-cliente")]
        public async Task<ActionResult> AuthorizeCliente([FromBody] AuthorizeDto dto)
        {
            return ResultJson(await _authorizeService.Execute(dto, GetApiKeyFromHeader() ?? string.Empty));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authorize-produto")]
        public async Task<ActionResult> AuthorizeProduto([FromBody] AuthorizeProdutoDto dto)
        {
            return ResultJson(await _authorizeService.ExecuteProduto(dto, GetApiKeyFromHeader() ?? string.Empty));
        }
    }
}
