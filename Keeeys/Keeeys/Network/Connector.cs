using Keeeys.Common.Models;
using System;
using System.Net;
using System.Collections.Generic;

namespace Keeeys.Common.Network
{
    public class Connector
    {
        #region Support
        protected static string PrepareUrl(string url)
        {
            string retUrl = (url.StartsWith("http://") || url.StartsWith("https://")) ? url : "https://" + url;
            if (url.LastIndexOf(':') < url.Length - 8)
                retUrl = retUrl + ":8080";
            return retUrl;
        }
        #endregion

        #region Singleton
        private Connector() { }

        private static bool isSecureHasBeenResetted = false;

        public static Connector TryGet(string server, string login, string password)
        {

            string url = PrepareUrl(server);
            string token = null;
            try
            {
                Client temporaryClient = new Client();
                temporaryClient.BaseUrl = url;
                var task = temporaryClient.LoginAsync(login, password);
                task.Wait();
                token = temporaryClient.Token;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    throw exception.InnerException;
                else throw exception;
            }

            if (token == null)
                throw new Exception("Protocol error");

            return new Connector(url, login, token);
        }

        public static Connector Get(string server, string login, string token)
        {
            return new Connector(server, login, token);
        }

        public static Connector Get(ServerConnectionInfo info)
        {
            return Get(info.Server, info.Login, info.Token);
        }

        public static Connector Get(int id)
        {
            var connection = DataProvider.Get().GetStoredConnection(id);
            if (connection == null)
                return null;
            return Get();
        }

        public static Connector Get()
        {
            if (!DataProvider.Get().HasStoredConnections())
                return null;
            return Get(DataProvider.Get().SelectStoredConnection());
        }
        #endregion

        public string Server { get; private set; }
        public string Login { get; private set; }
        public string Token { get; private set; }
        private Client Handler;

        public ServerConnectionInfo ConnectionInfo
        {
            get
            {
                return new ServerConnectionInfo(0, Server, Login, Token);
            }
        }

        private Connector(string server, string login, string token)
        {
            string url = PrepareUrl(server);

            Server = url;
            Login = login;
            Token = token;

            Handler = new Client();
            Handler.BaseUrl = Server;
            Handler.Token = token;
            Handler.LinkCookies();
        }

        public void Logout()
        {
            try
            {
                var task = Handler.LogoutAsync();
                task.ContinueWith(x => x);
                task.Wait();
                Token = null;
                Handler.Token = null;
            }
            catch { }
        }

        public List<Organization> GetOrganizations()
        {
            var task = Handler.GetUserOrganizationsAsync();
            task.Wait();
            return new List<Organization>(task.Result);
        }

        public Domain GetDomain(int id)
        {
            var task = Handler.GetDomainByIdAsync(id);
            task.Wait();
            return task.Result;
        }

        public Organization GetOrganization(int id)
        {
            var task = Handler.GetOrganizationByIdAsync(id);
            task.Wait();
            return task.Result;
        }

        public byte[] RegisterPaticipantForImage(int domainId, string participantName)
        {
            var task = Handler.RegisterParticipantAsync(domainId, true, participantName);
            task.Wait();
            return task.Result as byte[];
        }

        public string RegisterPaticipantForString(int domainId, string participantName)
        {
            var task = Handler.RegisterParticipantAsync(domainId, false, participantName);
            task.Wait();
            return task.Result as string;
        }

        public bool Authenticate(int domainId, int participantId, string data)
        {
            try
            {
                var task = Handler.AuthenticateAsync(domainId, participantId, data);
                task.Wait();
                return true;
            }
            catch (Exception exception)
            {
                SwaggerException swaggerException = exception.InnerException as SwaggerException;
                if (swaggerException != null && swaggerException.StatusCode == "404")
                    return false;
                if (swaggerException != null && swaggerException.StatusCode == "400")
                    return false;
                throw swaggerException;
            }


        }
    }
}
