using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Interfaces;

namespace ReformaTributaria.Controllers.V1
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FilaController : QControllerBaseController
    {

        private readonly IFilaService _filaService;

        public FilaController(IFilaService filaService)
        {
            _filaService = filaService;
        }

        /// <summary>
        /// Insere uma nova fila
        /// </summary>
        /// <param name="dto">Dados da fila a ser inserida</param>
        /// <returns>ID da fila criada</returns>
        [HttpPost]
        [MapToApiVersion(1.0)]
        public async Task<ActionResult<long>> InserirFila([FromBody] FilaDto dto)
        {
            var questorId = "108074";// GetQuestorIdFromClaims();
            var id = await _filaService.InserirFilaAsync(dto, questorId);
            return ResultJson(id);
        }

        /// <summary>
        /// Inativa uma fila baseada na inscrição federal e QuestorId
        /// </summary>
        /// <param name="inscricaoFederal">Inscrição Federal do cliente</param>
        /// <param name="questorId">QuestorId do cliente</param>
        /// <returns>Resultado da operação</returns>
        [HttpPatch("inativar")]
        [MapToApiVersion(1.0)]
        public async Task<ActionResult<bool>> AlterarStatusFila(
            [FromQuery] bool ativo, [FromQuery] string inscricaoFederal)
        {
            var questorId = GetQuestorIdFromClaims();
            var resultado = await _filaService.AlterarStatusFila(ativo, inscricaoFederal, questorId);
            return ResultJson(resultado);
        }

    }
}
