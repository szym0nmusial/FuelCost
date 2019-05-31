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
    public class RecyclerViewAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => LocalSet.VehicleDataList.ToArray().Length;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewViewHolder recyclerViewViewHolder = holder as RecyclerViewViewHolder;
            //  recyclerViewViewHolder.Caption.Text = LocalSet.VehicleDataList[position].Name;

            float Price = 0;
            switch (LocalSet.VehicleDataList[position].FuelType)
            {
                case VehicleData.FuelTypeEnum.lpg:
                    {
                        if (LocalSet.VehicleDataList[position].Pbinjection)
                        {
                            Price = LocalSet.LpgPrice + (float)(LocalSet.PbPrice * 0.1);
                        }
                        else
                        {
                            Price = LocalSet.LpgPrice;
                        }
                        break;
                    }
                case VehicleData.FuelTypeEnum.pb:
                    {
                        Price = LocalSet.PbPrice;
                        break;
                    }
                case VehicleData.FuelTypeEnum.diesel:
                    {
                        Price = LocalSet.OnPrice;
                        break;
                    }
            }

            recyclerViewViewHolder.Name.Text = LocalSet.VehicleDataList[position].Name;
            recyclerViewViewHolder.FuelType.Text = LocalSet.VehicleDataList[position].FuelType.ToString();
            recyclerViewViewHolder.Consuption.Text = LocalSet.VehicleDataList[position].consumption.ToString();
            recyclerViewViewHolder.Price.Text = Price.ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card, parent, false);
            RecyclerViewViewHolder vh = new RecyclerViewViewHolder(itemView);
            return vh;
        }
    }
}