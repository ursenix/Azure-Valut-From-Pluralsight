namespace PlayByPlay
{
    public interface IKeyVault
    {
        string CreateKey(string keyName);
        void DeleteKey(string keyName);
        byte[] Encrypt(string keyId, byte[] dataToEncrypt);
        byte[] Decrypt(string keyId, byte[] dataToDecrypt);
    }
}
