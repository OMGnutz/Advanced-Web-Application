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
    public partial class Category : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["Category"] as string))
            {
                Response.Redirect("Home.aspx");
            }

            else
            {
                initializedb(Request.QueryString["Category"].ToString());
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

        protected void initializedb(string category)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            switch (category)
            {
                case "special offers":
                    DataSet specialoffers = SpecialOffers();
                    RepeaterProd.DataSource = specialoffers;
                    RepeaterProd.DataBind();
                    break;

                case "top sellers":
                    DataSet topsellers = TopSellers();
                    RepeaterProd.DataSource = topsellers;
                    RepeaterProd.DataBind();
                    break;


                case "new releases":
                    DataSet newreleases = NewReleases();
                    RepeaterProd.DataSource = newreleases;
                    RepeaterProd.DataBind();
                    break;

                case "best reviews":
                    DataSet bestreviews = BestReviews();
                    RepeaterProd.DataSource = bestreviews;
                    RepeaterProd.DataBind();
                    break;

                default:
                    NoResults.Style.Add("display", "block");
                    break;
            }
        }


        private DataSet SpecialOffers()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts] WHERE DiscountedPrice IS NOT NULL", conn);
            DataSet catdataset = new DataSet();
            cmd.Fill(catdataset);
            return catdataset;
        }

        private DataSet NewReleases()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            List<string> newreleases = new List<string>();
            conn.Open();
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts] WHERE GETDATE() <= DATEADD(month, 1, ReleaseDate)", conn);
            DataSet catdataset = new DataSet();
            cmd.Fill(catdataset);
            conn.Close();
            return catdataset;
        }


        private DataSet BestReviews()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [GameProducts] WHERE OverallReviews=@review1 OR OverallReviews=@review2", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@review1", "Very Positive");
            cmd.SelectCommand.Parameters.AddWithValue("@review2", "Overwhelmingly Positive");
            DataSet catdataset = new DataSet();
            cmd.Fill(catdataset);
            return catdataset;
        }

        private DataSet TopSellers()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT TOP 10 * FROM [GameProducts] ORDER BY TimesBought DESC", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@review1", "Very Positive");
            cmd.SelectCommand.Parameters.AddWithValue("@review2", "Overwhelmingly Positive");
            DataSet catdataset = new DataSet();
            cmd.Fill(catdataset);
            return catdataset;
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


        protected void initializewishlist()
        {
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                LinkButton button = (LinkButton)this.FindControl("wishlistbutton");
                button.Text = "Add To Wishlist";
                button.Attributes.Add("class", "btn btn-outline-info");
                return;
            }
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