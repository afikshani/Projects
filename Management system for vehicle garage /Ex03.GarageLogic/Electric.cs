using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class Electric : Ex03.GarageLogic.EnergySource
    {
        internal Electric(Dictionary<string, string> i_VehicleProperties)
            : base(i_VehicleProperties)
        {
        }

        internal void Recharge(float i_AmountToAdd)
        {
            AddEnergy(i_AmountToAdd);
        }

        public static List<string> GetElectricProperties()
        {
            List<string> electricProperties = new List<string>();
            electricProperties.Add("Current battery status");

            return electricProperties;
        }
    }
}
