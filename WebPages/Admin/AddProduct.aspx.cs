using _211792H.App_Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _211792H.WebPages.Admin
{
    public partial class AddProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void insertprod(object sender, EventArgs e)
        {
            int result = 0;
            string image = "";
            string genre = "";
            Guid newGUID = Guid.NewGuid();

            switch (txt_gamename.Text.ToLower())
            {
                case "valorant":
                    genre = "fps";
                    break;

                case "league of legends":
                    genre = "mmo";
                    break;

                case "csgo":
                    genre = "fps";
                    break;

                case "among us":
                    genre = "horror";
                    break;

                case "escape simulator":
                    genre = "rpg";
                    break;

                case "brawhalla":
                    genre = "rpg";
                    break;

                case "minecraft":
                    genre = "horror";
                    break;

            }

            if (productimg.HasFile == true)
            {
                image = "../../images/" + productimg.FileName;
            }

            Product prod = new Product(newGUID.ToString() , txt_name.Text , txt_desc.Text , image , decimal.Parse(txt_price.Text) , txt_gamename.Text.ToLower() , genre , DateTime.UtcNow);
            result = prod.ProductInsert();

            if (result > 0)
            {
                string saveimg = Server.MapPath(" ") + "\\" + image;
                productimg.SaveAs(saveimg);
                insertSuccess.Style.Add("display", "block");
            }

            else
            {
                insertfailed.Style.Add("display", "block");
            }

            txt_desc.Text = "";
            txt_gamename.Text = "";
            txt_name.Text = "";
            txt_price.Text = "";
        }
    }
}