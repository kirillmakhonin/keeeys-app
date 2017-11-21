using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using Keeeys.Droid.Adapters;
using Keeeys.Droid.Helpers;
using Keeeys.Helpers;
using Keeeys.Models;


namespace Keeeys.Droid.Activities
{
    [Activity(Label = "MyKeysListActivity")]
    public class MyKeysListActivity : BaseActivity
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
            ActionBar.Title = Resources.GetString(Resource.String.layout__private_keys__title);

            listItems.Add(new PlainListItem(0, "One"));
            listItems.Add(new PlainListItem(1, "Two"));
            listAdapter = new CustomListAdapter(this, Resource.Layout.ListRowView, Resource.Id.ListItemName, listItems);

            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = listAdapter;
            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_ItemLongClick;

        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        private void StartAddingNewPrivateKey()
        {
            
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            listItems.Add(new PlainListItem(0, "Rand"));
            listAdapter.UpdateAll(listItems);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            
        }

        protected void UpdateList()
        {
            
        }
        
    }
}