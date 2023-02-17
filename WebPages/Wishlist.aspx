<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AfterLogin.Master" AutoEventWireup="true" CodeBehind="Wishlist.aspx.cs" Inherits="_211792H.WebPages.Wishlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!DOCTYPE html>
    <html>
    <head>
        <title>Home</title>
        <link rel="stylesheet" type="text/css" href="../Assets/css/Profile.css" />
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>

    </head>
    <body>
        <div style="margin-top:50px; margin-bottom:30px; margin-left:auto; margin-right:auto; width:850px;">
            <ol class="breadcrumb" style="background-image:none; border:none;">
              <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
              <li class="breadcrumb-item active">Wishlist</li>
            </ol>
        </div>

        <br />
        <p id="recentlyp">Wishlist</p>
        <div class="itemcontainer">
            <p style="display:none;text-align:center; margin-top:20px;" runat="server" id="noitems">There is currently no items in your wishlist</p>
            <asp:Repeater ID="RepeaterProd" runat="server">
                    <ItemTemplate> 
                    <div class="indicontainer">
                        <div class="indiproducts">
                            <asp:ImageButton PostBackUrl='<%# ResolveClientUrl("GameProduct.aspx?Id=" + Eval("Id"))%>' CssClass="productimg" runat="server" ImageUrl='<%#Eval("Image") %>'  />
                            <div class="prodDetails">
                                 <asp:Label CssClass="productname" ID="lblName" runat="server" Text='<%#Eval("Name").ToString().ToUpper()%>'></asp:Label>
                                 <asp:Label CssClass="productRD" runat="server" Text='<%#DateTime.Parse(Eval("ReleaseDate").ToString()).ToString("dd MMMM, yyyy")%>'></asp:Label>
                            </div>
                            <div class="pricecontainer">
                                <div class="discountperc">
                                </div>
                                <div style="display:inline-block; font-size:13px;">
                                    <asp:Label  CssClass="lblpriceclass" ID="lblPrice" runat="server" Text='<%# "$" + Eval("Price") %>' ></asp:Label>
                                    <asp:Label CssClass="lbldiscount" runat="server" Text='<%# "$" + Eval("DiscountedPrice") %>'></asp:Label>
                                </div>                               
                            </div>
                            <div class="OverallReviews">
                                <asp:Label runat="server" Text='<%#Eval("OverallReviews")%>'></asp:Label>
                            </div>
                            <div class="buttons">
                                <asp:LinkButton runat="server" CommandArgument='<%#Eval("Id")%>' CssClass="btn btn-outline-danger" Text="Remove From Wishlist" OnClick="wishlistinteract"/>
                                <asp:LinkButton runat="server" CommandArgument='<%#Eval("Id")%>' CssClass="btn btn-outline-success" OnClick="AddToCart" Text="Add To Cart"/>
                            </div>
                        </div>
                        <br />
                    </div>
                    </ItemTemplate>
            </asp:Repeater>
        </div>

        <script src="../Assets/js/product.js"></script>
    </body>
    </html>
</asp:Content>
