using System;
using System.Text;

namespace PlayByPlay
{
    class Program
    {
        static void Main()
        {

            var rsa = new RsaWithXmlKey();

            const string original = "Text to encrypt";
            const string publicKeyPath = "c:\\temp\\publickey.xml";
            const string privateKeyPath = "c:\\temp\\privatekey.xml";

            rsa.AssignNewKey(publicKeyPath, privateKeyPath);
            var encrypted = rsa.EncryptData(publicKeyPath, Encoding.UTF8.GetBytes(original));
            var decrypted = rsa.DecryptData(privateKeyPath, encrypted);

            Console.WriteLine("Xml Based Key");
            Console.WriteLine();
            Console.WriteLine("   Original Text = " + original);
            Console.WriteLine();
            Console.WriteLine("   Encrypted Text = " + Convert.ToBase64String(encrypted));
            Console.WriteLine();
            Console.WriteLine("   Decrypted Text = " + Encoding.Default.GetString(decrypted));
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
