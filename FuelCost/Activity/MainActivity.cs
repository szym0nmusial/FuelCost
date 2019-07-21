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

        public static List<string> Debug = new List<string>();

        private static bool isFabOpen;
        private FloatingActionButton fab1;
        private FloatingActionButton fab2;
        private FloatingActionButton fab3;
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
            fab3 = FindViewById<FloatingActionButton>(Resource.Id.fabdev);
            fabMain = FindViewById<FloatingActionButton>(Resource.Id.fabBtn);
            bgFabMenu = FindViewById<View>(Resource.Id.bg_fab_menu);

            fab1.Click += FabBtn1_Click;
            fab2.Click += FabBtn2_Click;
            fab3.Click += FabBtn3_Click;


            fabMain.Click += (o, e) =>
            {
                if (!isFabOpen)
                    ShowFabMenu();
                else
                    CloseFabMenu();
            };

            bgFabMenu.Click += (o, e) => CloseFabMenu();
        }

        private void FabBtn3_Click(object sender, EventArgs e)
        {
            CloseFabMenu();
            Intent DebugIntent = new Intent(this, typeof(LogActivity));
            StartActivityForResult(DebugIntent, 2);
        }

        private void FabBtn2_Click(object sender, EventArgs e)
        {
            CloseFabMenu();
            Intent CostIntent = new Intent(this, typeof(CostActivity));
            StartActivityForResult(CostIntent, 2);
        }

        private void FabBtn1_Click(object sender, EventArgs e)
        {
            CloseFabMenu();
            Intent AddVehicleIntent = new Intent(this, typeof(AddVehicleActicity));
            // StartActivity(AddVehicleIntent);
            OldVehicleDataList = LocalSet.VehicleDataList;
            StartActivityForResult(AddVehicleIntent, 1);
            //  mAdapter.NotifyDataSetChanged();//price
        }

        private void ShowFabMenu()
        {
            isFabOpen = true;
            fab3.Visibility = ViewStates.Visible;
            fab2.Visibility = ViewStates.Visible;
            fab1.Visibility = ViewStates.Visible;
            bgFabMenu.Visibility = ViewStates.Visible;

            fabMain.Animate().Rotation(135f);
            bgFabMenu.Animate().Alpha(1f);
            fab3.Animate()
                .TranslationY(-Resources.GetDimension(Resource.Dimension.standard_145))
                .Rotation(0f);
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
            fab3.Animate()
                .TranslationY(0f)
                .Rotation(90f);
            fab2.Animate()
                .TranslationY(0f)
                .Rotation(90f).SetListener(new FabAnimatorListener(bgFabMenu, fab1, fab2));
            fab1.Animate()
                .TranslationY(0f)
                .Rotation(90f).SetListener(new FabAnimatorListener(bgFabMenu, fab1, fab2, fab3));
        }

       

        private void MAdapter_ItemClick(object sender, int e)
        {
            Intent intent = new Intent(this, typeof(DetailsActivity));
            intent.PutExtra("position", e);
           // Bundle options = ActivityOptionsCompat.MakeScaleUpAnimation(MainActivity., 0, 0, 0, 0).ToBundle();
            StartActivity(intent);            
        }


        public static string Log(string text)
        {
            Debug.Add(text);
            return text;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            mAdapter.NotifyDataSetChanged();//price
            DiffUtil.DiffResult result = DiffUtil.CalculateDiff(new DiffCallback(LocalSet.VehicleDataList, OldVehicleDataList), true);
            result.DispatchUpdatesTo(mAdapter);            
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
    }
}

