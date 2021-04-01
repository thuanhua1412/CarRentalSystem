using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystemManagement
{
    public class SearchServiceReport
    {
        //List<List<string>> serviceAttribite; // will be saving attributes of all service here for searching

        ServiceReport serviceReport = new ServiceReport(); // instantiate the service class

        /// <summary>
        /// Constructor for SearchServiceReport class
        /// </summary>
        public SearchServiceReport()
        {
            serviceReport = new ServiceReport();
            //serviceAttribite = new List<List<string>>();
        }

        public List<Service> SearchReportOutput(string carRego) 
        {
            List<Service> report = new List<Service>();

            foreach (Service x in serviceReport.GetService())
            {
                foreach(Vehicle y in x.vehicles)
                {
                    if (carRego == y.VehicleRego)
                    {
                        List<Vehicle> car = new List<Vehicle>();
                        car.Add(y);
                        Service carService = new Service(car, x.isServiceEngine, x.isServiceTransmission, x.isServiceTires, x.date, x.mileAge, x.cost, x.garage);
                        report.Add(carService);
                    }
                }
            }
            return report;
        }

    }
}
