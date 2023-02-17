using System;
using _211792H.App_Code;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.HtmlControls;


namespace _211792H.MasterPages
{
    public partial class AfterLogin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                userdropdown.InnerText = Session["username"].ToString();
                initializenoti();
            }
            else
            {
                Session["CHANGE_MASTERPAGE2"] = "~/MasterPages/Base.Master";
                Session["CHANGE_MASTERPAGE"] = null;
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            
        }

        protected void initializenoti()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            SqlCommand slctnoti = new SqlCommand("SELECT COUNT(*) FROM [UserNotifications] WHERE UserId=@Id" , conn);
            slctnoti.Parameters.AddWithValue("@Id", Session["username"].ToString());
            int exist = (int) slctnoti.ExecuteScalar();
            if (exist > 0)
            {
                DataSet getnoti = notifications();
                RepeaterNoti.DataSource = getnoti;
                RepeaterNoti.DataBind();
            }
            else
            {
                DataSet getnoti = notifications();
                RepeaterNoti.DataSource = getnoti;
                RepeaterNoti.DataBind();
                nonewnoti.Style.Add("display", "block");
            }
        }

        protected void notiprd(object sender , EventArgs e) 
        {
            LinkButton butt = (LinkButton)sender;
            Response.Redirect("GameProduct.aspx?Id=" + butt.CommandArgument.ToString());

        }

        protected void clearnoti(object sender , EventArgs e)
        {
            LinkButton butt = (LinkButton)sender;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            SqlCommand delnoti = new SqlCommand("DELETE FROM [UserNotifications] WHERE ProductID=@id AND UserID=@usrid", conn);
            delnoti.Parameters.AddWithValue("@id", butt.CommandArgument.ToString());
            delnoti.Parameters.AddWithValue("@usrid" , Session["username"].ToString());
            delnoti.ExecuteNonQuery();
            
            initializenoti();
            conn.Close();
        }

        public static void readuserimg(Image fieldset, string user)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string slctuser = "SELECT * FROM [User] WHERE Username = @id";
            SqlCommand cmd = new SqlCommand(slctuser, conn);
            cmd.Parameters.AddWithValue("@id", user);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader["Profilepic"].ToString() == "")
            {
                fieldset.ImageUrl = "../../images/profilepic2.png";
            }
            else
            {
                fieldset.ImageUrl = reader["Profilepic"].ToString();
            }
            conn.Close();
        }

        public static void initProducts()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            //Special Offer Expires
            List<string> OfferEnded = new List<string>();
            string selectprod = "SELECT Id, SaleTime From [GameProducts] WHERE DiscountedPrice IS NOT NULL";
            SqlCommand slctcmd = new SqlCommand(selectprod, conn);
            SqlDataReader reader = slctcmd.ExecuteReader();
            while (reader.Read())
            {
                if (Convert.ToDateTime(reader["SaleTime"]) < DateTime.UtcNow)
                {
                    OfferEnded.Add(reader["Id"].ToString());
                }
            }

            reader.Close();

            foreach (string id in OfferEnded)
            {
                string updateprod = "UPDATE [GameProducts] SET DiscountedPrice=@price , SaleTime=@time WHERE Id=@id";
                SqlCommand uptcmd = new SqlCommand(updateprod, conn);
                uptcmd.Parameters.AddWithValue("@id", id);
                uptcmd.Parameters.AddWithValue("@price", DBNull.Value);
                uptcmd.Parameters.AddWithValue("@time", DBNull.Value);
                uptcmd.ExecuteNonQuery();
            }

            //Update Reviews
            List<string> productIDs = new List<string>();
            string slctprod = "SELECT Id FROM [GameProducts]";
            SqlCommand slctcmd2 = new SqlCommand(slctprod, conn);
            SqlDataReader reader1 = slctcmd2.ExecuteReader();
            while (reader1.Read())
            {
                productIDs.Add(reader1["Id"].ToString());
            }
            reader1.Close();

            foreach (string id in productIDs)
            {
                string slctreview = "SELECT COUNT(*) FROM [ProductReviews] WHERE ProductId=@id";
                SqlCommand slctreviewcmd = new SqlCommand(slctreview, conn);
                slctreviewcmd.Parameters.AddWithValue("@id", id);
                int allreviews = (int)slctreviewcmd.ExecuteScalar();
                if (allreviews == 0)
                {
                    string updateProd = "UPDATE [GameProducts] SET OverallReviews=@review WHERE Id=@id";
                    SqlCommand uptReviewcmd = new SqlCommand(updateProd, conn);
                    uptReviewcmd.Parameters.AddWithValue("@review", "No Reviews");
                    uptReviewcmd.Parameters.AddWithValue("@id", id);
                    uptReviewcmd.ExecuteNonQuery();
                }

                else
                {
                    string slctgdreviews = "SELECT Count(*) FROM [ProductReviews] WHERE ProductId=@id AND Reccomended=@recommend";
                    SqlCommand slctgdreviewcmd = new SqlCommand(slctgdreviews, conn);
                    slctgdreviewcmd.Parameters.AddWithValue("@id", id);
                    slctgdreviewcmd.Parameters.AddWithValue("@recommend", true);
                    int gdreviews = (int)slctgdreviewcmd.ExecuteScalar();
                    Debug.WriteLine(gdreviews);
                    Debug.WriteLine(allreviews.ToString());
                    int positiveperc = Convert.ToInt32(gdreviews / (Convert.ToDecimal(allreviews) / 100));
                    string updateProd = "UPDATE [GameProducts] SET OverallReviews=@review WHERE Id=@id";
                    SqlCommand uptReviewcmd = new SqlCommand(updateProd, conn);
                    uptReviewcmd.Parameters.AddWithValue("@id", id);
                    if (positiveperc >= 95)
                    {
                        uptReviewcmd.Parameters.AddWithValue("@review", "Overwhelmingly Positive");
                        uptReviewcmd.ExecuteNonQuery();
                    }

                    else if (positiveperc >= 80)
                    {
                        uptReviewcmd.Parameters.AddWithValue("@review", "Very Positive");
                        uptReviewcmd.ExecuteNonQuery();
                    }

                    else if (positiveperc >= 60)
                    {
                        uptReviewcmd.Parameters.AddWithValue("@review", "Positive");
                        uptReviewcmd.ExecuteNonQuery();
                    }

                    else if (positiveperc >= 45)
                    {
                        uptReviewcmd.Parameters.AddWithValue("@review", "Mixed");
                        uptReviewcmd.ExecuteNonQuery();
                    }

                    else if (positiveperc >= 30)
                    {
                        uptReviewcmd.Parameters.AddWithValue("@review", "Negative");
                        uptReviewcmd.ExecuteNonQuery();
                    }

                    else if (positiveperc >= 15)
                    {
                        uptReviewcmd.Parameters.AddWithValue("@review", "Very Negative");
                        uptReviewcmd.ExecuteNonQuery();
                    }

                    else
                    {
                        uptReviewcmd.Parameters.AddWithValue("@review", "Overwhelmingly Negative");
                        uptReviewcmd.ExecuteNonQuery();
                    }


                }
            }
            conn.Close();
        }

        protected void btnlogout_Click(object sender, EventArgs e)
        {
            Session["CHANGE_MASTERPAGE2"] = "~/MasterPages/Base.Master";
            Session["CHANGE_MASTERPAGE"] = null;
            Session["username"] = null;
            ShoppingCart.Instance.ClearCart();
            Response.Redirect("Home.aspx");
        }

        protected void Search(object sender, EventArgs e)
        {
            
            Session["Search"] = txtSearch.Text;
            Response.Redirect("Search.aspx");
        }

        private DataSet notifications()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT n.NotiDesc , g.Id, g.Image FROM [UserNotifications] as n inner join [GameProducts] as g on n.ProductID = g.Id WHERE n.UserId=@ID ORDER BY n.NotiTime DESC", conn);
            cmd.SelectCommand.Parameters.AddWithValue("@ID", Session["username"].ToString());
            DataSet notidataset = new DataSet();
            cmd.Fill(notidataset);
            return notidataset;
        }
    }
}