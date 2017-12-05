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
using Keeeys.Common.Helpers;
using Keeeys.Common.Models;


namespace Keeeys.Droid.Activities
{
    [Activity(Label = "MyKeysListActivity")]
    public class MyKeysListActivity : BaseActivity
    {
        private CustomListAdapter listAdapter;
        private ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.MyKeysList);

            ActionBar.Show();
            ActionBar.Title = Resources.GetString(Resource.String.layout__private_keys__title);

            listAdapter = new CustomListAdapter(this, Resource.Layout.ListRowView, Resource.Id.ListItemName, DataProvider.Get().GetPrivateKeys().ToList<ICustomListItem>());

            if (DataProvider.Get().GetPrivateKeys().Count == 0)
                AddPK();

            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = listAdapter;
            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_ItemLongClick;

        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            DataProvider.Get().RemovePrivateKey(GetItem(e.Position).Id);
            UpdateList();
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

        private void AddPK()
        {
            int id = Time.GetTimestamp() % 1000 * 10 + new Random().Next() % 10;
            DataProvider.Get().AddPrivateKey(new PrivateKey(id, "DN" + id, "EN" + id, "PK"));
            UpdateList();
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            AddPK();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);


        }

        protected void UpdateList()
        {
            List<PrivateKey> currentPrivateKeys = DataProvider.Get().GetPrivateKeys();
            listAdapter.UpdateAll(currentPrivateKeys.ToList<ICustomListItem>());
        }

        protected PrivateKey GetItem(int id)
        {
            var list = DataProvider.Get().GetPrivateKeys();
            return list[id];
        }

    }
}