using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    class ProductOrderMaterialUsage
    {
        public int ID { get; set; }
        public decimal Usage { get; set; }
        public ProductOrder ProductOrder { get; set; }
        public Material Material { get; set; }

        public ProductOrderMaterialUsage(int id, decimal usage, ProductOrder productOrder, Material material)
        {
            ID = id;
            Usage = usage;
            ProductOrder = productOrder;
            Material = material;
        }
    }
}
