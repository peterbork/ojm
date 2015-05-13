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

namespace ojm.Views {
    /// <summary>
    /// Interaction logic for QualityControl.xaml
    /// </summary>
    public partial class QualityControl : Window {
        public QualityControl() {
            InitializeComponent();
                List<User> items = new List<User>();
                items.Add(new User() { Name = "John Doe", Age = 42, Sex = 2 });
                items.Add(new User() { Name = "Jane Doe", Age = 39, Sex = 3 });
                items.Add(new User() { Name = "Sammy Doe", Age = 13, Sex = 4 });
                lvUsers.ItemsSource = items;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvUsers.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Sex");
                view.GroupDescriptions.Add(groupDescription);
            }
        

        public class User {
            public string Name { get; set; }

            public int Age { get; set; }

            public string Mail { get; set; }

            public int Sex { get; set; }
        }
    }
}
