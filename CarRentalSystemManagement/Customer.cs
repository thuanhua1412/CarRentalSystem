using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystemManagement
{
    /// <summary>
    /// 
    /// The customer object will be instantiated each time a new customer is created.
    /// It references attributes that are used throughout this program to charactise
    /// each custmomer. 
    /// 
    /// Each Vehicle shall have a unique customer ID which will be used
    /// to identify whether or not the customer already exists in the database.
    /// 
    /// Author: Myungjun CHoi
    /// 
    /// </summary>

    public class Customer
    {

        public enum Gender
        {
            Male,
            Female
        }

        // Auto Implemented Properties 
        public int CustomerID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public Gender Gender_ { get; set; }

        /// <summary>
        /// Consturctor for Customer class. Instantiates Customer object
        /// </summary>
        /// <param name="customerID"> unique customer ID </param>
        /// <param name="title"> Title of the customer</param>
        /// <param name="firstName"> first name </param>
        /// <param name="lastName"> last name</param>
        /// <param name="gender">gender </param>
        /// <param name="dateOfBirth">date of birth </param>
        public Customer(int customerID, string title, string firstName, string lastName, Gender gender, string dateOfBirth)
        {
            CustomerID = customerID;
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            Gender_ = gender;
            DateOfBirth = dateOfBirth;
        } // end method

        /// <summary>
        /// converts customer info into CSV formatted string
        /// </summary>
        /// <returns></returns>
        public string ToCSVString()
        {
            string CSV = string.Format("{0},{1},{2},{3},{4},{5}\n", CustomerID, Title, FirstName, LastName, Gender_, DateOfBirth);
            return CSV;
        } // end method

        /// <summary>
        ///  override the default tostring method and customise it to suit our needs
        /// </summary>
        /// <returns> string of customer information </returns>
        public override string ToString()
        {
            return base.ToString();
        } // end method 
    } // end customer class 
}
