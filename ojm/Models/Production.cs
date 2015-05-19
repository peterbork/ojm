using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    public class Production
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Deadline { get; set; }
        public ProductOrder ProductOrder { get; set; }
        public List<MachineSchedule> MachineSchedules { get; set; }

        public Production(int id, decimal amount, DateTime deadline, ProductOrder productionOrder)
        {
            MachineSchedules = new List<MachineSchedule>();
            ID = id;
            Amount = amount;
            Deadline = deadline;
            ProductOrder = productionOrder;
        }
    }
}
