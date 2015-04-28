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

        public Product(int id, string name, int instock)
        {
            ID = id;
            Name = name;
            InStock = instock;
        }
    }
}
