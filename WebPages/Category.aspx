<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Base.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="_211792H.WebPages.Category" %>
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
        <div class="itemcontainer">
            <h4 class="text-info" id="header"></h4>
            <div id="NoResults" ClientIDMode="static" runat="server">No Results Found</div>
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
                                    <asp:Label CssClass="lblpriceclass" ID="lblPrice" runat="server" Text='<%# "$" + Eval("Price") %>' ></asp:Label>
                                    <asp:Label CssClass="lbldiscount" runat="server" Text='<%# "$" + Eval("DiscountedPrice") %>'></asp:Label>
                                </div>                               
                            </div>
                            <div class="OverallReviews">
                                <asp:Label runat="server" Text='<%#Eval("OverallReviews")%>'></asp:Label>
                            </div>
                            
                            <div class="buttons">
                                <asp:LinkButton runat="server" ID="wishlistbutton" CommandArgument='<%#Eval("Id")%>' OnClick="wishlistinteract"/>
                                <asp:LinkButton runat="server" CommandArgument='<%#Eval("Id")%>' OnClick="AddToCart" CssClass="btn btn-outline-success" Text="Add To Cart"/>
                            </div>

                        </div>
                        <br />
                    </div>
                    </ItemTemplate>
            </asp:Repeater>
        </div>

        <script src="../Assets/js/product.js"></script>
        <script>
            let params = (new URL(document.location)).searchParams;
            let category = params.get("Category");
            const header = document.getElementById("header");
            header.innerHTML = toTitleCase(category);

            function toTitleCase(str) {
                return str.replace(/\w\S*/g, function (txt) {
                    return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
                });
            }


        </script>
    </body>
    </html>
</asp:Content>
