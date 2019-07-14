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
using Android.Widget;
using Android.Animation;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace FuelCost
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        List<VehicleData> OldVehicleDataList = new List<VehicleData>();

        RecyclerView mRecycleView;
        RecyclerView.LayoutManager mLayoutManager;
        RecyclerViewAdapter mAdapter;



        private static bool isFabOpen;
        private FloatingActionButton fab1;
        private FloatingActionButton fab2;
       // private FloatingActionButton fab3;
        private FloatingActionButton fabMain;
        private View bgFabMenu;

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

//            FindViewById<FloatingActionButton>(Resource.Id.fabBtn).Click += FabBtn_Click;

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

            fab1 = FindViewById<FloatingActionButton>(Resource.Id.fabcar);
            fab2 = FindViewById<FloatingActionButton>(Resource.Id.fabcash);
            fabMain = FindViewById<FloatingActionButton>(Resource.Id.fabBtn);
            bgFabMenu = FindViewById<View>(Resource.Id.bg_fab_menu);




            fabMain.Click += (o, e) =>
            {
                if (!isFabOpen)
                    ShowFabMenu();
                else
                    CloseFabMenu();
            };

            fab1.Click += (o, e) =>
            {
                CloseFabMenu();
                Toast.MakeText(this, "Car!", ToastLength.Short).Show();
                FabBtn_Click(o, e);
            };

            fab2.Click += (o, e) =>
            {
                CloseFabMenu();
                Toast.MakeText(this, "Airballoon!", ToastLength.Short).Show();
            };

            bgFabMenu.Click += (o, e) => CloseFabMenu();



        }

        private void ShowFabMenu()
        {
            isFabOpen = true;
            fab2.Visibility = ViewStates.Visible;
            fab1.Visibility = ViewStates.Visible;
            bgFabMenu.Visibility = ViewStates.Visible;

            fabMain.Animate().Rotation(135f);
            bgFabMenu.Animate().Alpha(1f);
            fab2.Animate()
                .TranslationY(-Resources.GetDimension(Resource.Dimension.standard_100))
                .Rotation(0f);
            fab1.Animate()
                .TranslationY(-Resources.GetDimension(Resource.Dimension.standard_55))
                .Rotation(0f);
        }

        private void CloseFabMenu()
        {
            isFabOpen = false;

            fabMain.Animate().Rotation(0f);
            bgFabMenu.Animate().Alpha(0f);
            fab2.Animate()
                .TranslationY(0f)
                .Rotation(90f);
            fab1.Animate()
                .TranslationY(0f)
                .Rotation(90f).SetListener(new FabAnimatorListener(bgFabMenu, fab1, fab2));
        }

        private class FabAnimatorListener : Java.Lang.Object, Animator.IAnimatorListener
        {
            View[] viewsToHide;

            public FabAnimatorListener(params View[] viewsToHide)
            {
                this.viewsToHide = viewsToHide;
            }

            public void OnAnimationCancel(Animator animation)
            {
            }

            public void OnAnimationEnd(Animator animation)
            {
                if (!isFabOpen)
                    foreach (var view in viewsToHide)
                        view.Visibility = ViewStates.Gone;
            }

            public void OnAnimationRepeat(Animator animation)
            {
            }

            public void OnAnimationStart(Animator animation)
            {
            }
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

