using System;
using System.Collections.Generic;
using System.Linq;
using Keeeys.Common.Helpers;
using Keeeys.Common.Exceptions;
using SQLite.Net;

namespace Keeeys.Common.Models
{
    public class DataProvider
    {
        public class IdOnly
        {
            public int Id { get; set; }
        }

        #region Singleton
        private static DataProvider instance;
        private static object threadLock = new object();

        public static DataProvider Get()
        {
            lock (threadLock)
            {
                if (instance == null)
                    instance = new DataProvider();

                return instance;
            }
        }
        #endregion

        #region Database
        public static SQLiteConnection Connection
        {
            get { return DatabaseManager.Connection; }
            set { DatabaseManager.Connection = value; }
        }
        #endregion


        public DataProvider()
        {
            if (DatabaseManager.Connection == null)
                throw new Exception("Database connection not set");

            PrepareDatabase();

        }

        private void PrepareDatabase()
        {
            Connection.CreateTable<PrivateKey>();
            Connection.CreateTable<ServerConnectionInfo>();
        }

        public List<PrivateKey> GetPrivateKeys()
        {
            return Connection.Table<PrivateKey>().ToList();
        }

        public PrivateKey GetPrivateKey(int id)
        {
            return Connection.Get<PrivateKey>(id);
        }

        public void RemovePrivateKey(int id)
        {
            Connection.Delete<PrivateKey>(id);
        }

        public int GetPrivateKeyNewId()
        {
            return GetNewId<PrivateKey>();
        }

        private int GetNewId<T>() where T : class
        {
            if (Connection.Table<T>().Count() > 0)
            {
                string query = "select id from " + typeof(T).Name + " order by Id desc limit 1";
                return Connection.Query<IdOnly>(query).First().Id + 1;

            }
            return 0;
        }

        public PrivateKey AddPrivateKey(PrivateKey key)
        {
            foreach (PrivateKey exists in GetPrivateKeys())
            {
                if (exists.DomainName == key.DomainName &&
                    exists.OrganizationName == key.OrganizationName)
                    throw new PrivateKeyAlreadyExistsException();
            }

            Connection.Insert(key);
            Connection.Commit();
            return key;
        }

        public bool HasStoredConnections()
        {
            return Connection.Table<ServerConnectionInfo>().Count() > 0;
        }

        public ServerConnectionInfo SelectStoredConnection()
        {
            if (HasStoredConnections())
                return Connection.Table<ServerConnectionInfo>().First();
            return null;
        }

        public ServerConnectionInfo GetStoredConnection(int id)
        {
            return Connection.Get<ServerConnectionInfo>(id);
        }

        public void AddConnectionInfo(ServerConnectionInfo connectionInfo)
        {
            ServerConnectionInfo newInfo = new ServerConnectionInfo(GetNewId<ServerConnectionInfo>(),
                connectionInfo.Server,
                connectionInfo.Login,
                connectionInfo.Token);
            Connection.Insert(connectionInfo);
        }

        public void RemoveConnectionInfo(int id)
        {
            Connection.Delete<ServerConnectionInfo>(id);
        }

        public void RemoveConnectionInfos()
        {
            Connection.DeleteAll<ServerConnectionInfo>();
        }
    }
}
