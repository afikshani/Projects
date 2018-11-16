using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class Truck : Vehicle
    {
        private bool m_HasDeadlyMaterials;
        private float m_MaxWeightAllowed;

        public Truck(Dictionary<string, string> i_VehicleProperties)
            : base(i_VehicleProperties)
        {
            m_HasDeadlyMaterials = FieldsChecker.ValidationOfDangerousAnswer(i_VehicleProperties["Does carry dangerous materials?"]);
            m_MaxWeightAllowed = FieldsChecker.StringProperyToFloat(i_VehicleProperties["Maximum weight allowed"]);
        }

        public bool IsCarringDangerousMaterial
        {
            get { return m_HasDeadlyMaterials; }
        }

        public float MaxCarryingWeightAllowed
        {
            get { return m_MaxWeightAllowed; }
        }

        public static List<string> GetTruckProperties()
        {
            List<string> TruckProperties = new List<string>();
            TruckProperties.Add("Does carry dangerous materials?");
            TruckProperties.Add("Maximum weight allowed");
            TruckProperties.AddRange(Wheel.GetWheelProperties());
            return TruckProperties;
        }
    }
}
