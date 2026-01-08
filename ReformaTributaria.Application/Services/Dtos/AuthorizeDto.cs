namespace ReformaTributaria.Application.Services.Dtos
{
    public class AuthorizeDto
    {
        public string QuestorId { get; set; } = string.Empty;
        public string ChavePrivada { get; set; } = string.Empty;
        public string ApiSecret {get; set; } = string.Empty;
    }
}
