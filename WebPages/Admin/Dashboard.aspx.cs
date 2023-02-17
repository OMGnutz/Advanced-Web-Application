using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _211792H.WebPages.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                initializedata();
                if(!IsPostBack)
                {
                    //initializechart();
                }

            }
            else
            {
                Response.Redirect("../Home.aspx");
            }
            
        }

        [WebMethod]
        public static List<object> GetChartData()
        {
            string query = "SELECT TOP 5 * FROM [GameProducts] ORDER BY TimesBought DESC";
            string constr = ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
                "Name", "Number Of Purchase"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                        sdr["Name"], sdr["TimesBought"]
                            });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }

        [WebMethod]
        public static List<object> GetChartData2()
        {
            string query = "SELECT TOP 5 Genre, sum(TimesBought) AS purchasecount FROM [GameProducts] GROUP BY Genre ORDER BY purchasecount DESC ";
            string constr = ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
                "Name", "Number Of Purchase"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                        sdr["Genre"], sdr["purchasecount"]
                            });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }


        protected void initializedata()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            //Users
            SqlCommand slctusers = new SqlCommand("SELECT COUNT(*) FROM [User]", conn);
            ttlusers.Text = slctusers.ExecuteScalar().ToString();
            //Products
            SqlCommand slctprd = new SqlCommand("SELECT COUNT(*) FROM [GameProducts]", conn);
            ttlProducts.Text = slctprd.ExecuteScalar().ToString();
            //Sales
            decimal sales = 0;
            string displaysales;
            Dictionary<string, int> salesdict = new Dictionary<string, int>();
            SqlCommand slctpurchases = new SqlCommand("SELECT * FROM [UserPurchases]",conn);
            SqlDataReader purchasesreader = slctpurchases.ExecuteReader();
            while(purchasesreader.Read())
            {
                salesdict.Add(purchasesreader["ProductID"].ToString(), int.Parse(purchasesreader["PurchaseCount"].ToString()));
            }
            purchasesreader.Close();
            for(int i=0; i<salesdict.Count; i++)
            {
                SqlCommand calcprice = new SqlCommand("SELECT Price FROM [GameProducts] WHERE Id=@prdID" , conn);
                calcprice.Parameters.AddWithValue("@prdID", salesdict.ElementAt(i).Key.ToString());
                SqlDataReader calcreader = calcprice.ExecuteReader();
                calcreader.Read();
                sales += (decimal.Parse(calcreader["Price"].ToString()) * salesdict.ElementAt(i).Value);
                calcreader.Close();
            }
            if (Math.Round(sales , 0).ToString().Length > 3)
            {
                displaysales = (Math.Round(sales, 0) / 100).ToString() + "K";
            }

            else if (Math.Round(sales, 0).ToString().Length > 6)
            {
                displaysales = (Math.Round(sales, 0) / 100000).ToString() + "M";
            }
            else
            {
                displaysales = Math.Round(sales, 0).ToString();
            }
            ttlSales.Text = "$" + displaysales;
            
        }
    }
}