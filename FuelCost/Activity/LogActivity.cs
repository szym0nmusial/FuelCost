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
    [Activity(Label = "Logs", Theme = "@style/AppTheme")]
    public class LogActivity : AppCompatActivity
    {

        EditText editTextdebug;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.log_layout);
            // Create your application here


            using (var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2))
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }

            editTextdebug = FindViewById<EditText>(Resource.Id.DebugId);
            foreach (var item in MainActivity.Debug)
            {
                editTextdebug.Text += MainActivity.Debug[0] + "\n";
            }
            

          


        }

       


    }

}