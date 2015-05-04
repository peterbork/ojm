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

        public void setController(Controller controller) {
            this.controller = controller;
        }

        public void SetProduct(int index, string productName) {
            productIndex = index;
            ProductName.Content = productName;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {

        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            try {
                int quantity = int.Parse(Quantity.Text);
                controller.OrderStorageItem(productIndex, Convert.ToDateTime(DeliveryDate.SelectedDate.ToString()), quantity);
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
