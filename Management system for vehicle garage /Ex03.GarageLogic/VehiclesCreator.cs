using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex03.GarageLogic;

namespace Ex03.GarageLogic
{
    public class VehiclesCreator
    {
        public enum VehicleType { ElectricCar = 1, FuelCar, ElectricBike, FuelBike, Truck }

        public static Vehicle SelectConstructor(VehiclesCreator.VehicleType io_TypeOfVehicle, Dictionary<string, string> io_VehicleProperties)
        {
            Vehicle newVehicle = null;
            switch (io_TypeOfVehicle)
            {
                case VehicleType.ElectricCar:
                    newVehicle = createNewElectricCar(io_VehicleProperties);
                    break;
                case VehicleType.FuelCar:
                    newVehicle = createNewFuelCar(io_VehicleProperties);
                    break;
                case VehicleType.ElectricBike:
                    newVehicle = createNewElectricBike(io_VehicleProperties);
                    break;
                case VehicleType.FuelBike:
                    newVehicle = createNewFuelBike(io_VehicleProperties);
                    break;
                case VehicleType.Truck:
                    newVehicle = createNewTruck(io_VehicleProperties);
                    break;
            }

            return newVehicle;
        }

        private static Car createNewElectricCar(Dictionary<string, string> io_VehicleProperties)
        {
            io_VehicleProperties.Add("Type of energy", "Electric");
            io_VehicleProperties.Add("Maximum battery capacity", "3.2");
            io_VehicleProperties.Add("Number of wheels", "4");
            io_VehicleProperties.Add("Wheels Maximum PSI amount", "32");

            return new Car(io_VehicleProperties);
        }

        private static Car createNewFuelCar(Dictionary<string, string> io_VehicleProperties)
        {
            io_VehicleProperties.Add("Type of energy", "Fuel");
            io_VehicleProperties.Add("Fuel type", "Octan98");
            io_VehicleProperties.Add("Maximum fuel capacity", "45");
            io_VehicleProperties.Add("Number of wheels", "4");
            io_VehicleProperties.Add("Wheels Maximum PSI amount", "32");

            return new Car(io_VehicleProperties);
        }

        private static MotorBike createNewElectricBike(Dictionary<string, string> io_VehicleProperties)
        {
            io_VehicleProperties.Add("Type of energy", "Electric");
            io_VehicleProperties.Add("Maximum battery capacity", "1.8");
            io_VehicleProperties.Add("Number of wheels", "2");
            io_VehicleProperties.Add("Wheels Maximum PSI amount", "30");

            return new MotorBike(io_VehicleProperties);
        }

        private static MotorBike createNewFuelBike(Dictionary<string, string> io_VehicleProperties)
        {
            io_VehicleProperties.Add("Type of energy", "Fuel");
            io_VehicleProperties.Add("Fuel type", "Octan96");
            io_VehicleProperties.Add("Maximum fuel capacity", "6");
            io_VehicleProperties.Add("Number of wheels", "2");
            io_VehicleProperties.Add("Wheels Maximum PSI amount", "30");

            return new MotorBike(io_VehicleProperties);
        }

        private static Truck createNewTruck(Dictionary<string, string> io_VehicleProperties)
        {
            io_VehicleProperties.Add("Type of energy", "Fuel");
            io_VehicleProperties.Add("Fuel type", "Octan96");
            io_VehicleProperties.Add("Maximum fuel capacity", "115");
            io_VehicleProperties.Add("Number of wheels", "12");
            io_VehicleProperties.Add("Wheels Maximum PSI amount", "28");

            return new Truck(io_VehicleProperties);
        }

    }
}
