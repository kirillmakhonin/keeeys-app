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
using Keeeys;

namespace Keeeys.Droid.Activities
{
    public class BaseActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //RequestWindowFeature(WindowFeatures.IndeterminateProgress);
        }

        protected void OpenLoadingDialog()
        {
            //SetProgressBarIndeterminateVisibility(true);
        }

        protected void CloseLoadingDialog()
        {
            //SetProgressBarIndeterminateVisibility(false);
        }
        

    }
}