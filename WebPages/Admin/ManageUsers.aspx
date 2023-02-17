<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin2.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="_211792H.WebPages.Admin.ManageUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!DOCTYPE html>
    <html>
    <head>
        <title></title>
        <link rel="stylesheet" type="text/css" href="../../Assets/css/ManageUser.css" runat="server" />
    </head>

    <body>
        <div style="margin-top:50px; margin-bottom:30px;">
            <div>
                 <div style="width:200px; margin-left:auto; margin-right:auto; position:relative; left:325px;">
                    <asp:TextBox ClientIDMode="Static" ID="search" CssClass="form-control me-sm-2" runat="server" PlaceHolder="Search for a user" OnKeyUp="filter()"></asp:TextBox>
                 </div>
            </div>

        </div>

        <br />    
       <div class="products" id="prodcontainer">
            <asp:Repeater ID="RepeaterProd" runat="server">
                <ItemTemplate>
                    <div class="indicontainer">         
                        <div class="indiproducts">
                        <asp:Image CssClass="productimg" runat="server" ImageUrl='<%# "../" + Eval("Profilepic") %>'  />
                        <asp:Label CssClass="productname" ID="lblName" runat="server" Text='<%#Eval("Username")%>'></asp:Label>         
                        <div class="buttons">
                            <asp:LinkButton runat="server" CssClass="btn btn-outline-danger" CommandArgument='<%#Eval("Id")%>' OnClick="Deleteuser" Text="Delete Account"></asp:LinkButton>
                        </div>   
                    </div>
                    <br/>
                    </div>   
                </ItemTemplate>
            </asp:Repeater>
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


            var reviews = document.getElementsByClassName("indiproducts")

            for (let el = 0; el < reviews.length; el++) {
                var img = reviews[el].getElementsByClassName("productimg")[0]
                if (String(img.getAttribute('src')) == "../") {
                    img.src = "../../images/profilepic2.png"
                }
            }


        </script>
        
    </body>
    </html>
</asp:Content>
