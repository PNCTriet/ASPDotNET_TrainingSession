using System;

namespace Online_ticket_platform_Model
{
    public class MOD_ImageLink
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? EventId { get; set; }
        public string UsageType { get; set; }
        public DateTime LinkedAt { get; set; }
        
        // Navigation properties
        public MOD_Image Image { get; set; }
        public MOD_Organization Organization { get; set; }
        public MOD_Event Event { get; set; }
    }
} 