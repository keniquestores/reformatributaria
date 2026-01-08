namespace ReformaTributaria.CrossCutting.Models
{
    public class GenericResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; } = null!;
        public List<string> Messages { get; set; } = [];
    }
}
