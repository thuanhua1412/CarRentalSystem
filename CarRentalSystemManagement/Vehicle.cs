using System.Collections.Generic;

namespace CarRentalSystemManagement
{
    /// <summary>
    /// 
    /// The Vehicle class will be instantiated each time a new Vehicle is created.
    /// It references attributes that are used throughout this program to charactise
    /// each Vehicle. 
    /// 
    /// Each Vehicle shall have a unique vehicles registration number which will be used
    /// to identify whether or not it is currently being rented to a Customer.
    /// 
    /// Author: Nicholas Jensen
    /// 
    /// </summary>

    public class Vehicle
    {
        public enum VehicleClass
        {
            Economy,
            Family,
            Luxury,
            Commercial
        }

        public enum FuelType
        {
            Petrol,
            Diesel
        }

        public enum TransmissionType
        {
            Automatic,
            Manual
        }

        // Declaring global vehicles variables
        public VehicleClass Vehicle_Class { get; set; }
        public TransmissionType Transmission_Type { get; set; }
        public FuelType Fuel_Type { get; set; }
        public string VehicleRego { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Colour { get; set; }
        public int Year { get; set; }
        public int NumSeats { get; set; }
        public bool GPS { get; set; }
        public bool SunRoof { get; set; }
        public double DailyRate { get; set; }

        /// <summary>
        /// Vehicle constructor with params for all Vehicle attributes
        /// </summary>
        /// 
        /// <param name="vehicleRego"> vehicles registration number </param>
        /// <param name="make"> Make of the vehicles </param>
        /// <param name="model"> Model name </param>
        /// <param name="year">Year of the Make</param>
        /// <param name="vehicleClass"> Type of the vehicles </param>
        /// <param name="numSeats"> Total number of seats </param>
        /// <param name="transmissionType"> Automatic vs Manual </param>
        /// <param name="fuelType"> Diesel vs Petrol </param>
        /// <param name="GPS"> Yes or No </param>
        /// <param name="sunRoof">Yes or No </param>
        /// <param name="colour"> Colour of the vehicles </param>
        /// <param name="dailyRate"> Rental cost per day </param>

        public Vehicle(string vehicleRego, string make, string model, int year, VehicleClass vehicleClass, int numSeats,
                       TransmissionType transmissionType, FuelType fuelType, bool GPS, bool sunRoof, string colour, double dailyRate)
        {
            // Setting the class variables to input param. values
            Vehicle_Class = vehicleClass;
            VehicleRego = vehicleRego;
            Make = make;
            Model = model;
            Year = year;
            NumSeats = numSeats;
            Transmission_Type = transmissionType;
            Fuel_Type = fuelType;
            this.GPS = GPS;
            SunRoof = sunRoof;
            DailyRate = dailyRate;
            Colour = colour;

            // Setting default values
            if (NumSeats.Equals(0))
            {
                NumSeats = 1;
            }
            if (dailyRate.Equals(0))
            {
                DailyRate = 50;
            }
            if (colour.Equals(""))
            {
                Colour = "Black";
            }
            if (vehicleClass == Vehicle.VehicleClass.Commercial)
            {
                Fuel_Type = Vehicle.FuelType.Diesel;
                DailyRate = 130;
            }
            else if (vehicleClass == Vehicle.VehicleClass.Economy)
            {
                Transmission_Type = Vehicle.TransmissionType.Automatic;
            }
            else if (vehicleClass == Vehicle.VehicleClass.Family)
            {
                DailyRate = 80;
            }
            else
            {
                GPS = true;
                SunRoof = true;
                DailyRate = 120;
            }
        } // end Vehicle() constructor

        /// <summary>
        /// 
        /// Return a string with the formated Vehicle local variables.
        /// This will be used to convert the vehicles attributes to a csv 
        /// formatted file.
        /// 
        /// </summary>
        /// 
        /// <returns> string representation of CSV line </returns>

        public string ToCSVString()
        {
            // Format csv string
            string CSV = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", VehicleRego, Make, Model,
                                       Year, Vehicle_Class, NumSeats, Transmission_Type, Fuel_Type, GPS, SunRoof, Colour, DailyRate);

            return CSV;
        }  // end ToCSVString() method

        /// <summary>
        /// Override the toString method and return string of suited vehicles
        /// </summary>
        /// 
        /// <returns> string representation of vehicles </returns>

        public override string ToString()
        {
            return base.ToString();
        } // end ToString() method

        /// <summary>
        /// Return a list of each Vehicle attribute
        /// </summary>
        /// 
        /// <returns> List of string of attributes for the current instance of vehicles </returns>

        public List<string> GetAttributeList()
        {

            List<string> attributeList = new List<string>();

            // Add each of the Vehicle attributes to the list 'attributeList'
            attributeList.Add(VehicleRego);
            attributeList.Add(Make);
            attributeList.Add(Model);
            attributeList.Add(Year.ToString());
            attributeList.Add(Vehicle_Class.ToString());
            attributeList.Add(Transmission_Type.ToString());
            attributeList.Add(string.Format("{0}-Seater", NumSeats.ToString()));
            attributeList.Add(Colour);
            attributeList.Add(DailyRate.ToString());
            attributeList.Add(Fuel_Type.ToString());
            if (GPS)
            {
                attributeList.Add("GPS");
            } // end if 
            if (SunRoof)
            {
                attributeList.Add("sunroof");
            } // end if 

            return attributeList;
        } // end GetAttributeList() method

    } // end Vehicle class
}
