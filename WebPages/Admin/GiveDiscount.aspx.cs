using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _211792H.WebPages.Admin
{
    public partial class GiveDiscount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataSet dataproducts = getproducts();
                RepeaterProd.DataSource = dataproducts;
                RepeaterProd.DataBind();
            }  
        }

        private DataSet getproducts()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT Name FROM [GameProducts] WHERE Id IN (SELECT Id FROM TempItemDiscount)", conn);
            DataSet dataproducts = new DataSet();
            cmd.Fill(dataproducts);
            return dataproducts;
        }

        protected void DiscountBut_Click(object sender, EventArgs e)
        {
            if (RepeaterProd.Items.Count < 1) {
                return;
            }

            Dictionary<string , decimal> OldPrices = new Dictionary<string , decimal>();
            string inputtime = txt_enddate.Text;
            DateTime saleend = DateTime.ParseExact(inputtime, "dd/MM/yy" , CultureInfo.InvariantCulture);
            int percentage = Convert.ToInt32(txt_perc.Text);
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string selectoriprice = "SELECT Price , Id FROM [GameProducts] WHERE Id IN (SELECT Id FROM TempItemDiscount)";
            SqlCommand cmdprice = new SqlCommand(selectoriprice, conn);
            SqlDataReader reader = cmdprice.ExecuteReader();
            while (reader.Read())
            {
                OldPrices.Add(reader["Id"].ToString() ,decimal.Parse(reader["Price"].ToString()));
            }
            reader.Close();

            foreach (var keys in OldPrices)
            {
                decimal discountedPrice = Math.Round(keys.Value / 100 * (100 - percentage), 2);
                string discountedPricestr = discountedPrice.ToString("0.00");
                discountedPrice = Convert.ToDecimal(discountedPricestr);
                Debug.WriteLine(discountedPrice);
                string updatestr = "UPDATE [GameProducts] SET DiscountedPrice=@discount , SaleTime=@time WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(updatestr, conn);
                cmd.Parameters.AddWithValue("@Id", keys.Key);
                cmd.Parameters.AddWithValue("@discount", discountedPrice);
                cmd.Parameters.AddWithValue("@time", saleend);
                cmd.ExecuteNonQuery();
                sendnoti(keys.Key, txt_perc.Text);
            }

            txt_enddate.Text = "";
            txt_perc.Text = "";
            alertsuccess.Style.Add("display", "block");
            conn.Close();
        }

        protected void sendnoti(string product , string discountperc)
        {
            List<string> userlist = new List<string>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            SqlCommand slctusers = new SqlCommand("SELECT Username FROM [User] WHERE Username IN (SELECT UserID FROM [UserWishlist] WHERE ProductID=@prdID)" , conn);
            slctusers.Parameters.AddWithValue("@prdID", product);
            SqlDataReader readerusers = slctusers.ExecuteReader();
            while (readerusers.Read())
            {
                userlist.Add(readerusers["Username"].ToString());
            }
            readerusers.Close();
            for (int i = 0; i < userlist.Count; i++)
            {
                SqlCommand slctprdname = new SqlCommand("SELECT Name FROM [GameProducts] WHERE Id=@id",conn);
                slctprdname.Parameters.AddWithValue("@id", product);
                SqlDataReader readerprd = slctprdname.ExecuteReader();
                readerprd.Read();
                string description = readerprd["Name"].ToString() + " is on " + discountperc + " percent discount";
                readerprd.Close();
                SqlCommand alrexist = new SqlCommand ("SELECT COUNT(*) FROM [UserNotifications] WHERE ProductID=@prdid AND UserID=@usrid", conn);
                alrexist.Parameters.AddWithValue("@prdid", product);
                alrexist.Parameters.AddWithValue("@usrid" , userlist[i].ToString());
                int existing = (int) alrexist.ExecuteScalar();
                if (existing > 0)
                {
                    SqlCommand insertnoti = new SqlCommand("UPDATE [UserNotifications] SET NotiDesc=@Notidesc WHERE ProductID=@prd AND UserID=@usr", conn);
                    insertnoti.Parameters.AddWithValue("@usr", userlist[i].ToString());
                    insertnoti.Parameters.AddWithValue("@prd", product);
                    insertnoti.Parameters.AddWithValue("@Notidesc", description);
                    insertnoti.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand insertnoti = new SqlCommand("INSERT INTO [UserNotifications] (UserID , ProductID , NotiDesc , NotiTime) VALUES (@usr , @prd , @Notidesc , @time)", conn);
                    insertnoti.Parameters.AddWithValue("@usr", userlist[i].ToString());
                    insertnoti.Parameters.AddWithValue("@prd", product);
                    insertnoti.Parameters.AddWithValue("@Notidesc", description);
                    insertnoti.Parameters.AddWithValue("@time", DateTime.UtcNow);
                    insertnoti.ExecuteNonQuery();
                }
               
            }
            conn.Close();
        }
    }
}