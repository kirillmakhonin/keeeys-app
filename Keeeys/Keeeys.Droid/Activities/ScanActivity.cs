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
using ZXing.Mobile;
using Keeeys.Droid.Helpers;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "ScanActivity")]
    public class ScanActivity : BaseActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            MobileBarcodeScanner.Initialize(Application);
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            string topString = Intent.GetStringExtra(ActivityPayloads.ScanTitle);
            string bottomString = Intent.GetStringExtra(ActivityPayloads.ScanHelper);

            if (topString != null)
                scanner.TopText = topString;
            if (bottomString != null)
                scanner.BottomText = bottomString;

            var task = await scanner.Scan(Options);

            if (task == null)
            {
                SetResult(Result.Canceled);
            }
            else
            {
                Intent resultIntent = new Intent();
                resultIntent.PutExtra(ActivityPayloads.ScannedCode, task.Text);
                SetResult(Result.Ok, resultIntent);
            }
            Finish();
        }

        private MobileBarcodeScanningOptions Options
        {
            get
            {
                MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions();
                options.PossibleFormats = new List<ZXing.BarcodeFormat>()
                {
                    ZXing.BarcodeFormat.QR_CODE
                };

                return options;
            }
        }


    }
}