using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models {
    class QualityControl {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Frequency { get; set; }
        public decimal MinTol { get; set; }
        public decimal MaxTol { get; set; }
        public ProductOrder ProductOrder { get; set; }
        public Machine Machine { get; set; }

        public QualityControl(int id, string name, string description, int frequency, decimal mintol, decimal maxtol) {
            ID = id;
            Name = name;
            Description = description;
            Frequency = frequency;
            MinTol = mintol;
            MaxTol = maxtol;
        }
    }
}
