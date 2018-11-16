using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class Car : Vehicle
    {
        public enum CarColor { Grey, Blue, White, Black }
        public enum DoorsNumber { Two=2, Three=3, Four=4, Five=5 }

        private CarColor m_ColorOfCar;
        private DoorsNumber m_NumberOfDoors;

        internal Car(Dictionary<string, string> i_VehicleProperties)
            : base(i_VehicleProperties)
        {
            m_ColorOfCar = FieldsChecker.CheckValidColor(i_VehicleProperties["Color of car"]);
            m_NumberOfDoors = FieldsChecker.CheckValidNumberOfDoors(i_VehicleProperties["Number of doors"]);
        }

        public CarColor ColorOfCar
        {
            get { return m_ColorOfCar; }
        }

        public DoorsNumber NumberOfDoors
        {
            get { return m_NumberOfDoors; }
        }

        public static List<string> GetCarProperties()
        {
            List<string> carProperties = new List<string>();
            carProperties.Add("Color of car");
            carProperties.Add("Number of doors");
            carProperties.AddRange(Wheel.GetWheelProperties());
            return carProperties;
        }

    }
}
