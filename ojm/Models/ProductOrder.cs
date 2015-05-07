using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    class ProductOrder
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Material> Materials { get; set; }
        public Customer Customer { get; set; }

        public ProductOrder(int id, string name, string description, Customer customer)
        {
            ID = id;
            Name = name;
            Description = description;
            Customer = customer;
            Materials = new List<Material>();
        }
        public ProductOrder(string name, string description, Customer customer, List<Material> materials)
        {
            Name = name;
            Description = description;
            Customer = customer;
            Materials = materials;
        }
    }
}
