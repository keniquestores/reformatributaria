namespace ReformaTributaria.CrossCutting.Models
{
    public class GenericResponseType<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; } = default!;
        public List<string> Messages { get; set; } = [];
    }
}
