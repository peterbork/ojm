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
using System.Globalization;

namespace ojm.Views {
    /// <summary>
    /// Interaction logic for ProductionView.xaml
    /// </summary>
    public partial class ProductionView : Window {

        Controller controller;
        int selectedproductorder;
        int selectedmachine;
        int currentweek;
        DateTime today = DateTime.Now;
        List<ListView> listviews = new List<ListView>();
        List<DateTime> times = new List<DateTime>();
        Dictionary<DateTime, string> datetimeandname;
        Dictionary<int, List<DateTime>> indexanddatetime = new Dictionary<int, List<DateTime>>();

        public ProductionView(Controller incontroller)
        {
            InitializeComponent();
            controller = incontroller;
            listviews.Add(ListBoxDay1);
            listviews.Add(ListBoxDay2);
            listviews.Add(ListBoxDay3);
            listviews.Add(ListBoxDay4);
            listviews.Add(ListBoxDay5);
            listviews.Add(ListBoxDay6);
            listviews.Add(ListBoxDay7);
            datetimeandname = controller.GetMachineScheduleDateTimeAndName();
            currentweek = Helper.GetWeekNumberFromDate(today);
            UpdateSchedule(currentweek);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            foreach (string machinename in controller.GetProductOrderMachineNames(selectedproductorder)) {
                ComboBoxMachines.Items.Add(machinename);
                indexanddatetime.Add(ComboBoxMachines.Items.Count - 1, new List<DateTime>());
            }
        }

        public void UpdateSchedule(int weeknumber) {
            // clear them
            foreach (ListView listview in listviews) {
                listview.Items.Clear();
            }
            DateTime today = DateTime.Now;
            int currentweek = weeknumber;
            LabelCurrentWeek.Content = currentweek;
            DateTime FirstDateOfWeek = Helper.FirstDateOfWeek(currentweek, CultureInfo.CurrentCulture);
            for (int i = 0; i < listviews.Count; i++) {
                FirstDateOfWeek = FirstDateOfWeek.AddHours(8);
                for (int j = 0; j < 9; j++) {
                    ListViewItem li = new ListViewItem();
                    List<int> selectedmachineids = new List<int>(indexanddatetime.Keys);
                        // if time has been selected
                    if (indexanddatetime.Count > 0) {
                        foreach (DateTime datetimelist in indexanddatetime[selectedmachine]) {

                            if (datetimelist.Date == FirstDateOfWeek.Date && datetimelist.Hour == FirstDateOfWeek.Hour) {
                                    li.Background = Brushes.Yellow;
                                    li.Foreground = Brushes.Black;
                                    li.Content = "Selected";
                            }
                        }
                    }

                    // if time is already in use
                    foreach (KeyValuePair<DateTime, string> machineSchedule in datetimeandname) {
                        if (machineSchedule.Key.Date == FirstDateOfWeek.Date && machineSchedule.Key.Hour >= FirstDateOfWeek.Hour && machineSchedule.Key.Hour < FirstDateOfWeek.AddHours(1).Hour) {
                            if (ComboBoxMachines.Items.Count > 0 && ComboBoxMachines.SelectedIndex > -1) { 
                                if (ComboBoxMachines.SelectedValue.ToString() == machineSchedule.Value.ToString()) { 
                                    li.Background = Brushes.Red;
                                    li.Foreground = Brushes.White;
                                    li.IsEnabled = false;
                                    li.Content = FirstDateOfWeek;
                                }
                            }
                        }
                        else {
                            li.Content = FirstDateOfWeek;
                        }
                    }
                    // if time is in the past
                    if (FirstDateOfWeek.Ticks < today.Ticks) {
                        li.Background = Brushes.Red;
                        li.Foreground = Brushes.White;
                        li.IsEnabled = false;
                    }
                    FirstDateOfWeek = FirstDateOfWeek.AddHours(1);
                    listviews[i].Items.Add(li);
                }
                FirstDateOfWeek = Helper.FirstDateOfWeek(currentweek, CultureInfo.CurrentCulture);
                FirstDateOfWeek = FirstDateOfWeek.AddDays(i + 1);
            }
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
            controller.AddMachineSchedules(selectedproductorder, indexanddatetime);
            controller.AddProduction(selectedproductorder,  Convert.ToDecimal(TextBoxProductionAmount.Text), Convert.ToDateTime(DatePickerProductionDeadline.SelectedDate));
            this.Close();
            MessageBox.Show("Ordren er blevet sat i production");
            
        }

        private void BtnNextWeek_Click(object sender, RoutedEventArgs e) {
            currentweek++;
            UpdateSchedule(currentweek);
        }

        private void BtnPreviousWeek_Click(object sender, RoutedEventArgs e) {
            currentweek--;
            UpdateSchedule(currentweek);
        }

        
        public void AddTimesToList(int number) {
            
            DateTime today = DateTime.Now;
            if (ComboBoxMachines.SelectedIndex <= -1) {
                MessageBox.Show("Vælg en maskine først");
            }
            else {
                
               
                int j = 0;
                for (int i = listviews[number].SelectedIndex; i <= listviews[number].SelectedItems.Count + listviews[number].SelectedIndex-1; i++) {
                    DateTime time = Helper.FirstDateOfWeek(currentweek, CultureInfo.CurrentCulture).AddHours(listviews[number].SelectedIndex + 8).AddDays(number);
                    
                    time = time.AddHours(j);
                    times.Add(time);
                    
                    j++;
                }
                if (times.Count > 0) {
                    indexanddatetime[selectedmachine] = new List<DateTime>();
                    indexanddatetime[selectedmachine] = times;
                    times = new List<DateTime>();
                }
                
            }
        }

        private void ComboBoxMachines_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedmachine = ComboBoxMachines.SelectedIndex;
            UpdateSchedule(currentweek);
            
        }
        private void ListBoxDay1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AddTimesToList(0);
        }

        private void ListBoxDay2_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AddTimesToList(1);
        }

        private void ListBoxDay3_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AddTimesToList(2);
        }

        private void ListBoxDay4_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AddTimesToList(3);
        }

        private void ListBoxDay5_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AddTimesToList(4);
        }

        private void ListBoxDay6_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AddTimesToList(5);
        }

        private void ListBoxDay7_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AddTimesToList(6);
        }

        
    }
}
