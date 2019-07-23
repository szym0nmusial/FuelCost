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

        RadioButton Slpg;
        RadioButton Spb;
        RadioButton Son;

        RadioGroup radioGroup;

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

            Slpg = FindViewById<RadioButton>(Resource.Id.slpg);
            Son = FindViewById<RadioButton>(Resource.Id.son);
            Spb = FindViewById<RadioButton>(Resource.Id.spb);

            // FindViewById<RadioGroup>(Resource.Id.SCBRB).Click += S_Click;

            Slpg.Click += S_Click;
            Son.Click += S_Click;
            Spb.Click += S_Click;

            //  Slpg.

            Button btn = FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;

            RootLayout = FindViewById<LinearLayout>(Resource.Id.AddRootLayout);

        }

        private void S_Click(object sender, EventArgs e)
        {
           
            var obj = sender as RadioButton;

            //obj.SetBackgroundResource()

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
                   if(name.Text == "" || consuption.Text == "")
                   {
                   RunOnUiThread(() => Snackbar.Make(RootLayout, "Pole nie może być puste", Snackbar.LengthLong).Show());
                       throw new ArgumentNullException("puste pola", "Nie podano danych");
                   }


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
               catch(Exception ex)
               {
                   Console.WriteLine(MainActivity.Log(ex.Message));
               }
           }).Start();

        }

    }

}