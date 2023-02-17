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

namespace _211792H.WebPages
{
    public partial class Wishlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("Home.aspx");
            }

            else
            {
                ((HtmlGenericControl)this.Master.FindControl("carticon")).Style.Add("display", "none");
                ((HtmlGenericControl)this.Master.FindControl("nav")).Style.Add("display", "none");
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
                conn.Open();
                SqlCommand slctpurchases = new SqlCommand("SELECT COUNT(*) FROM [GameProducts] WHERE Id IN (SELECT ProductID FROM [UserWishlist] WHERE UserID=@usrID)", conn);
                slctpurchases.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                int exist = (int)slctpurchases.ExecuteScalar();
                if (exist > 0)
                {
                    DataSet getindex = prodindex();
                    RepeaterProd.DataSource = getindex;
                    RepeaterProd.DataBind();
                }
                else
                {
                    noitems.Style.Add("display", "block");
                }
                conn.Close();
            }
        }

        private DataSet prodindex()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts] WHERE Id IN (SELECT ProductID FROM [UserWishlist] WHERE UserID=@usrID)", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@usrID", Session["username"].ToString());
            DataSet genredataset = new DataSet();
            cmd.Fill(genredataset);
            return genredataset;
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


        protected void wishlistinteract(object sender , EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            LinkButton wishlistbutton = (LinkButton)sender;
            SqlCommand slctwishlist = new SqlCommand("SELECT COUNT(*) FROM [UserWishlist] WHERE ProductID=@prdID AND UserID=@usrid", conn);
            slctwishlist.Parameters.AddWithValue("@prdID", wishlistbutton.CommandArgument.ToString());
            slctwishlist.Parameters.AddWithValue("@usrid", Session["username"].ToString());
            int exist = (int) slctwishlist.ExecuteScalar();
            if (exist > 0)
            {
                SqlCommand updatewishlist = new SqlCommand("DELETE FROM [UserWishList] WHERE ProductID=@prdID AND UserID=@usrid" , conn);
                updatewishlist.Parameters.AddWithValue("@prdID", wishlistbutton.CommandArgument.ToString());
                updatewishlist.Parameters.AddWithValue("@usrid", Session["username"].ToString());
                updatewishlist.ExecuteNonQuery();
            }

            else
            {
                SqlCommand updatewishlist = new SqlCommand("INSERT INTO [UserWishList] (ProductID , UserID) VALUES (@prdID , @usrID)" , conn);
                updatewishlist.Parameters.AddWithValue("@prdID", wishlistbutton.CommandArgument.ToString());
                updatewishlist.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                updatewishlist.ExecuteNonQuery();
            }
            DataSet getindex = prodindex();
            RepeaterProd.DataSource = getindex;
            RepeaterProd.DataBind();
            conn.Close();
        }


    }
}