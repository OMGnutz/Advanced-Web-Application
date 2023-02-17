using _211792H.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _211792H.MasterPages
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                userdropdown.InnerText = Session["username"].ToString();

            }
            else
            {
                btnlogout_Click(sender, e);
            }
        }

        protected void btnlogout_Click(object sender, EventArgs e)
        {
            Session["username"] = null;
            Response.Redirect("../Home.aspx");
        }
    }
}