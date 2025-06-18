using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_ticket_platform_Model
{
    public class MOD_Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        // public string Website { get; set; }
        // public string LogoUrl { get; set; }
        // public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
