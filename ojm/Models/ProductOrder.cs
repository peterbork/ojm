using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    public class ProductOrder
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductOrderMaterialUsage> Materials { get; set; }
        public List<Machine> Machines { get; set; }
        public Customer Customer { get; set; }

        public ProductOrder(int id) {
            ID = id;
        }
        public ProductOrder(int id, string name, string description, Customer customer)
        {
            ID = id;
            Name = name;
            Description = description;
            Customer = customer;
            Materials = new List<ProductOrderMaterialUsage>();
        }
        public ProductOrder(int id, string name, string description, Customer customer, List<ProductOrderMaterialUsage> materials) {
            ID = id;
            Name = name;
            Description = description;
            Customer = customer;
            Materials = materials;
        }
        public ProductOrder(string name, string description, Customer customer, List<ProductOrderMaterialUsage> materials)
        {
            Name = name;
            Description = description;
            Customer = customer;
            Materials = materials;
        }
        public ProductOrder(int id, string name, string description, Customer customer, List<ProductOrderMaterialUsage> materials, List<Machine> machines)
        {
            ID = id;
            Name = name;
            Description = description;
            Customer = customer;
            Materials = materials;
            Machines = machines;
        }
    }
}
