using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class MotorBike : Vehicle
    {
        public enum LicenseType { A, AB, A2, B1 }

        private LicenseType m_LicenseType;
        private int m_EngineCapacity;

        public MotorBike(Dictionary<string, string> i_VehicleProperties)
            : base(i_VehicleProperties)
        {
            m_LicenseType = FieldsChecker.CheckValidLicense(i_VehicleProperties["License type"]);
            m_EngineCapacity = FieldsChecker.StringProperyToInt(i_VehicleProperties["Engine capacity"]);
        }

        public LicenseType TypeOfLicense
        {
            get { return m_LicenseType; }
        }

        public int EngineCapacity
        {
            get { return m_EngineCapacity; }
        }

        public static List<string> GetMotorbikeProperties()
        {
            List<string> MotorbikeProperties = new List<string>();
            MotorbikeProperties.Add("License type");
            MotorbikeProperties.Add("Engine capacity");
            MotorbikeProperties.AddRange(Wheel.GetWheelProperties());
            return MotorbikeProperties;
        }

    }
}
