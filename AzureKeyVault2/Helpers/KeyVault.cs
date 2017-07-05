using System;
using System.Configuration;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PlayByPlay
{
    public class KeyVault : KeyVaultBase, IKeyVault
    {
        public KeyVault()
        {
            var clientId = ConfigurationManager.AppSettings["AuthClientId"];
            var clientSecret = ConfigurationManager.AppSettings["AuthClientSecret"];
            VaultAddress = ConfigurationManager.AppSettings["VaultUrl"];

            ClientCredential = new ClientCredential(clientId, clientSecret);
            KeyVaultClient = new KeyVaultClient(GetAccessToken, GetHttpClient());
        }

        public string CreateKey(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException(keyName);
            }

            var keyBundle = GetKeyBundle();
            var createdKey =
                KeyVaultClient.CreateKeyAsync(VaultAddress, keyName, keyBundle.Key.Kty,
                    keyAttributes: keyBundle.Attributes, tags: GetKeyTags()).GetAwaiter().GetResult();

            return createdKey.KeyIdentifier.Identifier;
        }

        public void DeleteKey(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException(keyName);
            }

            KeyVaultClient.DeleteKeyAsync(VaultAddress, keyName).GetAwaiter().GetResult();
        }

        public byte[] WrapSymmetricKey(string keyId, byte[] symmetricKey)
        {
            if (string.IsNullOrEmpty(keyId))
            {
                throw new ArgumentNullException(keyId, "Key Name is Null.");
            }

            if (symmetricKey == null)
            {
                throw new ArgumentNullException(nameof(symmetricKey), "Symetric key is Null.");
            }

            if (symmetricKey.Length == 0)
            {
                throw new ArgumentNullException(nameof(symmetricKey), "Symmetric key is Empty.");
            }

            var wrappedKey = KeyVaultClient.WrapKeyAsync(keyId, JsonWebKeyEncryptionAlgorithm.RSAOAEP, symmetricKey).GetAwaiter().GetResult();

            return wrappedKey.Result;
        }

        public byte[] UnwrapSymmetricKey(string keyId, byte[] wrappedSymmetricKey)
        {
            if (string.IsNullOrEmpty(keyId))
            {
                throw new ArgumentNullException(keyId, "Key Name is Null.");
            }

            if (wrappedSymmetricKey == null)
            {
                throw new ArgumentNullException(nameof(wrappedSymmetricKey), "Wrapped symetric key is Null.");
            }

            if (wrappedSymmetricKey.Length == 0)
            {
                throw new ArgumentNullException(nameof(wrappedSymmetricKey), "Wrapped symmetric key is Empty.");
            }

            var unwrappedKey = KeyVaultClient.UnwrapKeyAsync(keyId, JsonWebKeyEncryptionAlgorithm.RSAOAEP, wrappedSymmetricKey).GetAwaiter().GetResult();

            return unwrappedKey.Result;
        }

    }
}
