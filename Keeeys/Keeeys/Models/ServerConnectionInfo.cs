using SQLite.Net.Attributes;

namespace Keeeys.Common.Models
{
    public class ServerConnectionInfo : ICustomListItem
    {
        [PrimaryKey]
        public int Id { get; private set; }

        [MaxLength(128)]
        public string Server { get; private set; }
        [MaxLength(128)]
        public string Login { get; private set; }
        [MaxLength(256)]
        public string Token { get; private set; }

        public ServerConnectionInfo()
        {
            Id = 0;
            Server = "Undefined server";
            Login = "Undefined login";
            Token = "";
        }

        public ServerConnectionInfo(int Id, string Server, string Login, string Token)
        {
            this.Id = Id;
            this.Server = Server;
            this.Login = Login;
            this.Token = Token;
        }

        public int GetId()
        {
            return Id;
        }

        public string ToLabelString()
        {
            return Server;
        }
    }
}
