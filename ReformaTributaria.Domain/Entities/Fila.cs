namespace ReformaTributaria.Domain.Entities
{
    public class Fila : BaseEntity
    {

        public long ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public string InscricaoFederalContribuinte { get; set; } = null!;        

        public DateTime DataHoraInsercao { get; set; }

        public bool Ativo { get; set; } = true;

        public List<ApuracaoExecucaoCBS> ApuracoesExecucaoCBS { get; set; } = new();
    }
}
