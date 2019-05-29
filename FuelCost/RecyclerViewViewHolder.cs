using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    public class RecyclerViewViewHolder : RecyclerView.ViewHolder
    {
       // public ImageView Image { get; private set; }
        public TextView Caption { get; private set; }

        public RecyclerViewViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
           // Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.textView);
        }
    }
}