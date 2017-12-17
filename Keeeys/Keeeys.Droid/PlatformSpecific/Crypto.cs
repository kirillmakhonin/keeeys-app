using System;
using System.Linq;

using Java.Security;
using Java.Security.Spec;
using Javax.Crypto;
using Java.Nio;
using Java.Util;
using Android.Util;

namespace Keeeys.Droid.PlatformSpecific
{
    public class Crypto : Keeeys.Helpers.ICrypto
    {
        public Crypto() { }

        public static Keeeys.Helpers.ICrypto Instance
        {
            get { return new Crypto(); }
        }

        public string CryptTimestampWithPrivateKey(long timestamp, string privateKeyBase64)
        {
            KeyFactory kf = KeyFactory.GetInstance("RSA");
            byte[] privateKeyBytes = Base64.Decode(privateKeyBase64, Base64Flags.Default);
            IPrivateKey privateKey = kf.GeneratePrivate(new PKCS8EncodedKeySpec(privateKeyBytes));

            Cipher cipherEnc = Cipher.GetInstance("RSA/ECB/PKCS1Padding");
            cipherEnc.Init(Cipher.EncryptMode, privateKey);

            byte[] payload = BitConverter.GetBytes(timestamp);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(payload);

            byte[] encTime = cipherEnc.DoFinal(payload);
            String encodedTimeBase64 = Base64.EncodeToString(encTime, Base64Flags.Default | Base64Flags.NoWrap);

            return encodedTimeBase64;
        }
    }
}