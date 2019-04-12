using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace Final_Assignment_PROG8145
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isAddNew = true;

        public ObservableCollection<Customer> Schedule { get; set; }

        public ObservableCollection<Customer> BookedSchedule { get; set; }

        public Customer customerEntry { get; set; }

        Appointments appointments = new Appointments();

        String[] serviceMaleList =
        {
            "Consultation",
            "Blowout",
            "Blowout/Iron Work",
            "Brucedale",
            "Hotheads Application",
            "Women's Haircut",
            "Men's Haircut",
            "Bang Trim",
            "Neck Trim",
            "Single Process Color Retouch",
            "Single Process Color",
            "Base Color",
            "Double Process Blonde Retouch",
            "Double Process Color",
            "Partial Foil",
            "Full Foil",
            "Face Framing Balayage",
            "Full Balayage",
            "Keratin Express",
            "Keratin Treatment",
            "Gloss Treatment/Toner",
            "Ultra Blonde",
            "Color Correction",
            "Conditioning Treatment",
            "Updo/Style",
            "Bridal Updto/Style",
            "Bridal Makeup Application",
            "Brow Wax",
            "Lip Wax",
            "Brow Color"
        };

        String[] providerList =
        {
            "Adrienne Mason",
            "Brittany Massey",
            "Danielle Bruce",
            "Emily Winders",
            "Mel VanderBom",
            "Jaime Lewis",
            "Samantha Hayes"
        };

        String[] locationList =
        {
            "788 Catamaugus Ave, Seattle City, WA",
            "567 Pasadena Ave, Tustin City, CA",
            "577 First St, Los Gatos City, CA",
            "24-A Avogadro Way, Remulade City, WA",
            "89 Madison St, Fremont City, CA",
            "679 Carson St, Portland City, OR"
        };

        String[] timeList =
        {
            "8AM",
            "9AM",
            "10AM",
            "11AM",
            "12PM",
            "2PM",
            "3PM",
            "4PM",
            "5PM",
            "6PM"
        };

        public MainWindow()
        {
            InitializeComponent();
            Schedule = new ObservableCollection<Customer>();
            fillDataCombobox();
            readFromXML();
            getTotalRecords();
            setDefaultInputControl();
            if (isAddNew) { btnAdd.Content = "Save"; } else { btnAdd.Content = "Add"; }
            DataContext = this;
        }

        private void displayTopRowDataGrid()
        {
            try
            {
                Customer customerGridView = (Customer)gridSchedule.SelectedItem;
                this.txtName.Text = customerGridView.Name;
                this.txtAge.Text = customerGridView.Age.ToString();
                this.txtPhone.Text = customerGridView.Phone;
                if (customerGridView.Gender.Equals("Male"))
                {
                    this.rdbMale.IsChecked = true;
                }
                else
                {
                    this.rdbFemale.IsChecked = true;
                }
                this.cbxServiceList.SelectedItem = customerGridView.Service;
                this.cbxProvider.SelectedItem = customerGridView.Provider;
                this.dpkDate.SelectedDate = DateTime.Parse(customerGridView.Date);
                this.cbxTime.SelectedItem = customerGridView.Time;
                this.cbxLocation.SelectedItem = customerGridView.Location;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            if (btnAdd.Content.Equals("Add"))
            {
                clearInputControl();
                setEditableInputControl();
                txtName.Focusable = true;
                Keyboard.Focus(txtName);
                btnAdd.Content = "Save";
            }
            else if (btnAdd.Content.Equals("Save"))
            {
                bool isValid = true;
                validInputControl(ref isValid);

                if (isValid && !Validation.GetHasError(txtName) &&
                   !Validation.GetHasError(txtAge) && !Validation.GetHasError(txtPhone))
                {
                    Int16 age = Int16.Parse(txtAge.Text.ToString());
                    string date = "";
                    DateTime? selectedDate = dpkDate.SelectedDate;
                    if (selectedDate.HasValue)
                    {
                        date = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    string g1 = rdbFemale.Content.ToString();
                    string gender = (Convert.ToBoolean(rdbMale.IsChecked) ? rdbMale.Content.ToString() : g1);

                    Customer newCustomer = new Customer();
                    newCustomer.Name = txtName.Text;
                    newCustomer.Age = age;
                    newCustomer.Phone = txtPhone.Text;
                    newCustomer.Gender = gender;
                    newCustomer.Service = serviceMaleList[cbxServiceList.SelectedIndex - 1];
                    newCustomer.Provider = providerList[cbxProvider.SelectedIndex - 1];
                    newCustomer.Date = date;
                    newCustomer.Time = timeList[cbxTime.SelectedIndex - 1];
                    newCustomer.Location = locationList[cbxLocation.SelectedIndex - 1];

                    Schedule.Insert(0, newCustomer);
                    appointments.AddItem(newCustomer);
                    gridSchedule.ItemsSource = (gridSchedule.ItemsSource != Schedule) ? Schedule : gridSchedule.ItemsSource;
                    clearInputControl();
                    getTotalRecords();
                    saveToXML();
                }
                else
                {
                    MessageBoxImage icon = MessageBoxImage.Question;
                    MessageBox.Show("Please complete and accurate information!\n" +
                        "Tip: Check the red fields and see the tooltips of each input box " +
                        "so that you can enter the information exactly.", 
                        "Notification", MessageBoxButton.OK ,icon);
                }
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (gridSchedule.SelectedIndex >= 0)
            {
                Customer customerGridView = (Customer)gridSchedule.SelectedItem;

                if (btnUpdate.Content.Equals("Edit"))
                {
                    setEditableInputControl();
                    btnUpdate.Content = "Update";
                }
                else if (btnUpdate.Content.Equals("Update"))
                {
                    updateDataGridRow(customerGridView);
                    saveToXML();
                    btnUpdate.Content = "Edit";
                }
            }
            else
            {
                MessageBox.Show("Please select a row to edit.", "Notification", MessageBoxButton.OK, MessageBoxImage.Question);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                Customer customerDelete = (Customer)gridSchedule.SelectedItem;

                string messageBoxText = $"Booked schedule of {customerDelete.Name} will be deleted. Are you sure?";
                string caption = "Delete Record";
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxResult messageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon);
                MessageBoxResult yesResult = MessageBoxResult.Yes;

                if (messageBoxResult == yesResult)
                {
                    int indexSelect = gridSchedule.SelectedIndex;
                    Schedule.Remove(customerDelete);
                    appointments.RemoveItem(customerDelete);
                    gridSchedule.ItemsSource = (gridSchedule.ItemsSource != Schedule) ? Schedule : gridSchedule.ItemsSource;
                    clearInputControl();
                    setViewOnlyInputControl();
                    MessageBox.Show("Delete successfully!", "Delete Record", MessageBoxButton.OK, MessageBoxImage.Information);
                    getTotalRecords();
                    saveToXML();
                    btnDelete.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                } 
            }
            catch (Exception)
            {
               
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterName = this.txtFilter.Text.ToUpper();
            if (filterName.Trim().Length > 0)
            {
                var query = Schedule.Where(s => s.Name.ToUpper().Contains(filterName));
                gridSchedule.ItemsSource = query;
            }
            else
            {
                gridSchedule.ItemsSource = (gridSchedule.ItemsSource != Schedule) ? Schedule : gridSchedule.ItemsSource;
            }
        }

        private void BtnSaveSchedule_Click(object sender, RoutedEventArgs e)
        {
            saveToXML();
            Console.WriteLine("Saving booked schedule to file");
            gridSchedule.ItemsSource = null;
            getTotalRecords();
        }

        private void BtnShowSchedule_Click(object sender, RoutedEventArgs e)
        {
            readFromXML();
            getTotalRecords();
        }

        /// <summary>
        /// The function displays detailed information of a row in the DataGrid, all information appears in the input control.
        /// </summary>
        private void getDataFromGridView()
        {
            try
            {
                Customer customerGridView = (Customer)gridSchedule.SelectedItem;
                this.txtName.Text = customerGridView.Name;
                this.txtAge.Text = customerGridView.Age.ToString();
                this.txtPhone.Text = customerGridView.Phone;
                if (customerGridView.Gender.Equals("Male"))
                {
                    this.rdbMale.IsChecked = true;
                }
                else
                {
                    this.rdbFemale.IsChecked = true;
                }
                this.cbxServiceList.SelectedItem = customerGridView.Service;
                this.cbxProvider.SelectedItem = customerGridView.Provider;
                this.dpkDate.SelectedDate = DateTime.Parse(customerGridView.Date);
                this.cbxTime.SelectedItem = customerGridView.Time;
                this.cbxLocation.SelectedItem = customerGridView.Location;
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// The function to set view only.
        /// </summary>
        private void setViewOnlyInputControl()
        {
            this.txtName.IsReadOnly = true;
            this.txtAge.IsReadOnly = true;
            this.txtPhone.IsReadOnly = true;
            this.rdbFemale.IsEnabled = false;
            this.rdbMale.IsEnabled = false;
            this.cbxServiceList.IsEnabled = false;
            this.cbxProvider.IsEnabled = false;
            this.dpkDate.IsEnabled = false;
            this.cbxTime.IsEnabled = false;
            this.cbxLocation.IsEnabled = false;
        }

        private void setEditableInputControl()
        {
            this.txtName.IsReadOnly = false;
            this.txtAge.IsReadOnly = false;
            this.txtPhone.IsReadOnly = false;
            this.rdbFemale.IsEnabled = true;
            this.rdbMale.IsEnabled = true;
            this.cbxServiceList.IsEnabled = true;
            this.cbxProvider.IsEnabled = true;
            this.dpkDate.IsEnabled = true;
            this.cbxTime.IsEnabled = true;
            this.cbxLocation.IsEnabled = true;
            this.txtName.Focusable = true;
            Keyboard.Focus(txtName);
        }

        private void saveToXML()
        {
            XmlWriterSettings writerSetting = new XmlWriterSettings();
            writerSetting.Indent = true;
            writerSetting.IndentChars = "\t";

            XmlWriter writer = XmlWriter.Create("HairSalon_Appointments.xml", writerSetting);
            writer.WriteStartDocument();
            writer.WriteStartElement("Appointments");

            for (int i = 0; i < appointments.Count; i++)
            {
                Customer customer = appointments[i];
                writer.WriteStartElement("Appointment");
                writer.WriteElementString("Name", customer.Name);
                writer.WriteElementString("Age", customer.Age.ToString());
                writer.WriteElementString("Phone", customer.Phone);
                writer.WriteElementString("Gender", customer.Gender);
                writer.WriteElementString("Service", customer.Service);
                writer.WriteElementString("Provider", customer.Provider);
                writer.WriteElementString("Date", customer.Date);
                writer.WriteElementString("Time", customer.Time);
                writer.WriteElementString("Location", customer.Location);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
        }

        private void readFromXML()
        {
            XmlReaderSettings readerSetting = new XmlReaderSettings();
            readerSetting.IgnoreWhitespace = true;
            readerSetting.IgnoreComments = true;

            XmlReader reader = XmlReader.Create("HairSalon_Appointments.xml", readerSetting);
            if (reader.ReadToDescendant("Appointment"))
            {
                do
                {
                    reader.ReadStartElement("Appointment");
                    String name = reader.ReadElementContentAsString();
                    Int16 age = Int16.Parse(reader.ReadElementContentAsString());
                    String phone = reader.ReadElementContentAsString();
                    String gender = reader.ReadElementContentAsString();
                    String service = reader.ReadElementContentAsString();
                    String provider = reader.ReadElementContentAsString();
                    String date = reader.ReadElementContentAsString();
                    String time = reader.ReadElementContentAsString();
                    String location = reader.ReadElementContentAsString();
                    Customer cus = new Customer(name, age, phone, gender, service, provider, date, time, location);
                    Schedule.Add(cus);
                    appointments.AddItem(cus);
                } while (reader.ReadToNextSibling("Appointment"));
            }
            reader.Close();
            gridSchedule.ItemsSource = Schedule;
        }

        private void fillDataCombobox()
        {
            for (int i = 0; i < serviceMaleList.Length; i++)
            {
                cbxServiceList.Items.Add(serviceMaleList[i]);
            }
            for (int i = 0; i < timeList.Length; i++)
            {
                cbxTime.Items.Add(timeList[i]);
            }
            for (int i = 0; i < providerList.Length; i++)
            {
                cbxProvider.Items.Add(providerList[i]);
            }
            for (int i = 0; i < locationList.Length; i++)
            {
                cbxLocation.Items.Add(locationList[i]);
            }
        }

        private void clearInputControl()
        {
            this.txtName.Clear();
            this.txtName.Foreground = Brushes.Black;
            this.txtAge.Text = "";
            this.txtAge.Foreground = Brushes.Black;
            this.txtPhone.Text = "";
            this.txtPhone.Foreground = Brushes.Black;
            this.rdbMale.IsChecked = true;
            this.cbxServiceList.SelectedIndex = 0;
            this.cbxServiceList.Foreground = Brushes.Black;
            this.cbxProvider.SelectedIndex = 0;
            this.cbxProvider.Foreground = Brushes.Black;
            this.cbxTime.SelectedIndex = 0;
            this.cbxTime.Foreground = Brushes.Black;
            this.cbxLocation.SelectedIndex = 0;
            this.cbxLocation.Foreground = Brushes.Black;
        }

        private void setDefaultInputControl()
        {
            this.txtName.Foreground = Brushes.Black;
            this.txtAge.Foreground = Brushes.Black;
            this.txtPhone.Foreground = Brushes.Black;
            this.rdbMale.IsChecked = true;
            this.cbxServiceList.Foreground = Brushes.Black;
            this.cbxProvider.Foreground = Brushes.Black;
            this.cbxTime.Foreground = Brushes.Black;
            this.cbxLocation.Foreground = Brushes.Black;
            this.btnUpdate.IsEnabled = false;
            this.btnDelete.IsEnabled = false;
        }

        private void validInputControl(ref bool isValid)
        {
            if (txtName.Text.Trim().Length == 0)
            {
                isValid = false;
                txtName.BorderBrush = Brushes.Red;
            } else
            {
                txtName.BorderBrush = null;
            }
            if (txtAge.Text.Trim().Length == 0)
            {
                isValid = false;
                txtAge.BorderBrush = Brushes.Red;
            } else
            {
                txtAge.BorderBrush = null;
            }
            if (txtPhone.Text.Trim().Length == 0)
            {
                isValid = false;
                txtPhone.BorderBrush = Brushes.Red;
            } else
            {
                txtPhone.BorderBrush = null;
            }
            if (cbxServiceList.SelectedIndex < 1)
            {
                cbxServiceList.Foreground = Brushes.Red;
                isValid = false;
            }
            else
            {
                cbxServiceList.Foreground = Brushes.Black;
            }

            if (cbxProvider.SelectedIndex < 1)
            {
                cbxProvider.Foreground = Brushes.Red;
                isValid = false;
            }
            else
            {
                cbxProvider.Foreground = Brushes.Black;
            }
            if(!dpkDate.SelectedDate.HasValue)
            {
                dpkDate.Foreground = Brushes.Red;
            } else
            {
                dpkDate.Foreground = Brushes.Black;
            }
            if (cbxTime.SelectedIndex < 1)
            {
                cbxTime.Foreground = Brushes.Red;
                isValid = false;
            }
            else
            {
                cbxTime.Foreground = Brushes.Black;
            }

            if (cbxLocation.SelectedIndex < 1)
            {
                cbxLocation.Foreground = Brushes.Red;
                isValid = false;
            }
            else
            {
                cbxLocation.Foreground = Brushes.Black;
            }


        }

        private void GridSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getDataFromGridView();
            setViewOnlyInputControl();
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            isAddNew = false;
            btnAdd.Content = "Add";
            btnUpdate.Content = "Edit";
        }

        /// <summary>
        /// The function updates a row in the DataGrid when a row is selected
        /// </summary>
        private void updateDataGridRow(Customer customerGridView)
        {
            string name = this.txtName.Text;
            Int16 age = Int16.Parse(txtAge.Text.ToString());
            string date = "";
            DateTime? dateValue = dpkDate.SelectedDate;
            if (dateValue.HasValue)
            {
                date = dateValue.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            string g1 = rdbFemale.Content.ToString();
            string g2 = (Convert.ToBoolean(rdbMale.IsChecked) ? rdbMale.Content.ToString() : g1);
            string gender = g2;

            string phone = this.txtPhone.Text;
            string service = this.cbxServiceList.SelectedValue.ToString();
            string provider = this.cbxProvider.SelectedValue.ToString();
            string time = this.cbxTime.SelectedValue.ToString();
            string location = this.cbxLocation.SelectedValue.ToString();
            Customer customerToUpdate = new Customer(name, age, phone, gender, service, provider, date, time, location);

            // Update data in Schedulem, appointments, gridSchedule.
            int indexSelect = gridSchedule.SelectedIndex;
            Schedule.RemoveAt(indexSelect);
            Schedule.Insert(indexSelect, customerToUpdate);
            appointments.RemoveItem(customerGridView);
            appointments.AddItem(customerToUpdate);
            gridSchedule.ItemsSource = (gridSchedule.ItemsSource != Schedule) ? Schedule : gridSchedule.ItemsSource;
            gridSchedule.CurrentItem = gridSchedule.Items[indexSelect];
            MessageBox.Show("Updated successfully!", "Update Records", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        /// <summary>
        /// The function returns the number of all records in the DataGrid
        /// </summary>
        private void getTotalRecords()
        {
            int totalRecords = this.gridSchedule.Items.Count;
            this.lblTotalRecords.Content = totalRecords.ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string messageBoxText = "This will close down the whole application. Do you want to save changes to Booked Schedule?";
            string caption = "Close Application";
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxButton button = MessageBoxButton.YesNoCancel;

            MessageBoxResult messageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon);

            MessageBoxResult yesResult = MessageBoxResult.Yes;
            MessageBoxResult noResult = MessageBoxResult.No;

            if (messageBoxResult == yesResult)
            {
                saveToXML();
            }
            else if (messageBoxResult == noResult)
            {
            }
            else
            {
                e.Cancel = true;
                this.Activate();
            }
        }

        private void CbxServiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxServiceList.SelectedIndex < 1)
            {
                cbxServiceList.Foreground = Brushes.Red;
            }
            else
            {
                cbxServiceList.Foreground = Brushes.Black;
            }
        }

        private void CbxProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxProvider.SelectedIndex < 1)
            {
                cbxProvider.Foreground = Brushes.Red;
            }
            else
            {
                cbxProvider.Foreground = Brushes.Black;
            }
        }

        private void CbxLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxLocation.SelectedIndex < 1)
            {
                cbxLocation.Foreground = Brushes.Red;
            }
            else
            {
                cbxLocation.Foreground = Brushes.Black;
            }
        }

        private void CbxTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTime.SelectedIndex < 1)
            {
                cbxTime.Foreground = Brushes.Red;
            }
            else
            {
                cbxTime.Foreground = Brushes.Black;
            }
        }

        private void DpkDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dpkDate.SelectedDate.HasValue)
            {
                dpkDate.Foreground = Brushes.Red;
            }
            else
            {
                dpkDate.Foreground = Brushes.Black;
            }
        }

        private void TxtName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            txtName.BorderBrush = null;
        }

        private void TxtAge_SelectionChanged(object sender, RoutedEventArgs e)
        {
            txtAge.BorderBrush = null;
        }

        private void TxtPhone_SelectionChanged(object sender, RoutedEventArgs e)
        {
            txtPhone.BorderBrush = null;
        }

        
    }


}
