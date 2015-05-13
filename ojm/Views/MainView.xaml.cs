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
        int selectedMaterial = -1;
        int SelectedCustomerIndex = -1;
        int SelectedCustomerID;
        int selectedProductOrder;
        public MainView() {
            InitializeComponent();
            
            controller = new Controller();
            controller.SetView(this);

            ListviewCustomers.ItemsSource = controller.GetCustomers();
            ListviewStorage.ItemsSource = controller.GetMaterials();
            ListViewOrders.ItemsSource = controller.GetProductOrders();

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
            selectedMaterial = ListviewStorage.SelectedIndex;
            
            Dictionary<string, string> storageItem = controller.GetMaterial(selectedMaterial);

            TextBoxMaterialName.Text = storageItem["Name"];
            TextBoxInStock.Text = storageItem["InStock"];
            TextBoxType.Text = storageItem["Type"];
            TextBoxTolerance.Text = storageItem["Tolerance"];
            TextBoxReserved.Text = storageItem["Reserved"];
            if (storageItem.ContainsKey("Customer")) {
                ComboboxCustomer.SelectedIndex = int.Parse(storageItem["Customer"]);
            }
            else
            {
                ComboboxCustomer.SelectedIndex = -1;
            }

            BtnOrderStorageItem.IsEnabled = true;

            ListviewOrders.ItemsSource = controller.GetMaterialDeliveries(selectedMaterial);

            BtnAddMaterial.Content = "Opdater";
        }

        private void BtnAddMaterial_Click(object sender, RoutedEventArgs e) {
            // Check if InStock is a number
            int InStock = 0;
            int tolerance = 0;
            int reserved = 0;

            try
            {
                InStock = Convert.ToInt32(TextBoxInStock.Text);
                tolerance = Convert.ToInt32(TextBoxTolerance.Text);
                reserved = Convert.ToInt32(TextBoxReserved.Text);

                // Update material
                if (selectedMaterial != -1)
                {
                    controller.UpdateMaterial(selectedMaterial, TextBoxMaterialName.Text, InStock, TextBoxType.Text, tolerance, reserved, ComboboxCustomer.SelectedIndex);
                    ListviewStorage.ItemsSource = controller.GetMaterials();
                    MessageBox.Show("Materialet er blevet Opdateret", "OJM");
                }
                // Create material
                else
                {
                    controller.AddMaterial(TextBoxMaterialName.Text, InStock, TextBoxType.Text, tolerance, reserved, ComboboxCustomer.SelectedIndex);
                    ListviewStorage.ItemsSource = controller.GetMaterials();
                    MessageBox.Show("Materialet er blevet tilføjet", "OJM");
                    ClearInputFields();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Antal på lager, tolerance og reserveret skal være et tal.");
            }            
           
        }

        private void BtnStoreageCancel_Click(object sender, RoutedEventArgs e) {
            ClearInputFields();
        }

        private void ClearInputFields()
        {
            selectedMaterial = -1;
            BtnAddMaterial.Content = "Tilføj";
            TextBoxInStock.Text = "";
            TextBoxMaterialName.Text = "";
            TextBoxType.Text = "";
            TextBoxTolerance.Text = "";
            TextBoxReserved.Text = "";
            ComboboxCustomer.SelectedIndex = -1;
            BtnOrderStorageItem.IsEnabled = false;
        }

        private void BtnOrderStorageItem_Click(object sender, RoutedEventArgs e) {
            controller.NewDelivery(selectedMaterial);
        }

        public void UpdateMaterials() {
            ListviewStorage.ItemsSource = controller.GetMaterials();
            TextBoxInStock.Text = controller.GetMaterial(selectedMaterial)["InStock"];

            ListviewOrders.ItemsSource = controller.GetMaterialDeliveries(selectedMaterial);
        }

        private void ListviewOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            controller.UpdateDelivery(selectedMaterial, ListviewOrders.SelectedIndex);
        }

        private void BtnAddNewProductOrder_Click(object sender, RoutedEventArgs e) {
            Views.ProductOrderView window = new Views.ProductOrderView(controller);
            window.Show();
        }

        private void ListViewOrders_MouseDoubleClick_1(object sender, MouseButtonEventArgs e) {
            selectedProductOrder = ListViewOrders.SelectedIndex;
            SetProductOrder();
        }

        private void SetProductOrder() {
            TextBoxProductOrderDescription.Text = controller.GetProductOrder(selectedProductOrder)["Description"];
            LabelProductOrderName.Content = controller.GetProductOrder(selectedProductOrder)["Name"];
            ListViewProductOrderMaterials.ItemsSource = controller.GetProductOrderMaterials(selectedProductOrder);
            ListViewMachines.ItemsSource = controller.GetProductOrderMachines(selectedProductOrder);
        }

        public void UpdateProductOrders() {
            ListViewOrders.ItemsSource = controller.GetProductOrders();
            SetProductOrder();
        }

        private void BtnShowDetails_Click(object sender, RoutedEventArgs e) {
            Views.ProductOrderView window = new Views.ProductOrderView(controller);
            window.SetProductOrder(selectedProductOrder);
            window.Show();
        }

        private void btnAddMachines_Click(object sender, RoutedEventArgs e) {
            controller.GetMachines();
            Views.MachineView window = new Views.MachineView(controller);
            window.SetProductOrder(selectedProductOrder);
            window.Show();
        }

        private void btnOpenControl_Click(object sender, RoutedEventArgs e) {
            Views.QualityControl window = new Views.QualityControl(controller);
            window.Show();
        }



    }
}
