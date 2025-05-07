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
    }
}
