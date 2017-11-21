using Android.App;
using Android.OS;
using Android.Widget;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "AuthActivity")]
    public class AuthActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            SetContentView(Resource.Layout.AuthView);
            FindViewById<Button>(Resource.Id.layout__auth__form__try_auth).Click += delegate
            {
                TryLogin();
            };
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
            
        }

        protected void Logout()
        {            
            Finish();
        }
    }
}