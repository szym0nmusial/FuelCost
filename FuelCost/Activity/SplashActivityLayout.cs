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
using System.Diagnostics;

namespace FuelCost
{

    [Activity(Theme = "@style/AppTheme.SplashNoBackground", MainLauncher = false, NoHistory = true)]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeTypes = new[] { "text/*"/*, "/"*/ })]
    public class SplashActivityLayout : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivityLayout).Name;

        static TextView TextView;
       // bool RunOnce = false;

        static CoordinatorLayout MainLayout;

        ManualResetEvent blocker = new ManualResetEvent(true);

        double SharedDistance = 0.0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);



            SetContentView(Resource.Layout.SplashLayout);

            var ProgressBar = FindViewById<ContentLoadingProgressBar>(Resource.Id.ContentLoadingProgressBar);
            ProgressBar.Show();

            TextView = FindViewById<TextView>(Resource.Id.textView1);
            MainLayout = FindViewById<CoordinatorLayout>(Resource.Id.MainLayout);

            MainLayout.Touch += MainLayout_Touch;




            Task StartupTask = new Task(() =>
            {
                try
                {
                    //RunOnce = true;
                    LocalSet.Open();
                    RunOnUiThread(() => TextView.Text += "\nNawiązano połączenie z bazą");
                    LocalSet.Prices = new Dictionary<VehicleData.FuelTypeEnum, double>();
                    LocalSet.ReadPrices();
                    RunOnUiThread(() => TextView.Text += "\nOdczytano ceny paliw");
                    LocalSet.VehicleDataList = new List<VehicleData>();
                    LocalSet.ReadVehicles();
                    RunOnUiThread(() => TextView.Text += "\nOdczytano pojazdy");

                }
                catch (Exception e)
                {
                    TextView.Text += "\n" + e.Message;
                    //RunOnce = false;
                }
                finally
                {
                    RunOnUiThread(() =>
                    {
                        TextView.Text += "\n";
                        TextView.Text += MainActivity.Log("Zadanie wykonano");
                    });


                  //  StrtupWorkEnded();
                }
            });

            // StartupTask.ContinueWith(StrtupWorkEnded);

            //if (!RunOnce)
            //{
            //    StartupTask.Start();
            //}
            // StartupTask.Wait();
            Task DataShared = new Task(() =>
            {
                try
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    Intent intent = Intent;
                    var shareAction = intent.Action;

                    if (Intent.Extras != null)
                    {
                        Console.WriteLine(MainActivity.Log("Recived share with type: " + intent.Type));

                        if (intent.Type.Contains("text/plain"))
                        {
                            var Data = intent.GetStringExtra("android.intent.extra.TEXT");

                            for (int a = 0; a < Data.Length; a++)
                            {            
                               // var o2 = Data.IndexOf(')');
                                if(Data[a] == ')')
                                    if(Data[a-2] == 'k')
                                        if(Data[a-1] == 'm')
                                        {
                                            Console.WriteLine("Found km)");
                                            for(int b = a-3; b>=0;b--)
                                            {
                                                if (Data[b] == '(')
                                                {
                                                    var textkm = Data.Substring(b + 1, a-b-4);
                                                    Console.WriteLine(MainActivity.Log("Udostępniono: " + textkm + " km trasy"));
                                                    timer.Stop();
                                                    Console.WriteLine(MainActivity.Log("Co zajeło tyle czasu: "+ timer.Elapsed ));
                                                    SharedDistance = LocalSet.Convert(textkm);
                                                    return;
                                                }
                                            }
                                        }
                            }
                            #region Przykładowy string
                            /*
                             * Udostępniona trasa Z (53.6267613,10.1001760) do Praca przez Steilshooper Str.. 39 min (14 km) 39 min przy bieżącym natężeniu ruchu 1. Kieruj się Am Stühm-Süd na południe w stronę Quittenweg 2. Skręć w prawo, pozostając na Am Stühm-Süd 3. Wybierz dowolny pas, aby skręcić w lewo w Bramfelder Chaussee 4. Skręć w prawo w Steilshooper Allee 5. Skręć w lewo w Steilshooper Str. 6. Trzymaj się lewej strony, pozostając na Steilshooper Str. 7. Skręć w lewo w Wachtelstraße 8. Skręć w prawo w Bramfelder Str. 9. Wjedź na Hamburger Str. 10. Wybierz jeden z dwóch lewych pasów, aby skręcić w lewo w Hamburger Str./B5 11. Dalej prosto po Schürbeker Str. 12. Wybierz jeden z trzech prawych pasów, aby skręcić w prawo w Bürgerweide/B75 13. Wybierz jeden z dwóch prawych pasów, aby skręcić w prawo w Spaldingstraße 14. Skręć w lewo w Nagelsweg 15. Skręć w prawo w Amsinckstraße/B4 16. Skręć w prawo w Am Mittelkanal/Mittelkanalbrücke 17. Dojedź do lokalizacji: Sonninstraße 8 Aby wyznaczyć najlepszą trasę z uwzględnieniem aktualnego ruchu, wejdź na https://maps.app.goo.gl/EEXM3fzisZ6WSXwK8
                             */
                            #endregion
                            #region Sprawdzenie jakie dane zawiera intent
                            /*
                            var index = Intent.Extras.KeySet();
                            foreach (var temp in index)
                            {
                                Console.WriteLine(MainActivity.Log("Key:"));
                                Console.WriteLine(MainActivity.Log(temp));
                                Console.WriteLine(MainActivity.Log("Value:"));
                                Console.WriteLine(MainActivity.Log(intent.GetStringExtra(temp)));
                            }
                            */
                            #endregion
                            #region Output
                            /*

Recived share with type: text/plain

Key:
android.intent.extra.SUBJECT
Value:
Udostępniona trasa
Key:
android.intent.extra.TEXT
Value:
Udostępniona trasa
Z (53.6267613,10.1001760) do Praca przez Steilshooper Str..

39 min (14 km)
39 min przy bieżącym natężeniu ruchu


1. Kieruj się Am Stühm-Süd na południe w stronę Quittenweg
2. Skręć w prawo, pozostając na Am Stühm-Süd
3. Wybierz dowolny pas, aby skręcić w lewo w Bramfelder Chaussee
4. Skręć w prawo w Steilshooper Allee
5. Skręć w lewo w Steilshooper Str.
6. Trzymaj się lewej strony, pozostając na Steilshooper Str.
7. Skręć w lewo w Wachtelstraße
8. Skręć w prawo w Bramfelder Str.
9. Wjedź na Hamburger Str.
10. Wybierz jeden z dwóch lewych pasów, aby skręcić w lewo w Hamburger Str./B5
11. Dalej prosto po Schürbeker Str.
12. Wybierz jeden z trzech prawych pasów, aby skręcić w prawo w Bürgerweide/B75
13. Wybierz jeden z dwóch prawych pasów, aby skręcić w prawo w Spaldingstraße
14. Skręć w lewo w Nagelsweg
15. Skręć w prawo w Amsinckstraße/B4
16. Skręć w prawo w Am Mittelkanal/Mittelkanalbrücke
17. Dojedź do lokalizacji: Sonninstraße 8
Aby wyznaczyć najlepszą trasę z uwzględnieniem aktualnego ruchu, wejdź na https://maps.app.goo.gl/EEXM3fzisZ6WSXwK8
                            */
                            #endregion
                        }
                    }
                }
                catch { }
            });

            var Tasks = new Task[] { DataShared, StartupTask };

            //DataShared.Start();
            //StartupTask.Start();

            foreach(var Task in Tasks)
            {
                Task.Start();
            }
            Task.WaitAll(Tasks);
            StrtupWorkEnded();
            
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
            //  Thread.Sleep(500);
            blocker.WaitOne();

            var MainActivityIntent = new Intent(Application.Context, typeof(MainActivity));
            MainActivityIntent.PutExtra("SharedDistance", SharedDistance);
            StartActivity(MainActivityIntent);
            Finish();
        }

        public override void OnBackPressed() { }



    }
}