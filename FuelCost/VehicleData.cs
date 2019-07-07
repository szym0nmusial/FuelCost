using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FuelCost
{
    public class VehicleData
    {
        public enum FuelTypeEnum
        {
            Gas = 0,
            Benzya = 1,
            Diesel = 2,
        };

        public string Name;
        public FuelTypeEnum FuelType;
        public bool Pbinjection; // only on lpg
        public double consumption;

        public void AddRaw(string raw)
        {
            try
            {
                string[] tab = raw.Split(';');
                Name = tab[0];
                FuelType = (FuelTypeEnum)int.Parse(tab[1]);
                Pbinjection = bool.Parse(tab[2]);
                consumption = float.Parse(tab[3]);
            }
            catch
            {
                Console.WriteLine("Deserialize error");
            }
        }

        public string PrepareRaw() => Name + ";" + ((int)FuelType).ToString() + ";" + Pbinjection.ToString() + ";" + consumption.ToString();

    }
}