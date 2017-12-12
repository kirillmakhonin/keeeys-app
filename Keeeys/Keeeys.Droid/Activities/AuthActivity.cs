using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Keeeys.Droid.Helpers;
using Keeeys.Common.Exceptions;
using Keeeys.Common.Network;
using Keeeys.Common.Models;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "AuthActivity")]
    public class AuthActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                var authType = (ActivityPayloads.AuthModeType)Intent.GetIntExtra(ActivityPayloads.AuthMode, (int)ActivityPayloads.AuthModeType.LOGIN);

                if (authType == ActivityPayloads.AuthModeType.LOGOUT)
                {
                    Logout();
                }
                else if (authType == ActivityPayloads.AuthModeType.LOGIN)
                {
                    SetContentView(Resource.Layout.AuthView);
                    FindViewById<Button>(Resource.Id.layout__auth__form__try_auth).Click += delegate
                    {
                        TryLogin();
                    };
                }
                else
                {
                    throw new UserActionException();
                }
            }
            catch
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.error__wtf), ToastLength.Long).Show();
                Finish();
            }
        }

        protected void TryLogin()
        {
            OpenLoadingDialog();

            EditText server = FindViewById<EditText>(Resource.Id.layout__auth__form__server);
            EditText login = FindViewById<EditText>(Resource.Id.layout__auth__form__login);
            EditText password = FindViewById<EditText>(Resource.Id.layout__auth__form__password);

            bool serverValid = server.Text.Trim().Length > 0;
            bool loginValid = login.Text.Trim().Length > 0;
            bool passwordValid = password.Text.Trim().Length > 0;

            if (serverValid && loginValid && passwordValid)
            {
                FindViewById<Button>(Resource.Id.layout__auth__form__try_auth).Enabled = false;
                try
                {
                    Connector connector = Connector.TryGet(server.Text.Trim(), login.Text.Trim(), password.Text.Trim());
                    DataProvider.Get().AddConnectionInfo(connector.ConnectionInfo);
                    Toast.MakeText(this, Resources.GetString(Resource.String.layout__auth__login__logged_in), ToastLength.Long).Show();
                    Finish();
                }
                catch (SwaggerException swaggerException)
                {
                    string message = null;
                    if (swaggerException.StatusCode == "405")
                        message = Resources.GetString(Resource.String.layout__auth__login_error__invalid_creditnals);
                    else
                        message = Resources.GetString(Resource.String.protocol_error);

                    Toast.MakeText(this, message, ToastLength.Long).Show();
                }
                catch (System.Net.WebException webException)
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.protocol_error__cannot_connect) + ": " + webException.Message, ToastLength.Long).Show();
                }
                catch (Exception exception)
                {
                    String exceptionInfo = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
                    Toast.MakeText(this, Resources.GetString(Resource.String.error__wtf) + " (" + exceptionInfo + ")", ToastLength.Long).Show();
                }
            }
            else
            {
                if (!passwordValid)
                    password.RequestFocus();
                if (!loginValid)
                    login.RequestFocus();
                if (!serverValid)
                    server.RequestFocus();
                Toast.MakeText(this, Resources.GetString(Resource.String.error__field_not_filled), ToastLength.Long).Show();
            }

            FindViewById<Button>(Resource.Id.layout__auth__form__try_auth).Enabled = true;
            CloseLoadingDialog();
        }

        protected void Logout()
        {
            if (DataProvider.Get().HasStoredConnections())
            {
                DataProvider.Get().RemoveConnectionInfos();
                Toast.MakeText(this, Resources.GetString(Resource.String.layout__auth__logout__logged_out), ToastLength.Long).Show();
            }
            Finish();
        }
    }
}