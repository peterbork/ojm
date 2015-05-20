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
    /// Interaction logic for ProductionView.xaml
    /// </summary>
    public partial class ProductionView : Window {
        Controller controller;
        int selectedproductorder;
        public ProductionView(Controller incontroller)
        {
            InitializeComponent();
            controller = incontroller;

        }

        public void SetProductOrder(int selectedproductorder)
        {
            this.selectedproductorder = selectedproductorder;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnProduce_Click(object sender, RoutedEventArgs e) {
            controller.AddProduction(selectedproductorder,  Convert.ToDecimal(TextBoxProductionAmount.Text), Convert.ToDateTime(DatePickerProductionDeadline.SelectedDate));
            this.Close();
            MessageBox.Show("Ordren er blevet sat i production");
            
        }
    }
}
