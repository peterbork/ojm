using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    public class MachineSchedule
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public Machine Machine { get; set; }
        public Production Production { get; set; }

        public MachineSchedule(int id, DateTime date, Machine machine, Production production)
        {
            ID = id;
            Date = date;
            Machine = machine;
            Production = production;
        }
        public MachineSchedule(int id, DateTime date, Machine machine) {
            ID = id;
            Date = date;
            Machine = machine;
        }
    }
}
