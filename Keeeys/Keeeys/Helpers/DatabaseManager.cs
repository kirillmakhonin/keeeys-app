using System;

namespace Keeeys.Common.Helpers
{
    internal static class DatabaseManager
    {

        private static SQLite.Net.SQLiteConnection connection = null;

        public static SQLite.Net.SQLiteConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                if (connection == null)
                    connection = value;
                else
                    throw new Exception("Cannot change database connection");
            }
        }

    }
}
