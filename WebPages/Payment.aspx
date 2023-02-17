<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Base.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="_211792H.WebPages.Payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!DOCTYPE html>
    <html>
    <head>
        <title></title>
        <link rel="stylesheet" type="text/css" href="../Assets/css/Payment.css" />
    </head>

    <body>

            <div class="alert alert-dismissible alert-danger" id="insertfailed" runat="server" style="width:700px">
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                <strong>Failed</strong> Payment has failed.
            </div>

            <div class="alert alert-dismissible alert-success" id="PSalert" runat="server" style="top:0px; position:absolute; left: 50%; transform: translate(-50%, 0%); z-index:999;">
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                <strong>Purchase Successful</strong> Enjoy your new purchases.
            </div>

            <div class="modal" id="modalpopup" runat="server" style="display:none; top:250px;">
              <div class="modal-dialog" role="document">
                <div class="modal-content">
                  <div class="modal-body">
                    <p>You will soon be redirected to the home page</p>
                    <div class="progress">
                      <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%;"></div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
        <form action="#">
          <fieldset>
            <legend>Payment Method</legend>
            <div class="form-group">
               <asp:Label ID="lblPaymentMethod" runat="server" Text="Please select a payment method"></asp:Label>
               <asp:DropDownList CssClass="form-select" runat="server" id="paymentMethodDropdown" ClientIDMode="Static" >
                   <asp:ListItem Selected="True" Value="Visa">Visa</asp:ListItem>
                   <asp:ListItem Value="AmericanExpress">American Express</asp:ListItem>
                   <asp:ListItem Value="MasterCard">MasterCard</asp:ListItem>
               </asp:DropDownList>
            </div>
            <br />
            <div class="form-group">
               <asp:Label ID="lblCN" runat="server" Text="Card number"></asp:Label>
               <asp:TextBox runat="server" ID="txt_CN" CssClass="form-control" ClientIDMode="Static" onKeyPress="return numbersOnly(event)"></asp:TextBox>
               <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_CN" ErrorMessage="Please enter your card number" ForeColor="Red"></asp:RequiredFieldValidator>
               <div class="invalid-feedback" id="invalidCreditCard" ClientIDMode="Static" style="display:none;" runat="server">Please enter a valid credit card</div>
            </div>
            <br />
            <div class="form-group">
                <asp:Label ID="lbl_ExpiryDate" runat="server" Text="Expiry date"></asp:Label>
                <div class="datecontainer">
                    <asp:DropDownList CssClass="form-select" runat="server" ClientIDMode="Static" ID="MonthDropdown">
                    <asp:ListItem Selected="True" Value="--">--</asp:ListItem>
                    <asp:ListItem Value="01">01</asp:ListItem>
                    <asp:ListItem Value="02">02</asp:ListItem>
                    <asp:ListItem Value="03">03</asp:ListItem>
                    <asp:ListItem Value="04">04</asp:ListItem>
                    <asp:ListItem Value="05">05</asp:ListItem>
                    <asp:ListItem Value="06">06</asp:ListItem>
                    <asp:ListItem Value="07">07</asp:ListItem>
                    <asp:ListItem Value="08">08</asp:ListItem>
                    <asp:ListItem Value="09">09</asp:ListItem>
                    <asp:ListItem Value="10">10</asp:ListItem>
                    <asp:ListItem Value="11">11</asp:ListItem>
                    <asp:ListItem Value="12">12</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="form-select" runat="server" ClientIDMode="Static" ID="YearDropdown">
                        <asp:ListItem Selected="True" Value="----">----</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="invalid-feedback" id="invalidDate" ClientIDMode="Static" style="display:none; color:red;" runat="server">Please enter a valid date</div>
                <br />

                <div class="ccvcontainer">
                     <asp:Label ID="lbl_ccv" runat="server" Text="Security code"></asp:Label>
                     <asp:TextBox ID="txt_ccv" runat="server" CssClass="form-control" onKeyPress="return validateccv(event)" ></asp:TextBox>
                     <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_ccv" ErrorMessage="Please enter your ccv" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <br />            
            <asp:Button ID="paybutt" CssClass="btn btn-primary" runat="server" Text="Pay" data-dismiss="modal" UseSubmitBehavior="false" OnClick="payment"/>
          </fieldset>
        </form>

        <script>

            function validateccv(event) {
                var aCode = event.which ? event.which : event.keyCode;
                const length = document.getElementById('<%= txt_ccv.ClientID %>').value.length;
                if (length >= 3) {
                    return false;
                }

                if (aCode > 31 && (aCode < 48 || aCode > 57)) {
                    return false;
                }

                else {
                    return true;
                }

            }

            function numbersOnly(event) {
                var aCode = event.which ? event.which : event.keyCode;
                const length = document.getElementById('<%= txt_CN.ClientID %>').value.length;
                if (length > 15) {
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
