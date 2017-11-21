using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using Keeeys.Models;

namespace Keeeys.Droid.Adapters
{
    public class CustomListAdapter : BaseAdapter<string>
    {
        private ICustomListItem[] items;
        private Activity context;
        private int viewResourceId;
        private int viewFieldResourceId;

        public CustomListAdapter(Activity context, int viewResourceId, int viewFieldResourceId, List<ICustomListItem> items) : base()
        {
            this.items = items.ToArray();
            this.context = context;
            this.viewResourceId = viewResourceId;
            this.viewFieldResourceId = viewFieldResourceId;
        }

        public override string this[int position]
        {
            get
            {
                if (position < 0 || position >= Count)
                    return "Undefined element";
                return items[position].ToLabelString();
            }
        }

        public override int Count
        {
            get
            {
                return items.Length;
            }
        }

        public override long GetItemId(int position)
        {
            if (position < 0 || position >= Count)
                return 0;
            return items[position].GetId();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(viewResourceId, null);
            view.FindViewById<TextView>(viewFieldResourceId).Text = items[position].ToLabelString();
            return view;
        }

        public void UpdateAll(List<ICustomListItem> items)
        {
            this.items = items.ToArray();
            this.NotifyDataSetChanged();
        }
    }
}