﻿using System;
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
        List<QualityControl> qualitycontrols = new List<QualityControl>();
        List<Production> productions = new List<Production>();
        List<MachineSchedule> machineschedules = new List<MachineSchedule>();

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
            productorder.Add("ID", productorders[index].ID.ToString());
            productorder.Add("Name", productorders[index].Name);
            productorder.Add("Description", productorders[index].Description);
            productorder.Add("CompanyName", productorders[index].Customer.CompanyName);

            return productorder;
        }

        public List<ProductOrderMaterialUsage> GetProductOrderMaterials(int index) {
            return productorders[index].Materials;
        }

        public Dictionary<string, decimal> GetProductOrderMaterialStrings(int index) {
            Dictionary<string, decimal> materials = new Dictionary<string, decimal>();

            foreach (ProductOrderMaterialUsage material in productorders[index].Materials) {
                materials.Add(material.Material.Name, material.Usage);
            }

            return materials;
        }

        public List<Machine> GetProductOrderMachines(int productOrderIndex) {
            return productorders[productOrderIndex].Machines;
        }

        public List<string> GetProductOrderMachineNames(int index) {
            List<string> machines = new List<string>();

            foreach (Machine machine in productorders[index].Machines) {
                machines.Add(machine.Name);
            }

            return machines;
        }

        public void AddProductOrder(string name, string description, int customerIndex, List<int> materialIndexes, List<decimal> materialUsages) {
            ProductOrder productorder = new ProductOrder(0, name, description, customers[customerIndex]);
            int a = 0;
            foreach (int i in materialIndexes) {
                ProductOrderMaterialUsage m = new ProductOrderMaterialUsage(0, materialUsages[a], materials[i]);
                productorder.Materials.Add(m);
                a++;
            }
            DatabaseFacade.AddProductOrder(productorder);
            System.Windows.MessageBox.Show("Produktordren er blevet tilføjet", "OJM");
        }

        public void UpdateProductOrder(int productorderIndex, string name, string description, int customerIndex, List<int> materialIndexes, List<decimal> materialUsages) {
            ProductOrder productorder = productorders[productorderIndex];
            productorder.Name = name;
            productorder.Description = description;
            productorder.Customer = customers[customerIndex];

            int a = 0;
            productorder.Materials = new List<ProductOrderMaterialUsage>();
            foreach (int i in materialIndexes) {
                ProductOrderMaterialUsage m = new ProductOrderMaterialUsage(0, materialUsages[a], materials[i]);
                productorder.Materials.Add(m);
                a++;
            }

            DatabaseFacade.UpdateProductOrder(productorder);
            View.UpdateProductOrders();
            System.Windows.MessageBox.Show("Produktordren er blevet opdateret", "OJM");
        }

        public List<Machine> GetProductOrderAndMachine() {
            List<Machine> machineswithproductorders = new List<Machine>();
            foreach (ProductOrder productorder in GetProductOrders())
	        {
                foreach (Machine machine in productorder.Machines) {
                    machine.ProductOrder = productorder;
                    machineswithproductorders.Add(machine);
                }
	        }
            return machineswithproductorders;
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
        #region QualityControl
        public void AddQualityControl(string name, string description, string frequency, string mintol, string maxtol, int machineIndex) {
            QualityControl qualitycontrol = new QualityControl(name, description, int.Parse(frequency), Convert.ToDecimal(mintol), Convert.ToDecimal(maxtol));
            qualitycontrol.Machine = machines[machineIndex];
            qualitycontrol.ProductOrder = machines[machineIndex].ProductOrder;
            DatabaseFacade.AddQualityControl(qualitycontrol);

        }
        public void UpdateQualityControl(int index, string name, string description, int frequency, decimal mintol, decimal maxtol) {
            QualityControl qualitycontrol = qualitycontrols[index];
            qualitycontrol.Name = name;
            qualitycontrol.Description = description;
            qualitycontrol.Frequency = frequency;
            qualitycontrol.MinTol = mintol;
            qualitycontrol.MaxTol = maxtol;
            DatabaseFacade.UpdateQualityControl(qualitycontrol);
        }
        public List<Dictionary<string, string>> GetQualityControl(int selectedmachineindex) {
            List<Dictionary<string, string>> _qualitycontrols = new List<Dictionary<string, string>>();
           
            machines = GetProductOrderAndMachine();
            qualitycontrols = DatabaseFacade.GetQualityControlsFromProductOrderAndMachine(machines[selectedmachineindex].ProductOrder.ID, machines[selectedmachineindex].ID);
            foreach (QualityControl control in qualitycontrols) {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("ID", control.ID.ToString());
                dic.Add("Name", control.Name.ToString());
                dic.Add("Description", control.Description);
                dic.Add("Frequency", control.Frequency.ToString());
                dic.Add("MinTol", control.MinTol.ToString());
                dic.Add("MaxTol", control.MaxTol.ToString());
                _qualitycontrols.Add(dic);
            }
            return _qualitycontrols;
        }
        #endregion

        #region Production
        public List<Production> GetProductions() {
            productions = DatabaseFacade.GetProductions();
            foreach (Production production in productions) {
                foreach (ProductOrder productorder in productorders) {
                    if (productorder.ID == production.ProductOrder.ID) {
                        production.ProductOrder.Name = productorder.Name;
                        production.ProductOrder.Description = productorder.Description;
                        production.ProductOrder.Customer = productorder.Customer;
                        production.ProductOrder.Machines = productorder.Machines;
                        production.ProductOrder.Materials = productorder.Materials;
                    }
                }
            }
            return productions;

        }

        public void AddProduction(int idindex, decimal quantity, DateTime deadline){
            Models.ProductOrder productorder = productorders[idindex];
            DatabaseFacade.AddProduction(productorder, quantity, deadline);
        }

        #endregion

        #region MachineSchedule
        public List<MachineSchedule> GetMachineSchedules() {
            machineschedules = DatabaseFacade.GetMachineSchedules();
            return machineschedules;
        }
        public Dictionary<DateTime, string> GetMachineScheduleDateTimeAndName() {
            GetMachineSchedules();
            Dictionary<DateTime, string> datetimeandname = new Dictionary<DateTime, string>();
            foreach (MachineSchedule schedule in machineschedules) {
                datetimeandname.Add(schedule.Date, schedule.Machine.Name);
            }
            return datetimeandname;
        }
        public void AddMachineSchedule(int productorderindex, Dictionary<int, List<DateTime>> indexanddatetimes) {
            List<Machine> machines = GetProductOrderMachines(productorderindex);
            int productorderid = productorders[productorderindex].ID;
            Dictionary<int, List<DateTime>> idanddatetimes = new Dictionary<int, List<DateTime>>();
            foreach (int key in indexanddatetimes.Keys) {
                idanddatetimes.Add(machines[key].ID, indexanddatetimes[key]);
            }
            DatabaseFacade.AddMachineSchedules(productorderid, idanddatetimes);

            //2015-05-23 10:00:00.0000000
        }
        #endregion
    }
}
