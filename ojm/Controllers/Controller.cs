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
                System.Windows.MessageBox.Show("Kunden eksistere allerede", "OJM");
            }
           
        }
        public bool IsCustomerExsisting(string cvr) {
            Models.Customer _customer = DatabaseFacade.GetCustomerFromCVR(cvr);
            if (_customer.CVR != "") {
                return true;
            }
            else {
                return false;
            }

        }

        // STORAGE METHODS
        public List<string> GetStorageItems()
        {
            List<string> StorageItemsList = new List<string>();
            products = DatabaseFacade.GetStorageItems();

            foreach (Product product in products)
            {
                StorageItemsList.Add(product.Name + "\t\tPå lager: " + product.InStock);
            }

            return StorageItemsList;
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
