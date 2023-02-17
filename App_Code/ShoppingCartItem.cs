using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _211792H.App_Code
{
    public class ShoppingCartItem : IEquatable<ShoppingCartItem>
    {
        public int Quantity { get; set; }

        private string _ItemID;
        public string ItemID
        {
            get { return _ItemID; }
            set { _ItemID = value; }
        }

        private string _ItemName;
        public string Product_Name
        {
            get { return _ItemName; }
            set { _ItemName = value; }
        }

        private string _ItemImage;
        public string Product_Image
        {
            get { return _ItemImage; }
            set { _ItemImage = value; }

        }

        private decimal _ItemPrice;
        public decimal Product_Price
        {
            get { return _ItemPrice; }
            set { _ItemPrice = value; }
        }

        private decimal? _ItemDiscountPrice;

        public decimal? Product_DiscountPrice
        {
            get { return _ItemDiscountPrice; }
            set { _ItemDiscountPrice = value; }
        }

        public decimal TotalPrice
        {
            get {
                if (Product_DiscountPrice== null)
                {
                    return Product_Price * Quantity;
                }

                else
                {
                    return decimal.Parse(Product_DiscountPrice.ToString()) * Quantity;
                }
                 
            }
        }

        public ShoppingCartItem(string productID)
        {
            this.ItemID = productID;
        }

        public ShoppingCartItem(string productID, Product prod)
        {
            this.ItemID = productID;
            this.Product_Name = prod.Product_Name;
            this.Product_Image = prod.Product_Image;
            this.Product_Price = prod.Product_Price;
            this.Product_DiscountPrice = prod.Product_DiscountedPrice;
        }

        public ShoppingCartItem(string productID, string productName, string productImg, decimal productPrice , decimal? productDiscountedPrice)
        {
            this.ItemID = productID;
            this.Product_Name = productName;
            this.Product_Image = productImg;
            this.Product_Price = productPrice;
            this.Product_DiscountPrice = productDiscountedPrice;
        }

        public bool Equals(ShoppingCartItem anItem)
        {
            return anItem.ItemID == this.ItemID;
        }
    }
}