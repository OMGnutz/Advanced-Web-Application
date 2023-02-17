<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin2.Master" AutoEventWireup="true" CodeBehind="GiveDiscount.aspx.cs" Inherits="_211792H.WebPages.Admin.GiveDiscount" %>
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

         <div class="alert alert-dismissible alert-success" id="alertsuccess" runat="server" style="width:700px">
              <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
              <strong>Success</strong> Discount has been given.
        </div>

        <div style="margin-top:50px; margin-bottom:30px;">
            <ol class="breadcrumb" style="background-image:none; border:none;">
              <li class="breadcrumb-item"><a href="GameProducts.aspx">Manage Products</a></li>
              <li class="breadcrumb-item"><a href="GiveDiscountProd.aspx">Select Products</a></li>
              <li class="breadcrumb-item active">Give Discount</li>
            </ol>
        </div>
        <br />

        <form action="#">
          <fieldset>
            <legend>Give Discount</legend>
            <br />
            <p>Selected items</p>
            <div class="list-group" runat="server">  
            <asp:Repeater ID="RepeaterProd" runat="server">
                <ItemTemplate>              
                    <asp:Label ID="lblName" CssClass="list-group-item list-group-item-action" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                </ItemTemplate>
            </asp:Repeater>
            </div>
            
            <br />

            <div class="form-group">
               <asp:Label ID="lblPerc" runat="server" Text="Discount percentage"></asp:Label>
               <asp:TextBox runat="server" ID="txt_perc" CssClass="form-control" onKeyPress="return numbersOnly(event)" ></asp:TextBox>
               <asp:RangeValidator runat="server" ControlToValidate="txt_perc" MinimumValue="5" MaximumValue="95" Type="Integer" Text="Please enter a valid percentage" ForeColor="Red"></asp:RangeValidator>
               <br />
               <asp:RequiredFieldValidator ControlToValidate="txt_perc" runat="server" ErrorMessage="Please enter a valid percentage" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            <br />

            <div class="form-group">
                <asp:Label ID="lblEnddate" runat="server" Text="Sale End Date"></asp:Label> 
                <asp:TextBox ID="txt_enddate" runat="server" CssClass="form-control" CausesValidation="true" onKeyPress="return FilterDate(event)"></asp:TextBox>
                <asp:RegularExpressionValidator  ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please enter date in dd/mm/yy format" ForeColor="Red" ControlToValidate="txt_enddate" ValidationExpression="(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/\d{2}"></asp:RegularExpressionValidator>
                <br />
                <asp:CustomValidator ValidateEmptyText="true" runat="server" EnableClientScript="true" ClientValidationFunction="validatedate" ControlToValidate="txt_enddate" ErrorMessage="Please enter a valid date" ForeColor="Red"></asp:CustomValidator>
            </div>
            <br />
            <asp:Button ID="DiscountBut" CssClass="btn btn-primary" runat="server" Text="Give Discount" data-dismiss="modal" UseSubmitBehavior="false" CausesValidation="true" OnClick="DiscountBut_Click" />
          </fieldset>
        </form>


        <script type="text/javascript">

            function validatedate(sender , args) {
                var input = document.getElementById('<%= txt_enddate.ClientID %>').value;
                var inputlast2 = input.slice(-2);
                var inputmiddle2 = input.slice(3 , 5);
                var inputfirst2 = input.slice(0, 2);
                var today = new Date();
                var currentday = String(today.getDate()).padStart(2, '0');
                var currentmonth = String(today.getMonth() + 1).padStart(2, '0');
                var currentyear = String(today.getFullYear()).slice(-2);

                if (args.Value == "") {
                    args.IsValid = false;
                }

                if (parseInt(currentyear) > parseInt(inputlast2)) {
                    args.IsValid = false;
                }

                else if (parseInt(currentyear) == parseInt(inputlast2)) {
                    if (parseInt(currentmonth) > parseInt(inputmiddle2)) {
                        args.IsValid = false;
                    }

                    else if (parseInt(currentmonth) == parseInt(inputmiddle2)) {
                        if (parseInt(currentday) >= parseInt(inputfirst2)) {
                            args.IsValid = false;
                        }
                    }
                }   

            }


            function numbersOnly(event) {
                var aCode = event.which ? event.which : event.keyCode;
                if (aCode > 31 && (aCode < 48 || aCode > 57)) {
                    return false;
                }

                else {
                    return true;
                }   
            }

            function FilterDate(event) {
                var aCode = event.which ? event.which : event.keyCode;
                var inputlength = event.target.value.length;
                if (inputlength == 2 || inputlength == 5) {
                    event.target.value += '/'
                }

                if (inputlength > 7) {
                    return false;
                }

                if (aCode > 31 && (aCode < 48 || aCode > 57)) {
                    return false;
                }

                else {
                    return true;
                } 
            }



        </script>
       
        
    </body>
    </html>


</asp:Content>
