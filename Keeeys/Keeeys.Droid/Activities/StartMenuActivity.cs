using System;
using System.Net;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
using Keeeys.Droid.Helpers;
using Keeeys.Common.Network;
using Keeeys.Common.Models;
using Keeeys.Droid.PlatformSpecific;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/logo")]
    public class StartMenuActivity : BaseActivity
    {
        private static bool isSecureHasBeenResetted = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.StartMenu);

            LinkViewElements();
            
            if (!isSecureHasBeenResetted)
            {
                ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
                isSecureHasBeenResetted = true;
            }
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
                StartFaceControll();
            };

            FindViewById<Button>(Resource.Id.menu__domain_organize).Click += delegate
            {
                StartDomainOrganisation();
            };

            FindViewById<Button>(Resource.Id.menu__authorize).Click += delegate
            {
                var activity = new Intent(this, typeof(AuthActivity));
                activity.PutExtra(ActivityPayloads.AuthMode, (int)(IsLoggedIn ? ActivityPayloads.AuthModeType.LOGOUT : ActivityPayloads.AuthModeType.LOGIN));
                StartActivity(activity);
            };
        }

        private void StartFaceControll()
        {
            var activity = new Intent(this, typeof(ChooseDomainActivity));
            activity.PutExtra(ActivityPayloads.ServerConnectionId, DataProvider.Get().SelectStoredConnection().Id);
            activity.PutExtra(ActivityPayloads.WindowHeaderTitle, Resources.GetString(Resource.String.layout__face_controll_list__choose_domain));
            StartActivityForResult(activity, ActivityRequests.CHOOSEDOMAIN_FOR_CONTROL);
        }

        private void StartDomainOrganisation()
        {
            var activity = new Intent(this, typeof(ChooseDomainActivity));
            activity.PutExtra(ActivityPayloads.ServerConnectionId, DataProvider.Get().SelectStoredConnection().Id);
            activity.PutExtra(ActivityPayloads.WindowHeaderTitle, Resources.GetString(Resource.String.layout__domain_controll_list__choose_domain));
            StartActivityForResult(activity, ActivityRequests.CHOOSEDOMAIN_FOR_EDITION);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                int choosenDomain = data.GetIntExtra(ActivityPayloads.ChoosenDomain, -1);
                int choosenOrganization = data.GetIntExtra(ActivityPayloads.ChoosenOrganization, -1);
                if (requestCode == ActivityRequests.CHOOSEDOMAIN_FOR_CONTROL)
                {
                    var activity = new Intent(this, typeof(DomainControlActivity));
                    activity.PutExtra(ActivityPayloads.ServerConnectionId, DataProvider.Get().SelectStoredConnection().Id);
                    activity.PutExtra(ActivityPayloads.ChoosenDomain, (int)choosenDomain);
                    activity.PutExtra(ActivityPayloads.ChoosenOrganization, (int)choosenOrganization);
                    StartActivity(activity);
                }
                else if (requestCode == ActivityRequests.CHOOSEDOMAIN_FOR_EDITION)
                {
                    var activity = new Intent(this, typeof(DomainUsersActivity));
                    activity.PutExtra(ActivityPayloads.ServerConnectionId, DataProvider.Get().SelectStoredConnection().Id);
                    activity.PutExtra(ActivityPayloads.ChoosenDomain, (int)choosenDomain);
                    activity.PutExtra(ActivityPayloads.ChoosenOrganization, (int)choosenOrganization);
                    StartActivity(activity);
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
            get { return DataProvider.Get().HasStoredConnections(); }
        }

        private string Login
        {
            get { return IsLoggedIn ? DataProvider.Get().SelectStoredConnection().Login : ""; }
        }

        private string LoginButtonText
        {
            get
            {
                if (!IsLoggedIn)
                    return Resources.GetString(Resource.String.menu__authorize__need_login);

                return String.Format("{0} [{1}]", Resources.GetString(Resource.String.menu__authorize__logout), Login);
            }
        }

    }
}


