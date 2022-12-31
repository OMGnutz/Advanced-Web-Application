using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _211792H.App_Code
{
    public class Product
    {
        string _connStr = ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString;
        private string _prodID = null;
        private string _prodName = string.Empty;
        private string _prodDesc = "";
        private decimal _prodPrice = 0;
        private string _prodImg = "";
        private string _GameOrigin = "";
        private string _prodCategory = null;
        private string _prodGenre = null;
        private string _prodReviews = null;
        private decimal? _DiscountedPrice = null;
        private DateTime? _ReleaseDate = null;
        private int? _TimesBought = 0;


        public Product(string prodID, string prodName, string prodDesc, string prodImg,
                   decimal prodPrice, string prodGame, string prodCategory, string prodGenre ,string prodReviews,
                   decimal? discountedprice , DateTime releaseDate , int? TimesBought)
        {
            _prodID = prodID;
            _prodName = prodName;
            _prodDesc = prodDesc;
            _prodPrice = prodPrice;
            _prodImg= prodImg;
            _GameOrigin= prodGame;
            _prodCategory = prodCategory;
            _prodGenre= prodGenre;
            _prodReviews = prodReviews;
            _DiscountedPrice = discountedprice;
            _ReleaseDate = releaseDate;
            _TimesBought = TimesBought;
        }

        public Product(string prodID, string prodName, string prodDesc, string prodImg,
                       decimal prodPrice, string prodGame, string prodGenre, DateTime releaseDate)
            :this (prodID , prodName , prodDesc , prodImg , prodPrice , prodGame , null , prodGenre , null , null , releaseDate , null)
        {  
        }

        

        public string Product_ID
        {
            get { return _prodID; }
            set { _prodID = value; }
        }

        public string Product_Name
        {
            get { return _prodName; }
            set { _prodName = value; }

        }

        public string Product_Desc
        {
            get { return _prodDesc; }
            set
            {
                _prodDesc = value;
            }
        }

        public decimal Product_Price
        {
            get { return _prodPrice; }
            set
            {
                _prodPrice = value;
            }
        }

        public string Product_Image
        {
            get { return _prodImg; }
            set { _prodImg = value; }
        }


        public string Product_Game
        {
            get { return _GameOrigin; }
            set { _GameOrigin = value; }
        }

        public string Product_Category
        {
            get { return _prodCategory; }
            set { _prodCategory = value; }
        }


        public string Product_Genre
        {
            get { return _prodGenre; }
            set { _prodGenre = value; }
        }

        public string Product_Reviews
        {
            get { return _prodReviews; }
            set { _prodReviews = value; }
        }

        public decimal? Product_DiscountedPrice
        {
            get { return _DiscountedPrice; }
            set { _DiscountedPrice = value; }
        }

        public DateTime? Product_ReleasedDate
        {
            get { return _ReleaseDate; }
            set { _ReleaseDate = value; }
        }

        public int? Product_Sales
        {
            get { return _TimesBought; }
            set { _TimesBought = value; }
        }

        public Product getProduct(string prodID)
        {
            Product prodDetail = null;
            string prodName, prodDesc, prodGame, prodGenre, prodReviews, prodImage, prodCategory;
            decimal prodPrice, prodDiscountedPrice;
            int prodSales;
            DateTime prodReleasedDate;

            string query = "SELECT * FROM [GameProducts] WHERE Id=@prodid";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand selectprod = new SqlCommand(query, conn);
            selectprod.Parameters.AddWithValue("@prodid" , prodID);
            conn.Open();
            SqlDataReader reader = selectprod.ExecuteReader();
            if (reader.Read())
            {
                prodName = reader["Name"].ToString();
                prodDesc = reader["Description"].ToString();
                prodPrice = decimal.Parse(reader["Price"].ToString());
                prodImage = reader["Image"].ToString();
                prodGame = reader["Game"].ToString() ;
                prodCategory = reader["Category"].ToString();
                prodGenre = reader["Genre"].ToString();
                prodReviews = reader["OverallReviews"].ToString();
                prodDiscountedPrice = decimal.Parse(reader["DiscountedPrice"].ToString()) ;
                prodSales = Int32.Parse(reader["TimesBought"].ToString());
                prodReleasedDate = DateTime.Parse(reader["ReleaseDate"].ToString());

                prodDetail = new Product(prodID, prodName, prodDesc, prodImage, prodPrice, prodGame,
                                         prodCategory, prodGenre, prodReviews, prodDiscountedPrice, prodReleasedDate, prodSales);
            }

            else
            {
                prodDetail = null;
            }
            conn.Close();
            reader.Close();
            return prodDetail;
        }

        public int ProductInsert()
        {
            int result = 0;
            string query = "INSERT INTO [GameProducts]" +
                            "(ID , Name , Description , Price , Image , GameOrigin , Genre , ReleaseDate)" +
                            "VALUES (@id , @name , @desc , @price , @img , @GO , @genre , @date)";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand insertProd = new SqlCommand(query, conn);
            insertProd.Parameters.AddWithValue("@id", this.Product_ID);
            insertProd.Parameters.AddWithValue("@name" , this.Product_Name);
            insertProd.Parameters.AddWithValue("@desc", this.Product_Desc);
            insertProd.Parameters.AddWithValue("price", this.Product_Price);
            insertProd.Parameters.AddWithValue("@img", this.Product_Image);
            insertProd.Parameters.AddWithValue("@GO", this.Product_Game);
            insertProd.Parameters.AddWithValue("@genre", this.Product_Genre);
            insertProd.Parameters.AddWithValue("@date", this.Product_ReleasedDate);
            conn.Open();
            result += insertProd.ExecuteNonQuery();
            conn.Close();
            return result;
        }

    }



}