using FluentValidation;
using ReformaTributaria.Application.Services.Dtos;

namespace ReformaTributaria.Application.Validators.Dtos
{
    public class FilaDtoValidator : AbstractValidator<FilaDto>
    {

        public FilaDtoValidator()
        {
            RuleFor(x => x.InscricaoFederalContribuinte).NotNull().WithMessage("O Campo Inscrição Federal do Contribuinte é obrigatório.");
        }
    }
}
