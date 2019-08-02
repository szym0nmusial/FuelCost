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


        SwitchCompat AddPb;
        EditText Distance;
        EditText Cost;
        EditText Consuption;
        TextInputLayout DistanceTil;
        TextInputLayout CostTil;
        TextInputLayout ConsuptionTil;

        CoordinatorLayout RootLayout;

        private VehicleData.FuelTypeEnum FuelType;

        public double Price;

        TextView WelcomeTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SharedDistance = Intent.GetDoubleExtra("SharedDistance", 0.0);

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

            RootLayout = FindViewById<CoordinatorLayout>(Resource.Id.rootLayout);

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

            AddPb = FindViewById<SwitchCompat>(Resource.Id.checkBox1);
            Consuption = FindViewById<EditText>(Resource.Id.consuption);
            ConsuptionTil = FindViewById<TextInputLayout>(Resource.Id.consuptionTil);

            WelcomeTextView = FindViewById<TextView>(Resource.Id.Welcome);

            var SCBRB = FindViewById<RadioGroup>(Resource.Id.SCBRB);
            SCBRB.CheckedChange += MainActivity_CheckedChange;//(async (o,e) => await MainActivity_CheckedChange(o, e));
            SCBRB.CheckedChange += (async (o, e) => await UpdatePrice());

            FindViewById<RadioButton>(Resource.Id.spb).Checked = true;

            Consuption.FocusChange += Consuption_FocusChange;

            AddPb.CheckedChange += (async (o, e) => await UpdatePrice());
            AddPb.Checked = true;

            Distance = FindViewById<EditText>(Resource.Id.distance);
            Cost = FindViewById<EditText>(Resource.Id.cost);

            DistanceTil = FindViewById<TextInputLayout>(Resource.Id.distanceTil);
            CostTil = FindViewById<TextInputLayout>(Resource.Id.costTil);

            Distance.TextChanged += Distance_Changed;
            Cost.TextChanged += Cost_Changed;

            if (SharedDistance != 0.0)
                Distance.Text = LocalSet.Convert(SharedDistance);

            Consuption.Click += Consuption_Click;
            Consuption.TextChanged += Distance_Changed;
            Consuption.TextChanged += Consuption_Click;

            Consuption.FocusChange += (async (o,e) => await HideKeyboard());
            Distance.FocusChange += (async (o, e) => await HideKeyboard());
            Cost.FocusChange += (async (o, e) => await HideKeyboard());

            HideWelcome();

        }

        private void Consuption_Click(object sender, EventArgs e)
        {
            ConsuptionTil.Error = "";
        }

        private async void HideWelcome()
        {
            await Task.Run(async () =>
            {
                if (LocalSet.VehicleDataList.Count == 0)
                {
                    RunOnUiThread(() => WelcomeTextView.Visibility= ViewStates.Visible );
                    var Snackbars = new[] {
                        Snackbar.Make(RootLayout, "Witaj", 3500),
                        Snackbar.Make(RootLayout,"Może udostępnij mi trasę z Google Maps", 3500),
                        Snackbar.Make(RootLayout, "Albo oblicz bez zapisywania", 3500)
                    };

                    Snackbars[0].SetAction("Dodaj pojazd", async v =>
                    {
                        await Task.Run(async () =>
                        {
                            FloatingActionMenuHelper.ShowFabMenu();
                            await Task.Delay(1000);
                            FloatingActionMenuHelper.Button_Click(FindViewById<FloatingActionButton>(Resource.Id.fabcar), null);
                        });
                    });
                    foreach(var Snack in Snackbars) //  zeby duzo razy nie uzywac runonuithread
                    {
                        RunOnUiThread(() => Snack.Show());
                        await Task.Delay(5000);
                    }
                }
                else
                {
                    RunOnUiThread(() => WelcomeTextView.Visibility = ViewStates.Gone);
                }
            });
        }

        private async Task HideKeyboard()
        {
            await Task.Run(() =>
            {
                if (!Consuption.HasFocus)
                {
                    InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                    inputMethodManager.HideSoftInputFromWindow(ConsuptionTil.WindowToken, HideSoftInputFlags.None);
                }

                if (!Distance.HasFocus)
                {
                    InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                    inputMethodManager.HideSoftInputFromWindow(DistanceTil.WindowToken, HideSoftInputFlags.None);
                }
                if (!Cost.HasFocus)
                {
                    InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                    inputMethodManager.HideSoftInputFromWindow(CostTil.WindowToken, HideSoftInputFlags.None);
                }
            });
        }


        private async Task UpdatePrice()
        {
            await Task.Run(() =>
            {
                switch (FuelType)
                {
                    case VehicleData.FuelTypeEnum.Gas:
                        {
                            if (AddPb.Checked)
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


            });//.Start();
            Distance_Changed(null, null);
            Cost_Changed(null, null);
        }

        private void Consuption_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            new Task(() =>
            {
                if (!e.HasFocus)
                {
                    InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                    inputMethodManager.HideSoftInputFromWindow(ConsuptionTil.WindowToken, HideSoftInputFlags.None);
                }
            }).Start();
        }

        private async void MainActivity_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            ViewStates Visibility = ViewStates.Visible;
            await Task.Run(() =>
            {                
                switch (e.CheckedId)
                {
                    case Resource.Id.slpg:
                        {
                            FuelType = VehicleData.FuelTypeEnum.Gas;
                            Visibility = ViewStates.Visible;
                            break;
                        }
                    case Resource.Id.spb:
                        {
                            FuelType = VehicleData.FuelTypeEnum.Benzyna;
                            Visibility = ViewStates.Gone;
                            break;
                        }
                    case Resource.Id.son:
                        {
                            FuelType = VehicleData.FuelTypeEnum.Diesel;
                            Visibility = ViewStates.Gone;
                            break;
                        }
                    default:
                        break;
                }
            });//.Start();
            AddPb.Visibility = Visibility;
        }

        private void Cost_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {

            try
            {
                if (Consuption.Text == "")
                {
                    ConsuptionTil.Error = "Wpisz splalanie";
                    return;
                }
                Distance.TextChanged -= Distance_Changed;
                var dist = LocalSet.Convert(Cost.Text) * 100 / Price/* LocalSet.Convert(Price.Text)*/ / LocalSet.Convert(Consuption.Text);
                Distance.Text = String.Format("{0:0.00}", dist); //dist.ToString();
                Distance.TextChanged += Distance_Changed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(MainActivity.Log(ex));
            }

        }

        private void Distance_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {

            try
            {
                if (Consuption.Text == "")
                {
                    ConsuptionTil.Error = "Wpisz splalanie";
                    return;
                }
                Cost.TextChanged -= Cost_Changed;
                var cost = LocalSet.Convert(Consuption.Text) * 0.01f * Price/* LocalSet.Convert(Price.Text)*/ * LocalSet.Convert(Distance.Text);
                Cost.Text = String.Format("{0:0.00}", cost); //cost.ToString("{0:0.00}");
                Cost.TextChanged += Cost_Changed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(MainActivity.Log(ex));
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
            new Task(() =>
            {
                Intent intent = new Intent(this, typeof(DetailsActivity));
                intent.PutExtra("position", e);
                StartActivity(intent);
            }).Start();
        }

        public static string Log(string text)
        {
            var LogTask = new Task<string>(() =>
            {
                Debug.Add(text);
                return text;
            });
            LogTask.Start();
            LogTask.Wait();
            return LogTask.Result;
        }
        public static string Log(Exception exception)
        {
            return Log(exception.Message);
        }

        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            await Task.Run(() =>
            {
                base.OnActivityResult(requestCode, resultCode, data);
                RunOnUiThread(() => mAdapter.NotifyDataSetChanged());
                DiffUtil.DiffResult result = DiffUtil.CalculateDiff(new DiffCallback(LocalSet.VehicleDataList, OldVehicleDataList), true);
                RunOnUiThread(() => result.DispatchUpdatesTo(mAdapter));
                HideWelcome();
            });//.Start();
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
            public static void Button_Click(object sender, EventArgs e)
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
            public static void ShowFabMenu()
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
