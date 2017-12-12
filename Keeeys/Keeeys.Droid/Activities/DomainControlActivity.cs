using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Keeeys.Droid.Helpers;
using Keeeys.Common.Network;
using Keeeys.Common.Helpers;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "DomainControlActivity")]
    public class DomainControlActivity : BaseActivity
    {
        private Connector connection;
        private Organization organization;
        private Domain domain;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            int connectionId = Intent.GetIntExtra(ActivityPayloads.ServerConnectionId, -1);
            int choosenDomain = Intent.GetIntExtra(ActivityPayloads.ChoosenDomain, -1);
            int choosenOrganization = Intent.GetIntExtra(ActivityPayloads.ChoosenOrganization, -1);

            if (connectionId < 0 || choosenDomain < 0 || choosenOrganization < 0)
                Finish();

            connection = Connector.Get(connectionId);
            if (connection == null)
                Finish();

            organization = connection.GetOrganization(choosenOrganization);
            domain = connection.GetDomain(choosenDomain);

            StartScan();
        }

        protected void StartScan()
        {
            var activity = new Intent(this, typeof(ScanActivity));
            string title = string.Format("{0} / {1}", organization.Name, domain.Name);
            activity.PutExtra(ActivityPayloads.ScanTitle, title);
            activity.PutExtra(ActivityPayloads.ScanHelper, Resources.GetString(Resource.String.layout__face_controll_list__scan_description));
            StartActivityForResult(activity, ActivityRequests.QRSCAN);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && requestCode == ActivityRequests.QRSCAN)
            {
                int choosenDomain = data.GetIntExtra(ActivityPayloads.ChoosenDomain, -1);
                int choosenOrganization = data.GetIntExtra(ActivityPayloads.ChoosenOrganization, -1);
                string scannedCode = data.GetStringExtra(ActivityPayloads.ScannedCode);

                QrEncoder.PublicAuthInfo? code = QrEncoder.PublicAuthInfo.Decode(scannedCode);
                if (!code.HasValue)
                    Toast.MakeText(this, Resources.GetString(Resource.String.layout__private_keys__scan_error), ToastLength.Long).Show();
                try
                {
                    if (connection.Authenticate((int)domain.Id.Value, code.Value.ParticipantId, code.Value.PublicKey))
                        Toast.MakeText(this, Resources.GetString(Resource.String.layout__domain_controll__good_code), ToastLength.Long).Show();
                    else
                        Toast.MakeText(this, Resources.GetString(Resource.String.layout__domain_controll__wrong_code), ToastLength.Long).Show();
                }
                catch (Exception exception)
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.protocol_error), ToastLength.Long).Show();
                    System.Console.WriteLine(exception);
                }

                Finish();
            }
            else
            {
                Finish();
            }
        }
    }
}