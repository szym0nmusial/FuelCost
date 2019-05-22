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
using System.Threading;

namespace FuelCost
{
    public class Set : Fragment
    {

        public VehicleData Data;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        List<KeyValuePair<string, VehicleData.FuelTypeEnum>> planets;

        CheckBox checkBox1;
        EditText name;
        EditText consuption;

        EditText lpgprice;
        EditText pbprice;
        EditText onprice;
        

        VehicleData data = new VehicleData();


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            View view = inflater.Inflate(Resource.Layout.set, container, false);


            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinner);



            planets = new List<KeyValuePair<string, VehicleData.FuelTypeEnum>>
            {
                new KeyValuePair<string, VehicleData.FuelTypeEnum>("Gaz", VehicleData.FuelTypeEnum.lpg),
                new KeyValuePair<string, VehicleData.FuelTypeEnum>("Benzyna", VehicleData.FuelTypeEnum.pb),
                new KeyValuePair<string, VehicleData.FuelTypeEnum>("Diesel", VehicleData.FuelTypeEnum.diesel),
            };

            List<string> planetNames = new List<string>();
            foreach (var item in planets)
                planetNames.Add(item.Key);

            var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, planetNames);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;


            checkBox1 = view.FindViewById<CheckBox>(Resource.Id.checkBox1);
            name = view.FindViewById<EditText>(Resource.Id.name);
            consuption = view.FindViewById<EditText>(Resource.Id.consuption);

            lpgprice = view.FindViewById<EditText>(Resource.Id.lpgprice);
            pbprice = view.FindViewById<EditText>(Resource.Id.pbprice);
            onprice = view.FindViewById<EditText>(Resource.Id.onprice);

            lpgprice.Text = LocalSet.LpgPrice.ToString();
            pbprice.Text = LocalSet.PbPrice.ToString();
            onprice.Text = LocalSet.OnPrice.ToString();

            view.FindViewById<Button>(Resource.Id.button2).Click += Set_Click;


            Button btn = view.FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;




            return view;


            // return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void Set_Click(object sender, EventArgs e)
        {
            try
            {
                new Thread(() =>
                {
                    LocalSet.LpgPrice = float.Parse(lpgprice.Text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    LocalSet.PbPrice = float.Parse(pbprice.Text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    LocalSet.OnPrice = float.Parse(onprice.Text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
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
                    data.consumption = float.Parse(consuption.Text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    data.Pbinjection = checkBox1.Checked;

                    LocalSet.AddVehicle(data);
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