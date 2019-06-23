using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    [Activity(/*Label = "DetailsActivity",*/ Theme = "@style/AppTheme", MainLauncher = false)]
    public class DetailsActivity : AppCompatActivity
    {

        public TextView Name { get; private set; }
        public TextView FuelType { get; private set; }
        public TextView Consuption { get; private set; }
        public TextView Price { get; private set; }

        private EditText Cost;
        private EditText Distance;

        private float cost = 0;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.card_details);

            int position = Intent.GetIntExtra("position", -1);

            using (var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar))
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetDisplayShowHomeEnabled(true);
                toolbar.NavigationClick += Toolbar_NavigationClick;           
            }

            using (var CollapsingcoolbarLayout = FindViewById<Android.Support.Design.Widget.CollapsingToolbarLayout>(Resource.Id.collapsingToolbarLayout))
            {
                CollapsingcoolbarLayout.Title = LocalSet.VehicleDataList[position].Name;
            }
               

            // Locate and cache view references:
            Name = FindViewById<TextView>(Resource.Id.name);
            FuelType = FindViewById<TextView>(Resource.Id.typ);
            Consuption = FindViewById<TextView>(Resource.Id.cons);
            Price = FindViewById<TextView>(Resource.Id.price);

            Distance = FindViewById<EditText>(Resource.Id.distance);
            Distance.TextChanged += Distance_Changed;
            Cost = FindViewById<EditText>(Resource.Id.cost);
            Cost.TextChanged += Cost_Changed;

            
            switch (LocalSet.VehicleDataList[position].FuelType)
            {
                case VehicleData.FuelTypeEnum.lpg:
                    {
                        if (LocalSet.VehicleDataList[position].Pbinjection)
                        {
                            cost = LocalSet.LpgPrice + (float)(LocalSet.PbPrice * 0.1);
                        }
                        else
                        {
                            cost = LocalSet.LpgPrice;
                        }
                        break;
                    }
                case VehicleData.FuelTypeEnum.pb:
                    {
                        cost = LocalSet.PbPrice;
                        break;
                    }
                case VehicleData.FuelTypeEnum.diesel:
                    {
                        cost = LocalSet.OnPrice;
                        break;
                    }
            }

            Name.Text = LocalSet.VehicleDataList[position].Name;
            FuelType.Text = LocalSet.VehicleDataList[position].FuelType.ToString();
            Consuption.Text = LocalSet.VehicleDataList[position].consumption.ToString();
            Price.Text = cost.ToString();


            // Create your application here
        }

        private void Toolbar_NavigationClick(object sender, Android.Support.V7.Widget.Toolbar.NavigationClickEventArgs e)
        {
            Finish();
        }

        private void Cost_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                Distance.TextChanged -= Distance_Changed;
                var dist = float.Parse(Cost.Text) * 100 / cost / float.Parse(Consuption.Text);
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
                var cost = float.Parse(Consuption.Text) * 0.01f * this.cost * float.Parse(Distance.Text);
                Cost.Text = cost.ToString();
                Cost.TextChanged += Cost_Changed;
            }
            catch { }
        }
    }
}