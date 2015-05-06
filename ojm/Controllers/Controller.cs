using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ojm.Models;

namespace ojm.Controllers {
    public class Controller {

        List<Material> materials = new List<Material>();
        List<Customer> customers = new List<Customer>();
        MainView View;

        public void setView(MainView view) {
            View = view;
        }

        // CUSTOMER METHODS
        public bool AddCustomer(string companyname, string cvr, string address, string email, string phonenumber, string contactperson) {
            
            Customer _customer = new Customer(companyname, cvr, address, email, phonenumber, contactperson);
            if (IsCustomerExsisting(_customer.CVR) != true) {
                DatabaseFacade.AddCustomer(_customer);
                System.Windows.MessageBox.Show("Kunden blev oprettet", "OJM");
                return true;
            }
            else {
                System.Windows.MessageBox.Show("Kunden med CVR "+ cvr +" eksisterer allerede", "OJM");
                return false;
            }
        }
        public void UpdateCustomer(int id, string companyname, string cvr, string address, string email, string phonenumber, string contactperson) {
            Customer _customer = new Customer(id, companyname, cvr, address, email, phonenumber, contactperson);
            DatabaseFacade.UpdateCustomer(_customer);
        }
        public bool IsCustomerExsisting(string cvr) {
            string _cvr = DatabaseFacade.GetCustomerFromCVR(cvr).CVR;
            if (_cvr != null) {
                return true;
            }
            else {
                return false;
            }

        }
        public List<Models.Customer> GetCustomers() {
            customers = DatabaseFacade.GetCustomers();
            return customers;
        }

        public List<string> GetCustomerNames() {
            List<string> customerNames = new List<string>();

            foreach (Customer customer in customers) {
                customerNames.Add(customer.CompanyName);
            }

            return customerNames;
        }

        public Dictionary<string, string> GetCustomer(int index) {
            Dictionary<string, string> _customer = new Dictionary<string, string>();
            _customer.Add("ID", customers[index].ID.ToString());
            _customer.Add("CompanyName", customers[index].CompanyName);
            _customer.Add("CVR", customers[index].CVR);
            _customer.Add("Address", customers[index].Address);
            _customer.Add("Email", customers[index].Email);
            _customer.Add("Phonenumber", customers[index].Phonenumber);
            _customer.Add("ContactPerson", customers[index].ContactPerson);

            return _customer;
        }

        // STORAGE METHODS
        public List<Models.Material> GetStorageItems()
        {
            materials = DatabaseFacade.GetStorageItems();
            return materials;
        }

        public Dictionary<string, string> GetStorageItem(int index)
        {
            Dictionary<string, string> storageItem = new Dictionary<string, string>();

            storageItem.Add("Name", materials[index].Name);
            storageItem.Add("InStock", materials[index].InStock + "");
            storageItem.Add("Type", materials[index].Type);
            storageItem.Add("Tolerance", materials[index].Tolerance + "");
            storageItem.Add("Reserved", materials[index].Reserved + "");

            if (materials[index].Customer != null) {
                int customerIndex = -1;
                for (int i = 0; i < customers.Count && customerIndex == -1; i++) {
                    if (materials[index].Customer.CVR == customers[i].CVR)
                        customerIndex = i;
                }
                storageItem.Add("Customer", customerIndex + "");
            }
            

            return storageItem;
        }
        public void UpdateStorageItem(int index, string name, int instock,  string type, int tolerance, int reserved, int customerIndex)
        {
            Material material = materials[index];
            material.Name = name;
            material.InStock = instock;
            material.Type = type;
            material.Tolerance = tolerance;
            material.Reserved = reserved;
            if (customerIndex > -1) {
                material.Customer = customers[customerIndex];
            }
            
            DatabaseFacade.UpdateStorageItem(material);
        }

        public void RegisterDelivery(int deliveryIndex, int materialIndex)
        {
            Material material = materials[materialIndex];
            Delivery delivery = material.Deliveries[deliveryIndex];
            material.InStock += delivery.Quantity;

            DatabaseFacade.RegisterDelivery(delivery);
            DatabaseFacade.UpdateStorageItem(material);
        }


        internal void AddStorageItem(string name, int instock, string type, int tolerance, int reserved, int customer)
        {
            Material material;
            
            if (customer != -1 )
            {
                material = new Material(0, name, instock, type, tolerance, reserved, customers[customer]);
            }
            else
            {
                material = new Material(0, name, instock, type, tolerance, reserved);
            }
            
            DatabaseFacade.AddStorageItem(material);
        }

        internal List<Delivery> GetStorageItemOrders(int materialIndex) {

            materials[materialIndex].Deliveries = DatabaseFacade.GetStorageItemOrders(materials[materialIndex].ID);

            return materials[materialIndex].Deliveries;
        }

        internal void NewDelivery(int materialIndex) {
            Views.DeliveryView view = new Views.DeliveryView();
            view.Show();

            view.SetController(this);
            view.SetProduct(materialIndex, materials[materialIndex].Name);
        }

        public void UpdateDelivery(int materialIndex, int deliveryIndex) {
            Material material = materials[materialIndex];
            Dictionary<string, string> delivery = new Dictionary<string,string>();
            delivery.Add("DeliveryDate", material.Deliveries[deliveryIndex].DeliveryDate.ToString());
            delivery.Add("Quantity", material.Deliveries[deliveryIndex].Quantity.ToString());

            Views.DeliveryView view = new Views.DeliveryView();
            view.Show();

            view.SetController(this);
            view.SetProduct(materialIndex, material.Name);
            view.SetDelivery(deliveryIndex, delivery);
        }

        internal void OrderStorageItem(int materialIndex, DateTime deliveryDate, int quantity) {
            Delivery delivery = new Delivery(0, deliveryDate, quantity);
            materials[materialIndex].Deliveries.Add(delivery);

            DatabaseFacade.OrderStorageItem(materials[materialIndex].ID, delivery);

            View.UpdateStorageItems();
        }

        internal void UpdateStorageDelivery(int materialIndex, int deliveryIndex, DateTime deliveryDate, int quantity, bool Arrived) {
            Material material = materials[materialIndex];
            Delivery delivery = material.Deliveries[deliveryIndex];
            if (Arrived && !delivery.Arrived) {
                // Update the storage item's quantity
                material.InStock += quantity;
                DatabaseFacade.UpdateStorageItem(material);
            }

            // Update the Delivery
            delivery.DeliveryDate = deliveryDate;
            delivery.Quantity = quantity;
            delivery.Arrived = Arrived;
            DatabaseFacade.UpdateStorageOrder(delivery);

            View.UpdateStorageItems();
        }
    }
}
