using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _211792H.WebPages.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet datausers = getusers();
            RepeaterProd.DataSource = datausers;
            RepeaterProd.DataBind();
        }

        private DataSet getusers()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [User]", conn);
            DataSet datausers = new DataSet();
            cmd.Fill(datausers);
            return datausers;
        }

        protected void Deleteuser(object sender, EventArgs e)
        {
            var button = (LinkButton)sender;
            var agrs = button.CommandArgument.ToString();
            Debug.WriteLine(agrs);
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string deleteitem = "DELETE FROM [User] WHERE Id=@id";
            SqlCommand cmd = new SqlCommand(deleteitem, conn);
            cmd.Parameters.AddWithValue("id", agrs);
            cmd.ExecuteNonQuery();
            conn.Close();
            Response.Redirect(Request.RawUrl);
        }
    }
}