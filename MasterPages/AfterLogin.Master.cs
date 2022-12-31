using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _211792H.MasterPages
{
    public partial class AfterLogin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                userdropdown.InnerText = Session["username"].ToString();
            }
            else
            {
                Session["CHANGE_MASTERPAGE2"] = "~/MasterPages/Base.Master";
                Session["CHANGE_MASTERPAGE"] = null;
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            
        }

        protected void btnlogout_Click(object sender, EventArgs e)
        {
            Session["CHANGE_MASTERPAGE2"] = "~/MasterPages/Base.Master";
            Session["CHANGE_MASTERPAGE"] = null;
            Session["username"] = null;
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
}