using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Garage
    {

        private Dictionary<string, Vehicle> m_VehiclesInTheGarage;
        private List<VehiclesCreator.VehicleType> m_AvailbleVehicles;

        public Garage()
        {
            m_VehiclesInTheGarage = new Dictionary<string, Vehicle>();
            m_AvailbleVehicles = Enum.GetValues(typeof(VehiclesCreator.VehicleType)).Cast<VehiclesCreator.VehicleType>().ToList();
        }

        public List<VehiclesCreator.VehicleType> AvailbleVehicles
        {
            get { return m_AvailbleVehicles; }
        }

        public Dictionary<string, Vehicle> VehiclesInTheGarage
        {
            get { return m_VehiclesInTheGarage; }
        }




        public bool CheckIfInGarage(string i_LicenseNum)
        {
            bool vehicleIsInGarage = false;
            foreach (var licenseNum in VehiclesInTheGarage)
            {
                if (licenseNum.Key.Equals(i_LicenseNum))
                {
                    vehicleIsInGarage = true;
                }
            }

            return vehicleIsInGarage;
        }

        public void AddVehicle(Vehicle newVehicle, string licenseNum)
        {
            m_VehiclesInTheGarage.Add(licenseNum, newVehicle);
            Console.WriteLine("Your vehicle is now in our garage!");
        }

        public List<string> getProperties(VehiclesCreator.VehicleType vehicleType)
        {
            List<string> properties = new List<string>();
            switch (vehicleType)
            {
                case VehiclesCreator.VehicleType.ElectricCar:
                    properties.AddRange(Car.GetCarProperties());
                    properties.AddRange(Electric.GetElectricProperties());
                    break;
                case VehiclesCreator.VehicleType.FuelCar:
                    properties.AddRange(Car.GetCarProperties());
                    properties.AddRange(Fuel.GetFuelProperties());
                    break;
                case VehiclesCreator.VehicleType.ElectricBike:
                    properties.AddRange(MotorBike.GetMotorbikeProperties());
                    properties.AddRange(Electric.GetElectricProperties());
                    break;
                case VehiclesCreator.VehicleType.FuelBike:
                    properties.AddRange(MotorBike.GetMotorbikeProperties());
                    properties.AddRange(Fuel.GetFuelProperties());
                    break;
                case VehiclesCreator.VehicleType.Truck:
                    properties.AddRange(Truck.GetTruckProperties());
                    properties.AddRange(Fuel.GetFuelProperties());
                    break;
            }

            return properties;
        }

        public string displayAvailbleVehicles()
        {
            int numOfvehicle = 0;
            string typeOfVehicle;
            StringBuilder availableVehicles = new StringBuilder();
            foreach (var vehicle in m_AvailbleVehicles)
            {
                numOfvehicle++;
                typeOfVehicle = vehicle.ToString();
                availableVehicles.AppendFormat("{0}. {1}{2}", numOfvehicle, typeOfVehicle, Environment.NewLine);
            }
            return availableVehicles.ToString();
        }

        public bool ChangeVehicleStatus(string i_LicenseNum, string i_NewStatus)
        {
            if (!CheckIfInGarage(i_LicenseNum))
            {
                throw new ArgumentException("Vehicle is not in our garage");
            }
            Vehicle toChange;
            bool statusChanged = m_VehiclesInTheGarage.TryGetValue(i_LicenseNum, out toChange);
            Vehicle.StatusOfVehicle newStatus = FieldsChecker.CheckValidStatus(i_NewStatus);
            toChange.VehicleStatus = newStatus;

            return statusChanged;
        }

        public void RefuelVehicle(string i_License, string i_TypeOfFuel, string i_AmountToFuel)
        {
            Vehicle vehicleToRefuel;
            float amountToFuel;
            Fuel.FuelType fuelType = (Fuel.FuelType)Enum.Parse(typeof(Fuel.FuelType), i_TypeOfFuel);
            if (!CheckIfInGarage(i_License))
            {
                throw new ArgumentException();
            }

            bool validFormat = float.TryParse(i_AmountToFuel, out amountToFuel);

            if (!validFormat)
            {
                throw new FormatException();
            }

            m_VehiclesInTheGarage.TryGetValue(i_License, out vehicleToRefuel);

            ((vehicleToRefuel.SourceOfEnergy) as Fuel).Refuel(amountToFuel, fuelType);
        }

        public void RechargeVehicle(string i_License, string i_AmountToFuel)
        {
            Vehicle vehicleToRefuel;
            float amountToFuel;
            if (!CheckIfInGarage(i_License))
            {
                throw new ArgumentException("Vehicle is not in our garage");
            }

            bool validFormat = float.TryParse(i_AmountToFuel, out amountToFuel);

            if (!validFormat)
            {
                throw new FormatException();
            }

            m_VehiclesInTheGarage.TryGetValue(i_License, out vehicleToRefuel);

            ((vehicleToRefuel.SourceOfEnergy) as Electric).Recharge(amountToFuel);
        }

        public void AddPSIForVehicle(string i_License)
        {
            float newPsi = 0;
            if (!CheckIfInGarage(i_License))
            {
                throw new ArgumentException("Vehicle is not in our garage");
            }
            Vehicle vehicleToInflate;
            m_VehiclesInTheGarage.TryGetValue(i_License, out vehicleToInflate);

            foreach (var wheel in vehicleToInflate.m_WheelSet)
            {
                wheel.InflateWheel(wheel.MaxPSI - wheel.CurrentPSI);
                newPsi = wheel.MaxPSI;
            }

            vehicleToInflate.m_VehicleProperties["Wheels current PSI amount"] = newPsi.ToString();

        }

        public List<string> LicenseListGenerator(string i_Filter)
        {
            List<string> licenseList = new List<string>();
            bool toListAll = false;
            if (i_Filter.Equals("none"))
            {
                toListAll = true;
            }
            foreach (var vehicle in m_VehiclesInTheGarage)
            {
                if (toListAll == true)
                {
                    licenseList.Add(vehicle.Key);
                }

                else
                {
                    if (vehicle.Value.VehicleStatus.ToString().Equals(i_Filter))
                    {
                        licenseList.Add(vehicle.Key);
                    }
                }
            }

            return licenseList;
        }

        public string GetDataOfVehicle(string i_License)
        {
            if (!CheckIfInGarage(i_License))
            {
                throw new ArgumentException("vehicle is not in our garage");
            }
            Vehicle vehicle;
            m_VehiclesInTheGarage.TryGetValue(i_License, out vehicle);
            StringBuilder dataOfVehicle = new StringBuilder();

            foreach (string property in vehicle.m_VehicleProperties.Keys)
            {
                string data;
                vehicle.m_VehicleProperties.TryGetValue(property, out data);
                dataOfVehicle.AppendFormat("{0}: {1}{2}", property, data, Environment.NewLine);
            }

            return dataOfVehicle.ToString();
        }
    }
}