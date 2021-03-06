﻿using System;
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
        int selectedProductOrder;
        Dictionary<int, string> availablematerials;
        Dictionary<int, string> productordermaterials = new Dictionary<int,string>();
        List<decimal> usages = new List<decimal>();

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

                ListviewAvailableMaterials.Items.Clear();
                ListviewProductOrderMaterials.Items.Clear();
                foreach (string material in availablematerials.Values) {
                    ListviewAvailableMaterials.Items.Add(material);  
                }
            }
        }

        public void UpdateListViews() {
            ListviewAvailableMaterials.Items.Clear();
            ListviewProductOrderMaterials.Items.Clear();
            ListviewProductOrderMaterialsUsage.Items.Clear();
            foreach (string material in availablematerials.Values) {
                ListviewAvailableMaterials.Items.Add(material);
            }
            foreach (string material in productordermaterials.Values) {
                ListviewProductOrderMaterials.Items.Add(material);
            }
            foreach (int usage in usages) {
                ListviewProductOrderMaterialsUsage.Items.Add(usage + "");
            }
        }
        private void ListviewAvailableMaterials_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            //int selectedItem = ListviewAvailableMaterials.SelectedItem.;
            List<int> keylist = new List<int>(this.availablematerials.Keys);
            List<string> valuelist = new List<string>(this.availablematerials.Values);
            int selectedKey = keylist[ListviewAvailableMaterials.SelectedIndex];
            string selectedValue = valuelist[ListviewAvailableMaterials.SelectedIndex];
            productordermaterials.Add(selectedKey, selectedValue);
            Dialog dialog = new Dialog("Hvor mange skal der bruges i produktions ordren?");
            if (dialog.ShowDialog() == true) {
                usages.Add(int.Parse(dialog.Answer));
            }
            availablematerials.Remove(selectedKey);
            UpdateListViews();
        }

        private void ListviewProductOrderMaterials_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            List<int> keylist = new List<int>(this.productordermaterials.Keys);
            List<string> valuelist = new List<string>(this.productordermaterials.Values);
            int selectedKey = keylist[ListviewProductOrderMaterials.SelectedIndex];
            string selectedValue = valuelist[ListviewProductOrderMaterials.SelectedIndex];
            availablematerials.Add(selectedKey, selectedValue);
            productordermaterials.Remove(selectedKey);
            if (usages.Count >= ListviewProductOrderMaterialsUsage.SelectedIndex) {
                usages.RemoveAt(ListviewProductOrderMaterials.SelectedIndex);
            }
            UpdateListViews();
        }

        public void ListviewProductOrderMaterialsUsage_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (ListviewProductOrderMaterialsUsage.SelectedIndex > -1) {
                Dialog dialog = new Dialog("Hvor mange skal der bruges i produktions ordren?");
                if (dialog.ShowDialog() == true) {
                    usages[ListviewProductOrderMaterialsUsage.SelectedIndex] = int.Parse(dialog.Answer);
                    UpdateListViews();
                }
            }
        }

        private void BtnAddProductOrder_Click(object sender, RoutedEventArgs e) {
            List<int> materialkeys = productordermaterials.Keys.ToList();
            if (selectedProductOrder > -1)
                controller.UpdateProductOrder(selectedProductOrder, TextBoxProductOrderName.Text, TextBoxProductOrderDescription.Text, ComboBoxCustomers.SelectedIndex, materialkeys, usages);
            else
                controller.AddProductOrder(TextBoxProductOrderName.Text, TextBoxProductOrderDescription.Text, ComboBoxCustomers.SelectedIndex, materialkeys, usages);
        }

        public void SetProductOrder(int selectedProductOrder) {
            this.selectedProductOrder = selectedProductOrder;

            Dictionary<string, string> productorder = controller.GetProductOrder(selectedProductOrder);
            ComboBoxCustomers.SelectedItem = productorder["CompanyName"];
            TextBoxProductOrderName.Text = productorder["Name"];
            TextBoxProductOrderDescription.Text = productorder["Description"];

            Dictionary<string, decimal> selectedmaterials = controller.GetProductOrderMaterialStrings(selectedProductOrder);
            
            // Remove Productorders materials from available materials
            foreach (KeyValuePair<string, decimal> material in selectedmaterials) {
                foreach (KeyValuePair<int, string> amaterial in availablematerials.Reverse()) {
                    if (material.Key == amaterial.Value) {
                        productordermaterials.Add(amaterial.Key, amaterial.Value);
                        usages.Add(material.Value);
                        availablematerials.Remove(amaterial.Key);
                    }
                }
            }

            BtnAddProductOrder.Content = "Opdater product ordre";
            UpdateListViews();
        }
    }
}
