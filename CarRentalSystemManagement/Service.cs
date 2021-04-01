using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystemManagement
{
    public class Service
    {
        public List<Vehicle> vehicles { get; set; }
        public bool isServiceEngine { get; set; }
        public bool isServiceTransmission { get; set; }
        public bool isServiceTires { get; set; }
        public string date { get; set; }
        public int mileAge { get; set; }
        public float cost { get; set; }
        public string garage { get; set; }
        public Service(List<Vehicle> vehicles, bool isServiceEngine, bool isServiceTransmission, bool isServiceTires, string date, int distance_meter, float cost, string garage)
        {
            this.vehicles = new List<Vehicle>();
            this.vehicles = vehicles;
            this.isServiceEngine = isServiceEngine;
            this.isServiceTransmission = isServiceTransmission;
            this.isServiceTires = isServiceTires;
            this.date = date;
            this.mileAge = distance_meter;
            this.cost = cost;
            this.garage = garage;
        }
        /*
        public void serviceEngine(Vehicle vehicles, string date, int distance_meter)
        {
            this.date = date;
            this.mileAge = distance_meter;
            this.isServiceEngine = true;
            this.vehicles.Add(vehicles);
        }

        public void serviceTransmission(Vehicle vehicles, string date, int distance_meter)
        {
            this.isServiceTransmission = true;
            this.date = date;
            this.mileAge = distance_meter;
            this.vehicles.Add(vehicles);
        }

        public void serviceTires(Vehicle vehicles, string date, int distance_meter)
        {
            this.date = date;
            this.mileAge = distance_meter;
            this.vehicles.Add(vehicles);
            this.isServiceTires = true;
        }
        public void serviceFleet(Fleet fleet, string date, int distance_meter, bool isServiceEngine, bool isServiceTransmission, bool isServiceTires)
        {
            this.vehicles = fleet.fleet;
            for (int i = 0; i < fleet.fleet.Count; i++)
            {
                if (isServiceEngine)
                {
                    this.serviceEngine(fleet.fleet[i], date, distance_meter);
                }

                if (isServiceTires)
                {
                    this.serviceTires(fleet.fleet[i], date, distance_meter);
                }

                if (isServiceTransmission)
                {
                    this.serviceTransmission(fleet.fleet[i], date, distance_meter);
                }
            }
        }*/


        public List<List<string>> GetAttributeList()
        {
            List<List<string>> attributeList = new List<List<string>>();

            List<string> attributeVehicleList = new List<string>();
            // Add vehicles attribute to list
            foreach (Vehicle x in vehicles)
            {
                attributeVehicleList.Add(x.VehicleRego);
                attributeVehicleList.Add(x.Make);
                attributeVehicleList.Add(x.Model);
                attributeVehicleList.Add(x.Year.ToString());
                attributeVehicleList.Add(isServiceEngine.ToString());
                attributeVehicleList.Add(isServiceTransmission.ToString());
                attributeVehicleList.Add(isServiceTires.ToString());
                attributeVehicleList.Add(date);
                attributeVehicleList.Add(mileAge.ToString());
                attributeVehicleList.Add(cost.ToString());
                attributeVehicleList.Add(garage);
                attributeList.Add(attributeVehicleList);// Add service attribute to list
            }

            return attributeList;
        }// end method
    }
}
