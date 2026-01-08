using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Interfaces;
using ReformaTributaria.Domain.Entities;

namespace ReformaTributaria.Controllers.V1
{
    [Authorize(Policy = "IsProductPolicy")]
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ClienteController(IClienteService clienteService, ILogger<ClienteController> logger) : QControllerBaseController
    {
        private readonly IClienteService _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));

        /// <summary>
        /// Obtém todos os clientes
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        [MapToApiVersion(1.0)]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();

            return ResultJson(clientes);
        }

        /// <summary>
        /// Obtém um cliente por ID
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("{id}")]
        [MapToApiVersion(1.0)]
        public async Task<ActionResult<Cliente>> GetById(long id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);

            return ResultJson(cliente);
        }

        /// <summary>
        /// Obtém um cliente por QuestorId
        /// </summary>
        /// <param name="questorId">QuestorId do cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("questor/{questorId}")]
        [MapToApiVersion(1.0)]
        public async Task<ActionResult<Cliente>> GetByQuestorId(string questorId)
        {
            var cliente = await _clienteService.GetByQuestorIdAsync(questorId);

            return ResultJson(cliente);
        }

        /// <summary>
        /// Cria um novo cliente
        /// </summary>
        /// <param name="cliente">Dados do cliente a ser criado</param>
        /// <returns>ID do cliente criado</returns>
        [HttpPost]
        [MapToApiVersion(1.0)]
        public async Task<ActionResult<long>> Create([FromBody] ClienteDto cliente)
        {
            return ResultJson(await _clienteService.CriarAsync(cliente));
        }

        /// <summary>
        /// Atualiza um cliente existente
        /// </summary>
        /// <param name="id">ID do cliente a ser atualizado</param>
        /// <param name="cliente">Dados do cliente atualizados</param>
        /// <returns>Sem conteúdo</returns>
        [HttpPut("{id}")]
        [MapToApiVersion(1.0)]
        public async Task<IActionResult> Update(long id, [FromBody] ClienteDto cliente)
        {

            await _clienteService.AlterarAsync(cliente, id);

            return ResultJson();
        }

        /// <summary>
        /// Deleta um cliente
        /// </summary>
        /// <param name="id">ID do cliente a ser deletado</param>
        /// <returns>Sem conteúdo</returns>
        [HttpDelete("{id}")]
        [MapToApiVersion(1.0)]
        public async Task<IActionResult> Delete(long id)
        {
            await _clienteService.DeletarAsync(id);

            return ResultJson();
        }

    }
}
