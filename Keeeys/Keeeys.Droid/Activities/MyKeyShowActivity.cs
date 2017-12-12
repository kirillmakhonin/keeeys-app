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
using Keeeys;
using Keeeys.Common.Models;
using Android.Graphics;
using System.IO;
using Keeeys.Droid.PlatformSpecific;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "MyKeyShowActivity")]
    public class MyKeyShowActivity : BaseActivity
    {
        PrivateKey key;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MyKeyShow);

            int privateKeyId = Intent.GetIntExtra(ActivityPayloads.PrivateKeyId, 0);
            key = DataProvider.Get().GetPrivateKey(privateKeyId);

            FindViewById<TextView>(Resource.Id.eventName).Text = key.ToLabelString();
            ImageView image = FindViewById<ImageView>(Resource.Id.qrCodeImage);

            int width = (int)(Resources.DisplayMetrics.WidthPixels * 0.8);
            Console.WriteLine(width);
            image.LayoutParameters.Height = width;
            image.LayoutParameters.Width = width;
            try
            {
                Crypto crypto = new Crypto();
                Bitmap bitmap = QrBuilder.BuildImage(key.DataForAuthorisation(crypto), width, width);
                image.SetImageBitmap(bitmap);
            }
            catch (Exception)
            {
                Finish();
                Toast.MakeText(this, Resources.GetString(Resource.String.layout__private_key__qr_code_build_error), ToastLength.Long).Show();
            }
        }
    }
}