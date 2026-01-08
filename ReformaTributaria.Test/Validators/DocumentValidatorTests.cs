using ReformaTributaria.Application.Validators;
using Xunit;

namespace ReformaTributaria.Test.Validators
{
    /// <summary>
    /// Testes unitários para validação de CPF e CNPJ.
    /// </summary>
    public class DocumentValidatorTests
    {
        #region CPF Tests

        [Theory]
        [InlineData("123.456.789-09")]
        [InlineData("12345678909")]
        [InlineData("111.444.777-35")]
        [Trait("Category", "CPF")]
        public void ValidarCpf_WithValidCpf_ShouldReturnTrue(string cpf)
        {
            // Act
            var result = DocumentValidator.ValidarCpf(cpf);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("000.000.000-00")]
        [InlineData("11111111111")]
        [InlineData("123.456.789-00")] // Dígito verificador inválido
        [InlineData("12345678900")] // Dígito verificador inválido
        [InlineData("123456789")] // Menos de 11 dígitos
        [Trait("Category", "CPF")]
        public void ValidarCpf_WithInvalidCpf_ShouldReturnFalse(string cpf)
        {
            // Act
            var result = DocumentValidator.ValidarCpf(cpf);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region CNPJ Tests

        [Theory]
        [InlineData("11.222.333/0001-81")]
        [InlineData("11222333000181")]
        [InlineData("34.028.316/0001-03")]
        [Trait("Category", "CNPJ")]
        public void ValidarCnpj_WithValidCnpj_ShouldReturnTrue(string cnpj)
        {
            // Act
            var result = DocumentValidator.ValidarCnpj(cnpj);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("00.000.000/0000-00")]
        [InlineData("11111111111111")]
        [InlineData("11.222.333/0001-00")] // Dígito verificador inválido
        [InlineData("1122233300018")] // Menos de 14 dígitos
        [Trait("Category", "CNPJ")]
        public void ValidarCnpj_WithInvalidCnpj_ShouldReturnFalse(string cnpj)
        {
            // Act
            var result = DocumentValidator.ValidarCnpj(cnpj);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region CPF or CNPJ Tests

        [Theory]
        [InlineData("123.456.789-09")] // CPF válido
        [InlineData("11.222.333/0001-81")] // CNPJ válido
        [InlineData("12345678909")] // CPF sem formatação
        [InlineData("11222333000181")] // CNPJ sem formatação
        [Trait("Category", "CpfOrCnpj")]
        public void ValidarCpfOuCnpj_WithValidDocument_ShouldReturnTrue(string documento)
        {
            // Act
            var result = DocumentValidator.ValidarCpfOuCnpj(documento);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("000.000.000-00")] // CPF inválido
        [InlineData("00.000.000/0000-00")] // CNPJ inválido
        [InlineData("12345")] // Tamanho inválido
        [InlineData("")] // Vazio
        [InlineData(null)] // Nulo
        [Trait("Category", "CpfOrCnpj")]
        public void ValidarCpfOuCnpj_WithInvalidDocument_ShouldReturnFalse(string documento)
        {
            // Act
            var result = DocumentValidator.ValidarCpfOuCnpj(documento);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region LimparDocumento Tests

        [Theory]
        [InlineData("123.456.789-09", "12345678909")]
        [InlineData("11.222.333/0001-81", "11222333000181")]
        [InlineData("abc123def456", "123456")]
        [InlineData("   111   222   ", "111222")]
        [Trait("Category", "LimparDocumento")]
        public void LimparDocumento_ShouldRemoveNonNumericCharacters(string input, string expected)
        {
            // Act
            var result = DocumentValidator.LimparDocumento(input);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}