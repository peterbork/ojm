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
    /// Interaction logic for MachineView.xaml
    /// </summary>
    public partial class MachineView : Window {
        Controller controller;
        int selectedProductOrder;
        Dictionary<int, string> allmachines;
        Dictionary<int, string> chosenmachines = new Dictionary<int, string>();

        public MachineView(Controller incontroller) {
            InitializeComponent();
            controller = incontroller;
            foreach (string machinename in controller.GetMachineNames().Values) {
                ListBoxAllMachines.Items.Add(machinename);
            }
            allmachines = controller.GetMachineNames();
        }
        public void SetController(Controller controller) {
            this.controller = controller;
        }
        internal void SetProductOrder(int selectedProductOrder) {
            this.selectedProductOrder = selectedProductOrder;
            List<string> selectedMachines = controller.GetProductOrderMachineNames(selectedProductOrder);
            // Remove Productorders materials from available materials
            foreach (string machine in selectedMachines) {
                foreach (KeyValuePair<int, string> amachine in allmachines.Reverse()) {
                    if (machine == amachine.Value) {
                        chosenmachines.Add(amachine.Key, amachine.Value);
                        allmachines.Remove(amachine.Key);
                    }
                }
            }
            UpdateListViews();
        }
        public void UpdateListViews() {
            ListBoxAllMachines.Items.Clear();
            ListBoxChosenMachines.Items.Clear();
            foreach (string machine in allmachines.Values) {
                ListBoxAllMachines.Items.Add(machine);
            }
            foreach (string machine in chosenmachines.Values) {
                ListBoxChosenMachines.Items.Add(machine);
            }
        }
        private void ListBoxAllMachines_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            List<int> keylist = new List<int>(this.allmachines.Keys);
            List<string> valuelist = new List<string>(this.allmachines.Values);
            int selectedKey = keylist[ListBoxAllMachines.SelectedIndex];
            string selectedValue = valuelist[ListBoxAllMachines.SelectedIndex];
            chosenmachines.Add(selectedKey, selectedValue);
            allmachines.Remove(selectedKey);
            UpdateListViews();
        }
        private void ListBoxChosenMachines_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            List<int> keylist = new List<int>(this.chosenmachines.Keys);
            List<string> valuelist = new List<string>(this.chosenmachines.Values);
            int selectedKey = keylist[ListBoxChosenMachines.SelectedIndex];
            string selectedValue = valuelist[ListBoxChosenMachines.SelectedIndex];
            allmachines.Add(selectedKey, selectedValue);
            chosenmachines.Remove(selectedKey);
            UpdateListViews();
        }
        private void btnAddMachinesToProductOrder_Click(object sender, RoutedEventArgs e) {
            List<int> machineindexes = new List<int>(this.chosenmachines.Keys);
            List<int> sequences = new List<int>();
            for (int i = 0; i < machineindexes.Count; i++) {
                sequences.Add(i);
            }
            controller.AddMachineToProductOrder(sequences, machineindexes, selectedProductOrder);
            this.Close();
            MessageBox.Show("Maskiner er blevet tilføjet til produkt ordren");
        }

        
       

    }
}
