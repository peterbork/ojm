using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    public class Material
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int InStock { get; set; }
        public string Type { get; set; }
        public int Tolerance { get; set; }
        public int Reserved { get; set; }
        public Customer Customer { get; set; }

        public List<Delivery> Deliveries;

        public Material(int id, string name, int instock)
        {
            ID = id;
            Name = name;
            InStock = instock;
            Deliveries = new List<Delivery>();
        }

        public Material(int id, string name, int instock, string type, int tolerance, int reserved) {
            ID = id;
            Name = name;
            InStock = instock;
            Type = type;
            Tolerance = tolerance;
            Reserved = reserved;
            Deliveries = new List<Delivery>();
        }

        public Material(int id, string name, int instock, string type, int tolerance, int reserved, Customer customer) 
        : this(id, name, instock, type, tolerance, reserved) {
            Customer = customer;
        }
    }
}
