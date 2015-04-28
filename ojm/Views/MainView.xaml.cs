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
        Controller controller { get; set; }
        int selectedProduct { get; set; }

        public MainView() {
            InitializeComponent();

            controller = new Controller();
        }

        private void TabVarelager_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ListviewStorage.Items.Count == 0)
            {
                foreach (string StorageItem in controller.GetStorageItems())
                {
                    ListviewStorage.Items.Add(StorageItem);
                }
            }
        }

        private void ListviewStorage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedProduct = ListviewStorage.SelectedIndex;
            
            Dictionary<string, string> storageItem = controller.GetStorageItem(selectedProduct);

            TextBoxProductName.Text = storageItem["Name"];
            TextBoxInStock.Text = storageItem["InStock"];
        }
    }
}
