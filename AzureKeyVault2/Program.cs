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

            // Generate and wrap symmetric encryption key
            AesEncryption aes = new AesEncryption();
            byte[] testSymmetricKey = aes.GenerateRandomNumber(32);
            byte[] iv = aes.GenerateRandomNumber(16);

            // This wrapped key can be stored in your database
            byte[] wrappedSymmetricKey = vault.WrapSymmetricKey(keyId, testSymmetricKey);

            // Before we can encrypt or decrypt using the key we need to unwrap it with the vault key
            byte[] unwrappedSymmetricKey = vault.UnwrapSymmetricKey(keyId, wrappedSymmetricKey);

            // Perfor local encryption operation using the unwrapped vault key
            string dataToEncrypt = "Mary had a little lamb";
            byte[] encrypted = aes.Encrypt(Encoding.ASCII.GetBytes(dataToEncrypt), unwrappedSymmetricKey, iv);
            byte[] decrypted = aes.Decrypt(encrypted, unwrappedSymmetricKey, iv);

            string decryptedData = Encoding.UTF8.GetString(decrypted);

            // Remove HSM backed key
            vault.DeleteKey(keyName);
        }
    }
}
