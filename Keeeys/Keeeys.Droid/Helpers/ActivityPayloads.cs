namespace Keeeys.Droid.Helpers
{
    public class ActivityPayloads
    {
        public const string PrivateKeyId = "PrivateKeyId";
        public const string ScanTitle = "ScanTitle";
        public const string ScanHelper = "ScanHelper";
        public const string ScannedCode = "ScannedCode";
        public const string AuthMode = "AuthMode";
        public const string WindowHeaderTitle = "WindowHeaderTitle";
        public const string ChoosenDomain = "ChoosenDomain";
        public const string ChoosenOrganization = "ChoosenOrganization";
        public const string ServerConnectionId = "ServerConnectionId";

        public enum AuthModeType { LOGIN = 1, LOGOUT = 0 };
    }
}