﻿using Android.App;
using Android.Content;
using System.Collections.Generic;
using System.Threading;

namespace FuelCost
{
    static class LocalSet
    {
        private static int Lenght;
        private static List<VehicleData> VehicleDatas = new List<VehicleData>();

        private static ISharedPreferences VehicleSharedPreferences;
        private static ISharedPreferences PriceSharedPreferences;

        private static float lpg;
        private static float pb;
        private static float on;

        /// <summary>
        /// Pobiera dane
        /// </summary>
        public static void GetSetting()
        {
            new Thread(() =>
            {
                VehicleSharedPreferences = Application.Context.GetSharedPreferences("Vehicles", FileCreationMode.Private);

                Lenght = VehicleSharedPreferences.GetInt("lenght", 0);

                if (Lenght == 0)
                {
                    return;
                }
                Lenght++;

                for (int i = 1; i < Lenght; i++)
                {
                    VehicleData temp = new VehicleData();
                    string text = VehicleSharedPreferences.GetString(i.ToString(), string.Empty);
                    if (text != string.Empty)
                    {
                        temp.AddRaw(text);
                        VehicleDatas.Add(temp);
                    }
                }

                PriceSharedPreferences = Application.Context.GetSharedPreferences("Price", FileCreationMode.Private);

                lpg = PriceSharedPreferences.GetFloat("LpgPrice", 0);
                pb = PriceSharedPreferences.GetFloat("PbPrice", 0);
                on = PriceSharedPreferences.GetFloat("OnPrice", 0);
            }).Start();
        }

        public static List<VehicleData> VehicleDataList { get { return VehicleDatas; } }

        public static void AddVehicle(VehicleData vehicle)
        {
            new Thread(() =>
            {
                Lenght++;
                var editor = VehicleSharedPreferences.Edit();
                editor.PutString(Lenght.ToString(), vehicle.PrepareRaw());
                editor.PutInt("lenght", Lenght);
                editor.Commit();
                VehicleDatas.Add(vehicle);
            }).Start();
        }

        public static float LpgPrice
        {
            get
            {
                return lpg;
            }
            set
            {
                UpdatePrice("LpgPrice", value);
                lpg = value;
            }
        }
        public static float PbPrice
        {
            get
            {
                return pb;
            }
            set
            {
                UpdatePrice("PbPrice", value);
                lpg = value;
            }
        }
        public static float OnPrice
        {
            get
            {
                return on;
            }
            set
            {
                UpdatePrice("OnPrice", value);
                lpg = value;
            }
        }

        private static void UpdatePrice(string keyname, float value)
        {
            new Thread(() =>
            {
                var editor = PriceSharedPreferences.Edit();
                editor.PutFloat(keyname, value);
                editor.Commit();
            }).Start();
        }


        public static void DelVehicle(VehicleData data)
        {
            new Thread(() =>
            {
                for (int i = 0 ; i < VehicleDataList.Count; i++)
                {
                    if (VehicleDataList[i].Name == data.Name)
                    {
                        //im in 
                        i++;
                        VehicleSharedPreferences.Edit().Remove(i.ToString()).Commit();
                        GetSetting();
                    }
                }
            }).Start();
        }
    }
}