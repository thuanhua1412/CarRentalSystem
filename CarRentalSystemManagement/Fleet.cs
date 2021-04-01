using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace CarRentalSystemManagement
{
    /// <summary>
    /// 
    /// The Fleet class references each Vehicle that has currently been registered. A Vehicle 
    /// first needs to be instantaiated with a unique registration number and then can be added,
    /// removed and modified before being saved to the Fleet.
    /// 
    /// The Fleet class also contains methods to query which Vehicles are currently being rented
    /// and the Customer that is renting them.
    /// 
    /// Author: Nicholas Jensen
    /// 
    /// </summary>

    public class Fleet
    {
        // Declare global variables for the Fleet class
        public List<Vehicle> fleet;
        public List<string> rentedRego;
        public List<int> rentedCustomer;

        // Provide a link to each .csv file for saving fleet and rental information
        private string fleetFile = @"..\..\..\Data\fleet.csv";
        private string rentalsFile = @"..\..\..\Data\rentals.csv";

        /// <summary>
        /// Fleet constructor that populates each local list array with data from .csv files
        /// </summary>

        public Fleet()
        {        
            LoadFromFile();
        } // end Fleet() constructor

        /// <summary>
        /// Add a new Vehicle to the fleet 
        /// </summary>
        /// 
        /// <param name="vehicle"> Vehicle instance </param>
        /// 
        /// <returns> true or false to confirm sucess of the add </returns>

        public bool AddVehicle(Vehicle vehicle)
        {
            // Check to see if the Vehicle already exists by comparing rego.
            if (fleet.Exists(x => x.VehicleRego != vehicle.VehicleRego))
            {
                // If it does not exist
                fleet.Add(vehicle);
                return true;
            }
            else
            {
                return false;
            }
        } // end AddVehicle(Vehicle vehicles) method

        /// <summary>
        /// Remove Vehicle from the fleet if it is not currently being rented by a Customer
        /// </summary>
        /// 
        /// <param name="vehicle"> Vehicle instance </param>
        /// 
        /// <returns> true or false to indicate sucess of the action </returns>

        public bool RemoveVehicle(Vehicle vehicle)
        {
            // Check all vehicles rego in the rented array
            for (int j = 0; j < rentedRego.Count; j++)
            {
                if (vehicle.VehicleRego == rentedRego[j])
                {
                    // Vehicle is currently being rented
                    return false;
                }
            }

            // Check to see if vehicles rego. is present in the fleet
            for (int i = 0; i < fleet.Count; i++)
            {
                if (fleet[i].VehicleRego == vehicle.VehicleRego)
                {
                    // Vehicle has been found in the fleet
                    fleet.Remove(fleet[i]);
                    return true;
                }
            }

            // Param vehicles rego is not present in the Fleet and hence does not exist
            return false;

        } // end RemoveVehicle(Vehicle vehicles) method

        /// <summary>
        /// Remove Vehicle from the fleet with Rego number
        /// </summary>
        /// 
        /// <param name="vehicleRego">Registration Number of the Vehicle</param>
        /// 
        /// <returns> true or false to indicate sucess of the action </returns>

        public bool RemoveVehicle(string vehicleRego)
        {
            for (int j = 0; j < rentedRego.Count; j++)
            {
                if (vehicleRego == rentedRego[j])
                {
                    // Vehicle is currently being rented
                    return false;
                }
            }
            // Check to see if param vehicles rego is present in the fleet
            for (int i = 0; i < fleet.Count; i++)
            {
                // If the param vehicles rego is present then remove from the fleet
                if (fleet[i].VehicleRego == vehicleRego)
                {
                    // Present in the Fleet
                    fleet.Remove(fleet[i]);
                    return true;
                } 
            }
            return false;
        } // end RemoveVehicle(string vehicleRego) method

        /// <summary>
        /// 
        /// Checks to see if the Vehicle registration is present in the Fleet
        /// 
        /// </summary>
        /// 
        /// <param name="vehicleRego">Registration Number of the Vehicle</param>
        /// 
        /// <returns> true or false to indicate sucess of the action </returns>

        /// <summary>
        /// Check to see if the vehicles rego is present in the fleet
        /// </summary>
        /// <param name="vehicleRego">Registration Number of the Vehicle</param>
        /// <returns> true or false to indicate sucess of the action </returns>
        public bool PresentInFleet(string vehicleRego)
        {
            // Check to see if param vehicles rego is present in the fleet
            for (int i = 0; i < fleet.Count; i++)
            {
                // If the param vehicles rego is present then return true
                if (fleet[i].VehicleRego == vehicleRego)
                {
                    return true;
                } 
            } 
            return false;
        }

        /// <summary>
        /// Return a list of the Vehicle in the current fleet 
        /// </summary>
        /// 
        /// <returns> list of current vehicles in the fleet </returns>

        public List<Vehicle> GetFleet()
        {
            return fleet;
        }

        /// <summary>
        /// Check to see if Vehicle is currently rented 
        /// </summary>
        /// 
        /// <param name="vehicleRego"> Vehicle registration number used to check if the car is currently being rented </param>
        /// 
        /// <returns> true or false outcome depending on the sucess</returns>

        public bool IsRented(string vehicleRego)
        {
            for (int i = 0; i < rentedRego.Count; i++)
            {
                if (rentedRego[i] == vehicleRego)
                {
                    // Vehicle is currently being rented
                    return true;
                }
            }
            return false;
        } // end IsRented(string vehicleRego) method

        /// <summary>
        /// Check to see if the customer is currently renting any vehicles
        /// </summary>
        /// 
        /// <param name="customerID"> customerID key used to check this </param>
        /// 
        /// <returns>true or false depeneding on the success </returns>

        public bool IsRenting(int customerID)
        {
            for (int i = 0; i < rentedCustomer.Count; i++)
            {
                if (rentedCustomer[i] == customerID)
                {
                    // Customer is currently renting a Vehicle
                    return true;
                }
            }
            return false;
        } // end IsRenting(int customerID) method

        /// <summary>
        /// Return the customerID that is renting a Vehicle
        /// </summary>
        /// 
        /// <param name="vehicleRego"> Vehicle rego. of the car that user wants to check </param>
        /// 
        /// <returns> returns the customer ID. If no one is renting, return -1 </returns>

        public int RentedBy(string vehicleRego)
        {
            for (int i = 0; i < rentedRego.Count; i++)
            {
                if (rentedRego[i] == vehicleRego)
                {
                    return rentedCustomer[i];
                } // end if 
            } // end for 
            return -1; // no match exists
        } // end RentedBy(string vehicleRego) method

        /// <summary>
        /// Renting a vehicles to a customer 
        /// </summary>
        /// 
        /// <param name="vehicleRego"> Rego number of the vehicles that needs to be rented </param>
        /// <param name="customerID">customer ID of the customer who wants to rent it </param>
        /// 
        /// <returns>true or false depending on the success of the action </returns>

        public bool RentCar(string vehicleRego, int customerID)
        {
            if (!IsRented(vehicleRego))
            {
                // Vehicle is not rented and therefore Customer can rent
                rentedRego.Add(vehicleRego);
                rentedCustomer.Add(customerID);
                return true;
            }            
            return false;
        } // end RentCar(string vehicleRego, int customerID) method

        /// <summary>
        /// Return a rented vehicles to the fleet 
        /// </summary>
        /// 
        /// <param name="vehicleRego"> vehicles registration nubmer of the car that needs to be returned</param>
        /// 
        /// <returns> customerID of the customer who was renting it, otherwise return -1 </returns>

        public int ReturnCar(string vehicleRego)
        {
            int rentedCustomerID;

            for (int i = 0; i < rentedRego.Count; i++)
            {
                if (rentedRego[i] == vehicleRego)
                {
                    // Rented Vehicle rego matches CustomerID
                    rentedCustomerID = rentedCustomer[i];
                    rentedRego.Remove(rentedRego[i]);
                    rentedCustomer.Remove(rentedCustomer[i]);
                    // Return the CustomerID number
                    return rentedCustomerID;
                }
            }
            return -1;
        } // end  ReturnCar(string vehicleRego) method

        /// <summary>
        /// Return a string with the formatted fleet local variables for a .csv file type.
        /// Used for saving user changes when exiting the application
        /// </summary>
        /// 
        /// <param name="vehicleRego">Vehicle registration number</param>
        /// <param name="customerID">Customer ID number</param>
        /// 
        /// <returns> String representation for formatting to a csv file </returns>

        public string ToCSVString(string vehicleRego, int customerID)
        {
            string CSV = string.Format("{0},{1}\n", vehicleRego, customerID.ToString());
            return CSV;
        }

        /// <summary>
        /// Saving all new changes to the .csv files 
        /// </summary>

        public void SaveToFile()
        {
            // Clear all data in the files
            File.Delete(fleetFile);
            File.Delete(rentalsFile);

            // Adding the header column
            File.AppendAllText(fleetFile, string.Format("Rego", "Make", "Model", "Year", "Vehicle Class", "Num Seats", "Transmission", "Fuel", "GPS", "Sunroof", "Colour", "Daily Rate") + Environment.NewLine);
            File.AppendAllText(rentalsFile, string.Format("Vehicle", "Customer") + Environment.NewLine);

            // Line by line append the current Fleet list to Fleet.csv
            for (int i = 0; i < fleet.Count; i++)
            {
                File.AppendAllText(fleetFile, fleet[i].ToCSVString());
            }

            // Line by line append the Rentals list to Rentals.csv
            for (int i = 0; i < rentedRego.Count; i++)
            {
                // Rented vehicles to column 1, CustomerID to columns 2
                File.AppendAllText(rentalsFile, ToCSVString(rentedRego[i], rentedCustomer[i]));
            }
        } // end SaveToFile() method

        /// <summary>
        /// 
        /// Loading all data from the .csv files to local Variables. 
        /// This data can then be used to populate the data grid views of the Windows Form application
        /// 
        /// This method creates a temporary array that reads and writess each column of the .csv file as an element and
        /// Then instantiates a class with all of the temp. variables as input params. 
        /// 
        /// Each time adding the new instace to either the Fleet or the CRM
        /// 
        /// </summary>

        public void LoadFromFile()
        {
            // Create empty local arrays to append with .csv data
            fleet = new List<Vehicle>();
            rentedRego = new List<string>();
            rentedCustomer = new List<int>();

            if (File.Exists(fleetFile))
            {
                // Use StreamReader to read each line of the csv file
                using (StreamReader sr = new StreamReader(fleetFile))
                {
                    sr.ReadLine(); // Accounting for header row of each .csv file
                    while (sr.Peek() != -1)
                    {
                        string line = sr.ReadLine();
                        List<string> lineValues = line.Split(',').ToList();
                        string vehicleRego = lineValues[0];
                        string make = lineValues[1];
                        string model = lineValues[2];
                        int year = int.Parse(lineValues[3]);
                        Vehicle.VehicleClass vehicleClass = (Vehicle.VehicleClass)Enum.Parse(typeof(Vehicle.VehicleClass), lineValues[4]);
                        int numSeats = int.Parse(lineValues[5]);
                        Vehicle.TransmissionType transmissionType = (Vehicle.TransmissionType)Enum.Parse(typeof(Vehicle.TransmissionType), lineValues[6]);
                        Vehicle.FuelType fuelType = (Vehicle.FuelType)Enum.Parse(typeof(Vehicle.FuelType), lineValues[7]);
                        bool GPS = bool.Parse(lineValues[8]);
                        bool sunRoof = bool.Parse(lineValues[9]);
                        string colour = lineValues[10];
                        double dailyRate = double.Parse(lineValues[11]);

                        // Create instance of Class Vehicle
                        Vehicle current_vehicle = new Vehicle(vehicleRego, make, model, year, vehicleClass, numSeats,
                            transmissionType, fuelType, GPS, sunRoof, colour, dailyRate);

                        // Append Vehicle to fleet
                        fleet.Add(current_vehicle);
                    }
                } // end using
            }

            if (File.Exists(rentalsFile))
            {
                using (StreamReader sr = new StreamReader(rentalsFile))
                {
                    sr.ReadLine(); // Accounting for header row
                    while (sr.Peek() != -1)
                    {
                        string line = sr.ReadLine();
                        List<string> lineValues = line.Split(',').ToList();
                        string vehicleRego = lineValues[0];
                        int customerID = int.Parse(lineValues[1]);
                        // Append to rented array
                        rentedRego.Add(vehicleRego);
                        // Append to customer array
                        rentedCustomer.Add(customerID); 
                    }
                } // end using
            }
        } // end LoadFromFile() method  

    } // end Fleet class 
}
