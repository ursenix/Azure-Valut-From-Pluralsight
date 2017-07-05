﻿using System.Security.Cryptography;

namespace PlayByPlay
{
    public class RsaWithRsaParameterKey
    {
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;

        public void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {                
                rsa.PersistKeyInCsp = false;               
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);                
            }
        }

        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cipherbytes;

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;                
                rsa.ImportParameters(_publicKey);


                cipherbytes = rsa.Encrypt(dataToEncrypt, false);
            }

            return cipherbytes;
        }

        public byte[] DecryptData(byte[] dataToEncrypt)
        {
            byte[] plain;

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                                
                rsa.ImportParameters(_privateKey);
                plain = rsa.Decrypt(dataToEncrypt, false);
            }

            return plain;
        }
    }
}