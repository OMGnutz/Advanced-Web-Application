using _211792H.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace _211792H.WebPages
{
    public partial class Search : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["Search"] as string))
            {
                bc.Style.Add("display", "none");
                NoResults.Style.Add("display", "block");
            }

            else
            {
                usersearch.InnerHtml = "Searching for " + Session["Search"].ToString();
                exact();
                if (!string.IsNullOrEmpty(Session["username"] as string))
                {
                    initializewishlist();
                }
                else
                {
                    foreach (RepeaterItem item in RepeaterProd.Items)
                    {

                        LinkButton button = (LinkButton)item.FindControl("wishlistbutton");
                        button.Text = "Add To Wishlist";
                        button.Attributes.Add("class", "btn btn-outline-info");

                    }
                }
            }
        }

        protected void exact()
        {
            string searchtxt = Session["Search"].ToString().ToLower();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string exactgame = "SELECT COUNT(*) FROM [GameProducts] WHERE LOWER(GameOrigin)=@search";
            SqlCommand exctgamecmd = new SqlCommand(exactgame, conn);
            exctgamecmd.Parameters.AddWithValue("@search", searchtxt);
            string exactprod = "SELECT COUNT(*) FROM [GameProducts] WHERE LOWER(Name)=@search";
            SqlCommand exctprodcmd = new SqlCommand(exactprod, conn);
            exctprodcmd.Parameters.AddWithValue("@search", searchtxt);
            if ((int) exctgamecmd.ExecuteScalar() > 0)
            {
                Response.Redirect("Genre.aspx?Genre=" + searchtxt);
                return;
            }

            if ((int) exctprodcmd.ExecuteScalar() > 0)
            {
                string slctid = "SELECT Id FROM [GameProducts] WHERE LOWER(Name) = @search";
                SqlCommand slctidcmd = new SqlCommand(slctid, conn);
                slctidcmd.Parameters.AddWithValue("@search", searchtxt);
                SqlDataReader reader = slctidcmd.ExecuteReader();
                reader.Read();
                Response.Redirect("GameProduct.aspx?Id=" + reader["Id"].ToString());
                reader.Close();
                return;
            }

            indexOf(searchtxt);
        }


        protected void indexOf(string searchtxt)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string likeprod = "SELECT COUNT(*) FROM [GameProducts] WHERE LOWER(Name) LIKE @search";
            SqlCommand likeprodcmd = new SqlCommand(likeprod, conn);
            likeprodcmd.Parameters.AddWithValue("@search" , "%" + searchtxt + "%");
            if ((int) likeprodcmd.ExecuteScalar() > 0) {
                DataSet getindex = prodindex(searchtxt);
                RepeaterProd.DataSource = getindex;
                RepeaterProd.DataBind();
            }

            else
            {
                NoResults.Style.Add("display", "block");
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

        private DataSet prodindex(string searchtxt)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts] WHERE LOWER(Name) LIKE @search", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@search", "%" + searchtxt + "%");
            DataSet genredataset = new DataSet();
            cmd.Fill(genredataset);
            return genredataset;
        }

        protected void initializewishlist()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            foreach (RepeaterItem item in RepeaterProd.Items)
            {
                SqlCommand inwishlist = new SqlCommand("SELECT COUNT(*) FROM [UserWishlist] WHERE ProductID=@prdID AND UserID=@usrid", conn);
                LinkButton button = (LinkButton)item.FindControl("wishlistbutton");
                inwishlist.Parameters.AddWithValue("@prdID", button.CommandArgument.ToString());
                inwishlist.Parameters.AddWithValue("@usrid", Session["username"].ToString());
                int exist = (int)inwishlist.ExecuteScalar();
                if (exist > 0)
                {
                    button.Text = "Remove From Wishlist";
                    button.Attributes.Add("class", "btn btn-outline-danger");
                }
                else
                {
                    button.Text = "Add To Wishlist";
                    button.Attributes.Add("class", "btn btn-outline-info");
                }
            }
            conn.Close();
        }


        protected void wishlistinteract(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                ((HtmlGenericControl)this.Master.FindControl("LoginPopUp")).Style.Add("display", "block");
            }

            else
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
                conn.Open();
                LinkButton wishlistbutton = (LinkButton)sender;
                SqlCommand slctwishlist = new SqlCommand("SELECT COUNT(*) FROM [UserWishlist] WHERE ProductID=@prdID AND UserID=@usrid", conn);
                slctwishlist.Parameters.AddWithValue("@prdID", wishlistbutton.CommandArgument.ToString());
                slctwishlist.Parameters.AddWithValue("@usrid", Session["username"].ToString());
                int exist = (int)slctwishlist.ExecuteScalar();
                if (exist > 0)
                {
                    SqlCommand updatewishlist = new SqlCommand("DELETE FROM [UserWishList] WHERE ProductID=@prdID AND UserID=@usrid", conn);
                    updatewishlist.Parameters.AddWithValue("@prdID", wishlistbutton.CommandArgument.ToString());
                    updatewishlist.Parameters.AddWithValue("@usrid", Session["username"].ToString());
                    updatewishlist.ExecuteNonQuery();
                    wishlistbutton.Text = "Add To Wishlist";
                    wishlistbutton.Attributes.Add("class", "btn btn-outline-info");
                }

                else
                {
                    SqlCommand updatewishlist = new SqlCommand("INSERT INTO [UserWishList] (ProductID , UserID) VALUES (@prdID , @usrID)", conn);
                    updatewishlist.Parameters.AddWithValue("@prdID", wishlistbutton.CommandArgument.ToString());
                    updatewishlist.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                    updatewishlist.ExecuteNonQuery();
                    wishlistbutton.Text = "Remove From Wishlist";
                    wishlistbutton.Attributes.Add("class", "btn btn-outline-danger");
                }
            }

        }
    }
}