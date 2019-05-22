using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace FuelCost
{
    public class Lpg : Fragment
    {

        public VehicleData Data;

        private EditText Cost;
        private EditText Distance;
        private float Price = 0;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            var view =  inflater.Inflate(Resource.Layout.lpg, container, false);

            if(Data.Pbinjection)
            {
                Price = LocalSet.LpgPrice + (float) (LocalSet.PbPrice * 0.1);
            }
            else
            {
                Price = LocalSet.LpgPrice;
            }

            view.FindViewById<Button>(Resource.Id.del).Click += Del_Click;

            view.FindViewById<TextView>(Resource.Id.name).Text = Data?.Name;
            view.FindViewById<TextView>(Resource.Id.typ).Text = Data?.FuelType.ToString();
            view.FindViewById<TextView>(Resource.Id.cons).Text = Data?.consumption.ToString() + " l/100km";
            view.FindViewById<TextView>(Resource.Id.price).Text = Price.ToString() + " zł/l";

            Distance = view.FindViewById<EditText>(Resource.Id.distance);
            Distance.TextChanged += Distance_Changed;
            Cost = view.FindViewById<EditText>(Resource.Id.cost);
            Cost.TextChanged += Cost_Changed;

            return view;

           // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void Del_Click(object sender, EventArgs e)
        {
            LocalSet.DelVehicle(Data);
        }

        private void Cost_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                Distance.TextChanged -= Distance_Changed;
                var dist = float.Parse(Cost.Text) * 100 / Price / Data.consumption;
                Distance.Text = dist.ToString();
                Distance.TextChanged += Distance_Changed;
            }
            catch
            { }
        }

        private void Distance_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                Cost.TextChanged -= Cost_Changed;
                var cost = Data.consumption * 0.01f * Price * float.Parse(Distance.Text);
                Cost.Text = cost.ToString();
                Cost.TextChanged += Cost_Changed;
            }
            catch { }
        }
    }
}