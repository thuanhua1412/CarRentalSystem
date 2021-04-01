using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CarRentalSystemManagement
{
    /// <summary>
    /// 
    /// The CRM class references each Vehicle that has currently been registered. A customer object 
    /// first needs to be instantaiated with a unique registration number and then can be added,
    /// removed and modified before being saved to the CRM.
    /// 
    /// 
    /// Author: Myungjun Choi
    /// 
    /// </summary>

    public class CRM
    {

        // list of customers in the database
        public List<Customer> Customers;
        // directory of the customerfile 
        private string CustomerFile = @"..\..\..\Data\customer.csv";

        /// <summary>
        /// constructor for CRM. If customer file exists, load the data
        /// If not, make new list of customers
        /// </summary>
        public CRM()
        {
            if (File.Exists(CustomerFile))
            {
                LoadFromFile();
            }
            else
            {
                Customers = new List<Customer>();
            } // end if 
        } // end method

        /// <summary>
        /// add customer to the Customers list
        /// </summary>
        /// <param name="customer">customer object to be aded </param>
        /// <returns></returns>
        public bool AddCustomer(Customer customer)
        {
            if (Customers.Exists(x => x.CustomerID != customer.CustomerID))
            {
                Customers.Add(customer);
                return true;
            }
            else
            {
                return false;
            } // end if 
        } // end method

        /// <summary>
        ///  remove cusotmer from the list
        /// </summary>
        /// <param name="customer"> customer object to be removed </param>
        /// <param name="fleet"> fleet class instantiated </param>
        /// <returns>true or false depending on the success of the action </returns>
        public bool removecustomer(Customer customer, Fleet fleet)
        {
            if (fleet.IsRenting(customer.CustomerID) == false)
            {
                Customers.Remove(customer);
                return true;
            } // end if 
            return false;
        } // end method

        /// <summary>
        /// remove the customer using customer ID
        /// </summary>
        /// <param name="customerid">customer ID of the customer that needs to be removed</param>
        /// <param name="fleet"> fleet class instantiated </param>
        /// <returns>true or false depending on the succes of the action </returns>
        public bool removecustomer(int customerid, Fleet fleet)
        {
            if (fleet.IsRenting(customerid) == false)
            {
                Customers.Remove(Customers.Find(x => x.CustomerID == customerid));
                return true;
            } // end method
            return false;
        } // end method 

        /// <summary>
        /// retreive customer list 
        /// </summary>
        /// <returns> customer list</returns>
        public List<Customer> GetCustomers()
        {
            return Customers;
        } // end method 

        /// <summary>
        /// save the currernt customerLsit to the csv_file
        /// </summary>
        public void SaveToFile()
        {
            // deletes the existing file
            File.Delete(CustomerFile);
            File.AppendAllText(CustomerFile, string.Format("Customer", "Title", "Firstname", "Lastname", "Gender", "DOB") + Environment.NewLine);

            // append each customerList to the csv file
            for (int i = 0; i < Customers.Count; i++)
            {
                File.AppendAllText(CustomerFile, Customers[i].ToCSVString());
            } // end for 
        } // end method 

        /// <summary>
        /// load data to the local variable from the CSV file 
        /// </summary>
        public void LoadFromFile()
        {
            // flush the customer list
            Customers = new List<Customer>();

            // same algorithm for loadfromFIle in fleet class
            using (StreamReader sr = new StreamReader(CustomerFile))
            {
                sr.ReadLine();
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    List<string> lineValues = line.Split(',').ToList();
                    int customerID = int.Parse(lineValues[0]);
                    string title = lineValues[1];
                    string firstName = lineValues[2];
                    string lastName = lineValues[3];
                    Customer.Gender gender = (Customer.Gender)Enum.Parse(typeof(Customer.Gender), lineValues[4]);
                    string dateOfBirth = lineValues[5];
                    Customer current_customer = new Customer(customerID, title, firstName, lastName, gender, dateOfBirth);
                    Customers.Add(current_customer);
                } // end while
            } // end using
        } //end method
    } // end CRM class
}
