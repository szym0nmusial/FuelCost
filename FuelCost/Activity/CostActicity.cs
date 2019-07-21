using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    [Activity(Label = "Ceny paliw", Theme = "@style/AppTheme")]
    public class CostActivity : AppCompatActivity
    {

        EditText lpgprice;
        EditText pbprice;
        EditText onprice;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.cost_layout);
            // Create your application here


            using (var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2))
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }



           
            lpgprice = FindViewById<EditText>(Resource.Id.lpgprice);
            pbprice = FindViewById<EditText>(Resource.Id.pbprice);
            onprice = FindViewById<EditText>(Resource.Id.onprice);

            try
            {
                lpgprice.Text = LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Gas]);
                pbprice.Text = (LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Benzyna]));
                onprice.Text = (LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Diesel]));
            }
            catch { }
            FindViewById<Button>(Resource.Id.button2).Click += Set_Click;


        }

        private void Set_Click(object sender, EventArgs e)
        {
            try
            {
                new Thread(() =>
                {
                    LocalSet.Write(VehicleData.FuelTypeEnum.Gas, LocalSet.Convert(lpgprice.Text));
                    LocalSet.Write(VehicleData.FuelTypeEnum.Benzyna, LocalSet.Convert(pbprice.Text));
                    LocalSet.Write(VehicleData.FuelTypeEnum.Diesel, LocalSet.Convert(onprice.Text));

                }).Start();
            }
            catch { }

        }

      

    }

}