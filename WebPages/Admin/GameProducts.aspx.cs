using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace _211792H.WebPages.Admin
{
    public partial class GameProducts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet dataproducts = getproducts();
            RepeaterProd.DataSource = dataproducts;
            RepeaterProd.DataBind();
        }


        protected void Deleteprod(object sender, EventArgs e)
        {
            var button = (LinkButton)sender;
            var agrs = button.CommandArgument.ToString();
            Debug.WriteLine(agrs);
        }

        protected void Addprod(object sender, EventArgs e)
        {
            Response.Redirect("Addproduct.aspx");
        }

        private DataSet getproducts()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts]" , conn);
            DataSet dataproducts = new DataSet();
            cmd.Fill(dataproducts);
            return dataproducts;
        }


    }
}