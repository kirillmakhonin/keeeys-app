using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Keeeys.Droid.Helpers;
using Keeeys.Droid.Adapters;
using Keeeys.Common.Models;
using Keeeys.Common.Network;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "ChooseDomainActivity")]
    public class ChooseDomainActivity : BaseActivity
    {
        private Connector connection;
        private CustomListAdapter listAdapter;
        private ListView listView;
        private List<OrganizationDomain> organizationDomainPairs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string windowHeaderTitle = Intent.GetStringExtra(ActivityPayloads.WindowHeaderTitle);
            if (windowHeaderTitle == null)
                windowHeaderTitle = "";

            int connectionId = Intent.GetIntExtra(ActivityPayloads.ServerConnectionId, -1);
            if (connectionId < 0)
                Finish();

            connection = Connector.Get(connectionId);
            if (connection == null)
                Finish();

            Window.RequestFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.MyKeysList);

            ActionBar.Show();
            ActionBar.Title = windowHeaderTitle;

            organizationDomainPairs = OrganizationDomain.BuildFromOrganizationList(Connector.Get().GetOrganizations());
            listAdapter = new CustomListAdapter(this, Resource.Layout.ListRowView, Resource.Id.ListItemName, organizationDomainPairs.ToList<ICustomListItem>());

            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = listAdapter;
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent resultIntent = new Intent();
            resultIntent.PutExtra(ActivityPayloads.ChoosenDomain, (int)GetItem(e.Position).DomainInstance.Id.Value);
            resultIntent.PutExtra(ActivityPayloads.ChoosenOrganization, (int)GetItem(e.Position).OrganizationInstance.Id.Value);
            SetResult(Result.Ok, resultIntent);
            Finish();
        }

        protected OrganizationDomain GetItem(int id)
        {
            return organizationDomainPairs[id];
        }
    }
}