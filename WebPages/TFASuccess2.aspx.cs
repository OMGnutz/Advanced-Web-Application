using _211792H.MasterPages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _211792H.WebPages
{
    public partial class TFASuccess2 : System.Web.UI.Page
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

            else
            {
                try
                {
                    SqlCommand updataTFA = new SqlCommand("UPDATE [TFATemp] SET Verified=@status WHERE Email=@email", conn);
                    updataTFA.Parameters.AddWithValue("@status", true);
                    updataTFA.Parameters.AddWithValue("@email", Base.fpemail);
                    updataTFA.ExecuteNonQuery();
                    string linkexpired = "DELETE FROM [TempEmail] WHERE EmailCode=@emailid";
                    SqlCommand linkexpiredcmd = new SqlCommand(linkexpired, conn);
                    linkexpiredcmd.Parameters.AddWithValue("@emailid", emailID);
                    linkexpiredcmd.ExecuteNonQuery();
                }

                catch
                {
                    return;
                }

            }

            conn.Close();
        }
    }
}