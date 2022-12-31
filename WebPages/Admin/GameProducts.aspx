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
        <div style="margin-top:50px; margin-left:50px;">
            <asp:Button CssClass="btn btn-outline-success" runat="server" Text="Add Product" OnClick="Addprod" />
            <asp:HyperLink runat="server" href="GiveDiscountProd.aspx" CssClass="btn btn-outline-light" Text="Give discount to product"></asp:HyperLink>
            <asp:HyperLink runat="server" href="GiveDiscountCat.aspx" CssClass="btn btn-outline-light" Text="Give discount to categories"></asp:HyperLink>
        </div>

        

       <div class="products">
            <asp:Repeater ID="RepeaterProd" runat="server">
                <ItemTemplate>
                    <div class="indiproducts">
                        <asp:Image CssClass="productimg" runat="server" ImageUrl='<%#Eval("Image") %>' />
                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                        <br />
                        <asp:Label ID="lblPrice" runat="server" Text='<%#Eval("Price") %>' ></asp:Label>
                        <asp:HyperLink runat="server" href='<%# ResolveClientUrl("EditProducts.aspx?ProdID=" + Eval("Id"))%>' CssClass="btn btn-outline-light" Text="Edit Item"></asp:HyperLink>
                        <asp:LinkButton runat="server" CssClass="btn btn-outline-danger" CommandArgument='<%#Eval("Id")%>' OnClick="Deleteprod" Text="Delete Item"></asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        
    </body>
    </html>
</asp:Content>
