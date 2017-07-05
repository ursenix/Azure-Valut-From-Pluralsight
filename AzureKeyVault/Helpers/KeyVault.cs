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

        public byte[] Encrypt(string keyId, byte[] dataToEncrypt)
        {
            if (string.IsNullOrEmpty(keyId))
            {
                throw new ArgumentNullException(keyId, "Key Id is Null.");
            }

            if (dataToEncrypt == null)
            {
                throw new ArgumentNullException(nameof(dataToEncrypt), "Data to Encrypt is Null.");
            }

            if (dataToEncrypt.Length == 0)
            {
                throw new ArgumentNullException(nameof(dataToEncrypt), "Data to Encrypt is Empty.");
            }

            var operationResult = KeyVaultClient.EncryptAsync(keyId, JsonWebKeyEncryptionAlgorithm.RSAOAEP, dataToEncrypt).GetAwaiter().GetResult();

            return operationResult.Result;
        }

        public byte[] Decrypt(string keyId, byte[] dataToDecrypt)
        {
            if (string.IsNullOrEmpty(keyId))
            {
                throw new ArgumentNullException(keyId, "Key Name is Null.");
            }

            if (dataToDecrypt == null)
            {
                throw new ArgumentNullException(nameof(dataToDecrypt), "Data to Derypt is Null.");
            }

            if (dataToDecrypt.Length == 0)
            {
                throw new ArgumentNullException(nameof(dataToDecrypt), "Data to Encrypt is Empty.");
            }

            var operationResult = KeyVaultClient.DecryptAsync(keyId, JsonWebKeyEncryptionAlgorithm.RSAOAEP, dataToDecrypt).GetAwaiter().GetResult();

            return operationResult.Result;
        }
    }
}
