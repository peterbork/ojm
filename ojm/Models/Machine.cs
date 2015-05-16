using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    public class Machine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Models.ProductOrder ProductOrder { get; set; }

        public Machine(int id, string name, string type) {
            ID = id;
            Name = name;
            Type = type;
        }
    }
}
