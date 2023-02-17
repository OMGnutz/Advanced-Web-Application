using _211792H.App_Code;
using _211792H.MasterPages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Salt_Password;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace _211792H.WebPages
{
    public partial class ChangePassword : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("Home.aspx");
            }
               
           
            
        }

        protected void manualChangePass(object sender, EventArgs e)
        {
            Debug.WriteLine("Manual change pass");
            string ePass = Hash.ComputeHash(txt_password.Text, "SHA512", null);
            string emailID = Request.QueryString["token"];
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string chkpass = "SELECT Password From [User] WHERE Username=@user";
            SqlCommand chkpasscmd = new SqlCommand(chkpass, conn);
            chkpasscmd.Parameters.AddWithValue("@user", Session["username"].ToString());
            string userpassword = chkpasscmd.ExecuteScalar().ToString();
            bool flag = Hash.VerifyHash(txt_password.Text, "SHA512", userpassword);
            if (flag == true)
            {
                SamePass.Style.Add("display", "block");
            }

            else
            {
                string changepass = "UPDATE [User] SET Password=@hashpw WHERE Username=@user";
                SqlCommand changepasscmd = new SqlCommand(changepass, conn);
                changepasscmd.Parameters.AddWithValue("@user", Session["username"].ToString());
                changepasscmd.Parameters.AddWithValue("@hashpw", ePass);
                changepasscmd.ExecuteNonQuery();
                PSalert.Style.Add("display", "block");
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "3;url=home.aspx";
                this.Page.Controls.Add(meta);
            }
        }


    }
}