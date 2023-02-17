<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin2.Master" AutoEventWireup="true" CodeBehind="GiveDiscountProd.aspx.cs" Inherits="_211792H.WebPages.Admin.GiveDiscountProd" %>
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

        <div class="alert alert-dismissible alert-danger" id="submitfailed" runat="server" style="width:700px">
              <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
              <strong>Failed</strong> Please select at least one item.
        </div>

        <div class="alert alert-dismissible alert-success" id="alertsuccess" runat="server" style="width:700px">
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            <strong>Success</strong> Discount has been given.
        </div>

        <div style="margin-top:50px; margin-bottom:30px;">
            <div>
                <ol class="breadcrumb" style="background-image:none; border:none;">
                  <li class="breadcrumb-item"><a href="GameProducts.aspx">Manage Products</a></li>
                  <li class="breadcrumb-item active">Select Products</li>
                </ol>
                <div style="top:0px; float:right;">
                <asp:TextBox ID="search" CssClass="form-control me-sm-2" runat="server" PlaceHolder="Search for an item" OnKeyUp="filter()"></asp:TextBox>
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
       
       <div class="products">
           <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:Repeater ID="RepeaterProd" runat="server">
                <ItemTemplate>
                        <asp:UpdatePanel ID="produpdatepanel"  runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="indicontainer">
                                    <div class="indiproducts" runat="server">
                                     <asp:Image CssClass="productimg" runat="server" ImageUrl='<%#Eval("Image") %>' />
                                     <asp:Label ID="lblName" CssClass="productname" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                     <asp:Label CssClass="productgame" ID="prodgamename" runat="server" Text='<%#Eval("GameOrigin")%>'></asp:Label>
                                     <br />
                                     <div class="pricecontainer">
                                        <asp:Label CssClass="lblpriceclass" ID="lblPrice" runat="server" Text='<%# "$" + Eval("Price") %>' ></asp:Label>
                                        <asp:Label CssClass="lbldiscount" runat="server" Text='<%# "$" + Eval("DiscountedPrice") %>'></asp:Label>
                                     </div>
                                     <div class="buttons">
                                         <asp:Button runat="server" CssClass="btn btn-outline-primary select" CommandArgument='<%#Eval("Id")%>' OnClick="selectItem" Text="Select Item" UseSubmitBehavior="false" ></asp:Button>
                                     </div>                                    
                                    </div>
                                    <br />
                                </div>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div>
            <asp:Button  ID="submit" runat="server"  CssClass="btn btn-outline-primary" UseSubmitBehavior="false" OnClick="Submit" Text="Submit" />
        </div>


        <script type="text/javascript">
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

            var products = document.getElementsByClassName("indicontainer");
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

            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                var products = document.getElementsByClassName("indicontainer");
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
                        discountprice.style.display = "block";
                        discountprice.style.color = "white";
                    }
                }
            });

        </script>


        
    </body>
    </html>

    
</asp:Content>
