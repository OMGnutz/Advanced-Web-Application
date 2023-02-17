<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordHijack.aspx.cs" Inherits="_211792H.WebPages.ChangePasswordHijack" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Assets/css/bootstraptheme.css" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
          <div class="container-fluid">
            <a class="navbar-brand" href="#">Logo</a>
           
          </div>
        </nav>


    <form action="#">
      <fieldset style="width:800px; margin-left:20px; margin-top:50px;">
        <legend>Change Password</legend> 
          <br />
        <div class="form-group">
            <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
            <br />
            <div class="form-group">
            <asp:TextBox ID="txt_password" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
            <div class="invalid-feedback" runat="server" id="WeakPass">Please use a strong password</div> 
            <br />
            <asp:CustomValidator ValidateEmptyText="true" ValidationGroup="PassVerify" ID="CustomValidatorPassword" runat="server" EnableClientScript="true" ClientValidationFunction="StrongPassword" ControlToValidate="txt_password"></asp:CustomValidator>
            </div>
            <br />
            <br />
            <asp:Label ID="lblCPassword" runat="server" Text="Confirm Password"></asp:Label>
            <br />
            <div class="form-group">
            <asp:TextBox ID="txt_Cpassword" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
            <div class="invalid-feedback" runat="server" id="CPass">Passwords do not match</div> 
            <div class="invalid-feedback" runat="server" id="SamePass" style="display:none;">Password cannot be the same</div> 
            <br />
            <asp:CustomValidator ValidateEmptyText="true" ValidationGroup="PassVerify" ID="ComparePass" runat="server" EnableClientScript="true" ClientValidationFunction="ComparePassword" ControlToValidate="txt_Cpassword"></asp:CustomValidator>
            </div>
        </div>
        <asp:Button ID="Buttonchangepass" CssClass="btn btn-primary float-right" runat="server" Text="Change Password" OnClick="ChangePass" UseSubmitBehavior="false" ValidationGroup="PassVerify" data-dismiss="modal" />
      </fieldset>
    </form>
      
    </form>

    <script>
        function StrongPassword(sender, args) {
            var decimal = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$/;

            if (document.getElementById("txt_password").value.match(decimal)) {
                console.log("strong");
                document.getElementById("txt_password").classList.remove('is-invalid');
                document.getElementById("WeakPass").style.display = "none";
            }

            else {
                console.log("weak");
                console.log(document.getElementById("txt_password").value.match(decimal));
                document.getElementById("txt_password").classList.add('is-invalid');
                document.getElementById("WeakPass").style.display = "block";
                args.IsValid = false;
            }
        }

        function ComparePassword(sender, args) {
            if (document.getElementById("txt_Cpassword").value != document.getElementById("txt_password").value) {
                document.getElementById("CPass").style.display = "block";
                document.getElementById("txt_Cpassword").classList.add("is-invalid")
                args.IsValid = false;
            }

            else {
                document.getElementById("CPass").style.display = "none";
                document.getElementById("txt_Cpassword").classList.remove('is-invalid')
            }
        }
    </script>
</body>
</html>
