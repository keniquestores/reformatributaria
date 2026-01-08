using FluentValidation;
using ReformaTributaria.Application.Validators.Dtos;

namespace ReformaTributaria.Application.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteDtoValidator>
    {
        public ClienteValidator()
        {
            RuleFor(x => x.ClienteDto.QuestorId)
                .NotEmpty()
                .WithMessage("QuestorId não pode ser vazio.");

            RuleFor(x => x.ClienteDto.RazaoSocial)
                .NotEmpty()
                .WithMessage("Razão Social não pode ser vazia.");

            RuleFor(x => x.ClienteDto.InscricaoFederal)
                .NotEmpty()
                .WithMessage("Inscrição Federal não pode ser vazia.")
                .Must(BeValidCpfOrCnpj)
                .WithMessage("Inscrição Federal deve ser um CPF ou CNPJ válido.");

            RuleFor(x => x.ClienteDto.ClientId)
                .NotEmpty()
                .WithMessage("ClientId não pode ser vazio.");

            RuleFor(x => x.ClienteDto.ClientSecret)
                .NotEmpty()
                .WithMessage("ClientSecret não pode ser vazio.");
        }

        /// <summary>
        /// Valida se a inscrição federal é um CPF ou CNPJ válido.
        /// Remove formatação e valida apenas os números.
        /// </summary>
        private bool BeValidCpfOrCnpj(string inscricaoFederal)
        {
            if (string.IsNullOrWhiteSpace(inscricaoFederal))
                return false;

            return DocumentValidator.ValidarCpfOuCnpj(inscricaoFederal);
        }
    }
}
