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

namespace ojm.Views {
    /// <summary>
    /// Interaction logic for DeliveryView.xaml
    /// </summary>
    public partial class DeliveryView : Window {
        Controller controller;
        int productIndex = -1;
        int deliveryIndex = -1;
        public DeliveryView() {
            InitializeComponent();
        }

        public void SetController(Controller controller) {
            this.controller = controller;
        }

        public void SetProduct(int index, string productName) {
            productIndex = index;
            ProductName.Content = productName;
        }

        public void SetDelivery(int index, Dictionary<string, string> delivery) {
            deliveryIndex = index;
            DeliveryDate.SelectedDate = Convert.ToDateTime(delivery["DeliveryDate"]);
            Quantity.Text = delivery["Quantity"];
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {

        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            try {
                int quantity = int.Parse(Quantity.Text);
                if (deliveryIndex > -1) {
                    // Update 
                    controller.UpdateStorageDelivery(productIndex, deliveryIndex, Convert.ToDateTime(DeliveryDate.SelectedDate), quantity, Convert.ToBoolean(Arrived.IsChecked));
                }
                else {
                    // Create
                    controller.OrderStorageItem(productIndex, Convert.ToDateTime(DeliveryDate.SelectedDate), quantity);
                }                
                this.Close();
            }catch(Exception) {
                MessageBox.Show("Antal skal være et tal.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
