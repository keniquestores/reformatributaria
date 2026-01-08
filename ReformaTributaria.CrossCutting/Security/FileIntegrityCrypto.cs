using System.Security.Cryptography;
using System.Text;

namespace ReformaTributaria.CrossCutting.Security
{
    /// <summary>
    /// Helper estático para criptografar e descriptografar dados com garantia de integridade.
    /// Implementa AES-256-GCM com IV/Nonce gerado criptograficamente de forma segura.
    /// A chave é fornecida em cada operação, evitando estado estático compartilhado.
    /// </summary>
    public static class FileIntegrityCrypto
    {
        private const int AesKeySize = 32; // 256 bits para AES-256
        private const int GcmNonceSize = 12; // 96 bits para GCM (padrão recomendado)
        private const int GcmTagSize = 16; // 128 bits para autenticação

        /// <summary>
        /// Criptografa o texto fornecido usando AES-256-GCM.
        /// </summary>
        /// <param name="plainText">Texto a ser criptografado</param>
        /// <param name="encryptionKeyBase64">Chave em formato Base64 com 32 bytes (256 bits)</param>
        /// <returns>String Base64 contendo: [Nonce(12 bytes)][Ciphertext][AuthTag(16 bytes)]</returns>
        /// <exception cref="ArgumentException">Quando plainText ou chave estão inválidos</exception>
        /// <exception cref="CryptographicException">Quando ocorre erro na criptografia</exception>
        public static string Encrypt(string plainText, string encryptionKeyBase64)
        {
            ValidatePlainText(plainText);
            var encryptionKey = ValidateAndDecodeKey(encryptionKeyBase64);

            try
            {
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                var nonce = new byte[GcmNonceSize];

                // Gerar Nonce de forma criptograficamente segura
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(nonce);
                }

                var ciphertext = new byte[plainBytes.Length];
                var tag = new byte[GcmTagSize];

                // Usar AES-256-GCM para criptografia e autenticação
                using (var aesGcm = new AesGcm(encryptionKey))
                {
                    // Criptografar e gerar tag de autenticação
                    aesGcm.Encrypt(nonce, plainBytes, ciphertext, tag);
                }

                // Combinar Nonce + Ciphertext + Tag para transmissão
                var result = new byte[nonce.Length + ciphertext.Length + tag.Length];
                Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
                Buffer.BlockCopy(ciphertext, 0, result, nonce.Length, ciphertext.Length);
                Buffer.BlockCopy(tag, 0, result, nonce.Length + ciphertext.Length, tag.Length);

                return Convert.ToBase64String(result);
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException(
                    "Erro ao criptografar os dados.",
                    ex);
            }
            catch (Exception ex)
            {
                throw new CryptographicException(
                    "Erro inesperado durante a criptografia.",
                    ex);
            }
        }

        /// <summary>
        /// Descriptografa o texto fornecido usando AES-256-GCM.
        /// </summary>
        /// <param name="cipherText">Texto criptografado em formato Base64</param>
        /// <param name="encryptionKeyBase64">Chave em formato Base64 com 32 bytes (256 bits)</param>
        /// <returns>Texto original descriptografado</returns>
        /// <exception cref="ArgumentException">Quando cipherText ou chave estão inválidos</exception>
        /// <exception cref="CryptographicException">Quando ocorre erro na descriptografia ou falha na validação de autenticidade</exception>
        public static string Decrypt(string cipherText, string encryptionKeyBase64)
        {
            ValidateCipherText(cipherText);
            var encryptionKey = ValidateAndDecodeKey(encryptionKeyBase64);

            try
            {
                byte[] encryptedData;
                try
                {
                    encryptedData = Convert.FromBase64String(cipherText);
                }
                catch (FormatException ex)
                {
                    throw new CryptographicException(
                        "Formato Base64 inválido para dados criptografados.",
                        ex);
                }

                // Validar tamanho mínimo: Nonce (12) + AuthTag (16)
                if (encryptedData.Length < GcmNonceSize + GcmTagSize)
                {
                    throw new CryptographicException(
                        "Dados criptografados inválidos. Tamanho insuficiente.");
                }

                // Extrair componentes
                var nonce = new byte[GcmNonceSize];
                var tag = new byte[GcmTagSize];
                var cipherBytes = new byte[encryptedData.Length - GcmNonceSize - GcmTagSize];

                Buffer.BlockCopy(encryptedData, 0, nonce, 0, GcmNonceSize);
                Buffer.BlockCopy(
                    encryptedData,
                    GcmNonceSize,
                    cipherBytes,
                    0,
                    cipherBytes.Length);
                Buffer.BlockCopy(
                    encryptedData,
                    GcmNonceSize + cipherBytes.Length,
                    tag,
                    0,
                    GcmTagSize);

                // Descriptografar e validar tag de autenticação
                var plainBytes = new byte[cipherBytes.Length];

                using (var aesGcm = new AesGcm(encryptionKey))
                {
                    try
                    {
                        aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);
                    }
                    catch (CryptographicException ex)
                    {
                        throw new CryptographicException(
                            "Falha na verificação de autenticidade. Dados podem estar corrompidos ou inválidos.",
                            ex);
                    }
                }

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (CryptographicException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CryptographicException(
                    "Erro inesperado durante a descriptografia.",
                    ex);
            }
        }

        /// <summary>
        /// Valida e decodifica a chave de criptografia.
        /// </summary>
        /// <param name="encryptionKeyBase64">Chave em formato Base64</param>
        /// <returns>Array de bytes da chave decodificada</returns>
        /// <exception cref="ArgumentException">Quando a chave está inválida ou mal formatada</exception>
        private static byte[] ValidateAndDecodeKey(string encryptionKeyBase64)
        {
            if (string.IsNullOrWhiteSpace(encryptionKeyBase64))
            {
                throw new ArgumentException(
                    "Chave de criptografia não pode ser nula ou vazia.",
                    nameof(encryptionKeyBase64));
            }

            try
            {
                var key = Convert.FromBase64String(encryptionKeyBase64);

                if (key.Length != AesKeySize)
                {
                    throw new ArgumentException(
                        $"Chave de criptografia deve ter exatamente {AesKeySize} bytes (256 bits). " +
                        $"Tamanho atual: {key.Length} bytes.",
                        nameof(encryptionKeyBase64));
                }

                return key;
            }
            catch (FormatException)
            {
                throw new ArgumentException(
                    "Chave de criptografia inválida. Deve estar em formato Base64.",
                    nameof(encryptionKeyBase64));
            }
        }

        /// <summary>
        /// Valida o texto a ser criptografado.
        /// </summary>
        /// <param name="plainText">Texto a validar</param>
        /// <exception cref="ArgumentException">Quando o texto é nulo ou vazio</exception>
        private static void ValidatePlainText(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentException(
                    "Texto a ser criptografado não pode ser nulo ou vazio.",
                    nameof(plainText));
            }
        }

        /// <summary>
        /// Valida o texto criptografado.
        /// </summary>
        /// <param name="cipherText">Texto criptografado a validar</param>
        /// <exception cref="ArgumentException">Quando o texto é nulo ou vazio</exception>
        private static void ValidateCipherText(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
            {
                throw new ArgumentException(
                    "Texto criptografado não pode ser nulo ou vazio.",
                    nameof(cipherText));
            }
        }
    }
}