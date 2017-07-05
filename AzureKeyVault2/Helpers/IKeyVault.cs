namespace PlayByPlay
{
    public interface IKeyVault
    {
        string CreateKey(string keyName);
        void DeleteKey(string keyName);
        byte[] WrapSymmetricKey(string keyId, byte[] symmetricKey);
        byte[] UnwrapSymmetricKey(string keyId, byte[] wrappedSymmetricKey);
    }
}
