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
        List<VehicleData> vehicleDatas = new List<VehicleData>();


        private List<Android.Support.V4.App.Fragment> FragmentList = new List<Android.Support.V4.App.Fragment>();
        private List<string> FragmentListNames = new List<string>();
        

        public FragmentAdapter(Android.Support.V4.App.FragmentManager fm)  : base(fm)
        {
            //var tmp = new VehicleData();
            //var tmp2 = new VehicleData();

            //tmp.Name = "Mazda 3";
            //tmp.Pbinjection = true;
            //tmp.FuelType = VehicleData.FuelTypeEnum.lpg;
            //tmp.consumption = 10.5F;

            //vehicleDatas.Add(tmp);
            //tmp2.Name = "Honda";
            //tmp2.FuelType = VehicleData.FuelTypeEnum.pb;
            //vehicleDatas.Add(tmp2);

            DeSerialize();

            //


            for (int i = 0; i < vehicleDatas.Count; i++)
            {
                VehicleData.FuelTypeEnum Type = vehicleDatas[i].FuelType;
                switch (Type)
                {
                    case VehicleData.FuelTypeEnum.lpg:
                        {
                            var temp = new Lpg();
                            temp.Data = vehicleDatas[i];
                            FragmentList.Add(temp);
                            break;
                        }
                    case VehicleData.FuelTypeEnum.pb:
                    case VehicleData.FuelTypeEnum.diesel:
                        {
                            var temp = new Other();
                            temp.Data = vehicleDatas[i];
                            FragmentList.Add(temp);
                            break;
                        }
                }
                FragmentListNames.Add(vehicleDatas[i].Name);
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
            //return new Java.Lang.String("kude");
          //  return base.GetPageTitleFormatted(position);
        }

        public void DeSerialize()
        {
            ISharedPreferences preferences = Application.Context.GetSharedPreferences("Vehicles", FileCreationMode.Private);
            int lenght = preferences.GetInt("lenght", 0);

            if(lenght == 0)
            {
                return;
            }
            lenght++;

            string[] build = new string[lenght];

            for(int i = 1; i < lenght; i++)
            {
                VehicleData temp = new VehicleData();
                temp.AddRaw(preferences.GetString(i.ToString(), System.String.Empty));
                vehicleDatas.Add(temp);
            }




        }
    }
}