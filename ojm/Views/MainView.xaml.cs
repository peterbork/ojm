using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ojm.Controllers;

namespace ojm {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window {
        Controller controller;
        int selectedProduct = -1;
        int SelectedCustomerIndex = -1;
        int SelectedCustomerID;
        public MainView() {
            InitializeComponent();
            
            controller = new Controller();

            ListviewCustomers.ItemsSource = controller.GetCustomers();
            ListviewStorage.ItemsSource = controller.GetStorageItems();

            ComboboxCustomer.ItemsSource = controller.GetCustomerNames();
        }

        // Customer
        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e) {
            if (SelectedCustomerIndex != -1) {
                controller.UpdateCustomer(SelectedCustomerID, TextBoxCompanyName.Text, TextBoxCVR.Text, TextBoxAddress.Text, TextBoxEmail.Text, TextBoxPhonenumber.Text, TextBoxContactPerson.Text);
                SelectedCustomerIndex = -1;
                TextBoxCompanyName.Text = "";
                TextBoxCVR.Text = "";
                TextBoxAddress.Text = "";
                TextBoxEmail.Text = "";
                TextBoxPhonenumber.Text = "";
                TextBoxContactPerson.Text = "";
                ListviewCustomers.ItemsSource = controller.GetCustomers();
                BtnAddCustomer.Content = "Tilføj";
                MessageBox.Show("Kunden er blevet opdateret", "OJM");
            }
            else {
                if (controller.AddCustomer(TextBoxCompanyName.Text, TextBoxCVR.Text, TextBoxAddress.Text, TextBoxEmail.Text, TextBoxPhonenumber.Text, TextBoxContactPerson.Text)) {
                    TextBoxCompanyName.Text = "";
                    TextBoxCVR.Text = "";
                    TextBoxAddress.Text = "";
                    TextBoxEmail.Text = "";
                    TextBoxPhonenumber.Text = "";
                    TextBoxContactPerson.Text = "";
                    ListviewCustomers.ItemsSource = controller.GetCustomers();
                }
            }
        }

        private void BtnCustomerCancel_Click(object sender, RoutedEventArgs e) {
            TextBoxCompanyName.Text = "";
            TextBoxCVR.Text = "";
            TextBoxAddress.Text = "";
            TextBoxEmail.Text = "";
            TextBoxPhonenumber.Text = "";
            TextBoxContactPerson.Text = "";
            BtnAddCustomer.Content = "Tilføj";

        }

        private void ListviewCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            BtnAddCustomer.Content = "Opdater";
            SelectedCustomerIndex = ListviewCustomers.SelectedIndex;

            Dictionary<string, string> _customer = controller.GetCustomer(SelectedCustomerIndex);
            SelectedCustomerID = int.Parse(_customer["ID"]);

            TextBoxCompanyName.Text = _customer["CompanyName"];
            TextBoxCVR.Text = _customer["CVR"];
            TextBoxAddress.Text = _customer["Address"];
            TextBoxEmail.Text = _customer["Email"];
            TextBoxPhonenumber.Text = _customer["Phonenumber"];
            TextBoxContactPerson.Text = _customer["ContactPerson"];


        }

        // Storage

        private void ListviewStorage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedProduct = ListviewStorage.SelectedIndex;
            
            Dictionary<string, string> storageItem = controller.GetStorageItem(selectedProduct);

            TextBoxProductName.Text = storageItem["Name"];
            TextBoxInStock.Text = storageItem["InStock"];
            TextBoxType.Text = storageItem["Type"];
            TextBoxTolerance.Text = storageItem["Tolerance"];
            TextBoxReserved.Text = storageItem["Reserved"];
            if (storageItem.ContainsKey("Customer")) {
                ComboboxCustomer.SelectedIndex = int.Parse(storageItem["Customer"]);
            }
            BtnAddProduct.Content = "Opdater";
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e) {
            // Update product
            if (selectedProduct != -1) {
                // Tjek if InStock is a number
                int InStock = 0;
                try {
                    InStock = Convert.ToInt32(TextBoxInStock.Text);
                }
                catch (Exception) {
                    MessageBox.Show("Antal på lager skal være et tal.");
                }
                controller.UpdateStorageItem(selectedProduct, TextBoxProductName.Text, InStock);
                ListviewStorage.ItemsSource = controller.GetStorageItems();
                MessageBox.Show("Produktet er blevet tilføjet", "OJM");
            }
            // Create product
            else { 
                
            }
           
        }

        private void BtnStoreageCancel_Click(object sender, RoutedEventArgs e) {
            selectedProduct = -1;
            BtnAddProduct.Content = "Tilføj";
            TextBoxInStock.Text = "";
            TextBoxProductName.Text = "";
        }

    }
}
