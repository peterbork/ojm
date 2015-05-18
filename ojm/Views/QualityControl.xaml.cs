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
        List<Dictionary<string, string>> qualitycontrols;
        public QualityControl(Controller incontroller) {
            InitializeComponent();
            controller = incontroller;

            btnAddQualityControl.IsEnabled = false;
            ComboBoxQualityControls.IsEnabled = false;
            ListViewMachines.ItemsSource = controller.GetProductOrderAndMachine();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewMachines.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ProductOrder.Name");
            view.GroupDescriptions.Add(groupDescription);
            
            }

        private void ListViewMachines_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            ComboBoxQualityControls.Items.Clear();
            qualitycontrols = controller.GetQualityControl(ListViewMachines.SelectedIndex);
            if (ListViewMachines.SelectedIndex > -1) {
                btnAddQualityControl.IsEnabled = true;
                ComboBoxQualityControls.IsEnabled = true;
                
                foreach (Dictionary<string, string> dic in qualitycontrols) {
                    ComboBoxQualityControls.Items.Add(dic["Name"]);
                }
            }
            ComboBoxQualityControls.Items.Add("Ny Kvalitetskontrol");
            ComboBoxQualityControls.SelectedIndex = ComboBoxQualityControls.Items.Count - 1;
            LabelControlCount.Content = qualitycontrols.Count;
        }

        private void btnAddQualityControl_Click(object sender, RoutedEventArgs e) {

            if (ComboBoxQualityControls.SelectedIndex == ComboBoxQualityControls.Items.Count - 1) {
                controller.AddQualityControl(TextBoxName.Text, TextBoxDescription.Text, TextBoxFrequency.Text, TextBoxMinTol.Text, TextBoxMaxTol.Text, ListViewMachines.SelectedIndex);
                MessageBox.Show("Kvalitetskontrollen er blevet oprettet");
            }
            else {
                controller.UpdateQualityControl(ComboBoxQualityControls.SelectedIndex, TextBoxName.Text, TextBoxDescription.Text, int.Parse(TextBoxFrequency.Text), decimal.Parse(TextBoxMinTol.Text), decimal.Parse(TextBoxMaxTol.Text));
                MessageBox.Show("Kvalitetskontrollen er blevet opdateret");
            }
        }

        private void ComboBoxQualityControls_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ComboBoxQualityControls.SelectedIndex == ComboBoxQualityControls.Items.Count - 1) {
                TextBoxName.Text = "";
                TextBoxDescription.Text = "";
                TextBoxFrequency.Text = "";
                TextBoxMinTol.Text = "";
                TextBoxMaxTol.Text = "";
                btnAddQualityControl.Content = "Tilføj Kontrol";
            }
            else {
                TextBoxName.Text = qualitycontrols[ComboBoxQualityControls.SelectedIndex]["Name"];
                TextBoxDescription.Text = qualitycontrols[ComboBoxQualityControls.SelectedIndex]["Description"];
                TextBoxFrequency.Text = qualitycontrols[ComboBoxQualityControls.SelectedIndex]["Frequency"];
                TextBoxMinTol.Text = qualitycontrols[ComboBoxQualityControls.SelectedIndex]["MinTol"];
                TextBoxMaxTol.Text = qualitycontrols[ComboBoxQualityControls.SelectedIndex]["MaxTol"];
                btnAddQualityControl.Content = "Opdater Kontrol";
            }
        }
        

    }
}
