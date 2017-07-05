using System;
using System.Text;

namespace PlayByPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            IKeyVault vault =  new KeyVault();

            // Create HSM backed key.
            string keyName = "MyKey";
            string keyId = vault.CreateKey(keyName);

            // Test encryption and decryption.
            string dataToEncrypt = "Mary had a little lamb";

            byte[] encrypted = vault.Encrypt(keyId, Encoding.ASCII.GetBytes(dataToEncrypt));
            byte[] decrypted = vault.Decrypt(keyId, encrypted);

            var encryptedText = Convert.ToBase64String(encrypted);
            var decryptedData = Encoding.UTF8.GetString(decrypted);
  
            // Remove HSM backed key
            vault.DeleteKey(keyName);
        }
    }
}
