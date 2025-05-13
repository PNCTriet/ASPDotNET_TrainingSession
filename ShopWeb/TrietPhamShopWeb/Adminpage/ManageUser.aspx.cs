using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrietPhamShopWeb.Adminpage
{
    public partial class ManageUser : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            //using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            //{
            //    string query = @"SELECT UserID, FirstName, LastName, Email, Phone 
            //                   FROM Users 
            //                   ORDER BY UserID DESC";

            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        try
            //        {
            //            conn.Open();
            //            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            //            {
            //                DataTable dt = new DataTable();
            //                adapter.Fill(dt);
            //                gvUsers.DataSource = dt;
            //                gvUsers.DataBind();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            // Log error
            //            throw new Exception("Error loading users: " + ex.Message);
            //        }
            //    }
            //}
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                // Redirect to edit page
                Response.Redirect($"EditUser.aspx?id={e.CommandArgument}");
            }
            else if (e.CommandName == "DeleteUser")
            {
                DeleteUser(Convert.ToInt32(e.CommandArgument));
            }
        }

        private void DeleteUser(int userId)
        {
            //using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            //{
            //    string query = "DELETE FROM Users WHERE UserID = @UserID";
            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        try
            //        {
            //            cmd.Parameters.AddWithValue("@UserID", userId);
            //            conn.Open();
            //            cmd.ExecuteNonQuery();
            //            LoadUsers(); // Reload the grid
            //        }
            //        catch (Exception ex)
            //        {
            //            // Log error
            //            throw new Exception("Error deleting user: " + ex.Message);
            //        }
            //    }
            //}
        }
    }
}