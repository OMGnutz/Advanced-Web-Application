using _211792H.MasterPages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using _211792H.App_Code;
using System.Drawing;

namespace _211792H.WebPages
{
    public partial class AccountDetail : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("Home.aspx");
            }
            else
            {
                usrname.Text = Session["username"].ToString();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
                conn.Open();
                SqlCommand slctuser = new SqlCommand("SELECT * FROM [User] WHERE Username=@usrname", conn);
                slctuser.Parameters.AddWithValue("@usrname" , Session["username"].ToString());
                SqlDataReader reader = slctuser.ExecuteReader();
                reader.Read();
                usremail.Text = reader["Email"].ToString().Substring(0, 2) + "***" + reader["Email"].ToString().Substring(reader["Email"].ToString().Length - 9);
                if (reader["TFAEnabled"].ToString() == "True")
                {
                    TFAstatus.Text = "Two Factor Authentication is enabled";
                    TFAstatus.ForeColor = Color.Green;
                    TFAbutt.Text = "Disable TFA";
                    TFAbutt.Attributes.Add("class", "btn btn-outline-danger");
                }

                else
                {
                    TFAstatus.Text = "Two Factor Authentication is disabled";
                    TFAstatus.ForeColor = Color.Red;
                    TFAbutt.Text = "Enable TFA";
                    TFAbutt.Attributes.Add("class", "btn btn-outline-success");
                }

                reader.Close();
                conn.Close();
                ((HtmlGenericControl)this.Master.FindControl("carticon")).Style.Add("display", "none");
                ((HtmlGenericControl)this.Master.FindControl("nav")).Style.Add("display", "none");
            }
        }


        protected void changeTFAstatus(object sender , EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            SqlCommand slctTFAstat = new SqlCommand("SELECT TFAEnabled FROM [User] WHERE Username=@usrname", conn);
            SqlCommand updtTFAstat = new SqlCommand("UPDATE [User] SET TFAEnabled=@status WHERE Username=@usrname", conn);
            updtTFAstat.Parameters.AddWithValue("@usrname", Session["username"].ToString());
            slctTFAstat.Parameters.AddWithValue("@usrname", Session["username"].ToString());
            SqlDataReader reader = slctTFAstat.ExecuteReader();
            reader.Read();
            if (reader["TFAEnabled"].ToString() == "True")
            {
                updtTFAstat.Parameters.AddWithValue("@status", false);
                reader.Close();
                updtTFAstat.ExecuteNonQuery();
                TFAstatus.Text = "Two Factor Authentication is disabled";
                TFAstatus.ForeColor = Color.Red;
                TFAbutt.Text = "Enable TFA";
                TFAbutt.Attributes.Add("class", "btn btn-outline-success");
            }
            else
            {
                updtTFAstat.Parameters.AddWithValue("@status", true);
                reader.Close();
                updtTFAstat.ExecuteNonQuery();
                TFAstatus.Text = "Two Factor Authentication is enabled";
                TFAstatus.ForeColor = Color.Green;
                TFAbutt.Text = "Disable TFA";
                TFAbutt.Attributes.Add("class", "btn btn-outline-danger");
            }
            conn.Close();
        }

        protected void changepassbutt_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx");
        }
    }
}