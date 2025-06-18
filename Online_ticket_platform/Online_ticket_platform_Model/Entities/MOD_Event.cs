using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_ticket_platform_Model
{
    public class MOD_Event
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Name) && 
                       !string.IsNullOrEmpty(Location) && 
                       EventDate > DateTime.Now;
            }
        }
    }
}
