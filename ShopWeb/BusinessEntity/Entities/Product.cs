using System;
using System.Collections.Generic;

namespace BusinessEntity.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public List<ProductImage> Images { get; set; }
    }

    public class ProductImage
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string ImagePath { get; set; }
        public string AltText { get; set; }
        public string MainImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] ImageBlob { get; set; }
    }
} 