using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_ticket_platform_Model
{
    public class MOD_OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceSnapshot { get; set; }
    }
}
