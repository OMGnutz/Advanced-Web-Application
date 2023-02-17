<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Base.Master" AutoEventWireup="true" CodeBehind="ViewCart.aspx.cs" Inherits="_211792H.WebPages.ViewCart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!DOCTYPE html>
    <html>
    <head>
        <title>Shopping Cart</title>
        <link rel="stylesheet" type="text/css" href="../Assets/css/cart.css" />
    </head>

    <body>
        <div class="cartcontainer">
        <div class="cartItems">
            <div style="font-family: 'Playfair Display', serif; font-size: 25px;">My Shopping Cart</div>
            <asp:GridView ID="gv_CartView" runat="server" AutoGenerateColumns="False" DataKeyNames="ItemID" GridLines="None" ShowHeader="False" OnRowCommand="gv_CartView_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table style="border: 0.9px solid gray; table-layout:fixed; width:550px; max-width:500px;">
                                <tr class="indicontainer" style="border: none;">
                                    <td style="border: none; width: 150px;">
                                        <asp:Image CssClass="Cartimage" ID="imgPoster" ImageUrl='<%#Eval("Product_Image") %>' runat="server" Style="height: 220px; width: 140px;" />
                                    </td>
                                    <td style="border: none">
                                        <table>
                                            <tr style="border: none">
                                                <td style="border: none">
                                                    <asp:Label
                                                        Style="font-family: 'Poppins', sans-serif; font-size: 23px;"
                                                        ID="lblTitle"
                                                        runat="server"
                                                        Text='<%#Eval("Product_Name") %>'>
                                                    </asp:Label>
                                                    <br></br>
                                                </td>
                                            </tr>

                                            <tr style="border: none">
                                                <td>
                                                    <div class="pricecontainer">
                                                        <div class="discountperc">
                                                        </div>
                                                        <div style="display:inline-block; font-size:13px;">
                                                            <asp:Label  CssClass="lblpriceclass" ID="lblPrice" runat="server" Text='<%# "$" + Eval("Product_Price") %>' ></asp:Label>
                                                        <asp:Label CssClass="lbldiscount" runat="server" Text='<%# "$" + Eval("Product_DiscountPrice") %>'></asp:Label>
                                                        </div>                               
                                                    </div>
                                                    <br></br>
                                                </td>
                                            </tr>

                                            <tr style="border: none">
                                                <td style="border: none">
                                                    <asp:TextBox                                                        
                                                        ID="tb_Quantity"
                                                        runat="server"
                                                        Text='<%# Eval("Quantity") %>'
                                                        Width="50px"
                                                        ClientIdMode="Static"
                                                        ReadOnly="False">
                                                    </asp:TextBox>
                                                    <asp:Button
                                                        ID="btnUpdate"
                                                        ClientIdMode="Static"
                                                        CssClass="btn btn-outline-info"
                                                        runat="server"
                                                        Text="Update"
                                                        OnClick="btnUpdate_Click"/>
                                                </td>
                                            </tr>

                                            <tr style="border: none;">
                                                <td style="font-size: 12px;">
                                                    <asp:LinkButton
                                                        CssClass="btn btn-outline-danger"
                                                        ID="linkRemove"
                                                        ClientIdMode="Static"
                                                        runat="server"
                                                        Text="Remove"
                                                        CommandArgument='<%# Eval("ItemID") %>'
                                                        CommandName="Remove">
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="ordersummary">
            <div class="title" style="text-align: center; font-family: 'Playfair Display', serif; font-size: 25px;">
                Order Summary                                                                
            </div>

            <div class="detailscript" style="font-family: 'Poppins', sans-serif; font-size: 13px; float: left;">
                <p>Subtotal</p>
                <p>Estimated Shipping</p>
                <p>Estimated Tax</p>
            </div>

            <div class="pricescript" style="font-family: 'Poppins', sans-serif; font-size: 13px; float: right;">
                <p>
                    <asp:Label ID="lbl_TotalPrice" runat="server" Text="$0.00"></asp:Label>
                </p>
                <p>
                    <asp:Label ID="lbl_ShippingPrice" runat="server" Text="$0.00"></asp:Label>
                </p>
                <p>$0.00</p>
            </div>

            <div class="checkout" style="width: 100%; float: left; border-top: 0.5px solid black; border-bottom: 0.5px solid black;">
                <div class="ordertotal" style="font-size: 20px; font-weight: bold;">
                    <p style="float: left;">Order Total:</p>
                    <p style="float: right">
                        <asp:Label ID="lbl_TotalPrice2" runat="server" Text="Label"></asp:Label>
                    </p>
                </div>

                <asp:Button ClientIDMode="Static" CssClass="btn btn-outline-success" ID="btnCheckOut" OnClick="btnCheckOut_Click" type="button" runat="server" Text="CHECKOUT" class="checkoutbtn" />
                <br />
                <p style="font-size: 12px; text-align: center; font-weight: bold;">Or Checkout With</p>
            </div>

            <div class="altpayment">
                
            </div>
        </div>
        </div>
        <script src="../Assets/js/product.js"></script>
    </body>
    </html>

</asp:Content>
