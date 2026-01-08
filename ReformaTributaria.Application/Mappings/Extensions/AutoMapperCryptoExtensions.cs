using System.Linq.Expressions;
using AutoMapper;
using ReformaTributaria.CrossCutting.Security;
using ReformaTributaria.Domain.Common;

namespace ReformaTributaria.Application.Mappings.Extensions
{
    /// <summary>
    /// Extensões para facilitar criptografia/descriptografia no AutoMapper.
    /// </summary>
    public static class AutoMapperCryptoExtensions
    {
        /// <summary>
        /// Criptografa uma propriedade de destino usando um valor plaintext.
        /// </summary>
        /// <typeparam name="TSource">Tipo de origem</typeparam>
        /// <typeparam name="TDestination">Tipo de destino</typeparam>
        /// <typeparam name="TMember">Tipo do membro</typeparam>
        /// <param name="options">Opções de mapeamento</param>
        /// <param name="plainTextSelector">Lambda para obter o texto plano (null = gera GUID)</param>
        /// <returns>Opções de mapeamento configuradas</returns>
        public static void EncryptFrom<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> options,
            Expression<Func<TSource, string>>? plainTextSelector = null)
        {
            options.MapFrom((src, dest, destMember, context) =>
            {
                var appSettings = context.Items.TryGetValue("AppSettings", out var settings)
                    ? settings as AppSettings
                    : throw new InvalidOperationException("AppSettings não encontrado no contexto do AutoMapper.");

                // Se não forneceu lambda, gera GUID
                var plainText = plainTextSelector == null
                    ? Guid.NewGuid().ToString()
                    : plainTextSelector.Compile().Invoke(src);

                if (string.IsNullOrWhiteSpace(plainText))
                    return string.Empty;

                return FileIntegrityCrypto.Encrypt(plainText, appSettings!.Security.FileIntegrityCrypto.Key);
            });
        }

        /// <summary>
        /// Descriptografa uma propriedade de origem.
        /// </summary>
        /// <typeparam name="TSource">Tipo de origem</typeparam>
        /// <typeparam name="TDestination">Tipo de destino</typeparam>
        /// <typeparam name="TMember">Tipo do membro</typeparam>
        /// <param name="options">Opções de mapeamento</param>
        /// <param name="encryptedValueSelector">Lambda para obter o valor criptografado</param>
        /// <returns>Opções de mapeamento configuradas</returns>
        public static void DecryptFrom<TSource, TDestination, TMember>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> options,
            Expression<Func<TSource, string>> encryptedValueSelector)
        {
            options.MapFrom((src, dest, destMember, context) =>
            {
                var appSettings = context.Items.TryGetValue("AppSettings", out var settings)
                    ? settings as AppSettings
                    : throw new InvalidOperationException("AppSettings não encontrado no contexto do AutoMapper.");

                var encryptedValue = encryptedValueSelector.Compile().Invoke(src);

                if (string.IsNullOrWhiteSpace(encryptedValue))
                    return string.Empty;

                try
                {
                    return FileIntegrityCrypto.Decrypt(encryptedValue, appSettings.Security.FileIntegrityCrypto.Key);
                }
                catch
                {
                    return string.Empty;
                }
            });
        }
    }
}