using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using System;
using Android.Content;
using Android.Support.V7.Widget.Helper;

namespace FuelCost
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {


        RecyclerView mRecycleView;
        RecyclerView.LayoutManager mLayoutManager;
        RecyclerViewAdapter mAdapter;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LocalSet.GetSetting();
            VehicleData vd = new VehicleData();
            vd.Name = "tas";
            vd.consumption = 11;
            vd.FuelType = VehicleData.FuelTypeEnum.pb;

            // LocalSet.AddVehicle(vd);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            using (var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar))
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }

            FindViewById<FloatingActionButton>(Resource.Id.fabBtn).Click += FabBtn_Click;

            //var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            //PrepareViewPager(viewPager);

            //using (var tabs = FindViewById<TabLayout>(Resource.Id.tabs))
            //{
            //    tabs.SetupWithViewPager(viewPager);
            //}



            mRecycleView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(this);
            mRecycleView.SetLayoutManager(mLayoutManager);
            mAdapter = new RecyclerViewAdapter();
            mRecycleView.SetAdapter(mAdapter);


            ItemTouchHelper.Callback callback = new MyItemTouchHelper(this, mAdapter);
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(mRecycleView);



        }

        private void FabBtn_Click(object sender, EventArgs e)
        {

            Intent AddVehicleIntent = new Intent(this, typeof(AddVehicleActicity));
            StartActivity(AddVehicleIntent);
        }

        private void PrepareViewPager(ViewPager viewPager)
        {
            var adapter = new FragmentAdapter(SupportFragmentManager);
            viewPager.Adapter = adapter;
        }

    }
}

