using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ojm.Models;

namespace ojm.Controllers {
    class Controller {

        List<Product> products = new List<Product>();

        // CUSTOMER METHODS
        public void AddCustomer(string companyname, string cvr, string address, string email, string phonenumber, string contactperson) {
            
            Customer customer = new Customer(companyname, cvr, address, email, phonenumber, contactperson);
            if (IsCustomerExsisting(customer.CVR) != true) {
                DatabaseFacade.AddCustomer(customer);
                System.Windows.MessageBox.Show("Kunden blev oprettet", "OJM");
            }
            else {
                System.Windows.MessageBox.Show("Kunden eksisterer allerede", "OJM");
            }
           
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
            return DatabaseFacade.GetCustomers();
        }

        // STORAGE METHODS
        public List<Models.Product> GetStorageItems()
        {
            return DatabaseFacade.GetStorageItems();
        }

        public Dictionary<string, string> GetStorageItem(int index)
        {
            Dictionary<string, string> storageItem = new Dictionary<string, string>();

            storageItem.Add("Name", products[index].Name);
            storageItem.Add("InStock", products[index].InStock + "");

            return storageItem;
        }
        public void UpdateStorageItem(int id, string name, int instock)
        {
            Product product = new Product(id, name, instock);
            DatabaseFacade.UpdateStorageItem(product);
        }

        public void RegisterDelivery(int id, int quantity, int pid, string name, int instock)
        {
            Delivery delivery = new Delivery(id, quantity);
            Product product = new Product(pid, name, instock);

            DatabaseFacade.RegisterDelivery(delivery);
            DatabaseFacade.UpdateStorageItem(product);
        }

    }
}
