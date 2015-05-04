using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models
{
    public class Delivery
    {
        public int ID { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Quantity { get; set; }
        public bool Arrived { get; set; }

        public Delivery(int id, DateTime delivery, int quantity)
        {
            ID = id;
            DeliveryDate = delivery;
            Quantity = quantity;
            Arrived = false;
        }
        public Delivery(int id, int quantity)
        {
            ID = id;
            Quantity = quantity;
        }
    } 
}
