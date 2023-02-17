using _211792H.App_Code;
using _211792H.MasterPages;
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace _211792H.WebPages
{
    public partial class Genre : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["Genre"] as string))
            {
                Response.Redirect("Home.aspx");
            }

            else
            {
                initializedb(Request.QueryString["Genre"].ToString());
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

        protected void initializedb(string genre)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string genreExist = "SELECT COUNT(*) FROM [GameProducts] WHERE Genre=@genre";
            SqlCommand cmd = new SqlCommand(genreExist, conn);
            cmd.Parameters.AddWithValue("@genre", genre);
            if ((int) cmd.ExecuteScalar() == 0)
            {
                string gameTitle = "SELECT COUNT(*) FROM [GameProducts] WHERE GameOrigin=@title";
                SqlCommand cmd2 = new SqlCommand(gameTitle, conn);
                cmd2.Parameters.AddWithValue("@title", genre);
                if ((int) cmd2.ExecuteScalar() == 0)
                {
                    NoResults.Style.Add("display", "block");
                }

                else
                {
                    DataSet getGames = GameTitles(genre);
                    RepeaterProd.DataSource = getGames;
                    RepeaterProd.DataBind();
                }
               
            }

            else
            {
                DataSet getGenre = genres(genre);
                RepeaterProd.DataSource= getGenre;
                RepeaterProd.DataBind();
            }
        }

        private DataSet genres(string genre)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts] WHERE Genre=@genre", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@genre", genre);
            DataSet genredataset = new DataSet();
            cmd.Fill(genredataset);
            return genredataset;
        }

        private DataSet GameTitles(string title)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts] WHERE GameOrigin=@title", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@title", title);
            DataSet genredataset = new DataSet();
            cmd.Fill(genredataset);
            return genredataset;
        }


        protected void Unnamed_Click(object sender, EventArgs e)
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