using _211792H.App_Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace _211792H.WebPages
{
    public partial class ViewCart : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("Home.aspx");
            }

            else
            {
                ((HtmlGenericControl)this.Master.FindControl("carticon")).Style.Add("display", "none");
            }

            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        protected void LoadCart()
        {
            //bind the Items inside the Session/ShoppingCart Instance with the Datagrid
            gv_CartView.DataSource = ShoppingCart.Instance.Items;
            gv_CartView.DataBind();

            decimal shipping = 0.0m;
            decimal subtotal = 0.0m;
            decimal total = 0.0m;
            
            foreach (ShoppingCartItem item in ShoppingCart.Instance.Items)
            {
                subtotal = subtotal + item.TotalPrice;
            }

            total = subtotal + shipping;
            lbl_TotalPrice.Text = subtotal.ToString("C");
            lbl_TotalPrice2.Text = total.ToString("C");
            lbl_ShippingPrice.Text = shipping.ToString("C");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gv_CartView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string productId = gv_CartView.DataKeys[row.RowIndex].Value.ToString();

                    //row.Cells[2] means that the quantity textbox must be in column 3.
                    try
                    {
                        int quantity = int.Parse(((TextBox)row.Cells[0].FindControl("tb_Quantity")).Text);
                        if (quantity < 0)
                        {
                            return;
                        }
                        ShoppingCart.Instance.SetItemQuantity(productId, quantity);
                    }

                    catch 
                    {
                        return;
                    }
                    
                }
            }
            LoadCart();
        }

        protected void gv_CartView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                string productId = e.CommandArgument.ToString();
                ShoppingCart.Instance.RemoveItem(productId);
                LoadCart();
            }
        }

        protected void btnCheckOut_Click(object sender, EventArgs e)
        {
            if(ShoppingCart.Instance.Items.Count > 0)
            {
                Response.Redirect("Payment.aspx");
            }

            else
            {
                Debug.WriteLine(this.Master.FindControl("emptycart"));
                ((HtmlGenericControl)this.Master.FindControl("emptycart")).Style.Add("display", "block");
            }
            
        }

    }
}