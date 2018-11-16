using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class Fuel : Ex03.GarageLogic.EnergySource
    {
        public enum FuelType { Soler, Octan95, Octan96, Octan98 }

        private readonly FuelType m_FuelType;

        internal Fuel(Dictionary<string, string> i_VehicleProperties)
            : base(i_VehicleProperties)
        {
            m_FuelType = FieldsChecker.CheckValidFuelType(i_VehicleProperties["Fuel type"]);
        }

        public FuelType TypeOfFuel
        {
            get { return m_FuelType; }
        }

        internal void Refuel(float i_AmountToAdd, FuelType i_FuelType)
        {
            if (i_FuelType.Equals(m_FuelType))
            {
                AddEnergy(i_AmountToAdd);
            }
            else
            {
                throw new ArgumentException("The fuel you're trying to put isn't from the correct type.");
            }
        }

        public static List<string> GetFuelProperties()
        {
            List<string> fuelPropertiesList = new List<string>();
            fuelPropertiesList.Add("Current fuel amount");

            return fuelPropertiesList;
        }
    }
}
