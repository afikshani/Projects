using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal abstract class EnergySource
    {
        private readonly float m_MaxEnergy;
        private float m_EnergyLeft;

        internal EnergySource(Dictionary<string, string> i_VehicleProperties)
        {
            if (i_VehicleProperties["Type of energy"] == "Fuel")
            {
                m_MaxEnergy = FieldsChecker.StringProperyToFloat(i_VehicleProperties["Maximum fuel capacity"]);
                m_EnergyLeft = FieldsChecker.StringProperyToFloat(i_VehicleProperties["Current fuel amount"]);
                if (m_MaxEnergy < m_EnergyLeft)
                {
                    throw new ArgumentException("The amount of fuel is over the limit.");
                }
            }
            else  // It is a electric battery source
            {
                m_MaxEnergy = FieldsChecker.StringProperyToFloat(i_VehicleProperties["Maximum battery capacity"]);
                m_EnergyLeft = FieldsChecker.StringProperyToFloat(i_VehicleProperties["Current battery status"]);
                if (m_MaxEnergy < m_EnergyLeft)
                {
                    throw new ArgumentException("The battery reached its limit.");
                }
            }

        }

        public float MaxEnergy
        {
            get { return m_MaxEnergy; }
        }

        public float EnergyLeft
        {
            get { return m_EnergyLeft; }
        }

        internal void AddEnergy(float i_AmountToAdd)
        {
            if (i_AmountToAdd + m_EnergyLeft > m_MaxEnergy)
            {
                throw new ValueOutOfRangeException(0, m_MaxEnergy);
            }
            else
            {
                m_EnergyLeft += i_AmountToAdd;
            }
        }

        internal float EnergyPercentageCalculator()
        {
            return (EnergyLeft / MaxEnergy) * 100;
        }

    }
}