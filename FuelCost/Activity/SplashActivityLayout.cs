using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Threading;

namespace FuelCost
{
    [Activity(Theme = "@style/AppTheme.SplashNoBackground", MainLauncher = false, NoHistory = true)]
    public class SplashActivityLayout : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivityLayout).Name;

        static TextView TextView;

        static CoordinatorLayout MainLayout;

        ManualResetEvent blocker = new ManualResetEvent(true);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SplashLayout);

            var ProgressBar = FindViewById<ContentLoadingProgressBar>(Resource.Id.ContentLoadingProgressBar);
            ProgressBar.Show();

            TextView = FindViewById<TextView>(Resource.Id.textView1);
            MainLayout = FindViewById<CoordinatorLayout>(Resource.Id.MainLayout);

            MainLayout.Touch += MainLayout_Touch;

            //StartupTaskList.Add(new Task(delegate () { LocalSet.Open(); }));
            //StartupTaskList.Add(new Task(delegate () { LocalSet.ReadPrices(); }));
            //StartupTaskList.Add(new Task(delegate () { LocalSet.ReadVehicles(); }));

            //StartupTaskList[0].ContinueWith(((Task) => StartupTaskList[1].Start()));
            //StartupTaskList[1].ContinueWith(((Task) => StartupTaskList[2].Start()));
            //StartupTaskList[2].ContinueWith(StrtupWorkEnded);
            //StartupTaskList[0].Start();


            Thread StartupTask = new Thread(() =>
            {
                try
                {
                    LocalSet.Open();
                    RunOnUiThread(() => TextView.Text += "\nNawiązano połączenie z bazą");
                    LocalSet.ReadPrices();
                    RunOnUiThread(() => TextView.Text += "\nOdczytano ceny paliw");
                    LocalSet.ReadVehicles();
                    RunOnUiThread(() => TextView.Text += "\nOdczytano pojazdy");

                }
                catch (Exception e)
                {
                    TextView.Text += "\n" + e.Message;
                }
                finally
                {
                    RunOnUiThread(() =>
                    {
                        TextView.Text += "\n";
                        TextView.Text += MainActivity.Log("Zadanie wykonano pomyślnie");
                    });


                    StrtupWorkEnded();
                }
            });

            // StartupTask.ContinueWith(StrtupWorkEnded);

            StartupTask.Start();
            // StartupTask.Wait();

        }

        private void MainLayout_Touch(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Up:
                    {
                        blocker.Set();
                        break;
                    }

                case MotionEventActions.Down:
                    {
                        blocker.Reset();
                        break;
                    }
            }

            return;
            //  throw new NotImplementedException();
        }


        private void StrtupWorkEnded()//Task obj)
        {
            //  if (obj.IsCompletedSuccessfully)
            //  {

            //  }
            //  else
            //  {
            //      TextView.Text += "\nZadanie niewykonane";
            //  }

            //  TextView.Text += "\n ..Completed";
            Thread.Sleep(500);
            blocker.WaitOne();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public override void OnBackPressed() { }



    }
}