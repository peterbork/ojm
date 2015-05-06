using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ojm.Models;

namespace ojm.Controllers {
    public class Controller {

        List<Material> products = new List<Material>();
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
            products = DatabaseFacade.GetStorageItems();
            return products;
        }

        public Dictionary<string, string> GetStorageItem(int index)
        {
            Dictionary<string, string> storageItem = new Dictionary<string, string>();

            storageItem.Add("Name", products[index].Name);
            storageItem.Add("InStock", products[index].InStock + "");
            storageItem.Add("Type", products[index].Type);
            storageItem.Add("Tolerance", products[index].Tolerance + "");
            storageItem.Add("Reserved", products[index].Reserved + "");

            if (products[index].Customer != null) {
                int customerIndex = -1;
                for (int i = 0; i < customers.Count && customerIndex == -1; i++) {
                    if (products[index].Customer.CVR == customers[i].CVR)
                        customerIndex = i;
                }
                storageItem.Add("Customer", customerIndex + "");
            }
            

            return storageItem;
        }
        public void UpdateStorageItem(int index, string name, int instock,  string type, int tolerance, int reserved, int customerIndex)
        {
            Material product = products[index];
            product.Name = name;
            product.InStock = instock;
            product.Type = type;
            product.Tolerance = tolerance;
            product.Reserved = reserved;
            if (customerIndex > -1) {
                product.Customer = customers[customerIndex];
            }
            
            DatabaseFacade.UpdateStorageItem(product);
        }

        public void RegisterDelivery(int dindex, int pindex)
        {
            Material product = products[pindex];
            Delivery delivery = product.Deliveries[dindex];
            product.InStock += delivery.Quantity;

            DatabaseFacade.RegisterDelivery(delivery);
            DatabaseFacade.UpdateStorageItem(product);
        }


        internal void AddStorageItem(string name, int instock, string type, int tolerance, int reserved, int customer)
        {
            Material product;
            
            if (customer != -1 )
            {
                product = new Material(0, name, instock, type, tolerance, reserved, customers[customer]);
            }
            else
            {
                product = new Material(0, name, instock, type, tolerance, reserved);
            }
            
            DatabaseFacade.AddStorageItem(product);
        }

        internal List<Delivery> GetStorageItemOrders(int selectedProduct) {

            products[selectedProduct].Deliveries = DatabaseFacade.GetStorageItemOrders(products[selectedProduct].ID);

            return products[selectedProduct].Deliveries;
        }

        internal void NewDelivery(int productIndex) {
            Views.DeliveryView view = new Views.DeliveryView();
            view.Show();

            view.SetController(this);
            view.SetProduct(productIndex, products[productIndex].Name);
        }

        public void UpdateDelivery(int productIndex, int deliveryIndex) {
            Material product = products[productIndex];
            Dictionary<string, string> delivery = new Dictionary<string,string>();
            delivery.Add("DeliveryDate", product.Deliveries[deliveryIndex].DeliveryDate.ToString());
            delivery.Add("Quantity", product.Deliveries[deliveryIndex].Quantity.ToString());

            Views.DeliveryView view = new Views.DeliveryView();
            view.Show();

            view.SetController(this);
            view.SetProduct(productIndex, product.Name);
            view.SetDelivery(deliveryIndex, delivery);
        }

        internal void OrderStorageItem(int productIndex, DateTime deliveryDate, int quantity) {
            Delivery delivery = new Delivery(0, deliveryDate, quantity);
            products[productIndex].Deliveries.Add(delivery);

            DatabaseFacade.OrderStorageItem(products[productIndex].ID, delivery);

            View.UpdateStorageItems();
        }

        internal void UpdateStorageOrder(int productIndex, int deliveryIndex, DateTime deliveryDate, int quantity, bool Arrived) {
            Material product = products[productIndex];
            Delivery delivery = product.Deliveries[deliveryIndex];
            if (Arrived && !delivery.Arrived) {
                // Update the storage item's quantity
                product.InStock += quantity;
                DatabaseFacade.UpdateStorageItem(product);
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
