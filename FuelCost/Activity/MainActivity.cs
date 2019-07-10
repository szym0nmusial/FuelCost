using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using System;
using Android.Content;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Util;
using Android.Support.V4.App;
using Android.Runtime;
using System.Collections.Generic;
using Android.Support.V7.Util;

namespace FuelCost
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        List<VehicleData> OldVehicleDataList = new List<VehicleData>();

        RecyclerView mRecycleView;
        RecyclerView.LayoutManager mLayoutManager;
        RecyclerViewAdapter mAdapter;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            using (var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar))
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            }

            FindViewById<FloatingActionButton>(Resource.Id.fabBtn).Click += FabBtn_Click;

            mRecycleView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(this);
            mRecycleView.SetLayoutManager(mLayoutManager);
            mAdapter = new RecyclerViewAdapter();
            mRecycleView.SetAdapter(mAdapter);

            mAdapter.NotifyDataSetChanged();
            
            ItemTouchHelper.Callback callback = new MyItemTouchHelper(this, mAdapter);
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(mRecycleView);

            mAdapter.ItemClick += MAdapter_ItemClick;  

        }

        private void MAdapter_ItemClick(object sender, int e)
        {
            Intent intent = new Intent(this, typeof(DetailsActivity));
            intent.PutExtra("position", e);
           // Bundle options = ActivityOptionsCompat.MakeScaleUpAnimation(MainActivity., 0, 0, 0, 0).ToBundle();
            StartActivity(intent);
            
        }


        private void FabBtn_Click(object sender, EventArgs e)
        {
            Intent AddVehicleIntent = new Intent(this, typeof(AddVehicleActicity));
            // StartActivity(AddVehicleIntent);
            OldVehicleDataList = LocalSet.VehicleDataList;
            StartActivityForResult(AddVehicleIntent, 1);
          //  mAdapter.NotifyDataSetChanged();//price
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            mAdapter.NotifyDataSetChanged();//price
            DiffUtil.DiffResult result = DiffUtil.CalculateDiff(new DiffCallback(LocalSet.VehicleDataList, OldVehicleDataList), true);
            result.DispatchUpdatesTo(mAdapter);            
        }
    }
}

