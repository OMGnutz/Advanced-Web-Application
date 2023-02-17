using _211792H.App_Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using Microsoft.SqlServer.Server;

namespace _211792H.WebPages
{
    public partial class Payment : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["username"] as string) || ShoppingCart.Instance.Items.Count == 0)
            {
                Response.Redirect("Home.aspx");
            }

            DropDownList yeardp = (DropDownList)YearDropdown;

            for(int i=0; i<25; i++)
            {
                yeardp.Items.Add(new ListItem(DateTime.Now.AddYears(i).Year.ToString(), DateTime.Now.AddYears(i).Year.ToString()));
            }
        }

        protected void payment(object sender, EventArgs e)
        {
            try
            {
                Convert.ToInt32(YearDropdown.SelectedValue.ToString());
                int month = Convert.ToInt32(MonthDropdown.SelectedValue.ToString());
                DateTime dt = DateTime.Now;
                if (month < dt.Month)
                {
                    invalidDate.InnerText = "Your credit card has expired.";
                    invalidDate.Style.Add("display", "block"); 
                    return;
                }
            }
            catch 
            {
                invalidDate.InnerText = "Please enter a valid date";
                invalidDate.Style.Add("display", "block");
                return;
            }
  

            if (validateCreditCard())
            {
                invalidCreditCard.Style.Add("display", "none");
                PSalert.Style.Add("display", "block");
                modalpopup.Style.Add("display", "block");
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
                conn.Open();
                
                for(int i =0; i<ShoppingCart.Instance.Items.Count; i++)
                {
                    string BoughtBefore = "SELECT COUNT(*) FROM UserPurchases WHERE UserId = @ID AND ProductID IN (@productID)";
                    SqlCommand cmdBoughtBefore = new SqlCommand(BoughtBefore, conn);
                    cmdBoughtBefore.Parameters.AddWithValue("@ID", Session["username"].ToString());
                    cmdBoughtBefore.Parameters.AddWithValue("@productID", ShoppingCart.Instance.Items[i].ItemID);

                    int purchased = (int)cmdBoughtBefore.ExecuteScalar();

                    if (purchased > 0)
                    {
                        string updatepurchases= "UPDATE [UserPurchases] SET PurchaseCount+=@count , PurchaseTime=@time WHERE ProductID=@prdID AND UserID=@usrID";
                        SqlCommand cmdupdatepurchases = new SqlCommand(updatepurchases, conn);
                        cmdupdatepurchases.Parameters.AddWithValue("@prdID", ShoppingCart.Instance.Items[i].ItemID);
                        cmdupdatepurchases.Parameters.AddWithValue("@usrID" , Session["username"].ToString());
                        cmdupdatepurchases.Parameters.AddWithValue("@count", ShoppingCart.Instance.Items[i].Quantity);
                        cmdupdatepurchases.Parameters.AddWithValue("@time", DateTime.UtcNow);
                        cmdupdatepurchases.ExecuteNonQuery();
                    }

                    else
                    {
                        string insertpurchases = "INSERT INTO [UserPurchases] (ProductID , UserID , PurchaseCount, PurchaseTime) VALUES (@prdID , @usrID , @count , @time)";
                        SqlCommand cmdinsertpurchases = new SqlCommand(insertpurchases, conn);
                        cmdinsertpurchases.Parameters.AddWithValue("@prdID", ShoppingCart.Instance.Items[i].ItemID);
                        cmdinsertpurchases.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                        cmdinsertpurchases.Parameters.AddWithValue("@count", ShoppingCart.Instance.Items[i].Quantity);
                        cmdinsertpurchases.Parameters.AddWithValue("@time", DateTime.UtcNow);
                        cmdinsertpurchases.ExecuteNonQuery();
                    }
                    string slctprdpurchasecount = "SELECT TimesBought FROM [GameProducts] WHERE ID = @prdID";
                    SqlCommand slctprdpurchasecountcmd = new SqlCommand(slctprdpurchasecount, conn);
                    slctprdpurchasecountcmd.Parameters.AddWithValue("@prdID", ShoppingCart.Instance.Items[i].ItemID);
                    SqlDataReader reader = slctprdpurchasecountcmd.ExecuteReader();
                    reader.Read();
                    if (reader["TimesBought"].ToString() == "")
                    {
                        reader.Close();
                        string updtprdpurchasecount = "UPDATE [GameProducts] SET TimesBought=@count WHERE Id=@prdID";
                        SqlCommand cmdupdtprdpurchasecount = new SqlCommand(updtprdpurchasecount, conn);
                        cmdupdtprdpurchasecount.Parameters.AddWithValue("@prdID", ShoppingCart.Instance.Items[i].ItemID);
                        cmdupdtprdpurchasecount.Parameters.AddWithValue("@count", ShoppingCart.Instance.Items[i].Quantity);
                        cmdupdtprdpurchasecount.ExecuteNonQuery();
                    }

                    else
                    {
                        reader.Close();
                        string updtprdpurchasecount = "UPDATE [GameProducts] SET TimesBought +=@count WHERE Id=@prdID";
                        SqlCommand cmdupdtprdpurchasecount = new SqlCommand(updtprdpurchasecount, conn);
                        cmdupdtprdpurchasecount.Parameters.AddWithValue("@prdID", ShoppingCart.Instance.Items[i].ItemID);
                        cmdupdtprdpurchasecount.Parameters.AddWithValue("@count", ShoppingCart.Instance.Items[i].Quantity);
                        cmdupdtprdpurchasecount.ExecuteNonQuery();
                    }
                    
                }
                conn.Close();
                ShoppingCart.Instance.ClearCart();
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "3;url=home.aspx";
                this.Page.Controls.Add(meta);
            }

            else
            {
                invalidCreditCard.Style.Add("display", "block");
            }

        }


        protected bool validateCreditCard()
        {
            string regex = "";
            string creditcard = paymentMethodDropdown.SelectedValue.ToString().ToLower();
            if (creditcard == "visa")
            {
                regex = "^4[0-9]{12}(?:[0-9]{3})?$";
            }

            if (creditcard == "americanexpress")
            {
                regex = "^3[47][0-9]{13}$";
            }

            if(creditcard == "mastercard")
            {
                regex = "^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$";
            }
            Regex rx = new Regex(regex);
            Debug.WriteLine(regex);
            if (rx.IsMatch(txt_CN.Text))
            {
               
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}