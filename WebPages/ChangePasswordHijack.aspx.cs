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
using _211792H.MasterPages;
using Salt_Password;

namespace _211792H.WebPages
{
    public partial class ChangePasswordHijack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string emailexpired = Request.QueryString["token"];
            if (string.IsNullOrEmpty(emailexpired))
            {
                Response.Redirect("EmailExpired.aspx");
            }
            else
            {

                string emailID = Request.QueryString["token"];
                Base.validateEmailTime(emailID);
                validateEmail(sender, e);
            }
        }
        protected void validateEmail(object sender, EventArgs e)
        {
            string emailID = Request.QueryString["token"];
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string validate = "SELECT COUNT(*) From [TempEmail] WHERE EmailCode=@emailid";
            SqlCommand validatecmd = new SqlCommand(validate, conn);
            validatecmd.Parameters.AddWithValue("@emailid", emailID);
            int temp = Convert.ToInt32(validatecmd.ExecuteScalar().ToString());

            if (temp == 0)
            {
                Response.Redirect("EmailExpired.aspx");
            }

            conn.Close();
        }

        protected void ChangePass(object sender, EventArgs e)
        {
            Debug.WriteLine("2FA changepass");
            string ePass = Hash.ComputeHash(txt_password.Text, "SHA512", null);
            string emailID = Request.QueryString["token"];
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string getemail = "SELECT Email From [TempEmail] WHERE EmailCode=@code";
            SqlCommand getemailcmd = new SqlCommand(getemail, conn);
            getemailcmd.Parameters.AddWithValue("@code", emailID);
            SqlDataReader reader = getemailcmd.ExecuteReader();
            reader.Read();
            string useremail = reader["Email"].ToString();
            reader.Close();
            string chkpass = "SELECT Password From [User] WHERE Email=@email";
            SqlCommand chkpasscmd = new SqlCommand(chkpass, conn);
            chkpasscmd.Parameters.AddWithValue("@email", useremail);
            string userpassword = chkpasscmd.ExecuteScalar().ToString();
            bool flag = Hash.VerifyHash(txt_password.Text, "SHA512", userpassword);
            if (flag == true)
            {
                SamePass.Style.Add("display", "block");
            }

            else
            {
                string changepass = "UPDATE [User] SET Password=@hashpw WHERE Email=@email";
                SqlCommand changepasscmd = new SqlCommand(changepass, conn);
                changepasscmd.Parameters.AddWithValue("@email", useremail);
                changepasscmd.Parameters.AddWithValue("@hashpw", ePass);
                changepasscmd.ExecuteNonQuery();
                string linkexpired = "DELETE FROM [TempEmail] WHERE EmailCode=@emailid";
                SqlCommand linkexpiredcmd = new SqlCommand(linkexpired, conn);
                linkexpiredcmd.Parameters.AddWithValue("@emailid", emailID);
                linkexpiredcmd.ExecuteNonQuery();
                SamePass.Style.Add("display", "none");
                Response.Redirect("PasswordChanged.aspx");
            }

        }
    }
}