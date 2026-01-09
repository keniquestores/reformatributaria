using AutoMapper;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Interfaces;
using ReformaTributaria.CrossCutting.Helpers;
using ReformaTributaria.CrossCutting.Validation;
using ReformaTributaria.Domain.Entities;
using ReformaTributaria.Domain.Interfaces.Infra.Repositories;

namespace ReformaTributaria.Application.Services
{
    public class FilaService : IFilaService
    {
        private readonly IFilaRepository _filaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IValidatorHandler _validator;
        private readonly IMapper _mapper;

        public FilaService(
            IMapper mapper,
            IFilaRepository filaRepository, 
            IClienteRepository clienteRepository,
            IValidatorHandler validator)
        {
            _mapper = mapper;
            _filaRepository = filaRepository;
            _clienteRepository = clienteRepository;
            _validator = validator;
        }

        public async Task<long> InserirFilaAsync(FilaDto dto, string questorId)
        {
            var cliente = await _clienteRepository.GetByQuestorIdAsync(questorId);

            if (cliente == null)
                _validator.AddMsgErrorAndStopExecution("Cliente não localizado.");

            var filaExistente = await _filaRepository.GetByClienteQuestorIdEInscFederalContribuinteAsync(StringHelper.SomenteDigitos(dto.InscricaoFederalContribuinte), questorId);

            if (filaExistente != null)
                _validator.AddMsgErrorAndStopExecution("Já existe uma fila para esse contribuinte e esse escritório.");


            var fila = _mapper.Map<Fila>(dto);
            fila.DataHoraInsercao = DateTime.UtcNow;
            fila.ClienteId = cliente.Id;            

            await _filaRepository.AddAsync(fila);

            return fila.Id;
        }

        public async Task<bool> AlterarStatusFila(bool ativo, string inscricaoFederalContribuinte, string questorId)
        {
            var resultado = await _filaRepository.AtivarOuInativar(false, inscricaoFederalContribuinte, questorId);
            return resultado;
        }

        public async Task<IEnumerable<FilaClienteDto>> BuscarFilasParaExecucaoAsync(int page, int take)
        {
            var skip = Math.Max(0, (page - 1)) * take;

            var filas = await _filaRepository.GetFilasAtivasAsync(skip, take);

            var dtos = filas.Select(f => _mapper.Map<FilaClienteDto>(f));

            return dtos;
        }
    }
}
