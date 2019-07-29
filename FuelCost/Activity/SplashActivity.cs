using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    [Activity(Theme = "@style/AppTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;



        protected override void OnResume()
        {
            base.OnResume();
            //SetContentView(Resource.Layout.SplashLayout);
            //Task startupWork = new Task(() => {LocalSet.GetSetting();});
            //startupWork.Start();
            //startupWork.Wait();
            //StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            new System.Threading.Thread(() => StartActivity(new Intent(Application.Context, typeof(SplashActivityLayout)))).Start();
        }

        public override void OnBackPressed() { }      

    }
}