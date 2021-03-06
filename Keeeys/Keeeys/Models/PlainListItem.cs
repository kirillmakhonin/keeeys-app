﻿namespace Keeeys.Common.Models
{
    public class PlainListItem : ICustomListItem
    {
        private int id;
        private string title;

        public PlainListItem(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public int GetId()
        {
            return this.id;
        }

        public string ToLabelString()
        {
            return this.title;
        }
    }
}
