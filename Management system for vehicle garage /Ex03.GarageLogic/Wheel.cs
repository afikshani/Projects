using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class Wheel
    {
        private readonly string m_ManufacturerName;
        private float m_MaxPSI;
        private float m_CurrentPSI;

        internal Wheel(Dictionary<string, string> i_VehicleProperties)
        {
            m_ManufacturerName = i_VehicleProperties["Wheels manufacturer name"];
            m_CurrentPSI = FieldsChecker.StringProperyToFloat(i_VehicleProperties["Wheels current PSI amount"]);
            m_MaxPSI = FieldsChecker.StringProperyToFloat(i_VehicleProperties["Wheels Maximum PSI amount"]);
        }

        public string ManufacturerName
        {
            get { return m_ManufacturerName; }
        }

        public float MaxPSI
        {
            get { return m_MaxPSI; }
        }

        public float CurrentPSI
        {
            get { return m_CurrentPSI; }
            set { m_CurrentPSI = value; }
        }

        internal void InflateWheel(float i_ToAddPSI)
        {
            if (i_ToAddPSI + m_CurrentPSI > m_MaxPSI)
            {
                throw new ValueOutOfRangeException(0, m_MaxPSI);
            }
            else
            {
                m_CurrentPSI += i_ToAddPSI;
            }
        }

        public static List<string> GetWheelProperties()
        {
            List<string> wheelProperties = new List<string>();
            wheelProperties.Add("Wheels manufacturer name");
            wheelProperties.Add("Wheels current PSI amount");

            return wheelProperties;
        }


    }
}
