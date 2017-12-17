namespace Keeeys.Helpers
{
    public interface ICrypto
    {
        string CryptTimestampWithPrivateKey(long timestamp, string privateKeyBase64);
    }
}
