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
    [Activity(Label = "AddVehicleActicity", Theme = "@style/AppTheme")]
    public class AddVehicleActicity : AppCompatActivity
    {

        public VehicleData Data;

        List<KeyValuePair<string, VehicleData.FuelTypeEnum>> planets;

        CheckBox checkBox1;
        EditText name;
        EditText consuption;

        EditText lpgprice;
        EditText pbprice;
        EditText onprice;


        VehicleData data = new VehicleData();



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



            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);



            planets = new List<KeyValuePair<string, VehicleData.FuelTypeEnum>>
            {
                new KeyValuePair<string, VehicleData.FuelTypeEnum>("Gaz", VehicleData.FuelTypeEnum.Gas),
                new KeyValuePair<string, VehicleData.FuelTypeEnum>("Benzyna", VehicleData.FuelTypeEnum.Benzya),
                new KeyValuePair<string, VehicleData.FuelTypeEnum>("Diesel", VehicleData.FuelTypeEnum.Diesel),
            };

            List<string> planetNames = new List<string>();
            foreach (var item in planets)
                planetNames.Add(item.Key);

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, planetNames);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;


            checkBox1 = FindViewById<CheckBox>(Resource.Id.checkBox1);
            name = FindViewById<EditText>(Resource.Id.name);
            consuption = FindViewById<EditText>(Resource.Id.consuption);

            lpgprice = FindViewById<EditText>(Resource.Id.lpgprice);
            pbprice = FindViewById<EditText>(Resource.Id.pbprice);
            onprice = FindViewById<EditText>(Resource.Id.onprice);

            try
            {
                lpgprice.Text = LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Gas]);
                pbprice.Text = (LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Benzya]));
                onprice.Text = (LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Diesel]));
            }
            catch { }
            FindViewById<Button>(Resource.Id.button2).Click += Set_Click;


            Button btn = FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;


        }

        private void Set_Click(object sender, EventArgs e)
        {
            try
            {
                new Thread(() =>
                {
                LocalSet.Write(VehicleData.FuelTypeEnum.Gas,LocalSet.Convert(lpgprice.Text));
                LocalSet.Write(VehicleData.FuelTypeEnum.Benzya,LocalSet.Convert(pbprice.Text));
                LocalSet.Write(VehicleData.FuelTypeEnum.Diesel, LocalSet.Convert(onprice.Text));

                }).Start();
            }
            catch { }

        }

        private void Btn_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                try
                {
                    data.Name = name.Text;
                    data.consumption = LocalSet.Convert(consuption.Text);
                    data.Pbinjection = checkBox1.Checked;

                    LocalSet.Write(data);
                }
                catch
                { }
            }).Start();

        }


        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            data.FuelType = planets[e.Position].Value;
        }
    }

}