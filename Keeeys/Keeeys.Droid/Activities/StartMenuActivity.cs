using System;

using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Keeeys.Droid.Helpers;
using Keeeys.Common.Models;

namespace Keeeys.Droid.Activities
{
	[Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/logo")]
	public class StartMenuActivity : BaseActivity
	{

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.StartMenu);

            LinkViewElements();
        }

        protected override void OnResume()
        {
            base.OnResume();

            UpdateViewElements();
        }

        private void LinkViewElements()
        {
            FindViewById<Button>(Resource.Id.menu__private_keys).Click += delegate
            {
                StartActivity(typeof(MyKeysListActivity));
            };

            FindViewById<Button>(Resource.Id.menu__check_keys).Click += delegate
            {
                var activity = new Intent(this, typeof(ScanActivity));
                activity.PutExtra(ActivityPayloads.ScanTitle, "Title");
                activity.PutExtra(ActivityPayloads.ScanHelper, "Helper");
                StartActivityForResult(activity, ActivityRequests.QRSCAN);
            };

            FindViewById<Button>(Resource.Id.menu__domain_organize).Click += delegate
            {
                StartActivity(typeof(ChooseDomainActivity));
            };

            FindViewById<Button>(Resource.Id.menu__authorize).Click += delegate
            {
                StartActivity(typeof(AuthActivity));
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                string scannedCode = data.GetStringExtra(ActivityPayloads.ScannedCode);
                if (requestCode == ActivityRequests.QRSCAN)
                {
                    Toast.MakeText(this, "Code: " + scannedCode, ToastLength.Long).Show();
                }
            }
        }

        private void UpdateViewElements()
        {
            FindViewById<Button>(Resource.Id.menu__check_keys).Enabled = IsLoggedIn;
            FindViewById<Button>(Resource.Id.menu__domain_organize).Enabled = IsLoggedIn;
            FindViewById<Button>(Resource.Id.menu__authorize).Text = LoginButtonText;
        }

        private bool IsLoggedIn
        {
            get { return true; }
        }

        private string Login
        {
            get { return IsLoggedIn ? "" : ""; }
        }

        private string LoginButtonText
        {
            get
            {
                if (!IsLoggedIn)
                    return Resources.GetString(Resource.String.menu__authorize__need_login);

                return Resources.GetString(Resource.String.menu__authorize__need_login);
            }
        }
                
    }
}


