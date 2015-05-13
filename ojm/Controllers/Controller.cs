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
        List<ProductOrder> productorders = new List<ProductOrder>();
        List<Machine> machines = new List<Machine>();

        MainView View;

        public void SetView(MainView view) {
            View = view;
        }

        #region Customers

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

        #endregion
        #region Materials

        public List<Models.Material> GetMaterials()
        {
            materials = DatabaseFacade.GetMaterials();
            return materials;
        }

        public Dictionary<string, string> GetMaterial(int index)
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
        public void UpdateMaterial(int index, string name, int instock,  string type, int tolerance, int reserved, int customerIndex)
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
            
            DatabaseFacade.UpdateMaterial(material);
        }

        public void RegisterDelivery(int deliveryIndex, int materialIndex)
        {
            Material material = materials[materialIndex];
            Delivery delivery = material.Deliveries[deliveryIndex];
            material.InStock += delivery.Quantity;

            DatabaseFacade.RegisterDelivery(delivery);
            DatabaseFacade.UpdateMaterial(material);
        }


        public void AddMaterial(string name, int instock, string type, int tolerance, int reserved, int customer)
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
            
            DatabaseFacade.AddMaterial(material);
        }

        public List<Delivery> GetMaterialDeliveries(int materialIndex) {

            materials[materialIndex].Deliveries = DatabaseFacade.GetMaterialDeliveries(materials[materialIndex].ID);

            return materials[materialIndex].Deliveries;
        }

        public void NewDelivery(int materialIndex) {
            Views.DeliveryView view = new Views.DeliveryView();
            view.Show();

            view.SetController(this);
            view.SetMaterial(materialIndex, materials[materialIndex].Name);
        }

        public void UpdateDelivery(int materialIndex, int deliveryIndex) {
            Material material = materials[materialIndex];
            Dictionary<string, string> delivery = new Dictionary<string,string>();
            delivery.Add("DeliveryDate", material.Deliveries[deliveryIndex].DeliveryDate.ToString());
            delivery.Add("Quantity", material.Deliveries[deliveryIndex].Quantity.ToString());

            Views.DeliveryView view = new Views.DeliveryView();
            view.Show();

            view.SetController(this);
            view.SetMaterial(materialIndex, material.Name);
            view.SetDelivery(deliveryIndex, delivery);
        }

        public void OrderMaterial(int materialIndex, DateTime deliveryDate, int quantity) {
            Delivery delivery = new Delivery(0, deliveryDate, quantity);
            materials[materialIndex].Deliveries.Add(delivery);

            DatabaseFacade.OrderMaterial(materials[materialIndex].ID, delivery);

            View.UpdateMaterials();
        }

        public void UpdateMaterialDelivery(int materialIndex, int deliveryIndex, DateTime deliveryDate, int quantity, bool Arrived)
        {
            Material material = materials[materialIndex];
            Delivery delivery = material.Deliveries[deliveryIndex];
            if (Arrived && !delivery.Arrived) {
                // Update the storage item's quantity
                material.InStock += quantity;
                DatabaseFacade.UpdateMaterial(material);
            }

            // Update the Delivery
            delivery.DeliveryDate = deliveryDate;
            delivery.Quantity = quantity;
            delivery.Arrived = Arrived;
            DatabaseFacade.UpdateMaterialDelivery(delivery);

            View.UpdateMaterials();
        }
        public Dictionary<int, string> GetMaterialsFromCustomerIndex(int customerIndex) {
            Dictionary<int, string> _materials = new Dictionary<int, string>();
            int i = 0;
            foreach (Material material in materials) {
                if (material.Customer == null || material.Customer.ID == customers[customerIndex].ID) {
                    _materials.Add(i, material.Name);
                }
                i++;
            }
            return _materials;
        }

        #endregion
        #region ProductOrders

        public List<Models.ProductOrder> GetProductOrders() { 
            productorders = DatabaseFacade.GetProductOrders();
            return productorders;
        }
        public Dictionary<string, string> GetProductOrder(int index) {
            Dictionary<string, string> productorder = new Dictionary<string, string>();
            productorder.Add("Name", productorders[index].Name);
            productorder.Add("Description", productorders[index].Description);
            productorder.Add("CompanyName", productorders[index].Customer.CompanyName);

            return productorder;
        }

        public List<Material> GetProductOrderMaterials(int index) {
            return productorders[index].Materials;
        }

        public List<string> GetProductOrderMaterialStrings(int index) {
            List<string> materials = new List<string>();

            foreach (Material material in productorders[index].Materials) {
                materials.Add(material.Name);
            }

            return materials;
        }

        public List<Machine> GetProductOrderMachines(int productOrderIndex) {
            System.Windows.MessageBox.Show(productorders[productOrderIndex].Machines.Count + "");
            return productorders[productOrderIndex].Machines;
        }

        public List<string> GetProductOrderMachineNames(int index) {
            List<string> machines = new List<string>();

            foreach (Machine machine in productorders[index].Machines) {
                machines.Add(machine.Name);
            }

            return machines;
        }

        public void AddProductOrder(string name, string description, int customerIndex, List<int> materialIndexes) {
            ProductOrder productorder = new ProductOrder(0, name, description, customers[customerIndex]);
            foreach (int i in materialIndexes) {
                productorder.Materials.Add(materials[i]);
            }
            DatabaseFacade.AddProductOrder(productorder);
            System.Windows.MessageBox.Show("Produktordren er blevet tilføjet", "OJM");
        }

        public void UpdateProductOrder(int productorderIndex, string name, string description, int customerIndex, List<int> materials) {
            ProductOrder productorder = productorders[productorderIndex];
            productorder.Name = name;
            productorder.Description = description;
            productorder.Customer = customers[customerIndex];

            productorder.Materials = new List<Material>();
            foreach (int material in materials) {
                productorder.Materials.Add(this.materials[material]);
            }

            DatabaseFacade.UpdateProductOrder(productorder);
            View.UpdateProductOrders();
            System.Windows.MessageBox.Show("Produktordren er blevet opdateret", "OJM");
        }

        #endregion
        #region Machines

        public List<Models.Machine> GetMachines() {

            machines = DatabaseFacade.GetMachines();
            return machines;
        }
        public Dictionary<int, string> GetMachineNames() {
            Dictionary<int, string> machinenames = new Dictionary<int, string>();
            int i = 0;
            foreach (Machine machine in machines) {
                machinenames.Add(i, machine.Name);
                i++;
            }
            return machinenames;
        }

        public void AddMachineToProductOrder(List<int> sequence, List<int> machineindexes, int selectedproductorder) {
            List<int> machineids = new List<int>();
            productorders[selectedproductorder].Machines = new List<Machine>();
            foreach (int index in machineindexes) {
                machineids.Add(machines[index].ID);
                productorders[selectedproductorder].Machines.Add(machines[index]);
            }
            
            DatabaseFacade.AddMachinesToProductOrder(sequence, machineids, productorders[selectedproductorder].ID);
            View.UpdateProductOrders();
        }
        #endregion
    }
}
