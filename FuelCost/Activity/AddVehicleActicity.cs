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
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    [Activity(Label = "Dodaj pojazd", Theme = "@style/AppTheme")]
    public class AddVehicleActicity : AppCompatActivity
    {

        //  public VehicleData Data;



        Android.Support.V7.Widget.SwitchCompat addpb;
        EditText name;
        EditText consuption;
        private VehicleData data = new VehicleData();

        Button Slpg;
        Button Spb;
        Button Son;

        LinearLayout RootLayout;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.set);
            // Create your application here


            using (var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2))
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }

            

            addpb = FindViewById<Android.Support.V7.Widget.SwitchCompat>(Resource.Id.checkBox1);
            name = FindViewById<EditText>(Resource.Id.name);
            consuption = FindViewById<EditText>(Resource.Id.consuption);

            Slpg = FindViewById<Button>(Resource.Id.slpg);
            Son = FindViewById<Button>(Resource.Id.son);
            Spb = FindViewById<Button>(Resource.Id.spb);

            Slpg.Click += S_Click;
            Son.Click += S_Click;
            Spb.Click += S_Click;

            Button btn = FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;

            RootLayout = FindViewById<LinearLayout>(Resource.Id.AddRootLayout);

        }

        private void S_Click(object sender, EventArgs e)
        {
            var obj = sender as Button;
            switch (obj.Id)
            {
                case Resource.Id.slpg:
                    {
                        data.FuelType = VehicleData.FuelTypeEnum.Gas;
                        addpb.Visibility = ViewStates.Visible;
                        
                        break;
                    }
                case Resource.Id.spb:
                    {
                        data.FuelType = VehicleData.FuelTypeEnum.Benzyna;
                        addpb.Visibility = ViewStates.Gone;
                        break;
                    }
                case Resource.Id.son:
                    {
                        data.FuelType = VehicleData.FuelTypeEnum.Diesel;
                        addpb.Visibility = ViewStates.Gone;
                        break;
                    }
                default:
                    break;
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            new Thread(() =>
           {
               try
               {
                   data.Name = name.Text;
                   data.consumption = LocalSet.Convert(consuption.Text);
                   data.Pbinjection = addpb.Checked;

                   LocalSet.Write(data);



                   var snackbar = Snackbar.Make(RootLayout, "Dodano pojazd " + data.Name + " o spalaniu: " + LocalSet.Convert(data.consumption) ,Snackbar.LengthLong);

                   //var index = LocalSet.VehicleDataList.Count;
                   //Intent intentdata = new Intent();
                   //intentdata.PutExtra("index",  index );
                   //SetResult(Result.Ok, intentdata);


                   RunOnUiThread(() =>
                   {
                       snackbar.Show();
                       name.Text = "";
                       consuption.Text = "";
                   });
               }
               catch
               { }
           }).Start();

        }

    }

}