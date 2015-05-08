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
using System.Windows.Shapes;
using ojm.Controllers;

namespace ojm.Views
{
    /// <summary>
    /// Interaction logic for ProductOrderView.xaml
    /// </summary>
    public partial class ProductOrderView : Window
    {
        Controller controller;
        Dictionary<int, string> availablematerials;
        Dictionary<int, string> productordermaterials;

        public ProductOrderView(Controller incontroller)
        {
            InitializeComponent();
            controller = incontroller;

            //lists all company names in the combobox
            foreach (string name in controller.GetCustomerNames()) {
                ComboBoxCustomers.Items.Add(name);
            }

            TextBoxProductOrderName.IsEnabled = false;
            TextBoxProductOrderDescription.IsEnabled = false;
            ListviewAvailableMaterials.IsEnabled = false;
            ListviewProductOrderMaterials.IsEnabled = false;
            BtnAddProductOrder.IsEnabled = false;
            
        }
        public void SetController(Controller controller) {
            this.controller = controller;
            
        }

        private void ComboBoxCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ComboBoxCustomers.SelectedIndex != -1) {
                TextBoxProductOrderName.IsEnabled = true;
                TextBoxProductOrderDescription.IsEnabled = true;
                ListviewAvailableMaterials.IsEnabled = true;
                ListviewProductOrderMaterials.IsEnabled = true;
                availablematerials = controller.GetMaterialsFromCustomerIndex(ComboBoxCustomers.SelectedIndex);
                foreach (string material in controller.GetMaterialsFromCustomerIndex(ComboBoxCustomers.SelectedIndex).Values) {
                    ListviewAvailableMaterials.Items.Add(material);  
                }
            }
        }

        public void UpdateListViews() { 
            
        }
        private void ListviewAvailableMaterials_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            int selectedIndex = ListviewAvailableMaterials.SelectedIndex;
            ListviewProductOrderMaterials.Items.Add(controller.GetMaterialsFromCustomerIndex(ComboBoxCustomers.SelectedIndex)[selectedIndex]);
            ListviewAvailableMaterials.Items.RemoveAt(selectedIndex);
        }

        private void ListviewProductOrderMaterials_MouseDoubleClick(object sender, MouseButtonEventArgs e) {

        }

    }
}
