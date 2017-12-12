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
using Keeeys.Common.Models;
using Keeeys.Common.Helpers;
using Keeeys.Common.Exceptions;


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

            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = listAdapter;
            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_ItemLongClick;

        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(Resources.GetString(Resource.String.layout__private_keys__removing_dialog__question));
            alert.SetMessage(Resources.GetString(Resource.String.layout__private_keys__removing_dialog__text));
            alert.SetPositiveButton(Resources.GetString(Resource.String.remove), (senderAlert, args) => {
                DataProvider.Get().RemovePrivateKey(GetItem(e.Position).Id);
                UpdateList();
                Toast.MakeText(this, Resources.GetString(Resource.String.layout__private_keys__removing_dialog__has_been_removed), ToastLength.Short).Show();
            });

            alert.SetNegativeButton(Resources.GetString(Resource.String.cancel), (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
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
                    StartAddingNewPrivateKey();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void StartAddingNewPrivateKey()
        {
            var activity = new Intent(this, typeof(ScanActivity));
            activity.PutExtra(ActivityPayloads.ScanTitle, Resources.GetString(Resource.String.layout__private_keys__scan_title));
            activity.PutExtra(ActivityPayloads.ScanHelper, Resources.GetString(Resource.String.layout__private_keys__scan_helper));
            StartActivityForResult(activity, ActivityRequests.QRSCAN);
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var activity = new Intent(this, typeof(MyKeyShowActivity));
            activity.PutExtra(ActivityPayloads.PrivateKeyId, GetItem(e.Position).Id);
            StartActivity(activity);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok && requestCode == ActivityRequests.QRSCAN)
            {
                string scannedCode = data.GetStringExtra(ActivityPayloads.ScannedCode);
                QrEncoder.PrivateSharingKeyInfo? code = QrEncoder.PrivateSharingKeyInfo.Decode(scannedCode);
                if (!code.HasValue)
                    Toast.MakeText(this, Resources.GetString(Resource.String.layout__private_keys__scan_error), ToastLength.Long).Show();

                try
                {
                    DataProvider.Get().AddPrivateKey(PrivateKey.Build(code.Value, DataProvider.Get().GetPrivateKeyNewId()));
                    UpdateList();
                }
                catch (PrivateKeyAlreadyExistsException)
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.layout__private_keys__scan_error__key_already_exists), ToastLength.Long).Show();
                }
                catch (Exception badException)
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.layout__private_keys__scan_error), ToastLength.Long).Show();
                }

            }
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