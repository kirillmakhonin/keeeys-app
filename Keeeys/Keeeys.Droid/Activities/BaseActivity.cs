using System;

using Android.App;
using Android.OS;
using Keeeys.Droid.PlatformSpecific;
using Keeeys.Common.Models;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;

namespace Keeeys.Droid.Activities
{
    public class BaseActivity : Activity
    {
        private const string dbName = "database.sqlite";

        protected override void OnCreate(Bundle bundle)
        {
            CheckAndConnectToDatabase();
            base.OnCreate(bundle);
            //RequestWindowFeature(WindowFeatures.IndeterminateProgress);
        }

        protected void OpenLoadingDialog()
        {
            //SetProgressBarIndeterminateVisibility(true);
        }

        protected void CloseLoadingDialog()
        {
            //SetProgressBarIndeterminateVisibility(false);
        }

        private static bool connectedToDatabase = false;
        private void CheckAndConnectToDatabase()
        {
            if (!connectedToDatabase)
            {
                string databasePath = FileHelper.GetLocalFilePath(dbName);
                /// TODO: Database file removal
                //FileHelper.RemoveFile(databasePath);

                Android.Util.Log.Info("DB", "DB file {0}: {1}", databasePath, FileHelper.IsFileExists(databasePath) ? "exists" : "not exists");

                try
                {
                    DataProvider.Connection = new SQLiteConnection(new SQLitePlatformAndroid(), databasePath);
                }
                catch (Exception databaseConnectionException)
                {
                    Android.Util.Log.Error("DB", "Fail to create connection: {0}", databaseConnectionException);
                }

                Android.Util.Log.Info("DB", "Database path: {0}", DataProvider.Connection.DatabasePath);

                connectedToDatabase = true;

                DataProvider.Get();
            }
        }


    }
}