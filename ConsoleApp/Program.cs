using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalSystemCManagement;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Vehicle testVehicle = new Vehicle("456PUS", "Mazda", "3", 2000, Vehicle.VehicleClass.Economy, 4, Vehicle.TransmissionType.Automatic, Vehicle.FuelType.Petrol, false, false, 50, "Black");
            //List<Vehicle> vehicle = new List<Vehicle>();

            Fleet fleet = new Fleet();
            
            for(int i = 0; i < fleet.rentedRego.Count; i++)
            {
                Console.WriteLine(fleet.rentedRego[i]);
            }
            Console.ReadKey();
        }
    }
}
