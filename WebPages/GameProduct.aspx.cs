using _211792H.App_Code;
using _211792H.MasterPages;
using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class GameProduct : BaseMaster
    {
        Product prod = null;
        DataTable dt = new DataTable();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            initProduct();
            initializewishlist();
            if (!IsPostBack)
            {
                initreviews();
            }
            
            
        }

        protected void initProduct()
        {
            string productID = Convert.ToString(Request.QueryString["Id"]);
            Debug.WriteLine(productID);
            if (string.IsNullOrEmpty(productID))
            {
                Response.Redirect("Home.aspx");
            }

            else
            {
                try
                {
                    Product prodclass = new Product();
                    prod = prodclass.getProduct(productID);
                    breadcrumbGenre.Text = prod.Product_Genre;
                    breadcrumbGenre.NavigateUrl = "Genre.aspx" + "?Genre=" + prod.Product_Genre;
                    breadcrumbProduct.Text = prod.Product_Name;
                    breadcrumbGame.Text = prod.Product_Game;
                    breadcrumbGame.NavigateUrl = "Genre.aspx" + "?Genre=" + prod.Product_Game;
                    breadcrumbProduct.NavigateUrl = Request.RawUrl;
                    title.Text = prod.Product_Name;
                    prodGame.Text = prod.Product_Game;
                    prodimg.ImageUrl = prod.Product_Image;
                    prodDesc.Text = prod.Product_Desc;
                    lblPrice.Text = "$" + prod.Product_Price.ToString();
                    lblDiscountPrice.Text = "$" + prod.Product_DiscountedPrice.ToString();
                }

                catch
                {
                    Response.Redirect("Home.aspx");
                }
            }
        }

        protected void initializewishlist()
        {
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                
                wishlistbutton.Text = "Add To Wishlist";
                wishlistbutton.Attributes.Add("class", "btn btn-outline-info");
                return;
            }
            string iProductID = prod.Product_ID.ToString();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            Debug.WriteLine("initilize wishlist");
            SqlCommand inwishlist = new SqlCommand("SELECT COUNT(*) FROM [UserWishlist] WHERE ProductID=@prdID AND UserID=@usrID", conn);
            inwishlist.Parameters.AddWithValue("@prdID", iProductID);
            inwishlist.Parameters.AddWithValue("@usrID" , Session["username"].ToString());
            int exist = (int)inwishlist.ExecuteScalar();
            if (exist > 0)
            {
                wishlistbutton.Text = "Remove From Wishlist";
                wishlistbutton.Attributes.Add("class", "btn btn-outline-danger");
            }
            else
            {
                wishlistbutton.Text = "Add To Wishlist";
                wishlistbutton.Attributes.Add("class", "btn btn-outline-info");
            }
           
            conn.Close();
        }


        protected void wishlistinteract(object sender, EventArgs e)
        {
            LinkButton wishlistbutton = (LinkButton)sender;
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                ((HtmlGenericControl)this.Master.FindControl("LoginPopUp")).Style.Add("display", "block");
            }

            else
            {
                string iProductID = prod.Product_ID.ToString();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
                conn.Open(); 
                SqlCommand slctwishlist = new SqlCommand("SELECT COUNT(*) FROM [UserWishlist] WHERE ProductID=@prdID AND USerID=@usrid", conn);
                slctwishlist.Parameters.AddWithValue("@prdID", iProductID);
                slctwishlist.Parameters.AddWithValue("@usrid" , Session["username"].ToString());
                int exist = (int)slctwishlist.ExecuteScalar();
                if (exist > 0)
                {
                    SqlCommand updatewishlist = new SqlCommand("DELETE FROM [UserWishList] WHERE UserID=@usrID AND ProductID=@prdID", conn);
                    updatewishlist.Parameters.AddWithValue("@prdID", iProductID);
                    updatewishlist.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                    updatewishlist.ExecuteNonQuery();
                    wishlistbutton.Text = "Add To Wishlist";
                    wishlistbutton.Attributes.Add("class", "btn btn-outline-info");
                }

                else
                {
                    SqlCommand updatewishlist = new SqlCommand("INSERT INTO [UserWishList] (ProductID , UserID) VALUES (@prdID , @usrID)", conn);
                    updatewishlist.Parameters.AddWithValue("@prdID", iProductID);
                    updatewishlist.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                    updatewishlist.ExecuteNonQuery();
                    wishlistbutton.Text = "Remove From Wishlist";
                    wishlistbutton.Attributes.Add("class", "btn btn-outline-danger");
                }
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
                string iProductID = prod.Product_ID.ToString();
                ShoppingCart.Instance.AddItem(iProductID, prod);
            }
        }



        protected void submitReview(object sender, EventArgs e)
        {
            if (recommend.Checked ==true || notRecommend.Checked == true)
            {
                invalidReview.Style.Add("display", "none");
                reviewblock.Style.Add("display", "none");
                Guid newGUID = Guid.NewGuid();
                Boolean userReccomendation = false;
                if (recommend.Checked == true)
                {
                    userReccomendation = true;
                }
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
                conn.Open();
                string exist = "SELECT COUNT(*) FROM [ProductReviews] WHERE ProductId=@prdID AND UserId=@usrID";
                SqlCommand cmdexist = new SqlCommand(exist, conn);
                cmdexist.Parameters.AddWithValue("@prdID", Request.QueryString["Id"].ToString());
                cmdexist.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                int existcount = (int)cmdexist.ExecuteScalar();
                if (existcount > 0)
                {
                    string update = "UPDATE [ProductReviews] SET Reviewdesc=@desc , Reccomended=@recommend WHERE ProductId=@prdID AND UserId=@usrID";
                    SqlCommand updatecmd = new SqlCommand(update, conn);
                    updatecmd.Parameters.AddWithValue("@desc" , txt_Reviewdesc.Text);
                    updatecmd.Parameters.AddWithValue("@recommend", userReccomendation);
                    updatecmd.Parameters.AddWithValue("@prdID", Request.QueryString["Id"].ToString());
                    updatecmd.Parameters.AddWithValue("@usrID", Session["username"].ToString());
                    updatecmd.ExecuteNonQuery();
                }

                else
                {
                    SqlCommand slctuserpic = new SqlCommand("SELECT Profilepic FROM [User] Where Username=@usrID" , conn);
                    slctuserpic.Parameters.AddWithValue("@usrID" , Session["username"].ToString() );
                    SqlDataReader readpic = slctuserpic.ExecuteReader();
                    readpic.Read();
                    string insert = "INSERT INTO [ProductReviews] (ReviewId, ProductId, UserId, Reviewdesc, postedTime, Reccomended , UserPic) VALUES (@RevID, @prdID, @UsrID, @desc, @time, @recommend,@pic)";
                    SqlCommand cmdinsert = new SqlCommand(insert, conn);
                    cmdinsert.Parameters.AddWithValue("@RevID", newGUID.ToString());
                    cmdinsert.Parameters.AddWithValue("@prdID", Request.QueryString["Id"].ToString());
                    cmdinsert.Parameters.AddWithValue("@UsrID" , Session["username"].ToString());
                    cmdinsert.Parameters.AddWithValue("@desc", txt_Reviewdesc.Text.ToString());
                    cmdinsert.Parameters.AddWithValue("@time", DateTime.UtcNow);
                    cmdinsert.Parameters.AddWithValue("@recommend", userReccomendation);
                    cmdinsert.Parameters.AddWithValue("@pic", readpic["Profilepic"].ToString());
                    readpic.Close();
                    cmdinsert.ExecuteNonQuery();
                }
                conn.Close();
                initreviews();
                AfterLogin.initProducts();

            }

            else
            {
                invalidReview.Style.Add("display", "block");
                reviewblock.Style.Add("display", "block");
            }
        }

        protected void initreviews()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string slctreviews = "SELECT COUNT(*) FROM [ProductReviews] WHERE ProductId=@prdID";
            SqlCommand cmd = new SqlCommand(slctreviews, conn);
            cmd.Parameters.AddWithValue("@prdID", Request.QueryString["Id"].ToString());
            int count = (int)cmd.ExecuteScalar();
            if (count > 0)
            {
                DataSet getReviews = reviews();
                RepeaterReviews.DataSource = getReviews;
                RepeaterReviews.DataBind();
                NumberReviews.Text = count.ToString() + " Review";
                if (count > 1)
                {
                    NumberReviews.Text += "s";
                }
                Noreviews.Style.Add("display", "none");
            }

            else
            {
                Noreviews.Style.Add("display", "block");
            }

            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                writereview.Style.Add("display", "none");
                return;
            }

            string slctuserpurch = "SELECT COUNT(*) FROM [UserPurchases] WHERE UserID = @usrID AND ProductID=@prdID";
            SqlCommand userpurchcmd = new SqlCommand(slctuserpurch , conn);
            userpurchcmd.Parameters.AddWithValue("@usrID", Session["username"]);
            userpurchcmd.Parameters.AddWithValue("@prdID", Request.QueryString["Id"].ToString());
            int purched = (int) userpurchcmd.ExecuteScalar();
            if (purched > 0)
            {
                writereview.Style.Add("display", "block");
            }

            else
            {
                writereview.Style.Add("display", "none");
            }


            string slctuserreview = "SELECT * FROM [ProductReviews] WHERE UserId=@usrID AND ProductId=@prdID";
            SqlCommand cmdusrreview = new SqlCommand(slctuserreview, conn);
            cmdusrreview.Parameters.AddWithValue("@usrID", Session["username"]);
            cmdusrreview.Parameters.AddWithValue("@prdID", Request.QueryString["Id"].ToString());
            SqlDataReader readerreview = cmdusrreview.ExecuteReader();
            if (readerreview.Read())
            {
                txt_Reviewdesc.Text = readerreview["Reviewdesc"].ToString();
                if (readerreview["Reccomended"].ToString() == "True")
                {
                    recommend.Checked = true;
                    notRecommend.Checked = false;
                    var newClassValue = lbl1.Attributes["class"].Replace("btn btn-outline-info", "btn btn-info");
                    lbl1.Attributes.Remove("class");
                    lbl1.Attributes.Add("class", newClassValue);
                    var newClassValue2 = lbl2.Attributes["class"].Replace("btn btn-info", "btn btn-outline-info");
                    lbl2.Attributes.Remove("class");
                    lbl2.Attributes.Add("class", newClassValue2);
                }

                else
                {
                    notRecommend.Checked=true;
                    recommend.Checked = false;
                    var newClassValue = lbl2.Attributes["class"].Replace("btn btn-outline-info", "btn btn-info");
                    lbl2.Attributes.Remove("class");
                    lbl2.Attributes.Add("class", newClassValue);
                    var newClassValue2 = lbl1.Attributes["class"].Replace("btn btn-info", "btn btn-outline-info");
                    lbl1.Attributes.Remove("class");
                    lbl1.Attributes.Add("class", newClassValue2);
                }
            }
            else
            {
                txt_Reviewdesc.Text = "";
                recommend.Checked = false;
                notRecommend.Checked = false;
            }
            readerreview.Close();
            
            conn.Close();

        }

       

        private DataSet reviews()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM [ProductReviews] WHERE ProductId=@prdID ORDER BY postedTime DESC", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@prdID", Request.QueryString["Id"].ToString());
            DataSet reviewsdataset = new DataSet();
            cmd.Fill(reviewsdataset);
            return reviewsdataset;
        }
    }
}