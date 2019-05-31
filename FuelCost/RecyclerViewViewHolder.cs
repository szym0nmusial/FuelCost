﻿using System;
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
        public TextView Name { get; private set; }
        public TextView FuelType { get; private set; }
        public TextView Consuption { get; private set; }
        public TextView Price { get; private set; }

        public RecyclerViewViewHolder(View view) : base(view)
        {
            // Locate and cache view references:
            Name = view.FindViewById<TextView>(Resource.Id.name);
            FuelType = view.FindViewById<TextView>(Resource.Id.typ);
            Consuption = view.FindViewById<TextView>(Resource.Id.cons);
            Price = view.FindViewById<TextView>(Resource.Id.price);
        }
    }
}