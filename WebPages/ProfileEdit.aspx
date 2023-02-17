<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Base.Master" AutoEventWireup="true" CodeBehind="ProfileEdit.aspx.cs" Inherits="_211792H.WebPages.ProfileEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!DOCTYPE html>
    <html>
    <head>
        <title>Home</title>
        <link rel="stylesheet" type="text/css" href="../Assets/css/EditProfile.css" />
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>

    </head>
    <body>
        <div style="margin-top:50px; margin-bottom:30px; margin-left:auto; margin-right:auto; width:850px;">
            <ol class="breadcrumb" style="background-image:none; border:none;">
              <li class="breadcrumb-item"><a href="Profile.aspx">View Profile</a></li>
              <li class="breadcrumb-item active">Edit Profile</li>
            </ol>
        </div>

        <form action="#" >
          <fieldset style="width:850px; margin-left:auto; margin-right:auto;">
            <legend>Edit Profile</legend>
            <br />
 
            <div class="form-group">
               <asp:Label ID="lblFile" runat="server" Text="Image"></asp:Label>
               <asp:FileUpload CssClass="form-control" ID="productimg" runat="server" />
               <asp:RequiredFieldValidator runat="server" ControlToValidate="productimg" ErrorMessage="Please select an image" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            <br />
            <asp:Button ID="editprofilebut" CssClass="btn btn-outline-success" runat="server" Text="Edit profile" data-dismiss="modal" UseSubmitBehavior="false" CausesValidation="true" OnClick="editprofile" />
           
          </fieldset>
        </form>
        
        <script src="../Assets/js/product.js"></script>
    </body>
    </html>
</asp:Content>
