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
using Android.Views.InputMethods;

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

        LinearLayout DetailLinearLayout;
        Button MoreBtn;


        Android.Support.V7.Widget.SwitchCompat addpb;
        EditText consuption;
        TextInputLayout NameTil;
        TextInputLayout ConsuptionTil;

        EditText Distance;
        EditText Cost;

        private VehicleData.FuelTypeEnum FuelType;

        public double Price;

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

            DetailLinearLayout = FindViewById<LinearLayout>(Resource.Id.DetailLinearLayout);

            MoreBtn = FindViewById<Button>(Resource.Id.MoreBtn);
            MoreBtn.Click += More_Click;
            FindViewById<TextView>(Resource.Id.name).Click += More_Click;



            addpb = FindViewById<Android.Support.V7.Widget.SwitchCompat>(Resource.Id.checkBox1);
            consuption = FindViewById<EditText>(Resource.Id.consuption);
            NameTil = FindViewById<TextInputLayout>(Resource.Id.nameTil);
            ConsuptionTil = FindViewById<TextInputLayout>(Resource.Id.consuptionTil);

            var SCBRB = FindViewById<RadioGroup>(Resource.Id.SCBRB);
            SCBRB.CheckedChange += MainActivity_CheckedChange;
            SCBRB.CheckedChange += ((o,e) => UpdatePrice());

            FindViewById<RadioButton>(Resource.Id.spb).Checked = true;

            consuption.FocusChange += Consuption_FocusChange;

            addpb.CheckedChange += ((o,e) => UpdatePrice());

            Distance = FindViewById<EditText>(Resource.Id.distance);
            Cost = FindViewById<EditText>(Resource.Id.cost);

            Distance.TextChanged += Distance_Changed;
            Cost.TextChanged += Cost_Changed;

           
            addpb.Checked = true;

           // SharedDistance = 10;

            if(SharedDistance != 0.0)
            Distance.Text = LocalSet.Convert(SharedDistance);

            consuption.Click += Consuption_Click;
            consuption.TextChanged += Distance_Changed;
            consuption.TextChanged += Consuption_Click;
     
        }

        private void Consuption_Click(object sender, EventArgs e)
        {
            ConsuptionTil.Error = "";
        }

        private void UpdatePrice()
        {
            switch (FuelType)
            {
                case VehicleData.FuelTypeEnum.Gas:
                    {
                        if (addpb.Checked)
                        {
                            Price = LocalSet.Prices[VehicleData.FuelTypeEnum.Gas] + (LocalSet.Prices[VehicleData.FuelTypeEnum.Benzyna] * 0.1);
                        }
                        else
                        {
                            Price = LocalSet.Prices[VehicleData.FuelTypeEnum.Gas];
                        }
                        break;
                    }
                case VehicleData.FuelTypeEnum.Benzyna:
                    {
                        Price = LocalSet.Prices[VehicleData.FuelTypeEnum.Benzyna];
                        break;
                    }
                case VehicleData.FuelTypeEnum.Diesel:
                    {
                        Price = LocalSet.Prices[VehicleData.FuelTypeEnum.Diesel];
                        break;
                    }
            }

            Distance_Changed(null,null);
            Cost_Changed(null, null);
        }

        private void Consuption_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(ConsuptionTil.WindowToken, HideSoftInputFlags.None);
            }
        }

        private void MainActivity_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            switch (e.CheckedId)
            {
                case Resource.Id.slpg:
                    {
                        FuelType = VehicleData.FuelTypeEnum.Gas;
                        addpb.Visibility = ViewStates.Visible;
                        break;
                    }
                case Resource.Id.spb:
                    {
                        FuelType = VehicleData.FuelTypeEnum.Benzyna;
                        addpb.Visibility = ViewStates.Gone;
                        break;
                    }
                case Resource.Id.son:
                    {
                        FuelType = VehicleData.FuelTypeEnum.Diesel;
                        addpb.Visibility = ViewStates.Gone;
                        break;
                    }
                default:
                    break;
            }
        }

        private void Cost_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                if(consuption.Text == "")
                {
                    ConsuptionTil.Error = "Wpisz splalanie";
                    return;
                }
                Distance.TextChanged -= Distance_Changed;
                var dist = double.Parse(Cost.Text) * 100 / Price/* LocalSet.Convert(Price.Text)*/ /  double.Parse(consuption.Text);
                Distance.Text = dist.ToString();
                Distance.TextChanged += Distance_Changed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Distance_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                if (consuption.Text == "")
                {
                    ConsuptionTil.Error = "Wpisz splalanie";
                    return;
                }
                Cost.TextChanged -= Cost_Changed;
                var cost = double.Parse(consuption.Text) * 0.01f * Price/* LocalSet.Convert(Price.Text)*/ * double.Parse(Distance.Text);
                Cost.Text = cost.ToString();
                Cost.TextChanged += Cost_Changed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void More_Click(object sender, EventArgs e)
        {
            switch (DetailLinearLayout.Visibility)
            {
                case ViewStates.Visible:
                    {
                        MoreBtn.Animate().Rotation(0f);
                        DetailLinearLayout.Visibility = ViewStates.Gone;
                        break;
                    }
                case ViewStates.Gone:
                    {
                        MoreBtn.Animate().Rotation(180f);
                        DetailLinearLayout.Visibility = ViewStates.Visible;
                        break;
                    }
            }
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
