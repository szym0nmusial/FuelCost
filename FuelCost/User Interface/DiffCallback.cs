using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Util;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    class DiffCallback : DiffUtil.Callback
    {

        private List<VehicleData> oldList;
        private List<VehicleData> newList;
         
        public DiffCallback(List<VehicleData> newone, List<VehicleData> older)
        {
            newList = newone;
            oldList = older;
        }


        public override int NewListSize => newList.Count;

        public override int OldListSize => oldList.Count;

        public override bool AreContentsTheSame(int oldItemPosition, int newItemPosition)
        {
            //throw new NotImplementedException();
            //return true;// oldList[oldItemPosition].
            return oldList[oldItemPosition].Name == newList[newItemPosition].Name;
        }

        public override bool AreItemsTheSame(int oldItemPosition, int newItemPosition)
        {
            //return false;
            //throw new NotImplementedException();
            //  return JsonConvert
            // return false;
           // return oldList[oldItemPosition].Name == newList[newItemPosition].Name;

             return oldList[oldItemPosition].Equals(newList[newItemPosition]);

        }
    }
}