using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CarRentalSystemManagement
{
    public class ServiceReport
    {
        // Global variable for ServiceReport class
        public List<Service> serviceReport;

        // Provide a link to each .csv file for saving fleet and rental information
        private string serviceFile = @"..\..\..\Data\service.json";

        public ServiceReport()
        {
            if (File.Exists(serviceFile))
            {
                LoadFromFile();
            }
            else
            {
                serviceReport = new List<Service>();
            }
        }

        /// <summary>
        /// add service into report
        /// </summary>
        public void AddService(Service service)
        {
            serviceReport.Add(service);
        } // end method

        public List<Service> GetService()
        {
            return serviceReport;
        }

        /// <summary>
        /// save the serviceReport to the json_file
        /// </summary>
        
        public void SaveToFile()
        {
            File.Delete(serviceFile);
            File.WriteAllText(serviceFile, JsonConvert.SerializeObject(serviceReport));
        }

        public void LoadFromFile()
        {
            serviceReport = new List<Service>();
            if (File.Exists(serviceFile))
            {
                serviceReport = JsonConvert.DeserializeObject<List<Service>>(File.ReadAllText(serviceFile));
            }
        } // end LoadFromFile() method

    }// end ServiceReport Class
}
