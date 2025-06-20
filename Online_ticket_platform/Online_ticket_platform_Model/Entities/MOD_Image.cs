using System;

namespace Online_ticket_platform_Model
{
    public class MOD_Image
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int? FileSize { get; set; }
        public string AltText { get; set; }
        public int? UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }
    }
} 