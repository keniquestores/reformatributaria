using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReformaTributaria.Application.Services.Dtos;
using System.Text.Json;

namespace ReformaTributaria.Webhooks.Controllers
{
    public class TesteController : ControllerBase
    {


        [HttpGet("testecbs")]
        [HttpHead("testecbs")]
        public IActionResult Ping() => Ok("ok");

        [AllowAnonymous]
        [HttpPost("testecbs")]
        public async Task<IActionResult> TesteCbs([FromBody] JsonElement model)
        {
            return Ok();
        }

    }
}
