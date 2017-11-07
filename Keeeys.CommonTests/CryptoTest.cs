using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Keeeys.Helpers;
using System.Linq;
using PCLCrypto;
using System.Security.Cryptography.X509Certificates;

namespace Keeeys.CommonTests
{
    [TestClass]
    public class CryptoTest
    {
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        [TestMethod]
        public void TestMd5()
        {
            const string input = "test me";
            const string output = "7b0c2c2cbc980155d71ba3be4d174f56";

            byte[] outputBytes = StringToByteArray(output);
            byte[] decodedBytes = Convert.FromBase64String(Crypto.Md5Hash(input));

            CollectionAssert.AreEqual(outputBytes, decodedBytes);
        }

        [TestMethod]
        public void TestRSA()
        {
            const string publicKeyString = "MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAJeS/WvwRSOakUTuGyAXf8AWTh747eJQOCOZzHNHNTlAG5/Oz8Os9AEJX/M12crd0tqI8O+jDQo3n7Vpukm+eykCAwEAAQ==";
            const string privateKeyString = "MIIBVAIBADANBgkqhkiG9w0BAQEFAASCAT4wggE6AgEAAkEAl5L9a/BFI5qRRO4bIBd/wBZOHvjt4lA4I5nMc0c1OUAbn87Pw6z0AQlf8zXZyt3S2ojw76MNCjeftWm6Sb57KQIDAQABAkEAlLOhKc7blZVjZVOPiwlizGlVlO80Oe3nVY3iVUoLvjBSpbQpkANhlWTUrfPGSxR48z6ZuK5dVtWQ9Sf6QqvogQIhAM9TCHJdHUy0Y3c2gmisMq9GcQxrcO1j1Ew+fXyyD1xZAiEAuykp7MRgfxoCAiECPuNOm/8fLd+Fwg9/94N0DIcv+1ECIEwcJF6vlkEBe/5YsXkxtg2oY3n2u2c6ncY7rp+nUoJJAiBcgspQV/kCmk5n0v0TLLQMc5xrxlKNS7ALHhTcpG3ZIQIgAmncoXiWrb/nTjxRBit+qM7Fx/6oUkreMJejs270MNY==";
            byte[] data = new byte[] { 1 };
            const string encodedString = "MLMP62CuF/IV+0evFhK09YkljO0PlGGLyeXhmE+KwxoERWBZDPvzGJi+AAqXUsfb8EiJyIKCehU8mj+lvzgW9Q==";

            byte[] publicKey = Convert.FromBase64String(publicKeyString);
            byte[] privateKey = Convert.FromBase64String(privateKeyString);
            byte[] encoded = Convert.FromBase64String(encodedString);

            Console.WriteLine("PRIVATE: " + BitConverter.ToString(privateKey));
            Console.WriteLine("PUBLIC: " + BitConverter.ToString(publicKey));

            foreach (AsymmetricAlgorithm algo in Enum.GetValues(typeof(AsymmetricAlgorithm)))
            {
                try
                {
                    Console.WriteLine("TRY: {0}", algo);
                    var algorithm = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(algo);
                    ICryptographicKey publicKeyKey = algorithm.ImportKeyPair(privateKey, CryptographicPrivateKeyBlobType.Pkcs1RsaPrivateKey);

                    byte[] idealEncoded = WinRTCrypto.CryptographicEngine.Encrypt(publicKeyKey, data);

                    Console.WriteLine(BitConverter.ToString(idealEncoded));
                    Console.WriteLine(BitConverter.ToString(encoded));
                    //                CollectionAssert.AreEqual(idealEncoded, encoded);
                    Console.WriteLine("FOUNDED!");
                }
                catch (Exception e)
                {

                }
            }

            //var hasher = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
            //hasher.ImportPublicKey(publicKey);
            //hasher.ImportKeyPair(privateKey);



        }
    }
}
