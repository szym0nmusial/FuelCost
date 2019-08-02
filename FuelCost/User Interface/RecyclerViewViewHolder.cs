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

        public TextView Consuption { get; private set; }
        public TextView Price { get; private set; }

        private EditText Cost;

        private EditText Distance;

        private LinearLayout ExpandLinearLayout;
      
        public RecyclerViewViewHolder(View view, Action<int> listener) : base(view)
        {
            // Locate and cache view references:
            Name = view.FindViewById<TextView>(Resource.Id.name);
            FuelType = view.FindViewById<TextView>(Resource.Id.typ);
            Consuption = view.FindViewById<TextView>(Resource.Id.cons);
            Price = view.FindViewById<TextView>(Resource.Id.price);

            Distance = view.FindViewById<EditText>(Resource.Id.distance);
            Distance.TextChanged += Distance_Changed;
            Cost = view.FindViewById<EditText>(Resource.Id.cost);
            Cost.TextChanged += Cost_Changed;

            ExpandLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.MoreLinearLayout);

           /// view.Click += (sender, e) => listener(Position);
            view.Click += View_Click;

            //if(SharedDistance != 0.0)
            //{
            //    Distance.Text = LocalSet.Convert(SharedDistance);
            //}

        }

        private void View_Click(object sender, EventArgs e)
        {
            switch (ExpandLinearLayout.Visibility)
            {
                case ViewStates.Visible:
                    {
                        // MoreBtn.Animate().Rotation(0f);
                        ExpandLinearLayout.Visibility = ViewStates.Gone;
                        break;
                    }
                case ViewStates.Gone:
                    {
                        // MoreBtn.Animate().Rotation(180f);
                        ExpandLinearLayout.Visibility = ViewStates.Visible;
                        break;
                    }
            }
        }

        private void Cost_Changed(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                    Distance.TextChanged -= Distance_Changed;
                    var dist = LocalSet.Convert(Cost.Text) * 100 / LocalSet.Convert(Price.Text) /  double.Parse(Consuption.Text);
                    Distance.Text = String.Format("{0:0.00}", dist); //dist.ToString();
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
                    var cost = double.Parse(Consuption.Text) * 0.01f * LocalSet.Convert(Price.Text) * LocalSet.Convert(Distance.Text);
                    Cost.Text = String.Format("{0:0.00}", cost);// cost.ToString();
                Cost.TextChanged += Cost_Changed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}