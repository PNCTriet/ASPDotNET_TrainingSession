using System;

namespace BusinessEntity.Entities
{
    public class ProductImage
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string ImagePath { get; set; }
        public string AltText { get; set; }
        public string MainImage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 