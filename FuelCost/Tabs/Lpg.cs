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
        private float price = 2.08f;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            var view =  inflater.Inflate(Resource.Layout.lpg, container, false);

            view.FindViewById<TextView>(Resource.Id.name).Text = Data?.Name;
            view.FindViewById<TextView>(Resource.Id.typ).Text = Data?.FuelType.ToString();
            view.FindViewById<TextView>(Resource.Id.cons).Text = Data?.consumption.ToString();
            view.FindViewById<TextView>(Resource.Id.price).Text = "2,08";

            Distance = view.FindViewById<EditText>(Resource.Id.distance);
            Distance.TextChanged += Distance_Changed;
            Cost = view.FindViewById<EditText>(Resource.Id.cost);
            Cost.TextChanged += Cost_Changed;

            return view;

           // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void Cost_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
           // string tmp = 

           // Distance.Text =  e.Text
        }

        private void Distance_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                var tmp = Data.consumption * price * 0.01f * float.Parse(Distance.Text);
                Cost.Text = tmp.ToString();
            }
            catch { }
        }
    }
}