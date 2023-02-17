using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _211792H.App_Code
{
    public class CodeBehindCommsController : Controller
    {
        // GET: CodeBehindComms
        public ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult GetRows(string userinput)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [GameProducts] WHERE DiscountedPrice IS NOT NULL", conn);
                return Json((int)cmd.ExecuteScalar(), JsonRequestBehavior.AllowGet);
               
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Json("Error -" + ex);
            }



        }
    }
}