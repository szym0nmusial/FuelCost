using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using System;

namespace FuelCost
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
  

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //LocalSet.GetSetting();
            

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            using (var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar))
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }

            //var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            //PrepareViewPager(viewPager);

            //using (var tabs = FindViewById<TabLayout>(Resource.Id.tabs))
            //{
            //    tabs.SetupWithViewPager(viewPager);
            //}
        }

        private void PrepareViewPager(ViewPager viewPager)
        {
            var adapter = new FragmentAdapter(SupportFragmentManager);
            viewPager.Adapter = adapter;
        }
    }
}

