namespace ReformaTributaria.Application.Services.Dtos
{
    /// <summary>
    /// DTO de resposta para Cliente com ChavePrivada descriptografada.
    /// </summary>
    public class ClienteResponseDto
    {
        public long Id { get; set; }
        public string QuestorId { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string InscricaoFederal { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public string ChavePrivada { get; set; } = string.Empty; 
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}