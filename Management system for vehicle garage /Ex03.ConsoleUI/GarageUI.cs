using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    internal static class GarageUI
    {
        internal static void GarageManager()
        {
            Garage garage = new Garage();
            bool toContinue = true;

            while (toContinue == true)
            {
                Console.WriteLine(@"
Hello! Welcome to our garage;]
Please choose the number of service you would like:
1. Insert a new vehicle to the garage.
2. Display list of the current vehicles in the garage.
3. Change vehicle status.
4. Inflate air in the wheels to the maximum.
5. Refuel vehicle (fuel type).
6. Charge vehicle (electric type).
7. Display vehicle's full details.
8. Exit.");
                string serviceSelected = Console.ReadLine();
                int numOfService = checkIfValidOption(serviceSelected, 8, 0);

                switch (numOfService)
                {
                    case 1:
                        insertNewVehicle(garage);
                        break;
                    case 2:
                        displayLicenseNumbers(garage);
                        break;
                    case 3:
                        changeVehicleStatus(garage);
                        break;
                    case 4:
                        inflateMaxAir(garage);
                        break;
                    case 5:
                        refuel(garage);
                        break;
                    case 6:
                        recharge(garage);
                        break;
                    case 7:
                        displayFullDetails(garage);
                        break;
                    case 8:
                        exit();
                        toContinue = false;
                        break;
                }

                Console.ReadLine();
            }
        }

        private static int checkIfValidOption(string io_ServiceSelected, int upperBound, int lowerBound)
        {
            int numOfService = 0;
            bool succeedParsing = int.TryParse(io_ServiceSelected, out numOfService);
            while ((succeedParsing == false) || numOfService <= lowerBound || numOfService > upperBound)
            {
                Console.WriteLine("Please choose a valid option:");
                io_ServiceSelected = Console.ReadLine();
                succeedParsing = int.TryParse(io_ServiceSelected, out numOfService);
            }

            return numOfService;
        }

        private static void insertNewVehicle(Garage i_Garage)
        {
            Console.WriteLine("Please enter license number:");
            string licenseNum = checkIfnotEmpty(Console.ReadLine());

            bool alreadyInTheGarage = i_Garage.CheckIfInGarage(licenseNum);

            if (!alreadyInTheGarage)
            {
                Console.WriteLine("Please enter owner's name:");
                string ownerName = checkIfnotEmpty(Console.ReadLine());

                Console.WriteLine("Please enter owner's number:");
                string ownerNumber = checkIfnotEmpty(Console.ReadLine());

                Console.WriteLine("Please enter model's name:");
                string modelName = checkIfnotEmpty(Console.ReadLine());

                VehiclesCreator.VehicleType selectedType = vehicleSelection(i_Garage);

                Dictionary<string, string> vehicleDictionary = new Dictionary<string, string>();
                List<string> vehicleProperties = i_Garage.getProperties(selectedType);
                foreach (string property in vehicleProperties)
                {
                    string toPrint = string.Format("Please enter {0}:", property);
                    Console.WriteLine(toPrint);
                    vehicleDictionary.Add(property, Console.ReadLine());
                }
                vehicleDictionary.Add("Owner name", ownerName);
                vehicleDictionary.Add("Owner phone", ownerNumber);
                vehicleDictionary.Add("License number", licenseNum);
                vehicleDictionary.Add("Model name", modelName);

                try
                {
                    Vehicle newVehicle = VehiclesCreator.SelectConstructor(selectedType, vehicleDictionary);
                    i_Garage.AddVehicle(newVehicle, licenseNum);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            else
            {
                Console.WriteLine("Your vehicle is already in the garage.");
            }
        }

        private static void displayLicenseNumbers(Garage i_Garage)
        {
            if (i_Garage.VehiclesInTheGarage.Count == 0)
            {
                Console.WriteLine("\nNo vehicles in the garage");
            }

            else
            {
                Console.WriteLine(@"Would you like to use a filter?  
Please choose an option:
1. Do not use filter.
2. Show only fixed vehicles.
3. Show only paid vehicles.
4. Show only vehicles in repair.");

                List<string> licenseNumberList = new List<string>();
                int userSelection = checkIfValidOption(Console.ReadLine(), 4, 0);

                switch (userSelection)
                {
                    case 1:
                        licenseNumberList = i_Garage.LicenseListGenerator("None");
                        break;
                    case 2:
                        licenseNumberList = i_Garage.LicenseListGenerator("Fixed");
                        break;
                    case 3:
                        licenseNumberList = i_Garage.LicenseListGenerator("Paid");
                        break;
                    case 4:
                        licenseNumberList = i_Garage.LicenseListGenerator("InRepair");
                        break;
                }

                Console.WriteLine("License numbers:");
                foreach (string number in licenseNumberList)
                {
                    Console.WriteLine(number);
                }
            }
        }

        private static string checkIfnotEmpty(string io_OwnerInfo)
        {
            bool validName = false;

            while (validName == false)
            {
                if (io_OwnerInfo.Equals(string.Empty))
                {
                    Console.WriteLine("Please enter a valid input:");
                    io_OwnerInfo = Console.ReadLine();
                }

                else
                {
                    validName = true;
                }
            }

            return io_OwnerInfo;
        }


        private static VehiclesCreator.VehicleType vehicleSelection(Garage i_Garage)
        {
            VehiclesCreator.VehicleType typeSelected;
            int userSelection;
            Console.WriteLine("Please choose the type of your vehicle's type from the list:");
            Console.WriteLine(i_Garage.displayAvailbleVehicles());
            bool inputIsNumber = int.TryParse(Console.ReadLine(), out userSelection);
            typeSelected = (VehiclesCreator.VehicleType)userSelection;
            bool validSelection = false;

            while (!validSelection)
            {
                if (!inputIsNumber || (userSelection > i_Garage.AvailbleVehicles.Count || userSelection < 1))
                {
                    Console.WriteLine("Please enter a valid selection:");
                    inputIsNumber = int.TryParse(Console.ReadLine(), out userSelection);
                    typeSelected = (VehiclesCreator.VehicleType)userSelection;
                }

                else
                {
                    validSelection = true;
                }
            }

            return typeSelected;
        }

        private static void changeVehicleStatus(Garage i_Garage)
        {
            Console.WriteLine("Please enter your license number:");
            string licenseNum = checkIfnotEmpty(Console.ReadLine());
            Console.WriteLine(@"Please choose the new status from the list:
1. In repair.
2. Fixed.
3. Paid.");

            int userSelection = checkIfValidOption(Console.ReadLine(), 3, 0);
            try
            {
                switch (userSelection)
                {
                    case 1:
                        i_Garage.ChangeVehicleStatus(licenseNum, "InRepair");
                        break;
                    case 2:
                        i_Garage.ChangeVehicleStatus(licenseNum, "Fixed");
                        break;
                    case 3:
                        i_Garage.ChangeVehicleStatus(licenseNum, "Paid");
                        break;
                }
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void inflateMaxAir(Garage i_Garage)
        {
            Console.WriteLine("Please enter your license number:");
            string licenseNum = checkIfnotEmpty(Console.ReadLine());

            try
            {
                i_Garage.AddPSIForVehicle(licenseNum);
                Console.WriteLine("Air was added successfully");
            }
            catch (ValueOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static void refuel(Garage i_Garage)
        {
            string fuelType = "";
            Console.WriteLine("Please enter your license number:");
            string licenseNum = checkIfnotEmpty(Console.ReadLine());
            Console.WriteLine(@"Please choose type of fuel:
1. Soler.
2. Octan95.
3. Octan96.
4. Octan98.");
            int userSelection = checkIfValidOption(Console.ReadLine(), 4, 0);
            Console.WriteLine("Please enter the amount you wish to fuel:");
            string amountToFuelStr = Console.ReadLine();

            switch (userSelection)
            {
                case 1:
                    fuelType = "Soler";
                    break;
                case 2:
                    fuelType = "Octan95";
                    break;
                case 3:
                    fuelType = "Octan96";
                    break;
                case 4:
                    fuelType = "Octan98";
                    break;
            }

            try
            {
                i_Garage.RefuelVehicle(licenseNum, fuelType, amountToFuelStr);
                Console.WriteLine("Refuled successfully");
            }

            catch (ValueOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }

            catch (FormatException)
            {
                Console.WriteLine("Not a valid argument");
            }

            catch (ArgumentException)
            {
                Console.WriteLine("The vehicle is not in our garage");
            }

        }

        private static void recharge(Garage i_Garage)
        {
            Console.WriteLine("Please enter your license number:");
            string licenseNum = checkIfnotEmpty(Console.ReadLine());
            Console.WriteLine("Please enter minutes for charging:");
            string minutesToChargeStr = Console.ReadLine();

            try
            {
                i_Garage.RechargeVehicle(licenseNum, minutesToChargeStr);
                Console.WriteLine("Recharged successfully");
            }

            catch (ValueOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }

            catch (FormatException)
            {
                Console.WriteLine("Not a valid argument");
            }

            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void displayFullDetails(Garage i_Garage)
        {
            Console.WriteLine("Please enter your license number:");
            string licenseNum = checkIfnotEmpty(Console.ReadLine());

            try
            {
                string vehicleData = i_Garage.GetDataOfVehicle(licenseNum);
                Console.WriteLine(vehicleData);
            }


            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void exit()
        {
            Console.WriteLine("Bye Bye!");
        }
    }
}
