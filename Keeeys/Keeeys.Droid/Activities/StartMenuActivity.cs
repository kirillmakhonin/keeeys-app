using System;

using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;

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
            };

            FindViewById<Button>(Resource.Id.menu__check_keys).Click += delegate
            {
            };

            FindViewById<Button>(Resource.Id.menu__domain_organize).Click += delegate
            {
            };

            FindViewById<Button>(Resource.Id.menu__authorize).Click += delegate
            {
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
                        
        }

        private void UpdateViewElements()
        {
            FindViewById<Button>(Resource.Id.menu__check_keys).Enabled = IsLoggedIn;
            FindViewById<Button>(Resource.Id.menu__domain_organize).Enabled = IsLoggedIn;
            FindViewById<Button>(Resource.Id.menu__authorize).Text = LoginButtonText;
        }

        private bool IsLoggedIn
        {
            get { return false; }
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

                return "???";
            }
        }
                
    }
}


