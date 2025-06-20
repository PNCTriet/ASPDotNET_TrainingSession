using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_ticket_platform_Model
{
    public class MOD_Ticket
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public int Total { get; set; }
        public int Sold { get; set; }
        public DateTime StartSaleDate { get; set; }
        public DateTime EndSaleDate { get; set; }
        public bool IsActive { get; set; }
    }
}
