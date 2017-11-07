using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;

namespace Keeeys.Helpers
{
    public static class Crypto
    {
        public static string Md5Hash(string value)
        {
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Md5);
            var bytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = hasher.HashData(bytes);
            
            return Convert.ToBase64String(hashBytes);            
        }
                
    }
}
