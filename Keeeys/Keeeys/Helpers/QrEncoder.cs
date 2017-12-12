using System;

namespace Keeeys.Common.Helpers
{
    public class QrEncoder
    {
        public struct PublicAuthInfo
        {
            public int ParticipantId;
            public string PublicKey;

            public override string ToString()
            {
                return String.Format("{0}|{1}", ParticipantId, PublicKey);
            }

            public static PublicAuthInfo? Decode(string message)
            {
                string[] parts = message.Split('|');
                if (parts.Length != 2)
                    return null;

                PublicAuthInfo data = new PublicAuthInfo();
                if (!int.TryParse(parts[0], out data.ParticipantId))
                    return null;
                data.PublicKey = parts[1];

                return data;
            }
        }

        public struct PrivateSharingKeyInfo
        {
            public string OrganizationName;
            public string DomainName;
            public int ParticipantId;
            public string PrivateKey;

            public override string ToString()
            {
                return String.Format("{0}|{1}|{2}|{3}", OrganizationName, DomainName, ParticipantId, PrivateKey);
            }

            public static PrivateSharingKeyInfo? Decode(string message)
            {
                string[] parts = message.Split('|');
                if (parts.Length != 4)
                    return null;

                PrivateSharingKeyInfo data = new PrivateSharingKeyInfo();
                data.OrganizationName = parts[0];
                data.DomainName = parts[1];
                if (!int.TryParse(parts[2], out data.ParticipantId))
                    return null;
                data.PrivateKey = parts[3];

                return data;
            }
        }
    }
}
