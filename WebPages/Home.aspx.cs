using _211792H.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace _211792H.WebPages
{
    public partial class Home : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SOrepeaterBind();
               
        }



        private DataSet GetSpecialOffers1()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("With NumberedTable AS (SELECT * , Row_Number() OVER (ORDER BY Id) As Rownumber FROM [GameProducts] WHERE DiscountedPrice IS NOT NULL ) SELECT * FROM NumberedTable WHERE Rownumber BETWEEN 1 AND 3", conn);
            DataSet specialoffers = new DataSet();
            cmd.Fill(specialoffers);
            return specialoffers;
        }


        private DataSet GetSpecialOffers2()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("With NumberedTable AS (SELECT * , Row_Number() OVER (ORDER BY Id) As Rownumber FROM [GameProducts] WHERE DiscountedPrice IS NOT NULL) SELECT * FROM NumberedTable WHERE Rownumber BETWEEN 4 AND 6", conn);
            DataSet specialoffers = new DataSet();
            cmd.Fill(specialoffers);
            return specialoffers;
        }

        private DataSet GetSpecialOffers3()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("With NumberedTable AS (SELECT * , Row_Number() OVER (ORDER BY Id) As Rownumber FROM [GameProducts] WHERE DiscountedPrice IS NOT NULL ) SELECT * FROM NumberedTable WHERE Rownumber BETWEEN 7 AND 9", conn);
            DataSet specialoffers = new DataSet();
            cmd.Fill(specialoffers);
            return specialoffers;
        }


        protected void SOrepeaterBind()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string discounts = "SELECT COUNT(*) FROM [GameProducts] WHERE DiscountedPrice IS NOT NULL";
            SqlCommand cmd = new SqlCommand(discounts, conn);
            int rows = (int)cmd.ExecuteScalar();
            if (rows >= 3)
            {
                Debug.WriteLine(rows);
                DataSet specialOffers1 = GetSpecialOffers1();
                RepeaterSpecialOffer1.DataSource = specialOffers1;
                RepeaterSpecialOffer1.DataBind();
            } 

            if (rows >= 6)
            {
                Debug.WriteLine(rows);
                DataSet specialOffers2 = GetSpecialOffers2();
                RepeaterSpecialOffer2.DataSource = specialOffers2;
                RepeaterSpecialOffer2.DataBind();
            }

            if (rows >= 9)
            {
                DataSet specialOffers3 = GetSpecialOffers3();
                RepeaterSpecialOffer3.DataSource = specialOffers3;
                RepeaterSpecialOffer3.DataBind();
            }

            conn.Close();
        }

        protected void Unnamed_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Genre.aspx?Genre=");
        }
    }
}