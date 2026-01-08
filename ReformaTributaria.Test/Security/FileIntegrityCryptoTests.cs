using System.Security.Cryptography;
using ReformaTributaria.CrossCutting.Security;
using Xunit;

namespace ReformaTributaria.Test.Security
{
    /// <summary>
    /// Testes unitários para a classe FileIntegrityCrypto.
    /// Cobre cenários de criptografia, descriptografia e validações.
    /// </summary>
    public class FileIntegrityCryptoTests
    {
        private const string ValidKeyBase64 = "8mHbiqD8/FVXn5jS+PkwvZ7Q2X4rN8pL3K9mJ2eH5sI="; // 32 bytes em Base64
        private const string InvalidKeyBase64InvalidFormat = "not-a-valid-base64!!!";
        private const string ShortKeyBase64 = "aGVsbG8gd29ybGQ="; // "hello world" em Base64 (11 bytes)
        private const string LongKeyBase64 = "dGhpcyBpcyBhIHZlcnkgbG9uZyBrZXkgdGhhdCBleGNlZWRzIDMyIGJ5dGVzIGluIGxlbmd0aA=="; // > 32 bytes

        #region Encrypt Tests

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithValidPlainTextAndKey_ShouldReturnBase64String()
        {
            // Arrange
            var plainText = "Dados sensíveis para criptografar";

            // Act
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Assert
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
            Exception? exception = Record.Exception(() => Convert.FromBase64String(encrypted));
Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithShortPlainText_ShouldSucceed()
        {
            // Arrange
            var plainText = "x";

            // Act
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Assert
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithLongPlainText_ShouldSucceed()
        {
            // Arrange
            var plainText = new string('A', 10000);

            // Act
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Assert
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithSpecialCharacters_ShouldSucceed()
        {
            // Arrange
            var plainText = "!@#$%^&*()_+-=[]{}|;:',.<>?/~`中文テキスト";

            // Act
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Assert
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithSamePlainTextTwice_ShouldProduceDifferentCiphertexts()
        {
            // Arrange
            var plainText = "Mesmo texto";

            // Act
            var encrypted1 = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);
            var encrypted2 = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Assert
            Assert.NotEqual(encrypted1, encrypted2);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithNullPlainText_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Encrypt(null!, ValidKeyBase64));

            Assert.Contains("não pode ser nulo ou vazio", ex.Message);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithEmptyPlainText_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Encrypt(string.Empty, ValidKeyBase64));

            Assert.Contains("não pode ser nulo ou vazio", ex.Message);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithWhitespacePlainText_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Encrypt("   ", ValidKeyBase64));

            Assert.Contains("não pode ser nulo ou vazio", ex.Message);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithNullKey_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Encrypt("Texto", null!));

            Assert.Contains("não pode ser nula ou vazia", ex.Message);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithInvalidBase64Key_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Encrypt("Texto", InvalidKeyBase64InvalidFormat));

