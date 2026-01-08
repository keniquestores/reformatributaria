namespace ReformaTributaria.Domain.Entities
{
    public class ApuracaoExecucaoCBS : BaseEntity
    {

        public string? Tiquete { get; set; } = null;

        public string? TiqueteArquivo { get; set; } = null;

        public DateTime? DataHoraSolicitacao { get; set; } = null;

        public DateTime? DataHoraRetornoTicketArquivo { get; set; } = null;

        public string? Mensagem { get; set; } = null;

        //TODO: Definir se será necessário armazenar o arquivo retornado


    }
}
