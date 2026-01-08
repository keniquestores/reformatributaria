namespace ReformaTributaria.Application.Services.Dtos
{
    public class AuthorizeResponseDto
    {
        public string IdentityAccessToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresIn { get; set; }
    }
}