            Assert.Contains("Base64", ex.Message);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithShortKey_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Encrypt("Texto", ShortKeyBase64));

            Assert.Contains("256 bits", ex.Message);
        }

        [Fact]
        [Trait("Category", "Encryption")]
        public void Encrypt_WithLongKey_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Encrypt("Texto", LongKeyBase64));

            Assert.Contains("256 bits", ex.Message);
        }

        #endregion

        #region Decrypt Tests

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithValidCipherText_ShouldReturnOriginalPlainText()
        {
            // Arrange
            var plainText = "Dados sensíveis para criptografar";
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Act
            var decrypted = FileIntegrityCrypto.Decrypt(encrypted, ValidKeyBase64);

            // Assert
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithShortPlainText_ShouldSucceed()
        {
            // Arrange
            var plainText = "x";
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Act
            var decrypted = FileIntegrityCrypto.Decrypt(encrypted, ValidKeyBase64);

            // Assert
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithLongPlainText_ShouldSucceed()
        {
            // Arrange
            var plainText = new string('B', 10000);
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Act
            var decrypted = FileIntegrityCrypto.Decrypt(encrypted, ValidKeyBase64);

            // Assert
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithSpecialCharacters_ShouldSucceed()
        {
            // Arrange
            var plainText = "!@#$%^&*()_+-=[]{}|;:',.<>?/~`中文テキスト";
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);

            // Act
            var decrypted = FileIntegrityCrypto.Decrypt(encrypted, ValidKeyBase64);

            // Assert
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithNullCipherText_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Decrypt(null!, ValidKeyBase64));

            Assert.Contains("não pode ser nulo ou vazio", ex.Message);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithEmptyCipherText_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Decrypt(string.Empty, ValidKeyBase64));

            Assert.Contains("não pode ser nulo ou vazio", ex.Message);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithWhitespaceCipherText_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Decrypt("   ", ValidKeyBase64));

            Assert.Contains("não pode ser nulo ou vazio", ex.Message);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithNullKey_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                FileIntegrityCrypto.Decrypt("YWJj", null!));

            Assert.Contains("não pode ser nula ou vazia", ex.Message);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithInvalidBase64_ShouldThrowCryptographicException()
        {
            // Act & Assert
            var ex = Assert.Throws<CryptographicException>(() =>
                FileIntegrityCrypto.Decrypt("not-valid-base64!!!", ValidKeyBase64));

            Assert.Contains("Base64", ex.Message);
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithTamperedCipherText_ShouldThrowCryptographicException()
        {
            // Arrange
            var plainText = "Dados sensíveis";
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);
            
            var tamperedBytes = Convert.FromBase64String(encrypted);
            tamperedBytes[tamperedBytes.Length / 2] ^= 0xFF;
            var tamperedBase64 = Convert.ToBase64String(tamperedBytes);

            // Act & Assert
            var ex = Assert.Throws<CryptographicException>(() =>
                FileIntegrityCrypto.Decrypt(tamperedBase64, ValidKeyBase64));

            Assert.Contains("autenticidade", ex.Message.ToLower());
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithTruncatedCipherText_ShouldThrowCryptographicException()
        {
            // Arrange
            var plainText = "Dados";
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);
            
            // Truncar para um tamanho válido em Base64, mas menor que o mínimo requerido (Nonce 12 + Tag 16 = 28 bytes)
            // Um tamanho de 15 bytes decodificado seria menor que os 28 bytes mínimos
            var encryptedBytes = Convert.FromBase64String(encrypted);
            var truncatedBytes = new byte[15]; // Menor que 28 (Nonce + Tag)
            Array.Copy(encryptedBytes, truncatedBytes, 15);
            var truncatedBase64 = Convert.ToBase64String(truncatedBytes);

            // Act & Assert
            var ex = Assert.Throws<CryptographicException>(() =>
                FileIntegrityCrypto.Decrypt(truncatedBase64, ValidKeyBase64));

            Assert.Contains("insuficiente", ex.Message.ToLower());
        }

        [Fact]
        [Trait("Category", "Decryption")]
        public void Decrypt_WithDifferentKey_ShouldThrowCryptographicException()
        {
            // Arrange
            var plainText = "Dados sensíveis";
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);
            var differentKeyBase64 = "9nIcjqE9/GWYo6kT+QlxwaB8R3u5O9qM4L+nK3fI6tJ=";

            // Act & Assert
            var ex = Assert.Throws<CryptographicException>(() =>
                FileIntegrityCrypto.Decrypt(encrypted, differentKeyBase64));

            Assert.Contains("autenticidade", ex.Message.ToLower());
        }

        #endregion

        #region Round-Trip Tests

        [Theory]
        [InlineData("Simples")]
        [InlineData("Com espaços e números 12345")]
        [InlineData("!@#$%^&*()")]
        [InlineData("中文テキスト")]
        [Trait("Category", "RoundTrip")]
        public void Encrypt_Then_Decrypt_ShouldReturnOriginalValue(string plainText)
        {
            // Act
            var encrypted = FileIntegrityCrypto.Encrypt(plainText, ValidKeyBase64);
            var decrypted = FileIntegrityCrypto.Decrypt(encrypted, ValidKeyBase64);

            // Assert
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        [Trait("Category", "RoundTrip")]
        public void MultipleEncryptDecryptCycles_ShouldMaintainIntegrity()
        {
            // Arrange
            var originalTexts = new[]
            {
                "Texto 1",
                "Texto 2",
                "Texto 3"
            };

            // Act & Assert
            foreach (var text in originalTexts)
            {
                var encrypted = FileIntegrityCrypto.Encrypt(text, ValidKeyBase64);
                var decrypted = FileIntegrityCrypto.Decrypt(encrypted, ValidKeyBase64);
                Assert.Equal(text, decrypted);
            }
        }
       
        #endregion
    }
}