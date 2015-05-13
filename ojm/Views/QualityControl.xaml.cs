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
    /// Interaction logic for QualityControl.xaml
    /// </summary>
    public partial class QualityControl : Window {
        Controller controller;
        public QualityControl(Controller incontroller) {
            InitializeComponent();
            controller = incontroller;
            
                List<Models.Machine> items = new List<Models.Machine>();
                //items.Add(new Models.Machine() { ID = 1, Name = "John Doe", Type = "test"});
                //items.Add(new Models.Machine() { Name = "Jane Doe", Type = "tes", Sex = 3 });
                //items.Add(new Models.Machine() { Name = "Sammy Doe", Type = "dsf", Sex = 4 });
                lvUsers.ItemsSource = controller.GetProductOrders();

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvUsers.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("ID");
                view.GroupDescriptions.Add(groupDescription);
            }
        

    }
}
