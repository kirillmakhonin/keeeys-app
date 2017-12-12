using System;
using System.Linq;

using Java.Security;
using Java.Security.Spec;
using Javax.Crypto;
using Android.Util;

namespace Keeeys.Droid.PlatformSpecific
{
    public class Crypto : Keeeys.Helpers.ICrypto
    {
        public Crypto() { }

        public string EncryptRSA(string privateKeyString, byte[] data)
        {
            try
            {
                byte[] privateKey = Base64.Decode(privateKeyString, Base64Flags.NoWrap);
                KeyFactory kf = KeyFactory.GetInstance("RSA");
                PKCS8EncodedKeySpec privateKeySpec = new PKCS8EncodedKeySpec(privateKey);
                IPrivateKey privateKeyKey = kf.GeneratePrivate(privateKeySpec);
                Cipher cipherEnc = Cipher.GetInstance("RSA");
                cipherEnc.Init(CipherMode.EncryptMode, privateKeyKey);
                byte[] result = cipherEnc.DoFinal(data);
                return Base64.EncodeToString(result, Base64Flags.NoWrap);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public string CryptTimestampWithPrivateKey(long timestamp, string privateKeyBase64)
        {
            byte[] payload = BitConverter.GetBytes(timestamp);
            byte[] reversedHack = payload.Reverse().ToArray();
            return EncryptRSA(privateKeyBase64, reversedHack);
        }
    }
}