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
using Android.Views.InputMethods;
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


        TextInputLayout NameTil;
        TextInputLayout ConsuptionTil;

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

            //Slpg = FindViewById<RadioButton>(Resource.Id.slpg);
            //Son = FindViewById<RadioButton>(Resource.Id.son);
            //Spb = FindViewById<RadioButton>(Resource.Id.spb);

            NameTil = FindViewById<TextInputLayout>(Resource.Id.nameTil);
            ConsuptionTil = FindViewById<TextInputLayout>(Resource.Id.consuptionTil);

            FindViewById<RadioGroup>(Resource.Id.SCBRB).CheckedChange += AddVehicleActicity_CheckedChange;

            //Slpg.Click += S_Click;
            //Son.Click += S_Click;
            //Spb.Click += S_Click;

            //  Slpg.

            name.Click += Name_Click;
            consuption.Click += Consuption_Click;

            name.FocusChange += Name_FocusChange;
            consuption.FocusChange += Consuption_FocusChange;
            

            Button btn = FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;

            RootLayout = FindViewById<LinearLayout>(Resource.Id.AddRootLayout);

        }

        private void Consuption_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(ConsuptionTil.WindowToken, HideSoftInputFlags.None);
            }
        }

        private void Name_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
           if(!e.HasFocus)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(NameTil.WindowToken, HideSoftInputFlags.None);
            }
        }

        private void Consuption_Click(object sender, EventArgs e)
        {
            ConsuptionTil.Error = "";
        }

        private void Name_Click(object sender, EventArgs e)
        {
             NameTil.Error = "";
        }

        private void AddVehicleActicity_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {


            switch (e.CheckedId)
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

                       if (!consuption.HasFocus)
                       {
                           InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                           inputMethodManager.HideSoftInputFromWindow(ConsuptionTil.WindowToken, HideSoftInputFlags.None);
                       }
                   

                  
                       if (!name.HasFocus)
                       {
                           InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                           inputMethodManager.HideSoftInputFromWindow(NameTil.WindowToken, HideSoftInputFlags.None);
                       }
                   




                   if (consuption.Text == "" && name.Text == "")
                   {
                       RunOnUiThread(() =>
                       {
                           ConsuptionTil.Error = "Musisz podać spalanie";
                           NameTil.Error = "Musisz podać nazwę";
                           Snackbar.Make(RootLayout, "Pola nie mogą być puste", Snackbar.LengthLong).Show();
                       });
                       throw new ArgumentNullException("puste pola", "Nie podano danych");
                   }

                   if (name.Text == "")
                   {
                       RunOnUiThread(() =>
                       {
                           NameTil.Error = "Musisz podać nazwę";
                           Snackbar.Make(RootLayout, "Nazwa może być pusta", Snackbar.LengthLong).Show();                           
                       });
                       throw new ArgumentNullException("puste pola", "Nie podano danych");
                   }

                   if (consuption.Text == "")
                   {
                       RunOnUiThread(() =>
                       {
                           ConsuptionTil.Error = "Musisz podać spalanie";
                           Snackbar.Make(RootLayout, "Spalanie nie może być puste", Snackbar.LengthLong).Show();
                       });
                       throw new ArgumentNullException("puste pola", "Nie podano danych");
                   }

                   data.Name = name.Text;
                   data.consumption = LocalSet.Convert(consuption.Text);
                   data.Pbinjection = addpb.Checked;

                   LocalSet.Write(data);



                   var snackbar = Snackbar.Make(RootLayout, "Dodano pojazd " + data.Name + " o spalaniu: " + LocalSet.Convert(data.consumption), Snackbar.LengthLong);

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
               catch (Exception ex)
               {
                   Console.WriteLine(MainActivity.Log(ex.Message));
               }
           }).Start();

        }

    }

}