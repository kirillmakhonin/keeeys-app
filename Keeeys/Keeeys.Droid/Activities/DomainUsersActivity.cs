using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Keeeys.Droid.Helpers;
using Keeeys.Common.Network;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "DomainUsersActivity")]
    public class DomainUsersActivity : BaseActivity
    {
        private Connector connection;
        private Organization organization;
        private Domain domain;
        private int connectionId;
        private int choosenDomain;
        private int choosenOrganization;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            connectionId = Intent.GetIntExtra(ActivityPayloads.ServerConnectionId, -1);
            choosenDomain = Intent.GetIntExtra(ActivityPayloads.ChoosenDomain, -1);
            choosenOrganization = Intent.GetIntExtra(ActivityPayloads.ChoosenOrganization, -1);

            if (connectionId < 0 || choosenDomain < 0 || choosenOrganization < 0)
                Finish();

            connection = Connector.Get(connectionId);
            if (connection == null)
                Finish();

            organization = connection.GetOrganization(choosenOrganization);
            domain = connection.GetDomain(choosenDomain);

            Window.RequestFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.DomainUsersList);

            ActionBar.Show();
            ActionBar.Title = string.Format("{0} / {1}", organization.Name, domain.Name);

            FindViewById<Button>(Resource.Id.layout__domain_controll_list__add_guest).Click += delegate
            {
                StartAddingNewKey();
            };

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.AddItemMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_bar__add:
                    StartAddingNewKey();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void StartAddingNewKey()
        {
            var activity = new Intent(this, typeof(GuestCreatingActivity));
            activity.PutExtra(ActivityPayloads.ServerConnectionId, connectionId);
            activity.PutExtra(ActivityPayloads.ChoosenDomain, choosenDomain);
            activity.PutExtra(ActivityPayloads.ChoosenOrganization, choosenOrganization);
            StartActivity(activity);
        }


    }
}