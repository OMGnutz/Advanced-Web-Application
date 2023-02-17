using _211792H.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace _211792H.WebPages
{
    public partial class ProfileEdit : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("Home.aspx");
            }
            ((HtmlGenericControl)this.Master.FindControl("carticon")).Style.Add("display", "none");
            ((HtmlGenericControl)this.Master.FindControl("nav")).Style.Add("display", "none");
        }

        protected void editprofile(object sender, EventArgs e)
        {
            string image = "";
            int result = 0;
            if (productimg.HasFile == true)
            {
                image = "../images/" + productimg.FileName;
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
                conn.Open();
                SqlCommand updtprofile = new SqlCommand("UPDATE [User] SET Profilepic=@pic WHERE Username=@usrname", conn);
                SqlCommand updtreviewpic = new SqlCommand("UPDATE [ProductReviews] SET UserPic=@pic WHERE UserId=@usrname", conn);
                updtprofile.Parameters.AddWithValue("@pic", image);
                updtprofile.Parameters.AddWithValue("@usrname", Session["username"].ToString());
                result += updtprofile.ExecuteNonQuery();
                if (result > 0)
                {
                    updtreviewpic.Parameters.AddWithValue("@pic", image);
                    updtreviewpic.Parameters.AddWithValue("@usrname", Session["username"].ToString());
                    updtreviewpic.ExecuteNonQuery();
                    string saveimg = Server.MapPath(" ") + "\\" + image;
                    productimg.SaveAs(saveimg);
                    Response.Redirect("Profile.aspx");
                }
                conn.Close();
            }
            
        }
    }
}