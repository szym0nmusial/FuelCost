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
    [Activity(Label = "Ceny paliw", Theme = "@style/AppTheme")]
    public class CostActivity : AppCompatActivity
    {

        EditText lpgprice;
        EditText pbprice;
        EditText onprice;

        TextInputLayout lpgpriceTil;
        TextInputLayout pbpriceTil;
        TextInputLayout onpriceTil;

        LinearLayout RootLayout;

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

            lpgpriceTil = FindViewById<TextInputLayout>(Resource.Id.lpgpriceTil);
            pbpriceTil = FindViewById<TextInputLayout>(Resource.Id.pbpriceTil);
            onpriceTil = FindViewById<TextInputLayout>(Resource.Id.onpriceTil);

            lpgprice.Click += Lpgprice_Click;
            pbprice.Click += Pbprice_Click;
            onprice.Click += Onprice_Click;

            lpgprice.FocusChange += Lpgprice_FocusChange;
            pbprice.FocusChange += Pbprice_FocusChange;
            onprice.FocusChange += Onprice_FocusChange;


            try
            {
                lpgprice.Text = LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Gas]);
                pbprice.Text = (LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Benzyna]));
                onprice.Text = (LocalSet.Convert(LocalSet.Prices[VehicleData.FuelTypeEnum.Diesel]));
            }
            catch { }
            FindViewById<Button>(Resource.Id.button2).Click += Set_Click;

            RootLayout = FindViewById<LinearLayout>(Resource.Id.CostRootLayout);


        }

        private void Onprice_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(onpriceTil.WindowToken, HideSoftInputFlags.None);
            }
        }

        private void Pbprice_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(pbpriceTil.WindowToken, HideSoftInputFlags.None);
            }
        }

        private void Lpgprice_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(lpgpriceTil.WindowToken, HideSoftInputFlags.None);
            }
        }

        private void Onprice_Click(object sender, EventArgs e)
        {
            onpriceTil.Error = "";
        }

        private void Pbprice_Click(object sender, EventArgs e)
        {
            pbpriceTil.Error = "";
        }

        private void Lpgprice_Click(object sender, EventArgs e)
        {
            lpgpriceTil.Error = "";
        }

        private void Set_Click(object sender, EventArgs e)
        {

            new Thread(() =>
            {
                try
                {
                    if (lpgprice.Text == "")
                    {
                        RunOnUiThread(() =>
                            {
                        lpgpriceTil.Error = "Podaj cenę";
                        Snackbar.Make(RootLayout, "Podaj cenę", Snackbar.LengthLong).Show();
                    });
                        throw new ArgumentNullException("puste pola", "Nie podano danych");
                    }

                    if (onprice.Text == "")
                    {
                        RunOnUiThread(() =>
                            {
                        onpriceTil.Error = "Podaj cenę";
                        Snackbar.Make(RootLayout, "Podaj cenę", Snackbar.LengthLong).Show();
                    });
                        throw new ArgumentNullException("puste pola", "Nie podano danych");
                    }

                    if (pbprice.Text == "")
                    {
                        RunOnUiThread(() =>
                            {
                        pbpriceTil.Error = "Podaj cenę";
                        Snackbar.Make(RootLayout, "Podaj cenę", Snackbar.LengthLong).Show();
                    });
                        throw new ArgumentNullException("puste pola", "Nie podano danych");
                    }

                    InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);

                    if (lpgprice.HasFocus)
                    {
                        RunOnUiThread(() =>
                            {
                        inputMethodManager.HideSoftInputFromWindow(lpgpriceTil.WindowToken, HideSoftInputFlags.None);
                    });
                    }
                    if (pbprice.HasFocus)
                    {
                        RunOnUiThread(() =>
                            {
                        inputMethodManager.HideSoftInputFromWindow(pbpriceTil.WindowToken, HideSoftInputFlags.None);
                    });
                    }
                    if (onprice.HasFocus)
                    {
                        RunOnUiThread(() =>
                            {
                        inputMethodManager.HideSoftInputFromWindow(onpriceTil.WindowToken, HideSoftInputFlags.None);
                    });
                    }


                    LocalSet.Write(VehicleData.FuelTypeEnum.Gas, LocalSet.Convert(lpgprice.Text));
                    LocalSet.Write(VehicleData.FuelTypeEnum.Benzyna, LocalSet.Convert(pbprice.Text));
                    LocalSet.Write(VehicleData.FuelTypeEnum.Diesel, LocalSet.Convert(onprice.Text));


                }
                catch (Exception ex)
                {
                    Console.WriteLine(MainActivity.Log(ex.Message));
                }
            }).Start();
        }



    }

}