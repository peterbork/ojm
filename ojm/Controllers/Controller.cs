using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ojm.Models;

namespace ojm.Controllers {
    class Controller {

        // CUSTOMER METHODS
        public void AddCustomer(string companyname, string cvr, string address, string email, string phonenumber, string contactperson) {
            
            Customer customer = new Customer(companyname, cvr, address, email, phonenumber, contactperson);

            if (DatabaseFacade.IsCustomerExsisting(customer.CVR) != true) {
                DatabaseFacade.AddCustomer(customer);
                System.Windows.MessageBox.Show("Kunden blev oprettet", "OJM");
            }
            else {
                System.Windows.MessageBox.Show("Kunden eksistere allerede", "OJM");
            }
        }

        // STORAGE METHODS
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
