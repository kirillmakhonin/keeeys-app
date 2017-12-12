using Keeeys.Helpers;
using SQLite.Net.Attributes;
using System;
using Keeeys.Common.Helpers;

namespace Keeeys.Common.Models
{
    public class PrivateKey : ICustomListItem
    {
        [PrimaryKey]
        public int Id { get; private set; }

        public int PatricipentId { get; private set; }

        [MaxLength(128)]
        public string DomainName { get; private set; }
        [MaxLength(128)]
        public string OrganizationName { get; private set; }
        [MaxLength(256)]
        public string PrivateKeyString { get; private set; }

        public PrivateKey()
        {
            Id = 0;
            PatricipentId = 0;
            DomainName = "Undefined domain";
            OrganizationName = "Undefined organization";
            PrivateKeyString = "Private key string";
        }

        public PrivateKey(int Id, string DomainName, string OrganizationName, string PrivateKey)
        {
            this.Id = Id;
            this.DomainName = DomainName;
            this.OrganizationName = OrganizationName;
            this.PrivateKeyString = PrivateKeyString;
        }

        public int GetId()
        {
            return Id;
        }

        public string ToLabelString()
        {
            return String.Format("{0} / {1}", OrganizationName, DomainName);
        }

        public string DataForAuthorisation(ICrypto crypto, long? timestamp = null)
        {
            long timeForCalculating = timestamp.HasValue ? timestamp.Value : Time.GetTimestamp();
            string calculatedString = crypto.CryptTimestampWithPrivateKey(timeForCalculating, PrivateKeyString);

            QrEncoder.PublicAuthInfo data = new QrEncoder.PublicAuthInfo();
            data.ParticipantId = PatricipentId;
            data.PublicKey = calculatedString;

            return data.ToString();
        }

        public static PrivateKey Build(QrEncoder.PrivateSharingKeyInfo data, int id)
        {
            PrivateKey key = new PrivateKey();
            key.DomainName = data.DomainName;
            key.OrganizationName = data.OrganizationName;
            key.PatricipentId = data.ParticipantId;
            key.PrivateKeyString = data.PrivateKey;
            key.Id = id;
            return key;
        }
    }
}
