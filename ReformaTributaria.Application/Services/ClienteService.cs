using AutoMapper;
using ReformaTributaria.Application.Services.Dtos;
using ReformaTributaria.Application.Services.Interfaces;
using ReformaTributaria.Application.Validators.Dtos;
using ReformaTributaria.CrossCutting.Validation;
using ReformaTributaria.Domain.Common;
using ReformaTributaria.Domain.Entities;
using ReformaTributaria.Domain.Interfaces.Infra.Repositories;

namespace ReformaTributaria.Application.Services
{
    public class ClienteService(
        IClienteRepository clienteRepository,
        IValidatorHandler validator,
        IMapper mapper,
        AppSettings appSettings) : IClienteService
    {
        private readonly IClienteRepository _clienteRepository = clienteRepository;
        private readonly IValidatorHandler _validator = validator;
        private readonly IMapper _mapper = mapper;
        private readonly AppSettings _appSettings = appSettings;

        public async Task<ClienteResponseDto> GetByIdAsync(long id)
        {
            if (id <= 0)
                _validator.AddMsgErrorAndStopExecution("O Id tem que ser maior que zero.");

            var cliente = await _clienteRepository.GetByIdAsync(id);

            if (cliente == null)
                _validator.AddMsgErrorAndStopExecution("Cliente não encontrado.");

            return _mapper.Map<ClienteResponseDto>(cliente, opts => opts.Items["AppSettings"] = _appSettings);
        }

        public async Task<IEnumerable<ClienteResponseDto>> GetAllAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes, opts =>
                opts.Items["AppSettings"] = _appSettings);
        }

        public async Task<ClienteResponseDto> GetByQuestorIdAsync(string questorId)
        {
            if (string.IsNullOrWhiteSpace(questorId))
                _validator.AddMsgErrorAndStopExecution("QuestorId não pode ser vazio.");

            var cliente = await _clienteRepository.GetByQuestorIdAsync(questorId);

            if (cliente == null)
                _validator.AddMsgErrorAndStopExecution("Cliente não encontrado.");

            return _mapper.Map<ClienteResponseDto>(cliente, opts => opts.Items["AppSettings"] = _appSettings);
        }

        public async Task<ClienteResponseDto> CriarAsync(ClienteDto dto)
        {
            _validator.ValidateAndStopExecution(new ClienteDtoValidator(dto));

            var cliente = _mapper.Map<Cliente>(dto, opts => opts.Items["AppSettings"] = _appSettings);

            await _clienteRepository.AddAsync(cliente);

            return _mapper.Map<ClienteResponseDto>(cliente, opts => opts.Items["AppSettings"] = _appSettings); ;
        }

        public async Task AlterarAsync(ClienteDto dto, long id)
        {
            _validator.ValidateAndStopExecution(new ClienteDtoValidator(dto));

            if (id <= 0)
                _validator.AddMsgErrorAndStopExecution("O Id tem que ser maior que zero.");

            var cliente = await _clienteRepository.GetByIdAsync(id);

            if (cliente == null)
                _validator.AddMsgErrorAndStopExecution("Cliente não encontrado.");

            _mapper.Map(dto, cliente, opts => opts.Items["AppSettings"] = _appSettings);

            await _clienteRepository.UpdateAsync(cliente!);
        }

        public async Task DeletarAsync(long id)
        {
            if (id <= 0)
                _validator.AddMsgErrorAndStopExecution("O Id tem que ser maior que zero.");

            var cliente = await _clienteRepository.GetByIdAsync(id);

            if (cliente == null)
                _validator.AddMsgErrorAndStopExecution("Cliente não encontrado.");

            await _clienteRepository.DeleteAsync(id);
        }
    }
}
