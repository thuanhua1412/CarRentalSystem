using CarRentalSystemManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalSystem
{

    public partial class Form1 : Form
    {
        
        // Khai báo biến toàn cục sử dụng để theo dõi tương tác
        private string[] fleetColumns = new string[] { "Rego", "Model", "Make", "Year", "Vehicle Class", "Seats", "Transmission", "Fuel Type",
                                                       "GPS", "Sunroof", "Daily Rate", "Colour"};
        private string[] customerColumns = new string[] { "CustomerID", "Title", "First Name", "Last Name", "Gender", "DOB" };
        private string[] rentalColumns = new string[] { "Vehicle Rego", "Customer ID", "Daily Rate" };
        private string[] serviceReportColumns = new string[] { "Rego", "Make", "Model", "Year", "Service Engine", "Service Transmission", "Service Tires", "Date", "Mileage", "Cost", "Garage" };

        private String selectedRego;
        private int selectedID;

        private static int REG_COL = 0;
        private static int ID_COL = 0;

        Fleet currentFleet = new Fleet();
        CRM currentCustomer = new CRM();
        Search current_search = new Search();
        SearchServiceReport current_search_report = new SearchServiceReport();
        ServiceReport currentService = new ServiceReport();

        /// <summary>
        /// Constructor is run when the Form is starting and sets up the GUI
        /// </summary>

        public Form1()
        {
            InitializeComponent();
            SetupComponents();
            PopulateAllDataGridViews();
        }

        /// <summary>
        /// 
        /// Populating each data grid view with all Fleet and CRM information. 
        /// 
        /// Setting the defaults and values for comboboxes and numeric up down controls
        /// 
        /// </summary>

        private void SetupComponents()
        {
            // Maximise the GUI view to fullscreen
            this.WindowState = FormWindowState.Maximized;

            SetupAllDataGridViewColumns();
            SetAllComboBoxes();
            SetUpAllNumericUpDowns();
            ClearFleetGroupBoXAttributes();

            // Prevent trailling row that allows users to add data to data grid views
            this.dgvFleet.AllowUserToAddRows = false;
            this.dgvCustomer.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvSearch.AllowUserToAddRows = false;

            // Prevent users from editing cell data
            this.dgvFleet.ReadOnly = true;
            this.dgvCustomer.ReadOnly = true;
            this.dgvReport.ReadOnly = true;
            this.dgvSearch.ReadOnly = true;
        } // end SetupComponents() method

        /// <summary>
        /// 
        /// Setting the values of the combobox search property to be any Customer that is
        /// current not renting a Vehicle. 
        /// 
        /// This is due to the constraint that a single customer can not rent multiple cars
        /// 
        /// </summary>

        private void SetupcmbSearchCusomter()
        {
            List<Customer> customerList = currentCustomer.GetCustomers();
            cmbSearchCustomer.Items.Clear();
            for (int i = 0; i < customerList.Count; i++)
            {
                if (currentFleet.IsRenting(customerList[i].CustomerID) == false)
                {
                    // Append information if customers are not renting
                    cmbSearchCustomer.Items.Add(string.Format("{0} - {1} {2} {3}", customerList[i].CustomerID,
                    customerList[i].Title, customerList[i].FirstName, customerList[i].LastName));
                }
            }
        } // end SetupcmbSearchCustomer() method

        /// <summary>
        /// Setting the values of the combobox search property to be any Car Name that is
        /// serviced
        /// </summary>
        private void SetupcmbServiceReportCarName()
        {
            List<Service> serviceList = currentService.GetService();
            cmbServiceReportCarName.Items.Clear();
            for (int i = 0; i < serviceList.Count; i++)
            {
                for (int j = 0; j < serviceList[i].vehicles.Count; j++)
                {
                    cmbServiceReportCarName.Items.Add(string.Format("{0} - {1}", serviceList[i].vehicles[j].Make, serviceList[i].vehicles[j].VehicleRego));
                }
            }
        }


        /// <summary>
        /// Setting the default values for all numeric up down properties
        /// </summary>
        /// 


        private void SetUpAllNumericUpDowns()
        {
            nudFleetSeats.Minimum = 0;
            nudFleetSeats.Maximum = 20;
            nudFleetDailyRate.Minimum = 0;
            nudFleetDailyRate.Maximum = 1000;
        }

        /// <summary>
        /// Populates the Fleet and Customer data grid views with the header information
        /// </summary>

        private void SetupAllDataGridViewColumns()
        {
            dgvFleet.ColumnCount = fleetColumns.Length;
            dgvCustomer.ColumnCount = customerColumns.Length;
            dgvServiceReport.ColumnCount = serviceReportColumns.Length;

            // Setup fleet column headers
            for (int i = 0; i < fleetColumns.Length; i++)
            {
                dgvFleet.Columns[i].Name = fleetColumns[i];
            }
            // Setup customer column headers
            for (int i = 0; i < customerColumns.Length; i++)
            {
                dgvCustomer.Columns[i].Name = customerColumns[i];
            }
            //Setup service report colum headers
            for (int i = 0; i < serviceReportColumns.Length; i++)
            {
                dgvServiceReport.Columns[i].Name = serviceReportColumns[i];
            }
        }

        /// <summary>
        /// Populates the data grid view for Fleet with each Vehicle that is saved in the corresponding .csv file
        /// </summary>

        private void PopulateDataGridViewFleet()
        {
            dgvFleet.Rows.Clear();

            List<Vehicle> vehicles = currentFleet.GetFleet();

            foreach (Vehicle x in vehicles)
            {
                dgvFleet.Rows.Add(x.VehicleRego, x.Make, x.Model, x.Year, x.Vehicle_Class, x.NumSeats, x.Transmission_Type, x.Fuel_Type, x.GPS, x.SunRoof, x.DailyRate, x.Colour);
            }
        }

        /// <summary>
        /// Populates the data grid view for Customer with each Customer that is saved in the corresponding .csv file
        /// </summary>

        private void PopulateDataGridViewCustomer()
        {
            dgvCustomer.Rows.Clear();

            List<Customer> customers = currentCustomer.GetCustomers();

            foreach (Customer x in customers)
            {
                dgvCustomer.Rows.Add(x.CustomerID, x.Title, x.FirstName, x.LastName, x.Gender_, x.DateOfBirth);
            }
        }

        /// <summary>
        /// Populates the data grid view for Service with each Vehicle that is saved in the corresponding .json file
        /// </summary>
        /*
        private void PopulateDataGridViewService()
        {
            dgvService.Rows.Clear();

            List<Vehicle> vehicles = currentFleet.GetFleet();

            foreach (Vehicle x in vehicles)
            {
                dgvService.Rows.Add(x.VehicleRego, x.Make, x.Model, x.Year, x.Vehicle_Class, x.NumSeats, x.Transmission_Type, x.Fuel_Type, x.GPS, x.SunRoof, x.DailyRate, x.Colour);
            }
        }
        */
        /// <summary>
        /// Populates the data grid view for Service Report with each Service that is saved in the corresponding .json file
        /// </summary>




        /// <summary>
        /// Populates the data grid view for Rentals with each Vehicle and Customer that is saved in the corresponding .csv file
        /// </summary>

        private void PopulateDataGridViewReport()
        {
            dgvReport.Rows.Clear();

            List<string> RentedRego = currentFleet.rentedRego;
            List<int> RentedCustomer = currentFleet.rentedCustomer;
            List<double> RentedDailyRate = new List<double>();

            foreach (string Rego in RentedRego)
            {
                List<Vehicle> matchingCar = currentFleet.GetFleet().Where(x => x.VehicleRego == Rego).ToList();
                RentedDailyRate.Add(matchingCar[0].DailyRate);
            }

            for (int i = 0; i < RentedRego.Count; i++)
            {
                dgvReport.Rows.Add(RentedRego[i], RentedCustomer[i], string.Format("${0}", RentedDailyRate[i]));
            }
            lblReportTotalVehiclesFound.Text = string.Format("Total Vehicles Found: {0}", RentedRego.Count);
            lblReportTotalDailyRentalCharge.Text = string.Format("Total daily rental charge: ${0}", RentedDailyRate.Sum());
        }


        /// <summary>
        /// Populates all of the data grid views in the Windows Form
        /// </summary>

        private void PopulateAllDataGridViews()
        {
            PopulateDataGridViewFleet();
            PopulateDataGridViewCustomer();
            PopulateDataGridViewReport();
        }

        /// <summary>
        /// This handler is called each time a row is selected in the data grid view for Fleet
        /// Also storing the selected Vehicle registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dgvFleet_SelectionChanged(object sender, EventArgs e)
        {
            int rowsCount = dgvFleet.SelectedRows.Count;

            // Check if only 1 row is selected
            if (rowsCount == 1)
            {
                selectedRego = (String)(dgvFleet.SelectedRows[0].Cells[REG_COL].Value);
                btnFleetRemove.Enabled = true;
                btnFleetModify.Enabled = true;
            }
        }

        /// <summary>
        /// This handler is called each time a row is selected in the data grid view for Customer
        /// Also storing the selected CustomerID number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dgvCustomer_SelectionChanged(object sender, EventArgs e)
        {
            int rowsCount = dgvCustomer.SelectedRows.Count;

            // Check if only 1 row is selected
            if (rowsCount == 1)
            {
                selectedID = (Int32)(dgvCustomer.SelectedRows[0].Cells[ID_COL].Value);
                btnCustomerRemove.Enabled = true;
                btnCustomerModify.Enabled = true;
            }
        }

        /// <summary>
        /// Setting all of the possible values for each combobox
        /// </summary>

        private void SetAllComboBoxes()
        {
            // Set vehicles combo boxes (from Vehicle class enums)
            cmbFleetClass.DataSource = Vehicle.VehicleClass.GetNames(typeof(Vehicle.VehicleClass));
            cmbFleetFuel.DataSource = Vehicle.FuelType.GetNames(typeof(Vehicle.FuelType));
            cmbFleetTransmission.DataSource = Vehicle.TransmissionType.GetNames(typeof(Vehicle.TransmissionType));

            // Set customer combo boxes
            cmbCustomerGender.Items.Add("Male");
            cmbCustomerGender.Items.Add("Female");
            cmbCustomerTitle.Items.Add("Mr");
            cmbCustomerTitle.Items.Add("Mrs");
            cmbCustomerTitle.Items.Add("Ms");

            // Set Search combobox
            SetupcmbSearchCusomter();

            // Set CarName combobox
            SetupcmbServiceReportCarName();
        }

        /// <summary>
        /// 
        /// Called when the used clicks to remove a Vehicle. The event handler will then show a dialog result
        /// asking whether they are sure. If yes is selected and Vehicle is not currently being rented then will
        /// be removed from Fleet and the dgv for Fleet will be updated.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnFleetRemove_Click(object sender, EventArgs e)
        {
            DialogResult drVehicleRemoveConfirmation = MessageBox.Show(String.Format("Do you really want to remove vehicles {0}?", selectedRego),
                "Remove vehicles confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (drVehicleRemoveConfirmation == DialogResult.Yes)
            {
                if (currentFleet.IsRented(selectedRego))
                {
                    DialogResult dbFailedToRemoveVehicle = MessageBox.Show(String.Format("Vehicle {0} is currently being rented and can not be removed", selectedRego),
                        "Remove vehicles error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    currentFleet.RemoveVehicle(selectedRego);
                    currentFleet.SaveToFile();
                    SetupcmbServiceReportCarName();
                    PopulateDataGridViewFleet();
                }
            }
        }

        /// <summary>
        /// 
        /// Called when the used clicks to remove a Customer. The event handler will then show a dialog result
        /// asking whether they are sure. If yes is selected and Customer is not currently renting then will
        /// be removed from CRM and the dgv for Customer will be updated.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnCustomerRemove_Click(object sender, EventArgs e)
        {
            DialogResult drCustomerRemoveConfirmationi = MessageBox.Show(String.Format("Do you really want to remove customer {0}?", selectedID),
                "Remove customer confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (drCustomerRemoveConfirmationi == DialogResult.Yes)
            {
                if (currentCustomer.removecustomer(selectedID, currentFleet))
                {
                    currentCustomer.SaveToFile();
                    SetupcmbSearchCusomter();
                    PopulateDataGridViewCustomer();
                }
                else
                {
                    DialogResult drFailedToRemoveCustomer = MessageBox.Show(String.Format("Customer {0} is currently renting a vehicles and can not be deleted", selectedID),
                    "Error removing customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 
        /// Event handler called when a user wished to modify Vehicles attributes. 
        /// 
        /// When clicked a view group will become visible with all of the Vehicles attributes
        /// already filled out. The registration number will be disabled for this needs to remain
        /// the same. The user will then enter/change any desired field and then press submit to 
        /// save the changes.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnFleetModify_Click(object sender, EventArgs e)
        {
            gbFleetAddVehicle.Text = "Modify Vehicle";

            gbFleetAddVehicle.Visible = true;
            btnFleetCancel.Enabled = true;
            btnFleetSubmit.Enabled = false;
            btnFleetModify.Enabled = false;
            btnFleetRemove.Enabled = false;
            btnFleetAdd.Enabled = false;

            // Setting the invalid warning text to empty
            lblFleetRegoInvalid.Text = "";
            lblFleetMakeInvalid.Text = "";
            lblFleetModelInvalid.Text = "";
            lblFleetClassInvalid.Text = "";
            lblFleetYearInvalid.Text = "";

            // Filling the modify fields with the selected vehicles info.
            gbFleetModifyVehicleSelectedVehicle();
        }

        /// <summary>
        /// 
        /// Event handler called when a user wished to modify Custoemr attributes. 
        /// 
        /// When clicked a view group will become visible with all of the Customer attributes
        /// already filled out. The CustomerID will be disabled for this needs to remain
        /// the same. The user will then enter/change any desired field and then press submit to 
        /// save the changes.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnCustomerModify_Click(object sender, EventArgs e)
        {
            gbCustomerAddCustomer.Text = "Modify Customer";

            gbCustomerAddCustomer.Visible = true;
            btnCustomerCancel.Enabled = true;
            btnCustomerSubmit.Enabled = true;
            btnCustomerModify.Enabled = false;
            btnCustomerRemove.Enabled = false;
            btnCustomerAdd.Enabled = false;

            // Setting the invalid labels to invisible
            lblCustomerFirstnameInvalid.Text = "";
            lblCustomerLastnameInvalid.Text = "";
            lblCustomerDOBInvalid.Text = "";
            lblCustomerTitleInvalid.Text = "";
            lblCustomerGenderInvalid.Text = "";

            // Filling the modify fields with the selected vehicles info.
            gbCustomerModifyVehicleSelectedVehicle();
        }

        /// <summary>
        /// 
        /// This method will be called when a user clicks to modify a Vehicle. 
        /// 
        /// For each attribute the method will extract the value present in the dgv and then set the
        /// corresponding field in the view group to equal the value. 
        /// 
        /// </summary>

        public void gbFleetModifyVehicleSelectedVehicle()
        {
            btnFleetSubmit.Enabled = true;
            txtFleetRego.Enabled = false;
            txtFleetRego.Text = (String)(dgvFleet.SelectedRows[0].Cells[REG_COL].Value); // Rego
            txtFleetMake.Text = (String)(dgvFleet.SelectedRows[0].Cells[1].Value); // Make
            txtFleetModel.Text = (String)(dgvFleet.SelectedRows[0].Cells[2].Value); // Model
            txtFleetYear.Text = ((Int32)(dgvFleet.SelectedRows[0].Cells[3].Value)).ToString(); // Year
            cmbFleetClass.SelectedIndex = cmbFleetClass.FindStringExact((dgvFleet.SelectedRows[0].Cells[4].Value).ToString()); // Class
            nudFleetSeats.Value = (Int32)(dgvFleet.SelectedRows[0].Cells[5].Value); // Seats
            cmbFleetTransmission.SelectedIndex = cmbFleetTransmission.FindStringExact((dgvFleet.SelectedRows[0].Cells[6].Value).ToString()); // Transmission
            cmbFleetFuel.SelectedIndex = cmbFleetFuel.FindStringExact((dgvFleet.SelectedRows[0].Cells[7].Value).ToString()); // Fuel
            cbFleetGPS.Checked = (Boolean)(dgvFleet.SelectedRows[0].Cells[8].Value); // GPS
            cbFleetSunroof.Checked = (Boolean)(dgvFleet.SelectedRows[0].Cells[9].Value); // Sunroof
            nudFleetDailyRate.Value = decimal.Parse(dgvFleet.SelectedRows[0].Cells[10].Value.ToString()); // Daily Rate
            txtFleetColour.Text = (String)(dgvFleet.SelectedRows[0].Cells[11].Value); // Colour
        } // end gbFleetModifyVehicleSelectedVehicle() method

        /// <summary>
        /// 
        /// This method will be called when a user clicks to modify a Customer. 
        /// 
        /// For each attribute the method will extract the value present in the dgv and then set the
        /// corresponding field in the view group to equal the same value. 
        /// 
        /// </summary>

        public void gbCustomerModifyVehicleSelectedVehicle()
        {
            btnCustomerSubmit.Enabled = true;
            txtCustomerID.Enabled = false;
            txtCustomerID.Text = ((Int32)(dgvCustomer.SelectedRows[0].Cells[0].Value)).ToString(); // CustomerID
            cmbCustomerTitle.SelectedIndex = cmbCustomerTitle.FindStringExact((dgvCustomer.SelectedRows[0].Cells[1].Value).ToString()); // Title
            txtCustomerFirstname.Text = (String)(dgvCustomer.SelectedRows[0].Cells[2].Value); // Firstname
            txtCustomerLastname.Text = (String)(dgvCustomer.SelectedRows[0].Cells[3].Value); // Lastname
            cmbCustomerGender.SelectedIndex = cmbCustomerGender.FindStringExact((dgvCustomer.SelectedRows[0].Cells[4].Value).ToString()); // Gender
            txtCustomerDOB.Text = (String)(dgvCustomer.SelectedRows[0].Cells[5].Value); // DOB
        } // end gbCustomerModifyVehicleSelectedVehicle() method

        /// <summary>
        /// 
        /// Event handler to deal with user clicking the cancel button during the modification or addition of
        /// any new Vehicles.
        /// 
        /// The view group containing the attributes should toggle to invisible and all of the attributes should
        /// be cleared so they are not present next time the group box is made visible.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnFleetCancel_Click(object sender, EventArgs e)
        {
            gbFleetAddVehicle.Visible = false;
            btnFleetModify.Enabled = true;
            btnFleetRemove.Enabled = true;
            btnFleetAdd.Enabled = true;

            // Clear all entries
            ClearFleetGroupBoXAttributes();
        }

        /// <summary>
        /// 
        /// Event handler to deal with user clicking the cancel button during the modification or addition of
        /// any new Customers.
        /// 
        /// The view group containing the attributes should toggle to invisible and all of the attributes should
        /// be cleared so they are not present next time the group box is made visible.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnCustomerCancel_Click(object sender, EventArgs e)
        {
            gbCustomerAddCustomer.Visible = false;
            btnCustomerModify.Enabled = true;
            btnCustomerRemove.Enabled = true;
            btnCustomerAdd.Enabled = true;

            // Clear all entries
            ClearCustomerGroupBoxAttributes();
        }

        /// <summary>
        /// 
        /// Event handler run when the user clicks the add button to submit a new Vehicle.
        /// 
        /// This results in each of the error tags being set to red "!" to symbolise that
        /// it is a mandatory field and must be verified before submit is allowed.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnFleetAdd_Click(object sender, EventArgs e)
        {
            txtFleetRego.Enabled = true;
            gbFleetAddVehicle.Text = "Add Vehicle";
            gbFleetAddVehicle.Visible = true;
            btnFleetCancel.Enabled = true;
            btnFleetSubmit.Enabled = false;
            btnFleetModify.Enabled = false;
            btnFleetRemove.Enabled = false;
            btnFleetAdd.Enabled = false;

            // Setting the invalid labels text and colour
            lblFleetRegoInvalid.Text = "!";
            lblFleetRegoInvalid.ForeColor = Color.Red;
            lblFleetMakeInvalid.Text = "!";
            lblFleetMakeInvalid.ForeColor = Color.Red;
            lblFleetModelInvalid.Text = "!";
            lblFleetModelInvalid.ForeColor = Color.Red;
            lblFleetClassInvalid.Text = "!";
            lblFleetClassInvalid.ForeColor = Color.Red;
            lblFleetYearInvalid.Text = "!";
            lblFleetYearInvalid.ForeColor = Color.Red;
        }

        /// <summary>
        /// 
        /// Event handler run when the user clicks the add button to submit a new Customer.
        /// 
        /// This results in each of the error tags being set to red "!" to symbolise that
        /// it is a mandatory field and must be verified before submit is allowed.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnCustomerAdd_Click(object sender, EventArgs e)
        {
            int lastRow = dgvCustomer.Rows.Count - 1;

            // Get the last ID number
            int currentID = (Int32)(dgvCustomer.Rows[lastRow].Cells[0].Value);
            currentID++;
            string currentStringID = currentID.ToString();
            txtCustomerID.Text = currentStringID;

            gbCustomerAddCustomer.Text = "Add Customer";

            txtCustomerID.Enabled = false; // ID is unique
            gbCustomerAddCustomer.Visible = true;
            btnCustomerCancel.Enabled = true;
            btnCustomerSubmit.Enabled = false;
            btnCustomerRemove.Enabled = false;
            btnCustomerModify.Enabled = false;
            btnCustomerAdd.Enabled = true;

            // Setting the invalid labels to visible            
            lblCustomerTitleInvalid.Text = "!";
            lblCustomerTitleInvalid.ForeColor = Color.Red;
            lblCustomerFirstnameInvalid.Text = "!";
            lblCustomerFirstnameInvalid.ForeColor = Color.Red;
            lblCustomerLastnameInvalid.Text = "!";
            lblCustomerLastnameInvalid.ForeColor = Color.Red;
            lblCustomerGenderInvalid.Text = "!";
            lblCustomerGenderInvalid.ForeColor = Color.Red;
            lblCustomerDOBInvalid.Text = "!";
            lblCustomerDOBInvalid.ForeColor = Color.Red;
        }

        /// <summary>
        /// Clears all of the attributes in the group box for Vehicle
        /// </summary>

        private void ClearFleetGroupBoXAttributes()
        {
            txtFleetRego.Clear();
            txtFleetMake.Clear();
            txtFleetModel.Clear();
            txtFleetYear.Clear();
            cmbFleetClass.SelectedIndex = -1;
            nudFleetSeats.ResetText();
            cmbFleetTransmission.SelectedIndex = 0;
            cmbFleetFuel.SelectedIndex = 0;
            cbFleetGPS.Checked = false;
            cbFleetSunroof.Checked = false;
            nudFleetDailyRate.ResetText();
            txtFleetColour.Clear();
        }

        /// <summary>
        /// Clears all of the attributes in the group box for Customer
        /// </summary>

        private void ClearCustomerGroupBoxAttributes()
        {
            txtCustomerID.Clear();
            cmbCustomerTitle.SelectedIndex = -1;
            txtCustomerFirstname.Clear();
            txtCustomerLastname.Clear();
            cmbCustomerGender.SelectedIndex = -1;
            txtCustomerDOB.Clear();
        }

        /// <summary>
        /// Clears all of the attributes in the group box for Customer
        /// </summary>

        private void ClearServiceGroupBoxAttributes()
        {
            cbServiceEngine.Checked = false;
            cbServiceTransmission.Checked = false;
            cbServiceTires.Checked = false;
            txtServiceDate.Clear();
            txtServiceMileage.Clear();
        }

        /// <summary>
        /// 
        /// Checks to see if the text for all of the compulsary fields is set to nothing and if so
        /// returns true. Since the Vehicle is now verified for submission
        /// 
        /// </summary>
        /// <returns> true or false depending on whether the inputs are valid </returns>

        public bool ValidInputsFleet()
        {
            if (lblFleetRegoInvalid.Text == "" && lblFleetMakeInvalid.Text == "" && lblFleetModelInvalid.Text == ""
                && lblFleetClassInvalid.Text == "" && lblFleetYearInvalid.Text == "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// Checks to see if the text for all of the compulsary fields is set to nothing and if so
        /// returns true. Since the Vehicle is now verified for submission
        /// 
        /// </summary>
        /// <returns> true or false depending on whether the inputs are valid </returns>

        public bool ValidInputsCustomer()
        {
            if (lblCustomerTitleInvalid.Text == "" && lblCustomerFirstnameInvalid.Text == "" && lblCustomerLastnameInvalid.Text == ""
                && lblCustomerGenderInvalid.Text == "" && lblCustomerDOBInvalid.Text == "")
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 
        /// Checks to see if the text for all of the compulsary fields is set to nothing and if so
        /// returns true. Since the Vehicle is now verified for submission
        /// 
        /// </summary>
        /// <returns> true or false depending on whether the inputs are valid </returns>

        public bool ValidInputsService()
        {
            if (lblServiceDateInvalid.Text == "" && lblServiceMileageInvalid.Text == "")
            {
                return true;
            }

            return false;
        }


        /// 
        /// Event handler that is run when a user clicks to submit a Vehicle into the Fleet.
        /// 
        /// This will then check whether the entered registration number is present in the fleet and whether
        /// it is a valid input for 3 integers followed by 3 capital letters. Also checks whether the cosntraint of
        /// Vehicle class being Economy and Transmission being manual and if this is violated will show a warning that
        /// the entry is invalid. Allowing for the user to change their inputs.
        /// 
        /// If valid entry then the Vehicle will be added to the Fleet and the Fleet dgv will be updated accordingly. 
        /// 
        /// The sbumit button is also used for submitting modifications and hence if the Vehicle is present in the Fleet
        /// it will run the modification code and update all of the attribute fields updating the Fleet accordingly.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnFleetSubmit_Click(object sender, EventArgs e)
        {
            Vehicle vehicle;
            int year;
            int numSeats;
            int dailyRate;
            bool GPS;
            bool sunroof;

            Vehicle.VehicleClass vehicleClass;
            Vehicle.FuelType fuelType;
            Vehicle.TransmissionType transmissionType;

            btnFleetRemove.Enabled = true;
            btnFleetModify.Enabled = true;
            btnFleetAdd.Enabled = true;

            // Check to see if the rego is rented
            if (!currentFleet.PresentInFleet(txtFleetRego.Text) && gbFleetAddVehicle.Text == "Add Vehicle" && ValidInputsFleet())
            {
                if (cmbFleetClass.SelectedValue.ToString() == "Economy" && cmbFleetTransmission.SelectedValue.ToString() == "Manual")
                {
                    // Constraint has been violated
                    DialogResult drConstraintViolated = MessageBox.Show(String.Format("Economy class vehicles can not have manual transmission"),
                    "Input constraint violated", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string vehicleRego = txtFleetRego.Text;
                    string vehicleMake = txtFleetMake.Text;
                    string vehicleModel = txtFleetModel.Text;
                    Enum.TryParse(cmbFleetClass.SelectedValue.ToString(), out vehicleClass);
                    int.TryParse(txtFleetYear.Text, out year);
                    int.TryParse(nudFleetSeats.Text, out numSeats);
                    Enum.TryParse(cmbFleetTransmission.SelectedValue.ToString(), out transmissionType);
                    Enum.TryParse(cmbFleetFuel.SelectedValue.ToString(), out fuelType);

                    if (cbFleetGPS.Checked == true)
                    {
                        GPS = true;
                    }
                    else
                    {
                        GPS = false;
                    }

                    if (cbFleetSunroof.Checked == true)
                    {
                        sunroof = true;
                    }
                    else
                    {
                        sunroof = false;
                    }

                    int.TryParse(nudFleetDailyRate.Text, out dailyRate);
                    string colour = txtFleetColour.Text;

                    // Add vehicles to the fleet
                    gbFleetAddVehicle.Visible = false;
                    vehicle = new Vehicle(vehicleRego, vehicleMake, vehicleModel, year, vehicleClass, numSeats, transmissionType, fuelType, GPS, sunroof, colour, dailyRate);
                    currentFleet.AddVehicle(vehicle);
                    SetupcmbServiceReportCarName();
                    currentFleet.SaveToFile();

                    // Clear all entries
                    ClearFleetGroupBoXAttributes();
                }
            }
            else if (currentFleet.PresentInFleet(txtFleetRego.Text) && gbFleetAddVehicle.Text == "Add Vehicle" && ValidInputsFleet())
            {
                // Registration is not unique
                DialogResult drRegoIsNotUnqiue = MessageBox.Show(String.Format("Vehicle Rego is already in the Fleet"),
                "Incorrect vehicles input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Update the vehicles
                int rowIndex = dgvFleet.CurrentCell.RowIndex;
                currentFleet.GetFleet()[rowIndex].Make = txtFleetMake.Text;
                currentFleet.GetFleet()[rowIndex].Model = txtFleetModel.Text;
                currentFleet.GetFleet()[rowIndex].Year = Int32.Parse(txtFleetYear.Text);
                currentFleet.GetFleet()[rowIndex].Vehicle_Class = (Vehicle.VehicleClass)cmbFleetClass.SelectedIndex;
                currentFleet.GetFleet()[rowIndex].NumSeats = Int32.Parse(nudFleetSeats.Value.ToString());
                currentFleet.GetFleet()[rowIndex].Fuel_Type = (Vehicle.FuelType)cmbFleetFuel.SelectedIndex;
                currentFleet.GetFleet()[rowIndex].Transmission_Type = (Vehicle.TransmissionType)cmbFleetTransmission.SelectedIndex;
                currentFleet.GetFleet()[rowIndex].GPS = cbFleetGPS.Checked;
                currentFleet.GetFleet()[rowIndex].SunRoof = cbFleetSunroof.Checked;
                currentFleet.GetFleet()[rowIndex].DailyRate = double.Parse(nudFleetDailyRate.Value.ToString());
                currentFleet.GetFleet()[rowIndex].Colour = txtFleetColour.Text;
                SetupcmbServiceReportCarName();
                currentFleet.SaveToFile();
                gbFleetAddVehicle.Visible = false;
                btnFleetAdd.Enabled = true;

                // Clear all entries
                ClearFleetGroupBoXAttributes();
            }

            PopulateDataGridViewFleet();
        }

        /// <summary>
        /// 
        /// Event handler that controls when the user clicks submit to adding or modifying a Customer.
        /// 
        /// This method checks whether the Customer is valid by checking that all fields have been filled
        /// out by the user. If not the submit button will not be visible to the user. 
        /// 
        /// If the user is modifying an existing Customer then all attribtues will be updated and saved 
        /// accordingly
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnCustomerSubmit_Click(object sender, EventArgs e)
        {
            Customer customer;
            Customer.Gender customerGender;

            btnCustomerSubmit.Enabled = true;
            btnCustomerRemove.Enabled = true;
            btnCustomerModify.Enabled = true;

            if (gbCustomerAddCustomer.Text == "Add Customer" && ValidInputsCustomer())
            {
                int customerID = Int32.Parse(txtCustomerID.Text);
                string title = (String)(cmbCustomerTitle.SelectedItem.ToString());
                string firstName = txtCustomerFirstname.Text;
                string lastName = txtCustomerLastname.Text;
                Enum.TryParse(cmbCustomerGender.SelectedItem.ToString(), out customerGender);
                string DOB = txtCustomerDOB.Text;

                // Add Customer to the fleet
                gbCustomerAddCustomer.Visible = false;
                customer = new Customer(customerID, title, firstName, lastName, customerGender, DOB);
                currentCustomer.AddCustomer(customer);
                SetupcmbSearchCusomter();
                currentCustomer.SaveToFile();

                // Clear all entries
                ClearCustomerGroupBoxAttributes();
            }
            else
            {
                // Update the Customer
                int rowIndex = dgvCustomer.CurrentCell.RowIndex;
                currentCustomer.GetCustomers()[rowIndex].Title = (String)(cmbCustomerTitle.SelectedItem.ToString());
                currentCustomer.GetCustomers()[rowIndex].FirstName = txtCustomerFirstname.Text;
                currentCustomer.GetCustomers()[rowIndex].LastName = txtCustomerLastname.Text;
                currentCustomer.GetCustomers()[rowIndex].Gender_ = (Customer.Gender)cmbCustomerGender.SelectedIndex;
                currentCustomer.GetCustomers()[rowIndex].DateOfBirth = txtCustomerDOB.Text;
                currentCustomer.SaveToFile();
                gbCustomerAddCustomer.Visible = false;
                btnCustomerAdd.Enabled = true;
                SetupcmbSearchCusomter();
                currentCustomer.SaveToFile();

                // Clear all entries
                ClearCustomerGroupBoxAttributes();
            }

            PopulateDataGridViewCustomer();
        }

        /// <summary>
        /// Event Hanlder is triggered each time the user changes text in the registration field. This method
        /// checks whether the registration is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtFleetRego_TextChanged(object sender, EventArgs e)
        {
            int validInput = 0;

            if (txtFleetRego.Text.Length == 9 || txtFleetRego.Text.Length == 8)
            {
                for (int i = 0; i < txtFleetRego.Text.Length; i++)
                {
                    // Test the first 2 elements
                    if (System.Char.IsDigit(txtFleetRego.Text[i]) && i < 2)
                    {
                        validInput++;
                    }
                    // Test the next element
                    if (System.Char.IsUpper(txtFleetRego.Text[i]) && i == 2)
                    {
                        validInput++;
                    }
                    // Test the next element
                    if (txtFleetRego.Text[i] == '-' && i == 3)
                    {
                        validInput++;
                    }
                    // Test the next 5 elements
                    if (System.Char.IsDigit(txtFleetRego.Text[i]) && i > 3)
                    {
                        validInput++;
                    }
                }
            }

            // Rego is valid and unique 
            if (((validInput == 8 || validInput == 9) && !currentFleet.PresentInFleet(txtFleetRego.Text)) || txtFleetRego.Enabled == false)
            {
                lblFleetRegoInvalid.Text = "";
            }
            else if ((validInput == 8 || validInput == 9) && currentFleet.PresentInFleet(txtFleetRego.Text))
            {
                lblFleetRegoInvalid.Text = "!";
                lblFleetRegoInvalid.ForeColor = Color.Red;
            }
            else
            {
                lblFleetRegoInvalid.Text = "!";
                lblFleetRegoInvalid.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event Handler that's triggered each time a user edit the text in the Make field.
        /// This tests whether the field is empty or not. If empty then the field is invalid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtFleetMake_TextChanged(object sender, EventArgs e)
        {
            if (txtFleetMake.Text != "")
            {
                lblFleetMakeInvalid.Text = "";
            }
            else
            {
                lblFleetMakeInvalid.Text = "!";
                lblFleetMakeInvalid.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event Handler that's triggered each time a user edit the text in the Model field.
        /// This tests whether the field is empty or not. If empty then the field is invalid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtFleetModel_TextChanged(object sender, EventArgs e)
        {
            if (txtFleetMake.Text != "")
            {
                lblFleetModelInvalid.Text = "";
            }
            else
            {
                lblFleetModelInvalid.Text = "!";
                lblFleetModelInvalid.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event Handler that's triggered each time a user edit the text in the Year field.
        /// This tests whether the field consists of 4 integers between the limits of 1800 to 2018. 
        /// If not then the input is considered invalid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtFleetYear_TextChanged(object sender, EventArgs e)
        {
            int maxYearInput = 2020;
            int minYearInput = 1990;
            int lengthOfYear = 4;
            int validInput = 0;
            bool validYear;
            int year = 0;

            if (txtFleetYear.Text.Length == lengthOfYear)
            {
                validYear = Int32.TryParse(txtFleetYear.Text, out year);

                for (int i = 0; i < lengthOfYear; i++)
                {
                    if (System.Char.IsDigit(txtFleetYear.Text[i]) && validYear)
                    {
                        validInput++;
                    }
                }
            }

            if (validInput == lengthOfYear && year >= minYearInput && year <= maxYearInput)
            {
                lblFleetYearInvalid.Text = "";
            }
            else
            {
                lblFleetYearInvalid.Text = "!";
                lblFleetYearInvalid.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event Handler that is triggered each time a user changes the item in a combobox. 
        /// This will be valid as long as the user picks an item that is not null. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmbFleetClass_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbFleetClass.SelectedValue != null)
            {
                lblFleetClassInvalid.Text = "";
            }
        }

        /// <summary>
        /// 
        /// Event Handler that is triggered each time the mouse moves within the Fleet group box 
        /// of adding or modifying a Vehicle.
        /// 
        /// This is used to check whether each of the compulsary fields for the class are valid. If
        /// there are no "!" present then the submit button is enabled and the user may submit the change.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void gbFleetAddVehicle_MouseHover(object sender, EventArgs e)
        {
            if (lblFleetClassInvalid.Text == "" && lblFleetRegoInvalid.Text == "" && lblFleetYearInvalid.Text == "" &&
            lblFleetMakeInvalid.Text == "" && lblFleetModelInvalid.Text == "")
            {
                btnFleetSubmit.Enabled = true;
            }
            else
            {
                btnFleetSubmit.Enabled = false;
            }
        }

        /// <summary>
        /// Event Handler that is triggered each time a user edits the DOB field in the Customer view.
        /// The validity of the input is monitored using system.DateTime.TryParse 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtCustomerDOB_TextChanged(object sender, EventArgs e)
        {
            DateTime DOB;
            if (DateTime.TryParse(txtCustomerDOB.Text, out DOB))
            {
                lblCustomerDOBInvalid.Text = "";
            }
            else
            {
                lblCustomerDOBInvalid.Text = "!";
                lblCustomerDOBInvalid.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event Handler that is triggered each time a user edits the Firstname field in the Customer view.
        /// This is valid if the text in this field is not empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtCustomerFirstname_TextChanged(object sender, EventArgs e)
        {
            if (txtCustomerFirstname.Text != "")
            {
                lblCustomerFirstnameInvalid.Text = "";
            }
            else
            {
                lblCustomerFirstnameInvalid.Text = "!";
                lblCustomerFirstnameInvalid.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Event Handler that is triggered each time a user edits the Lastname field in the Customer view.
        /// This is valid if the text in this field is not empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtCustomerLastname_TextChanged(object sender, EventArgs e)
        {
            if (txtCustomerLastname.Text != "")
            {
                lblCustomerLastnameInvalid.Text = "";
            }
            else
            {
                lblCustomerLastnameInvalid.Text = "!";
                lblCustomerLastnameInvalid.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// 
        /// Event Handler that is triggered each time the mouse moves within the Customer group box 
        /// of adding or modifying a Vehicle.
        /// 
        /// This is used to check whether each of the compulsary fields for the class are valid. If
        /// there are no "!" present then the submit button is enabled and the user may submit the change.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void gbCustomerAddCustomer_MouseHover(object sender, EventArgs e)
        {
            if (lblCustomerTitleInvalid.Text == "" && lblCustomerFirstnameInvalid.Text == "" && lblCustomerLastnameInvalid.Text == "" &&
            lblCustomerGenderInvalid.Text == "" && lblCustomerDOBInvalid.Text == "")
            {
                btnCustomerSubmit.Enabled = true;
            }
            else
            {
                btnCustomerSubmit.Enabled = false;
            }
        }

        /// <summary>
        /// Event Handler that is triggered each time a user changes the item in a combobox. 
        /// This will be valid as long as the user picks an item that is not null. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmbServiceReportCarName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbCustomerTitle.SelectedItem != null)
            {
                lblCustomerTitleInvalid.Text = "";
            }
        }

        /// <summary>
        /// Event Handler that is triggered each time a user changes the item in a combobox. 
        /// This will be valid as long as the user picks an item that is not null. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmbCustomerGender_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbCustomerGender.SelectedItem != null)
            {
                lblCustomerGenderInvalid.Text = "";
            }
        }

        /// <summary>
        /// This event-handler is called when the user presses search button after entering the query.
        /// The search button can handle any advanced search query, and it is not case sensitive. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSearchSearch_Click(object sender, EventArgs e)
        {
            current_search = new Search();
            string query = cmbSearchEnterQueryHere.Text.ToLower();
            List<string> queryList = current_search.getQueryAttributes(query);
            List<string> operatorList = current_search.getOperaterList(query);
            List<List<Vehicle>> eachAttributeSearch = current_search.eachattributesearch(queryList,
                (int)nudSearchDailyCostRangeLower.Value, (int)nudSearchDailyCostRangeUpper.Value);
            List<Vehicle> final_result = current_search.searchoutput(eachAttributeSearch, operatorList);
            dgvSearch.Rows.Clear();
            dgvSearch.Visible = true;
            gbSearchCreateRental.Enabled = true;

            foreach (Vehicle x in final_result)
            {
                dgvSearch.Rows.Add(x.VehicleRego, x.Model, x.Make, x.Year, x.Vehicle_Class, x.NumSeats, x.Transmission_Type, x.Fuel_Type, x.GPS, x.SunRoof, x.DailyRate, x.Colour);
            }

            if (final_result.Count == 0)
            {
                lblSearchNoMatchingVehicleFound.Visible = true;
                dgvSearch.Visible = false;
            }
        }

        /// <summary>
        /// This event-handler enables the search button when user begins to type something on the query textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSearchEnterQueryHere_TextChanged(object sender, EventArgs e)
        {
            btnSearchSearch.Enabled = true;
        }

        /// <summary>
        ///  When 'Show All' button is clicked, it will display all of the current available fleets 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchShowAll_Click(object sender, EventArgs e)
        {
            current_search = new Search();
            dgvSearch.Rows.Clear();
            List<Vehicle> result = currentFleet.GetFleet();
            result = result.Where(x => (((int)nudSearchDailyCostRangeLower.Value <= x.DailyRate) && (x.DailyRate <= (int)nudSearchDailyCostRangeUpper.Value))).ToList();
            foreach (Vehicle x in result)
            {
                dgvSearch.Rows.Add(x.VehicleRego, x.Model, x.Make, x.Year, x.Vehicle_Class, x.NumSeats, x.Transmission_Type, x.Fuel_Type, x.GPS, x.SunRoof, x.DailyRate, x.Colour);
            }
            dgvSearch.Visible = true;
            gbSearchCreateRental.Enabled = true;
        }

        /// <summary>
        /// This event-handler wipes previously typed query so that user can type again 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void cmbSearchEnterQueryHere_Click(object sender, EventArgs e)
        {
            cmbSearchEnterQueryHere.Text = "";
        }

        /// <summary>
        /// This method is called when the user changes rental duration. It also checks whether the user 
        /// has selected a vehicles shown in search result. If yes, then it enables rent button which allows 
        /// user to then rent the car
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudSearchRentalDuration_ValueChanged(object sender, EventArgs e)
        {
            double totalCost = (double)dgvSearch.SelectedRows[0].Cells[10].Value * (int)nudSearchRentalDuration.Value;
            lblSearchTotalCost.Text = String.Format("Total Cost: ${0}", totalCost);
            if (dgvSearch.SelectedRows.Count > 0 && cmbSearchCustomer.SelectedItem != null && (int)nudSearchRentalDuration.Value > 0)
            {
                btnSearchRent.Enabled = true;
            }
        }

        /// <summary>
        /// This event-handler controls what will happen if rent button has been clicked.
        /// It will first ask the customer if they really want to rent it by displaying the total 
        /// cost, selected vehicles and selected customer.
        /// If customer proceeds by clicking yes, then it will rent the car. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchRent_Click(object sender, EventArgs e)
        {
            int customerID;
            var comboboxMessage = (string)cmbSearchCustomer.SelectedItem;
            customerID = int.Parse(comboboxMessage.Split(' ')[0]);
            string rego = (string)dgvSearch.SelectedRows[0].Cells[0].Value;
            double totalCost = (double)dgvSearch.SelectedRows[0].Cells[10].Value * (int)nudSearchRentalDuration.Value;

            DialogResult dialogResult = MessageBox.Show(String.Format("Do you want to rent vehicles {0} to  customer {1} for a total cost of ${2}?",
                                        rego, comboboxMessage, totalCost), "Rental Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool success = currentFleet.RentCar(rego, customerID);
                if (success)
                {
                    DialogResult dialogResult2 = MessageBox.Show("Successful");
                    PopulateDataGridViewReport();
                }
                else
                {
                    DialogResult dialogResult2 = MessageBox.Show("Unsuccessful");
                }
            }
        }

        /// <summary>
        /// This event handler checks whether a vehicles from report section has been clicked.
        /// If yes, it enables return button which allows the customer to return the car. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowsCount = dgvReport.SelectedRows.Count;

            if (rowsCount == 1)
            {
                btnReportReturnVehicle.Enabled = true;
            }
        }

        /// <summary>
        /// if the return button has been checked, it confirms with the user whether they really want to 
        /// return the vehicles. If yes, it returns the car to the fleet. Depending on the outcome of the 
        /// return action, it will display the success/unsuccess message accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReportReturnVehicle_Click(object sender, EventArgs e)
        {
            string rego = (string)dgvReport.SelectedRows[0].Cells[0].Value;
            DialogResult dialogResult = MessageBox.Show(String.Format("Do you want return rego number {0}", rego), "Return Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int outcome = currentFleet.ReturnCar(rego);
                if (outcome == -1)
                {
                    DialogResult dialogResult2 = MessageBox.Show("UnSuccessful");
                }
                else
                {
                    DialogResult dialogResult2 = MessageBox.Show(string.Format("Successfully returned CustomerID: {0}'s rental car", outcome));
                }
            }
            PopulateDataGridViewReport();
        }

        /// <summary>
        ///  this event handler checks once again if the selection of the vehicles in the report section 
        /// has been changed. if a vehicles is no longer selected, it hides the return button. 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReport_SelectionChanged(object sender, EventArgs e)
        {
            int rowsCount = dgvReport.SelectedRows.Count;
            if (rowsCount != 1)
            {
                btnReportReturnVehicle.Enabled = false;
            }
        }

        /// <summary>
        /// this method is called if user clicks the 'No Matching vehicles Found' message. 
        /// It gives further instruction on how to properly enter the search query.
        /// It is used as an information display for the user. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSearchNoMatchingVehicleFound_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("A combination of any number of attributes, using both" +
                "AND and OR, where the operators AND and OR should have the same priority(precedence) - E.g.: " +
                "Economy OR Family AND 4 - Cylinders " +
                "Red OR Blue OR Green OR Purple AND GPS", "Search Instruction");
        }

        /// <summary>
        /// 
        /// Event handler triggered when the user closes the Windows Form application. A dialog result box then queries
        /// whether the user wishes to save the changes that they have made. Yes will save these changes and update the
        /// .csv files accordingly whereas no will not change the .csv files and activity will not be saves.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult drSaveAllChangedData = MessageBox.Show("Would you like to save changes any changes you've made?", "Exit", MessageBoxButtons.YesNo);
            if (drSaveAllChangedData == DialogResult.Yes)
            {
                // Save all user activity
                currentFleet.SaveToFile();
                currentCustomer.SaveToFile();
                currentService.SaveToFile();
            }
        }

        private void gbSearchCreateRental_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbSearchCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbFleetClass_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cmbServiceReportCarName_TextChanged(object sender, EventArgs e)
        {
            btnServiceReportSearch.Enabled = true;
        }

        private void btnServiceReportSearch_Click(object sender, EventArgs e)
        {
            current_search_report = new SearchServiceReport();
            string comboboxMessage = (string)cmbServiceReportCarName.SelectedItem;
            string carRego = comboboxMessage.Split(' ')[2];
            List<Service> report = new List<Service>();
            report = current_search_report.SearchReportOutput(carRego);
            dgvServiceReport.Rows.Clear();
            foreach (Service x in report)
            {
                dgvServiceReport.Rows.Add(x.vehicles[0].VehicleRego, x.vehicles[0].Make, x.vehicles[0].Model, x.vehicles[0].Year, x.isServiceEngine, x.isServiceTransmission, x.isServiceTires, x.date, x.mileAge, x.cost, x.garage);
                Console.Write(x.vehicles[0].VehicleRego);

            }
            dgvServiceReport.Visible = true;

        }

        private void btnServiceSearch_Click(object sender, EventArgs e)
        {
            current_search = new Search();
            string query = cmbServiceEnterQueryHere.Text.ToLower();
            List<string> queryList = current_search.getQueryAttributes(query);
            List<string> operatorList = current_search.getOperaterList(query);
            List<List<Vehicle>> eachAttributeSearch = current_search.eachattributesearch(queryList);
            List<Vehicle> final_result = current_search.searchoutput(eachAttributeSearch, operatorList);
            dgvService.Rows.Clear();
            dgvService.Visible = true;
            btnServiceMaintainanceThisCar.Enabled = true;

            foreach (Vehicle x in final_result)
            {
                dgvService.Rows.Add(x.VehicleRego, x.Model, x.Make, x.Year, x.Vehicle_Class, x.NumSeats, x.Transmission_Type, x.Fuel_Type, x.GPS, x.SunRoof, x.DailyRate, x.Colour);
            }

            if (final_result.Count == 0)
            {
                lblServiceNoMatchingVehicleFound.Visible = true;
                dgvService.Visible = false;
                btnServiceMaintainanceThisCar.Enabled = false;
            }
        }

        private void btnServiceShowAll_Click(object sender, EventArgs e)
        {
            current_search = new Search();
            dgvService.Rows.Clear();
            List<Vehicle> result = currentFleet.GetFleet();
            foreach (Vehicle x in result)
            {
                dgvService.Rows.Add(x.VehicleRego, x.Model, x.Make, x.Year, x.Vehicle_Class, x.NumSeats, x.Transmission_Type, x.Fuel_Type, x.GPS, x.SunRoof, x.DailyRate, x.Colour);
            }
            dgvService.Visible = true;
            btnServiceMaintainanceThisCar.Enabled = true;

        }

        private void cmbServiceEnterQueryHere_Click(object sender, EventArgs e)
        {
            cmbServiceEnterQueryHere.Text = "";
        }

        private void cmbServiceEnterQueryHere_TextChanged(object sender, EventArgs e)
        {
            btnServiceSearch.Enabled = true;
        }

        private void btnServiceMaintainanceThisCar_Click(object sender, EventArgs e)
        {
            btnServiceMaintainanceThisCar.Enabled = false;
            btnServiceMaintainanceFleet.Enabled = false;
            gbServiceMaintainance.Text = "Maintainance This Car";
            gbServiceMaintainance.Visible = true;
        }

        private void btnServiceMaintainanceFleet_Click(object sender, EventArgs e)
        {
            btnServiceMaintainanceThisCar.Enabled = false;
            btnServiceMaintainanceFleet.Enabled = false;
            gbServiceMaintainance.Text = "Maintainance Fleet";
            gbServiceMaintainance.Visible = true;
        }

        private void btnServiceSubmit_Click(object sender, EventArgs e)
        {
            if (gbServiceMaintainance.Text == "Maintainance Fleet" && ValidInputsService())
            {
                bool isEngine = cbServiceEngine.Checked;
                bool isTransmission = cbServiceTransmission.Checked;
                bool isTires = cbServiceTires.Checked;
                string date = txtServiceDate.Text;
                int distanceMeter = Int32.Parse(txtServiceMileage.Text);
                float cost = float.Parse(txtServiceCost.Text);
                string garage = txtServiceGarage.Text;
                gbServiceMaintainance.Visible = false;

                if (dgvService.Rows.Count != 1)
                {
                    btnServiceMaintainanceThisCar.Enabled = true;
                }
                btnServiceMaintainanceFleet.Enabled = true;

                string maintainceJob = "";
                if (isEngine)
                {
                    maintainceJob += "Engine";
                }
                if (isTransmission)
                {
                    if (isEngine && !isTires)
                    {
                        maintainceJob += " and Transmission";
                    }
                    else if (isEngine && isTires)
                    {
                        maintainceJob += ", Transmission";
                    }
                    else
                    {
                        maintainceJob += "Transmission";
                    }
                }
                if (isTires)
                {
                    if (isEngine || isTransmission)
                    {
                        maintainceJob += " and Tires";
                    }
                    else
                    {
                        maintainceJob += "Tires";
                    }
                }


                DialogResult submit = MessageBox.Show("Would you like to maintaince: " + maintainceJob + " for all car in fleet?", "Maintainance", MessageBoxButtons.YesNo);
                if (submit == DialogResult.Yes)
                {
                    Service service = new Service(currentFleet.fleet, isEngine, isTransmission, isTires, date, distanceMeter, cost, garage);
                    currentService = new ServiceReport();
                    currentService.AddService(service);
                    currentService.SaveToFile();
                    SetupcmbServiceReportCarName();
                }




                ClearServiceGroupBoxAttributes();
            }

            if (gbServiceMaintainance.Text == "Maintainance This Car" && ValidInputsService())
            {
                bool isEngine = cbServiceEngine.Checked;
                bool isTransmission = cbServiceTransmission.Checked;
                bool isTires = cbServiceTires.Checked;
                string date = txtServiceDate.Text;
                int distanceMeter = Int32.Parse(txtServiceMileage.Text);
                float cost = float.Parse(txtServiceCost.Text);
                string garage = txtServiceGarage.Text;

                List<Vehicle> vehicles = new List<Vehicle>();
                foreach (Vehicle x in currentFleet.fleet)
                {
                    if (x.VehicleRego == dgvService.SelectedRows[0].Cells[0].Value.ToString())
                    {
                        vehicles.Add(x);
                    }
                }

                gbServiceMaintainance.Visible = false;

                if (dgvService.Rows.Count != 1)
                {
                    btnServiceMaintainanceThisCar.Enabled = true;
                }
                btnServiceMaintainanceFleet.Enabled = true;

                string maintainceJob = "";
                if (isEngine)
                {
                    maintainceJob += "Engine";
                }
                if (isTransmission)
                {
                    if (isEngine && !isTires)
                    {
                        maintainceJob += " and Transmission";
                    }
                    else if (isEngine && isTires)
                    {
                        maintainceJob += ", Transmission";
                    }
                    else
                    {
                        maintainceJob += "Transmission";
                    }
                }
                if (isTires)
                {
                    if (isEngine || isTransmission)
                    {
                        maintainceJob += " and Tires";
                    }
                    else
                    {
                        maintainceJob += "Tires";
                    }
                }

                DialogResult submit = MessageBox.Show("Would you like to maintaince: " + maintainceJob + " for this car?", "Maintainance", MessageBoxButtons.YesNo);
                if (submit == DialogResult.Yes)
                {
                    Service service = new Service(vehicles, isEngine, isTransmission, isTires, date, distanceMeter, cost, garage);

                    currentService = new ServiceReport();
                    currentService.AddService(service);
                    currentService.SaveToFile();
                    SetupcmbServiceReportCarName();
                }


                ClearServiceGroupBoxAttributes();
            }

        }

        private void tab_control1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtServiceDate_TextChanged(object sender, EventArgs e)
        {
            DateTime date;
            if (DateTime.TryParse(txtServiceDate.Text, out date))
            {
                lblServiceDateInvalid.Text = "";
            }
            else
            {
                lblServiceDateInvalid.Text = "!";
                lblServiceDateInvalid.ForeColor = Color.Red;
            }
        }

        private void txtServiceDistanceMeter_TextChanged(object sender, EventArgs e)
        {
            bool IsValid = false;
            for (int i = 0; i < txtServiceMileage.Text.Length; i++)
            {
                if (System.Char.IsDigit(txtServiceMileage.Text[i]))
                {
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                    break;
                }
            }
            if (IsValid)
            {
                lblServiceMileageInvalid.Text = "";
            }
            else
            {
                lblServiceMileageInvalid.Text = "!";
                lblServiceMileageInvalid.ForeColor = Color.Red;
            }
        }

        private void btnServiceCancle_Click(object sender, EventArgs e)
        {
            gbServiceMaintainance.Visible = false;
            btnServiceMaintainanceFleet.Enabled = true;

            if (dgvService.Rows.Count != 1)
            {
                btnServiceMaintainanceThisCar.Enabled = true;
            }
            Console.WriteLine(dgvService.Rows.Count);

            // Clear all entries
            ClearServiceGroupBoxAttributes();
        }

        private void txtServiceCost_TextChanged(object sender, EventArgs e)
        {
            bool IsValid = false;
            for (int i = 0; i < txtServiceCost.Text.Length; i++)
            {
                if (System.Char.IsDigit(txtServiceCost.Text[i]))
                {
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                    break;
                }
            }
            if (IsValid)
            {
                lblServiceCostInvalid.Text = "";
            }
            else
            {
                lblServiceCostInvalid.Text = "!";
                lblServiceCostInvalid.ForeColor = Color.Red;
            }
        }
    }
}
