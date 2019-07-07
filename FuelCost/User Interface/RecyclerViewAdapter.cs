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
        public event EventHandler<int> ItemClick;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewViewHolder recyclerViewViewHolder = holder as RecyclerViewViewHolder;
            double Price = 0;

            switch (LocalSet.VehicleDataList[position].FuelType)
            {
                case VehicleData.FuelTypeEnum.Gas:
                    {
                        if (LocalSet.VehicleDataList[position].Pbinjection)
                        {
                            Price = LocalSet.Prices[VehicleData.FuelTypeEnum.Gas] + (LocalSet.Prices[VehicleData.FuelTypeEnum.Benzyna] * 0.1);
                        }
                        else
                        {
                            Price = LocalSet.Prices[VehicleData.FuelTypeEnum.Gas];
                        }
                        break;
                    }
                case VehicleData.FuelTypeEnum.Benzyna:
                    {
                        Price = LocalSet.Prices[VehicleData.FuelTypeEnum.Benzyna];
                        break;
                    }
                case VehicleData.FuelTypeEnum.Diesel:
                    {
                        Price = LocalSet.Prices[VehicleData.FuelTypeEnum.Diesel];
                        break;
                    }
            }
            
            recyclerViewViewHolder.Name.Text = LocalSet.VehicleDataList[position].Name;
            recyclerViewViewHolder.FuelType.Text = LocalSet.VehicleDataList[position].FuelType.ToString();
            recyclerViewViewHolder.Consuption.Text = LocalSet.VehicleDataList[position].consumption.ToString();
            recyclerViewViewHolder.Price.Text = Price.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card, parent, false);
            RecyclerViewViewHolder vh = new RecyclerViewViewHolder(itemView, OnClick);
            return vh;
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}