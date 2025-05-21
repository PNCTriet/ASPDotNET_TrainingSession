using System.Collections.Generic;
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

        public static bool UpdateProduct(int productId, string productName, decimal price, int stock)
        {
            // Validate input
            if (string.IsNullOrEmpty(productName))
                return false;
            if (price < 0)
                return false;
            if (stock < 0)
                return false;

            // Call DAL to update product
            return ProductDAL.UpdateProduct(productId, productName, price, stock);
        }

        public static int AddProductImage(int productId, string imagePath, string altText, string mainImage)
        {
            // Validate input
            if (string.IsNullOrEmpty(imagePath))
                return -1;
            if (string.IsNullOrEmpty(mainImage))
                mainImage = "N";

            // Call DAL to insert image
            return ProductDAL.InsertProductImage(productId, imagePath, altText, mainImage);
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
    }
}
