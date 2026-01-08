namespace ReformaTributaria.Application.Services.Dtos.Responses
{
    public class TokenCbsResponseDto
    {
        public int Expires_In { get; set; }

        public string Token_Type { get; set; } = string.Empty;

        public string Access_Token { get; set; } = string.Empty;

    }
}
