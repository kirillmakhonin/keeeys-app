namespace Keeeys.Helpers
{
    public interface ICrypto
    {
        string EncryptRSA(string privateKeyString, byte[] data);
        string CryptTimestampWithPrivateKey(long timestamp, string privateKeyBase64);
    }
}
