using Microsoft.Ajax.Utilities;
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
    public partial class GiveDiscountProd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DataSet dataproducts = getproducts();
            RepeaterProd.DataSource = dataproducts;
            RepeaterProd.DataBind();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();


            if (!IsPostBack)
            {
                string restarttable = "DELETE FROM [TempItemDiscount]";
                SqlCommand cmd = new SqlCommand(restarttable, conn);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        private DataSet getproducts()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts]", conn);
            DataSet dataproducts = new DataSet();
            cmd.Fill(dataproducts);
            return dataproducts;
        }

        protected void selectItem(object sender, EventArgs e)
        {
            Button butt = (Button)sender;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string itemexist = "Select * FROM [TempItemDiscount] Where Id=@id";
            SqlCommand cmd = new SqlCommand(itemexist, conn);
            cmd.Parameters.AddWithValue("@id", butt.CommandArgument.ToString());
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader["Id"].ToString() != "")
                {
                    reader.Close();
                    string removeitem = "DELETE FROM [TempItemDiscount] WHERE Id=@id";
                    SqlCommand removecmd = new SqlCommand(removeitem, conn);
                    removecmd.Parameters.AddWithValue("@id", butt.CommandArgument.ToString());
                    removecmd.ExecuteNonQuery();
                    butt.Text = "Select Item";
                }
            }

            else
            {
                reader.Close();
                string insertitem = "INSERT INTO [TempItemDiscount](Id) Values (@id)";
                SqlCommand insertcmd = new SqlCommand(insertitem, conn);
                insertcmd.Parameters.AddWithValue("@id", butt.CommandArgument.ToString());
                insertcmd.ExecuteNonQuery();
                butt.Text = "Unselect Item";
            }
        
            conn.Close();

        }

        protected void Submit(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string itemexist = "Select COUNT(*) FROM [TempItemDiscount]";
            SqlCommand cmd = new SqlCommand(itemexist, conn);
            bool exists = false;

            exists = (int)cmd.ExecuteScalar() > 0;

            if (exists)
            {
                submitfailed.Style.Add("display", "None");
                Response.Redirect("GiveDiscount.aspx");
            }

            else
            {
                gametitles.SelectedIndex = -1;
                gametitles.Items.FindByText("All").Selected = true;
                submitfailed.Style.Add("display", "Block");
            }

            conn.Close();
        }
    }
}