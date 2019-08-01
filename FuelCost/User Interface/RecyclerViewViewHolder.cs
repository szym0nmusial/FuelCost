using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    public class RecyclerViewViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; private set; }
        public TextView FuelType { get; private set; }
        public double SharedDistance { set  { if (value != 0.0) { Distance.Text = LocalSet.Convert(value); } } }

        public double Consuption;// { get; private set; }
        public double Price;//{ get; private set; }
      //  private double sharedDistance = 0.0;

        private EditText Cost;

        private EditText Distance;
       

        [Obsolete]
        public RecyclerViewViewHolder(View view, Action<int> listener) : base(view)
        {
            // Locate and cache view references:
            Name = view.FindViewById<TextView>(Resource.Id.name);
            FuelType = view.FindViewById<TextView>(Resource.Id.typ);
          //    Consuption = view.FindViewById<TextView>(Resource.Id.cons);
           // Price = view.FindViewById<TextView>(Resource.Id.price);

            Distance = view.FindViewById<EditText>(Resource.Id.distance);
            Distance.TextChanged += Distance_Changed;
            Cost = view.FindViewById<EditText>(Resource.Id.cost);
            Cost.TextChanged += Cost_Changed;

            view.Click += (sender, e) => listener(Position);

            //if(SharedDistance != 0.0)
            //{
            //    Distance.Text = LocalSet.Convert(SharedDistance);
            //}

        }

        private void Cost_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                    Distance.TextChanged -= Distance_Changed;
                    var dist = double.Parse(Cost.Text) * 100 /Price/* LocalSet.Convert(Price.Text)*/ / Consuption /* double.Parse(Consuption.Text)*/;
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
                    Cost.TextChanged -= Cost_Changed;
                    var cost = Consuption /*double.Parse(Consuption.Text)*/ * 0.01f * Price/* LocalSet.Convert(Price.Text)*/ * double.Parse(Distance.Text);
                    Cost.Text = cost.ToString();
                    Cost.TextChanged += Cost_Changed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}