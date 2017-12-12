using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Keeeys.Common.Network;
using Keeeys.Droid.Helpers;
using Android.Graphics;

namespace Keeeys.Droid.Activities
{
    [Activity(Label = "GuestCreatingActivity")]
    public class GuestCreatingActivity : BaseActivity
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

            AddUserDialog();
        }

        protected void AddUserDialog()
        {
            EditText userNameInput = new EditText(this);
            userNameInput.Hint = Resources.GetString(Resource.String.layout__guest_creating__guest_name_dialog__hint);

            var dialog = new AlertDialog.Builder(this);
            dialog.SetTitle(Resource.String.layout__guest_creating__guest_name_dialog__title);
            dialog.SetMessage(Resource.String.layout__guest_creating__guest_name_dialog__message);
            dialog.SetView(userNameInput);
            dialog.SetPositiveButton(Resource.String.layout__guest_creating__guest_name_dialog__next, (senderAlert, args) => {
                if (userNameInput.Text.Trim().Length > 4)
                {
                    SendRequest(userNameInput.Text);
                }
                else
                {
                    Toast.MakeText(this, Resource.String.layout__guest_creating__guest_name_dialog__invalid_name, ToastLength.Short).Show();
                    AddUserDialog();
                }
            });
            dialog.Show();
        }

        protected void SendRequest(string newGuestName)
        {
            var dialog = new AlertDialog.Builder(this);
            dialog.SetTitle(Resource.String.layout__guest_creating__pre_request__title);

            StringBuilder message = new StringBuilder();
            message.Append(Resources.GetString(Resource.String.layout__guest_creating__pre_request__user_will_be_created));
            message.Append(" " + newGuestName);
            message.Append("\n");
            message.Append("\n");
            message.Append("\n");
            message.Append(Resources.GetString(Resource.String.layout__guest_creating__pre_request__warning_about_key));
            dialog.SetMessage(message.ToString());

            dialog.SetPositiveButton(Resource.String.layout__guest_creating__pre_request__next, (senderAlert, args) => {
                ProcessRequest(newGuestName);
            });
            dialog.Show();
        }

        protected void ProcessRequest(string newGuestName)
        {
            string qrCode = connection.RegisterPaticipantForString(choosenDomain, newGuestName);
            string stringForQrCode = String.Format("{0}|{1}|{2}", organization.Name, domain.Name, qrCode);

            SetContentView(Resource.Layout.GuestKeyShow);

            FindViewById<TextView>(Resource.Id.eventName).Text = string.Format("{0} / {1}", organization.Name, domain.Name);
            ImageView image = FindViewById<ImageView>(Resource.Id.qrCodeImage);

            int width = (int)(Resources.DisplayMetrics.WidthPixels * 0.8);
            Console.WriteLine(width);
            image.LayoutParameters.Height = width;
            image.LayoutParameters.Width = width;
            try
            {
                Bitmap bitmap = QrBuilder.BuildImage(stringForQrCode, width, width);
                image.SetImageBitmap(bitmap);
            }
            catch (Exception)
            {
                Finish();
                Toast.MakeText(this, Resources.GetString(Resource.String.layout__private_key__qr_code_build_error), ToastLength.Long).Show();
            }
            FindViewById<Button>(Resource.Id.layout__guest_creating__after_request__next).Click += GuestCreatingActivity_Click;

        }

        private void GuestCreatingActivity_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}