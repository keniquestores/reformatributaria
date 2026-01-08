namespace ReformaTributaria.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        
        public string QuestorId { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string InscricaoFederal { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public string ChavePrivada { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
