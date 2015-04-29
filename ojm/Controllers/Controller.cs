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
            
            Customer _customer = new Customer(companyname, cvr, address, email, phonenumber, contactperson);
            if (IsCustomerExsisting(_customer.CVR) != true) {
                DatabaseFacade.AddCustomer(_customer);
                System.Windows.MessageBox.Show("Kunden blev oprettet", "OJM");
            }
            else {
                System.Windows.MessageBox.Show("Kunden eksisterer allerede", "OJM");
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
            return DatabaseFacade.GetCustomers();
        }
        public int GetIDFromCustomerModel(Models.Customer _customer){
            return _customer.ID;
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
