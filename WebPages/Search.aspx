<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Base.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="_211792H.WebPages.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!DOCTYPE html>
    <html>
    <head>
        <title>Home</title>
        <link rel="stylesheet" type="text/css" href="../Assets/css/Products.css" />
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>

    </head>
    <body>
        <div style="width:850px; margin-left:auto; margin-right:auto; margin-top:30px;">
            <ol class="breadcrumb" style="width:300px;" runat="server" id="bc">
              <li class="breadcrumb-item active" id="usersearch" runat="server"></li>
            </ol>
            <br />
            <div id="NoResults" ClientIDMode="static" runat="server">No Results Found</div>
        </div>
        <div class="itemcontainer">
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
                                <asp:LinkButton runat="server" ID="wishlistbutton" CommandArgument='<%#Eval("Id")%>' OnClick="wishlistinteract"/>
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
