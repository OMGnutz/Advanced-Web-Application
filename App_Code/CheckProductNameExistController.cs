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
    public class CheckProductNameExistController : Controller
    {
        // GET: CheckProductNameExist
        public ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult GetResult(string userinput)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            bool exists = false;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [GameProducts] WHERE Name = @name", conn);
                cmd.Parameters.AddWithValue("@name", userinput);
                exists = (int)cmd.ExecuteScalar() > 0;
                if (exists)
                {
                    conn.Close();
                    return Json("exist", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    conn.Close();
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Json("Error -" + ex);
            }



        }
    }
}