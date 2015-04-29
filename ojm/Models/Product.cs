using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int InStock { get; set; }
        public string Type { get; set; }
        public int Tolerance { get; set; }
        public Customer Customer { get; set; }

        public List<Delivery> Deliveries;

        public Product(int id, string name, int instock)
        {
            ID = id;
            Name = name;
            InStock = instock;
            Deliveries = new List<Delivery>();
        }
    }
}
