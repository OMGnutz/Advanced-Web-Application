<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin2.Master" AutoEventWireup="true" CodeBehind="AddProduct.aspx.cs" Inherits="_211792H.WebPages.Admin.AddProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <!DOCTYPE html>
    <html>
    <head>
        <title></title>

        <script>
            var test = false;

            function validateprice(sender, args) {
                var re = /^(?!^0\.00$)(([1-9][\d]{0,6})|([0]))\.[\d]{2}$/;
                var passed = args.Value.match(re);
                if (args.Value == "") {
                    args.IsValid = false;
                }
                if (passed == null) {
                    args.IsValid = false;
                }

                
            }

            function validateproductgame(sender, args) {
                const games = ["valorant", "csgo", "league of legends", "among us", "escape simulator", "brawhalla", "minecraft"]
               

                if (args.Value == ""){
                    args.IsValid = false;
                };

                for (let i = 0; i < games.length; i++) {
                    if (args.Value.toLowerCase().match(games[i])) {
                        return
                    }

                    if (i == games.length - 1) {
                        args.IsValid = false;
                    }
                }

            }

            function validateproductname() {
                $.ajax({
                    url: '/CheckProductNameExist/GetResult',
                    type: "GET",
                    dataType: "JSON",
                    data: { userinput: $("input[id$='txt_name']").val() },
                    success: function (data) {
                        if (data == "exist") {
                            test = true;  
                            console.log(test)
                        }

                        else {
                            test = false
                            console.log(test)
                        }
                    }
                })
            }

            function validatedname(sender, args) {
                if (test == true) { 
                    args.IsValid = false;
                }
            }



        </script>
        
        
    </head>

    <body>
        <form action="#">
            <div class="alert alert-dismissible alert-success" id="insertSuccess" runat="server" style="width:700px">
              <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
              <strong>Success</strong> Product has successfully been added.
            </div>

            <div class="alert alert-dismissible alert-danger" id="insertfailed" runat="server" style="width:700px">
              <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
              <strong>Failed</strong> Product has failed to be added.
            </div>

          <fieldset>
            <legend>Add Product</legend>
            <div class="form-group">
               <asp:Label ID="lblFile" runat="server" Text="Image"></asp:Label>
               <asp:FileUpload CssClass="form-control" ID="productimg" runat="server" />
               <asp:RequiredFieldValidator runat="server" ControlToValidate="productimg" ErrorMessage="Please select an image" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
              <br />
            <div class="form-group">
               <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
               <asp:TextBox runat="server" ID="txt_name" CssClass="form-control" onKeyUp="validateproductname()"></asp:TextBox>
               <asp:CustomValidator ValidateEmptyText="true" runat="server" ControlToValidate="txt_name" EnableClientScript="true" ClientValidationFunction="validatedname" ErrorMessage="Product exists" ForeColor="Red"></asp:CustomValidator>
                <br />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_name" ErrorMessage="Please enter a name" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            <br />
            <div class="form-group">
                <asp:Label ID="lbl_gamename" runat="server" Text="Game Name"></asp:Label>
                <asp:TextBox ID="txt_gamename" runat="server" CssClass="form-control" CausesValidation="true"></asp:TextBox>
                <asp:CustomValidator runat="server" ControlToValidate="txt_gamename" ValidateEmptyText="true" EnableClientScript="true" ClientValidationFunction="validateproductgame" ErrorMessage="Please enter a valid game" ForeColor="Red"></asp:CustomValidator>
            </div>
            <br />
            <div class="form-group">
               <asp:Label runat="server" ID="lbl_desc" Text="Description"></asp:Label>
               <asp:TextBox runat="server" ID="txt_desc" CssClass="form-control" TextMode="MultiLine" Style="width: 100%; height: 80px; border: 0.5px solid"></asp:TextBox>
                
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_desc" ErrorMessage="Please enter a description" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
              <br />
            <div class="form-group">
              <asp:Label runat="server" ID="lbl_price" Text="Price"></asp:Label>
               <asp:TextBox CssClass="form-control" runat="server" ID="txt_price" CausesValidation="true"></asp:TextBox>
               <asp:CustomValidator ID="pricevalidator" runat="server" ControlToValidate="txt_price" ValidateEmptyText="true" EnableClientScript="true" ClientValidationFunction="validateprice" ErrorMessage="Please enter a valid price" ForeColor="Red"></asp:CustomValidator>
            </div>
            <asp:Button ID="insertprodbutton" CssClass="btn btn-primary" runat="server" Text="Insert Product" data-dismiss="modal" UseSubmitBehavior="false" OnClick="insertprod"/>
          </fieldset>
        </form>



    </body>
    </html>

</asp:Content>
