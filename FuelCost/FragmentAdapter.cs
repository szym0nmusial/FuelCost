using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace FuelCost
{
    class FragmentAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> FragmentList = new List<Android.Support.V4.App.Fragment>();
        private List<string> FragmentListNames = new List<string>();

        public FragmentAdapter(Android.Support.V4.App.FragmentManager fm)  : base(fm)
        {
 

            for (int i = 0; i < LocalSet.VehicleDataList.Count; i++)
            {
                VehicleData.FuelTypeEnum Type = LocalSet.VehicleDataList[i].FuelType;
                switch (Type)
                {
                    case VehicleData.FuelTypeEnum.lpg:
                        {
                            var temp = new Lpg();
                            temp.Data = LocalSet.VehicleDataList[i];
                            FragmentList.Add(temp);
                            break;
                        }
                    case VehicleData.FuelTypeEnum.pb:
                    case VehicleData.FuelTypeEnum.diesel:
                        {
                            var temp = new Other();
                            temp.Data = LocalSet.VehicleDataList[i];
                            FragmentList.Add(temp);
                            break;
                        }
                }
                FragmentListNames.Add(LocalSet.VehicleDataList[i].Name);
            }
            
            FragmentList.Add(new Set());
            FragmentListNames.Add("List");
        }

        public override int Count => FragmentList.Count;

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return FragmentList[position];
        }
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(FragmentListNames[position]);

        }
    }
}