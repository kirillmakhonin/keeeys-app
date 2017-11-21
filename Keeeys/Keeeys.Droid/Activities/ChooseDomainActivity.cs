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
using Keeeys.Models;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "ChooseDomainActivity")]
    public class ChooseDomainActivity : BaseActivity
    {
        private CustomListAdapter listAdapter;
        private ListView listView;
        private List<ICustomListItem> listItems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            listItems = new List<ICustomListItem>();

            Window.RequestFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.MyKeysList);

            ActionBar.Show();
            ActionBar.Title = "Action Bar";

            listItems.Add(new PlainListItem(0, "One"));
            listItems.Add(new PlainListItem(1, "Two"));
            listAdapter = new CustomListAdapter(this, Resource.Layout.ListRowView, Resource.Id.ListItemName, listItems);

            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = listAdapter;
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
        }
    }
}