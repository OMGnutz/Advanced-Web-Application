<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin2.Master" AutoEventWireup="true" CodeBehind="GameProducts.aspx.cs" Inherits="_211792H.WebPages.Admin.GameProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!DOCTYPE html>
    <html>
    <head>
        <title></title>
        <link rel="stylesheet" type="text/css" href="../../Assets/css/AdminProduct.css" runat="server" />
    </head>

    <body>
        <div style="margin-top:50px; margin-bottom:30px;">
            <div>
                <asp:Button CssClass="btn btn-outline-success" runat="server" Text="Add Product" OnClick="Addprod" />
                <asp:HyperLink runat="server" href="GiveDiscountProd.aspx" CssClass="btn btn-outline-light" Text="Give discount to product"></asp:HyperLink>
                <asp:HyperLink runat="server" href="#" CssClass="btn btn-outline-light" Text="Give discount to categories"></asp:HyperLink>
                 <div style="top:0px; float:right;">
                    <asp:TextBox ClientIDMode="Static" ID="search" CssClass="form-control me-sm-2" runat="server" PlaceHolder="Search for an item" OnKeyUp="filter()"></asp:TextBox>
                 </div>
            </div>

        </div>

        <div>
            <asp:DropDownList CssClass="nav-item dropdown" id="gametitles" runat="server">
                <asp:ListItem Value="all">All</asp:ListItem>
                <asp:ListItem Value="valorant">Valorant</asp:ListItem>
                <asp:ListItem Value="csgo">CSGO</asp:ListItem>
                <asp:ListItem Value="league of legends">League Of Legends</asp:ListItem>
                <asp:ListItem Value="brawhalla">Brawhalla</asp:ListItem>
                <asp:ListItem Value="minecraft">Minecraft</asp:ListItem>
                <asp:ListItem Value="among us">Among Us</asp:ListItem>
                <asp:ListItem Value="escape simulator">Escape Simulator</asp:ListItem>
            </asp:DropDownList>
        </div>

        <br />    
       <div class="products" id="prodcontainer">
            <asp:Repeater ID="RepeaterProd" runat="server">
                <ItemTemplate>
                    <div class="indicontainer">
                        
                        <div class="indiproducts">
                        <asp:Image CssClass="productimg" runat="server" ImageUrl='<%#Eval("Image") %>'  />
                        <asp:Label CssClass="productname" ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                        <asp:Label CssClass="productgame" ID="productgamename" runat="server" Text='<%#Eval("GameOrigin")%>'></asp:Label>
                        <br />
                        <div class="pricecontainer">
                            <asp:Label CssClass="lblpriceclass" ID="lblPrice" runat="server" Text='<%# "$" + Eval("Price") %>' ></asp:Label>
                            <asp:Label CssClass="lbldiscount" runat="server" Text='<%# "$" + Eval("DiscountedPrice") %>'></asp:Label>
                        </div>
                        <div class="buttons">
                            <asp:HyperLink runat="server" href='#' CssClass="btn btn-outline-light" Text="Edit Item"></asp:HyperLink>
                            <asp:LinkButton runat="server" CssClass="btn btn-outline-danger" CommandArgument='<%#Eval("Id")%>' OnClick="Deleteprod" Text="Delete Item"></asp:LinkButton>
                        </div>
                        
                    </div>
                    <br/>
                    </div>   
                </ItemTemplate>
            </asp:Repeater>
        </div>



        <script type="text/javascript">


            var products = document.getElementsByClassName("indicontainer")
            var prices = document.getElementsByClassName("lblpriceclass")
            var game = document.getElementsByClassName("productgame")
            for (let el = 0; el < products.length; el++) {
                prices[el].style.color = "white";
                game[el].style.display = "none";
            }

            for (let el = 0; el < products.length; el++) {
                var discountprice = products[el].getElementsByClassName("lbldiscount")[0]
                if (discountprice.innerText == "$") {
                    discountprice.style.display = "none";
                }
                else {
                    var originalprice = products[el].getElementsByClassName("lblpriceclass")[0];
                    var string = new String(originalprice.innerText);
                    originalprice.innerHTML = string.strike();
                    originalprice.style.color = "grey";
                    discountprice.style.display = "inline-block";
                    discountprice.style.color = "white";
                }
            }




            function filter() {
               
                var products = document.getElementsByClassName("indicontainer");
                var input = document.getElementById('<%= search.ClientID %>').value.toLowerCase();
                for (i = 0; i < products.length; i++) {
                    var productname = products[i].getElementsByClassName("productname")[0].innerText;
                    if (productname.toLowerCase().indexOf(input) > -1) {
                        products[i].style.display = "block";
                    }

                    else {
                        products[i].style.display = "none";
                        
                    }
                }
            }

            var dropdown = document.getElementById('<%= gametitles.ClientID %>')
            dropdown.addEventListener("change", function () {
                if (this.value == "all") {
                    for (i = 0; i < products.length; i++) {
                        products[i].style.display = "block";
                    }
                    return;
                }
                else {
                    for (i = 0; i < products.length; i++) {
                        var productgametitle = products[i].getElementsByClassName("productgame")[0].innerText;
                        if (productgametitle.toLowerCase().indexOf(this.value) > -1) {
                            products[i].style.display = "block";
                        }
                        else {
                            products[i].style.display = "none";
                        }
                    }
                }
            })
        </script>
        
    </body>
    </html>
</asp:Content>
