using _211792H.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using _211792H.MasterPages;
using System.Diagnostics;

namespace _211792H.WebPages
{
    public partial class Profile : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("Home.aspx");
            }
            else
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
                conn.Open();
                SqlCommand slctpurchases = new SqlCommand("SELECT COUNT(*) FROM [GameProducts] WHERE Id IN (SELECT TOP 5 ProductID FROM [UserPurchases] WHERE UserID=@usrID ORDER BY PurchaseTime Desc)", conn);
                slctpurchases.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                int exist = (int)slctpurchases.ExecuteScalar();
                if (exist > 0)
                {
                    DataSet getindex = prodindex();
                    RepeaterProd.DataSource = getindex;
                    RepeaterProd.DataBind();
                }
                usrname.Text = Session["username"].ToString();
                conn.Close();
                AfterLogin.readuserimg(usrimg , Session["username"].ToString());
                ((HtmlGenericControl)this.Master.FindControl("carticon")).Style.Add("display", "none");
                ((HtmlGenericControl)this.Master.FindControl("nav")).Style.Add("display", "none");
            }      
        }



        protected void AddToCart(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                ((HtmlGenericControl)this.Master.FindControl("LoginPopUp")).Style.Add("display", "block");
            }

            else
            {
                Product aProd = new Product();
                LinkButton button = (LinkButton)sender;
                string iProductID = button.CommandArgument.ToString();
                Product prod = aProd.getProduct(button.CommandArgument.ToString());
                ShoppingCart.Instance.AddItem(iProductID, prod);
            }
        }

        private DataSet prodindex()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts] WHERE Id IN (SELECT TOP 5 ProductID FROM [UserPurchases] WHERE UserID=@usrID ORDER BY PurchaseTime Desc)", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@usrID" , Session["username"].ToString());
            DataSet genredataset = new DataSet();
            cmd.Fill(genredataset);
            return genredataset;
        }

        protected void editprofile_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProfileEdit.aspx");
        }
    }
}