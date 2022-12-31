using _211792H.MasterPages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace _211792H.WebPages
{
    public partial class EmailVerified : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            ValidateEmailTime();
            try
            {
                string emailID = Request.QueryString["token"];
                conn.Open();
                string linkused = "SELECT EmailLinkUsed From [TempUser] WHERE Email=@email";
                SqlCommand cmdlinkused = new SqlCommand(linkused, conn);
                cmdlinkused.Parameters.AddWithValue("@email", Base.tempemail);
                SqlDataReader reader = cmdlinkused.ExecuteReader();
                reader.Read();
                if (reader["EmailLinkUsed"].ToString() == emailID)
                {
                    reader.Close();
                    using (SqlCommand cmd = new SqlCommand("UPDATE [TempUser] SET Verified=@status , EmailLinkUsed=@expired WHERE Email=@Email", conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", Base.tempemail);
                        cmd.Parameters.AddWithValue("@expired", "");
                        cmd.Parameters.AddWithValue("@status", true);
                        cmd.ExecuteNonQuery();
                    }
                }

                else
                {
                    Response.Redirect("VerifyEmailExpired.aspx");
                }

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Console.WriteLine("Error -" + ex);
            }


            finally
            {
                conn.Close();
            }
        }

        protected void ValidateEmailTime()
        {
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan duration = TimeSpan.FromMinutes(10);
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string expire = "SELECT EmailSentTime From [TempUser] WHERE Email=@email";
            SqlCommand expirecmd = new SqlCommand(expire, conn);
            expirecmd.Parameters.AddWithValue("@email", Base.tempemail);
            SqlDataReader reader = expirecmd.ExecuteReader();
            if (reader.Read())
            {
                if (currentTime - Convert.ToDateTime(reader["EmailSentTime"]) >= duration)
                {
                    reader.Close();
                    string linkexpired = "UPDATE [TempUser] SET EmailLinkUsed=@expired WHERE Email=@email";
                    SqlCommand linkexpiredcmd = new SqlCommand(linkexpired, conn);
                    linkexpiredcmd.Parameters.AddWithValue("@email", Base.tempemail);
                    linkexpiredcmd.Parameters.AddWithValue("@expired", "");
                    linkexpiredcmd.ExecuteNonQuery();
                }
                else
                {
                    return;
                }
            }
            conn.Close();
        }
    }
}