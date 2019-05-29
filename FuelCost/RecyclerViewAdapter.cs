using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.Widget;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    class RecyclerViewAdapter : RecyclerView.Adapter
    {
        public override int ItemCount =>  LocalSet.VehicleDataList.ToArray().Length;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewViewHolder recyclerViewViewHolder = holder as RecyclerViewViewHolder;
            recyclerViewViewHolder.Caption.Text = LocalSet.VehicleDataList[position].Name;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card, parent, false);
            RecyclerViewViewHolder vh = new RecyclerViewViewHolder(itemView);
            return vh;
        }
    }
}