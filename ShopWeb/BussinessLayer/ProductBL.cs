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
    }
}
