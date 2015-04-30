using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ojm.Models;

namespace ojm.Controllers {
    class Controller {

        List<Product> products = new List<Product>();
        List<Customer> customers = new List<Customer>();

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
        public List<Models.Product> GetStorageItems()
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
        public void UpdateStorageItem(int index, string name, int instock)
        {
            Product product = products[index];
            product.Name = name;
            product.InStock = instock;
            
            DatabaseFacade.UpdateStorageItem(product);
        }

        public void RegisterDelivery(int dindex, int pindex)
        {
            Product product = products[pindex];
            Delivery delivery = product.Deliveries[dindex];
            product.InStock += delivery.Quantity;

            DatabaseFacade.RegisterDelivery(delivery);
            DatabaseFacade.UpdateStorageItem(product);
        }

    }
}
