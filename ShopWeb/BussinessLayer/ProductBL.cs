using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntity.Entities;
using DataAccessLayer;

namespace BusinessLogic
{
    public class ProductBLL
    {
        public static List<Product> GetAllProducts()
        {
            return ProductDAL.GetAllProducts();
        }

        public static void DeleteProduct(int productId)
        {
            ProductDAL.DeleteProduct(productId);
        }

        public static bool UpdateProduct(int productId, string productName, decimal price, int stock, 
            string quantityPerUnit, int unitsOnOrder, int reorderLevel, bool discontinued)
        {
            // Validate input
            if (string.IsNullOrEmpty(productName))
                return false;
            if (price < 0)
                return false;
            if (stock < 0)
                return false;
            if (unitsOnOrder < 0)
                return false;
            if (reorderLevel < 0)
                return false;

            // Call DAL to update product
            return ProductDAL.UpdateProduct(productId, productName, price, stock, 
                quantityPerUnit, unitsOnOrder, reorderLevel, discontinued);
        }

        public static bool AddProductImage(int productId, string imagePath, string altText, string mainImage, byte[] imageBlob)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(imagePath))
                    return false;
                if (string.IsNullOrEmpty(mainImage))
                    mainImage = "N";

                // Call DAL to insert image with BLOB
                return ProductDAL.InsertProductImage(productId, imagePath, altText, mainImage, imageBlob) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<ProductImage> GetProductImages(int productId)
        {
            return ProductDAL.GetProductImages(productId);
        }

        public static bool DeleteProductImage(int imageId)
        {
            return ProductDAL.DeleteProductImage(imageId);
        }

        public static bool SetMainImage(int productId, int imageId)
        {
            return ProductDAL.UpdateMainImage(productId, imageId);
        }

        public static List<Category> GetAllCategories()
        {
            return ProductDAL.GetAllCategories();
        }

        public static int InsertProduct(string productName, int categoryId, decimal price, int stock, string quantityPerUnit = null)
        {
            // Validate input
            if (string.IsNullOrEmpty(productName))
                return -1;
            if (categoryId <= 0)
                return -1;
            if (price < 0)
                return -1;
            if (stock < 0)
                return -1;

            // Call DAL to insert product
            return ProductDAL.InsertProduct(productName, categoryId, price, stock, quantityPerUnit);
        }

        public static Product GetProductById(int productId)
        {
            try
            {
                return ProductDAL.GetProductById(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy thông tin sản phẩm: " + ex.Message);
            }
        }

        public static string GetMainImage(List<ProductImage> images)
        {
            if (images == null || !images.Any()) 
                return "Uploads/products/no-image.png";

            var mainImage = images.FirstOrDefault(i => i.MainImage == "Y");
            return mainImage?.ImagePath ?? images.First().ImagePath;
        }

        public static string GetHoverImage(List<ProductImage> images)
        {
            if (images == null || !images.Any()) 
                return "Uploads/products/no-image.png";

            var hoverImage = images.FirstOrDefault(i => i.MainImage == "N");
            return hoverImage?.ImagePath ?? images.First().ImagePath;
        }

        public static bool UpdateProductImage(int imageId, string imagePath, string altText, string mainImage, byte[] imageBlob)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(imagePath))
                    return false;
                if (string.IsNullOrEmpty(mainImage))
                    mainImage = "N";

                // Call DAL to update image
                return ProductDAL.UpdateProductImage(imageId, imagePath, altText, mainImage, imageBlob);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
