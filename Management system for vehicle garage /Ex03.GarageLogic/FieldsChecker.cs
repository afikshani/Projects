using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    internal class FieldsChecker
    {
        internal static float StringProperyToFloat(string io_StringToFloat)
        {
            float value = 0;
            bool isValidToConvert = float.TryParse(io_StringToFloat, out value);

            if (!isValidToConvert)
            {
                throw new FormatException("The input should be only digits. Please enter valid float input.");
            }

            return value;
        }

        internal static int StringProperyToInt(string io_StringToInt)
        {
            int value = 0;
            bool isValidToConvert = int.TryParse(io_StringToInt, out value);

            if (!isValidToConvert)
            {
                throw new FormatException("The input should be only digits. Please insert valid integer input.");
            }

            return value;
        }

        internal static bool ValidationOfDangerousAnswer(string io_AnswerToCheck)
        {
            bool answer = false;
            if (!io_AnswerToCheck.Equals("yes") && !io_AnswerToCheck.Equals("no"))
            {
                throw new FormatException("not a valid answer for dangerous materials! enter please only yes/no");
            }
            if (io_AnswerToCheck.Equals("yes"))
            {
                answer = true;
            }

            return answer;
        }

        internal static Car.CarColor CheckValidColor(string io_ColorToCheck)
        {
            try
            {
                Object color = Enum.Parse(typeof(Car.CarColor), io_ColorToCheck);
                return (Car.CarColor)color;
            }
            catch
            {
                throw new ArgumentException("Not a valid color. Please enter Grey/Blue/White/Black");
            }
        }

        internal static Car.DoorsNumber CheckValidNumberOfDoors(string io_NumberOfDoorsToCheck)
        {
            try
            {
                Object doors = Enum.Parse(typeof(Car.DoorsNumber), io_NumberOfDoorsToCheck);
                return (Car.DoorsNumber)doors;
            }
            catch
            {
                throw new ArgumentException("Not a valid doors number. Please enter 2/3/4/5");
            }
        }

        internal static Fuel.FuelType CheckValidFuelType(string io_FuelTypeToCheck)
        {
            Object fuelType = Enum.Parse(typeof(Fuel.FuelType), io_FuelTypeToCheck);
            return (Fuel.FuelType)fuelType;
        }

        internal static MotorBike.LicenseType CheckValidLicense(string io_LicenseToCheck)
        {
            Object licenseType = Enum.Parse(typeof(MotorBike.LicenseType), io_LicenseToCheck);
            return (MotorBike.LicenseType)licenseType;
        }

        internal static Vehicle.StatusOfVehicle CheckValidStatus(string io_StatusOfVehicleToCheck)
        {
            Object statusOfCar = Enum.Parse(typeof(Vehicle.StatusOfVehicle), io_StatusOfVehicleToCheck);
            return (Vehicle.StatusOfVehicle)statusOfCar;
        }

    }
}
