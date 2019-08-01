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
using System.Threading.Tasks;

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

        private double SharedDistance = 0.0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //if(Intent!= null)
            //{        
                SharedDistance = Intent.GetDoubleExtra("SharedDistance", 0.0);
            //}

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            using (var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar))
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            }

            mRecycleView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(this);
            mRecycleView.SetLayoutManager(mLayoutManager);
            if (SharedDistance != 0.0)
            {
                mAdapter = new RecyclerViewAdapter(SharedDistance);
            }
            else
            {
                mAdapter = new RecyclerViewAdapter();
            }
            mRecycleView.SetAdapter(mAdapter);

            mAdapter.NotifyDataSetChanged();

            ItemTouchHelper.Callback callback = new MyItemTouchHelper(this, mAdapter);
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(mRecycleView);

            mAdapter.ItemClick += MAdapter_ItemClick;

            FloatingActionMenuHelper.activity = this;
            FloatingActionMenuHelper.BackgroundFab = FindViewById<View>(Resource.Id.bg_fab_menu);
            FloatingActionMenuHelper.MainButton = FindViewById<FloatingActionButton>(Resource.Id.fabBtn);
            FloatingActionMenuHelper.Buttons = new[] { FindViewById<FloatingActionButton>(Resource.Id.fabcar), FindViewById<FloatingActionButton>(Resource.Id.fabcash), FindViewById<FloatingActionButton>(Resource.Id.fabdev) };
            FloatingActionMenuHelper.ConnectEvents();

        }

      

        private void MAdapter_ItemClick(object sender, int e)
        {
            Intent intent = new Intent(this, typeof(DetailsActivity));
            intent.PutExtra("position", e);
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
            mAdapter.NotifyDataSetChanged();
            DiffUtil.DiffResult result = DiffUtil.CalculateDiff(new DiffCallback(LocalSet.VehicleDataList, OldVehicleDataList), true);
            result.DispatchUpdatesTo(mAdapter);
        }

        static class FloatingActionMenuHelper
        {
            public static View BackgroundFab;
            public static FloatingActionButton MainButton;
            public static FloatingActionButton[] Buttons;
            public static Activity activity;

            /// <summary>
            /// Skonfiguruj zdarzenia
            /// </summary>
            public static void ConnectEvents()
            {
                foreach (var Button in Buttons)
                {
                    Button.Click += Button_Click;
                }
                MainButton.Click += MainButton_Click;
                BackgroundFab.Click += (o, e) => CloseFabMenu();
            }

            /// <summary>
            /// Otwórz/zamknij menu
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private static void MainButton_Click(object sender, EventArgs e)
            {
                if (!isFabOpen)
                {
                    ShowFabMenu();
                }
                else
                {
                    CloseFabMenu();
                }
            }

            /// <summary>
            /// Obsługa kliknięcia na przycisk
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private static void Button_Click(object sender, EventArgs e)
            {
                CloseFabMenu();
                var type = typeof(Nullable);
                switch ((sender as FloatingActionButton).Id)
                {
                    case Resource.Id.fabcar:
                        {
                            type = typeof(AddVehicleActicity);
                            break;
                        }
                    case Resource.Id.fabcash:
                        {
                            type = typeof(CostActivity);
                            break;
                        }
                    case Resource.Id.fabdev:
                        {
                            type = typeof(LogActivity);
                            break;
                        }
                }
                Intent intent = new Intent(activity, type);
                activity.StartActivityForResult(intent, 2);
            }

            /// <summary>
            /// Pokaż Menu
            /// </summary>
            static void ShowFabMenu()
            {
                new Task(() =>
                {
                    isFabOpen = true;
                    foreach (var Button in Buttons) // Pokaż wszystkie buttony
                    {
                        activity.RunOnUiThread(() =>
                        {
                            Button.Visibility = ViewStates.Visible;
                        });
                    }
                    activity.RunOnUiThread(() =>
                    {
                        MainButton.Visibility = ViewStates.Visible;
                        BackgroundFab.Visibility = ViewStates.Visible;
                    });

                    MainButton.Animate().Rotation(135f); // obróć
                    BackgroundFab.Animate().Alpha(1f);   // pokaż                 

                    var metrics = new DisplayMetrics();
                    activity.WindowManager.DefaultDisplay.GetMetrics(metrics); // konwersja dp

                    for (int i = (Buttons.Length - 1); i >= 0; i--)
                    {
                        Buttons[i].Animate() // animuj o wyliczoną odległośc 
                        .TranslationY(-TypedValue.ApplyDimension(ComplexUnitType.Dip, (55 + 45 * i), metrics))
                        .Rotation(0f);
                    }
                }).Start();
            }
            /// <summary>
            /// Schowaj menu
            /// </summary>
            static void CloseFabMenu()
            {
                new Task(() =>
                {
                    isFabOpen = false;
                    MainButton.Animate().Rotation(0f); // ukryj, obróć
                    BackgroundFab.Animate().Alpha(0f);
                    Buttons[Buttons.Length - 1].Animate()
                    .TranslationY(0f)
                    .Rotation(90f);

                    for (int i = (Buttons.Length - 2); i >= 0; i--)
                    {
                        Buttons[i].Animate()
                        .TranslationY(0f) // animuj podaj do listenera argumenty
                        .Rotation(90f).SetListener(new FabAnimatorListener(BackgroundFab, Buttons[i], Buttons[i + 1]));
                    }
                }).Start();
            }
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