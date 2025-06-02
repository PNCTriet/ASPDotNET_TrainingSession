using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessEntity.Entities;

namespace TrietPhamShopWeb
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            try
            {
                // Lấy danh sách sản phẩm từ Business Layer
                var products = ProductBLL.GetAllProducts();
                
                // Bind dữ liệu vào Repeater
                ProductGridRepeater.DataSource = products;
                ProductGridRepeater.DataBind();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                Response.Write($"Error loading products: {ex.Message}");
            }
        }

        protected string GetColorOptionsHtml(List<string> colorOptions)
        {
            if (colorOptions == null || colorOptions.Count == 0)
                return string.Empty;

            var html = new System.Text.StringBuilder();
            foreach (var color in colorOptions)
            {
                html.Append($"<span class=\"color-option\" style=\"background-color: {color}\"></span>");
            }
            return html.ToString();
        }

        protected string GetProductImage(object productImages)
        {
            var images = productImages as List<ProductImage>;
            return ProductBLL.GetMainImage(images);
        }
    
        protected string GetProductHoverImage(object productImages)
        {
            var images = productImages as List<ProductImage>;
            return ProductBLL.GetHoverImage(images);
        }

        protected string ShowNewBadge(object unitsInStock)
        {
            if (unitsInStock == null) return "";
            int stock = Convert.ToInt32(unitsInStock);
            return stock > 0 ? "<span class=\"badge-new\">New</span>" : "";
        }

        protected string ShowDiscount(object unitPrice)
        {
            if (unitPrice == null) return "";
            decimal price = Convert.ToDecimal(unitPrice);
            
            // Logic giảm giá: nếu giá < 1,000,000đ thì giảm 20%
            if (price < 1000000)
            {
                decimal discountedPrice = price * 0.8m;
                return $"<span class=\"original-price\">{price:N0}đ</span>" +
                       $"<span class=\"badge-discount\">-20%</span>";
            }
            return "";
        }
    }
}