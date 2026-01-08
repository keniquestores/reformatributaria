namespace ReformaTributaria.CrossCutting.Helpers
{
    public static class StringHelper
    {
        public static string SomenteDigitos(string value)
            => new string(value.Where(char.IsDigit).ToArray());
    }
}
