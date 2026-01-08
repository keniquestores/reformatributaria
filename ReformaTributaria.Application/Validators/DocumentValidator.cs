namespace ReformaTributaria.Application.Validators
{
    /// <summary>
    /// Classe auxiliar para validação de documentos brasileiros (CPF e CNPJ).
    /// </summary>
    public static class DocumentValidator
    {
        /// <summary>
        /// Remove caracteres não numéricos de uma string.
        /// </summary>
        public static string LimparDocumento(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return string.Empty;

            return new string(documento.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Valida se o documento é um CPF ou CNPJ válido.
        /// </summary>
        public static bool ValidarCpfOuCnpj(string documento)
        {
            var documentoLimpo = LimparDocumento(documento);

            return documentoLimpo.Length switch
            {
                11 => ValidarCpf(documentoLimpo),
                14 => ValidarCnpj(documentoLimpo),
                _ => false
            };
        }

        /// <summary>
        /// Valida um CPF (11 dígitos).
        /// </summary>
        public static bool ValidarCpf(string cpf)
        {
            var cpfLimpo = LimparDocumento(cpf);

            if (cpfLimpo.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (ex: 111.111.111-11)
            if (cpfLimpo.Distinct().Count() == 1)
                return false;

            // Valida primeiro dígito verificador
            var soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpfLimpo[i].ToString()) * (10 - i);

            var resto = soma % 11;
            var digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpfLimpo[9].ToString()) != digitoVerificador1)
                return false;

            // Valida segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpfLimpo[i].ToString()) * (11 - i);

            resto = soma % 11;
            var digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpfLimpo[10].ToString()) == digitoVerificador2;
        }

        /// <summary>
        /// Valida um CNPJ (14 dígitos).
        /// </summary>
        public static bool ValidarCnpj(string cnpj)
        {
            var cnpjLimpo = LimparDocumento(cnpj);

            if (cnpjLimpo.Length != 14)
                return false;

            // Verifica se todos os dígitos são iguais (ex: 00.000.000/0000-00)
            if (cnpjLimpo.Distinct().Count() == 1)
                return false;

            // Valida primeiro dígito verificador
            var multiplicadores1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpjLimpo[i].ToString()) * multiplicadores1[i];

            var resto = soma % 11;
            var digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cnpjLimpo[12].ToString()) != digitoVerificador1)
                return false;

            // Valida segundo dígito verificador
            var multiplicadores2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpjLimpo[i].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            var digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cnpjLimpo[13].ToString()) == digitoVerificador2;
        }
    }
}