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
        int selectedProduct { get; set; }
        int SelectedCustomerIndex = -1;
        int SelectedCustomerID;
        public MainView() {
            InitializeComponent();
            
            controller = new Controller();
            ListviewCustomers.ItemsSource = controller.GetCustomers();
        }

        // Storage
        private void TabVarelager_GotFocus(object sender, RoutedEventArgs e)
        {
            ListviewStorage.ItemsSource = controller.GetStorageItems();
        }

        private void ListviewStorage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedProduct = ListviewStorage.SelectedIndex;
            MessageBox.Show(ListviewStorage.SelectedIndex + "");
            
            Dictionary<string, string> storageItem = controller.GetStorageItem(selectedProduct);

            TextBoxProductName.Text = storageItem["Name"];
            TextBoxInStock.Text = storageItem["InStock"];
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
                controller.AddCustomer(TextBoxCompanyName.Text, TextBoxCVR.Text, TextBoxAddress.Text, TextBoxEmail.Text, TextBoxPhonenumber.Text, TextBoxContactPerson.Text);
                TextBoxCompanyName.Text = "";
                TextBoxCVR.Text = "";
                TextBoxAddress.Text = "";
                TextBoxEmail.Text = "";
                TextBoxPhonenumber.Text = "";
                TextBoxContactPerson.Text = "";
                ListviewCustomers.ItemsSource = controller.GetCustomers();
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
            
            SelectedCustomerID = controller.GetIDFromCustomerModel(DatabaseFacade.GetCustomers()[SelectedCustomerIndex]);
            // Should be optimized, way to many database connections
            TextBoxCompanyName.Text = DatabaseFacade.GetCustomers()[SelectedCustomerIndex].CompanyName;
            TextBoxCVR.Text = DatabaseFacade.GetCustomers()[SelectedCustomerIndex].CVR;
            TextBoxAddress.Text = DatabaseFacade.GetCustomers()[SelectedCustomerIndex].Address;
            TextBoxEmail.Text = DatabaseFacade.GetCustomers()[SelectedCustomerIndex].Email;
            TextBoxPhonenumber.Text = DatabaseFacade.GetCustomers()[SelectedCustomerIndex].Phonenumber;
            TextBoxContactPerson.Text = DatabaseFacade.GetCustomers()[SelectedCustomerIndex].ContactPerson;
            
            
        }

    }
}
