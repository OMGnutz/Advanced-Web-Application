<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Base.Master" AutoEventWireup="true" CodeBehind="AccountDetail.aspx.cs" Inherits="_211792H.WebPages.AccountDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!DOCTYPE html>
    <html>
    <head>
        <title>Home</title>
        <link rel="stylesheet" type="text/css" href="../Assets/css/AccountDetails.css" />
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>

    </head>
    <body>
        <div id="contents" style="margin-left:auto; margin-right:auto; width:850px;">
            <div style="margin-top:50px; margin-bottom:30px;">
                <ol class="breadcrumb" style="background-image:none; border:none;">
                  <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                  <li class="breadcrumb-item active">Account Details</li>
                </ol>
            </div>

            <div class="box" id="personalInfo">
                <div class="boxheader">
                    <p class="title">Personal Info</p>
                </div>
                <div class="boxcontents">
                    <p>Username</p>
                    <asp:Label runat="server" ID="usrname"></asp:Label>
                </div>
            </div>

            <div class="box" style="height:350px;">
                <div class="boxheader">
                    <p class="title">Account Security</p>
                </div>
                <div class="boxcontents">
                    <div style="display:inline-block">
                        <p>Email Adress</p>
                        <asp:Label runat="server" ID="usremail"></asp:Label>
                    </div>
                    
                    <div style="display:inline-block; vertical-align:top; margin-left:120px;">
                        <p>Password</p>
                        <asp:Label runat="server" ID="usrpass" Text="********"></asp:Label>  
                        <asp:LinkButton runat="server" Text="Change Password" CssClass="btn btn-outline-secondary" ClientIDMode="Static" ID="changepassbutt" OnClick="changepassbutt_Click" ></asp:LinkButton>    
                    </div>
                    <br />
               
                    <div style="margin-top:20px;">
                        <asp:UpdatePanel ID="UpdatePanel1"  runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="TFAstatus" ClientIDMode="Static"></asp:Label>
                                <br />
                                <asp:LinkButton runat="server" ID="TFAbutt" ClientIDMode="Static" OnClick="changeTFAstatus"></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                     
                </div>
            </div>

            <div class="box">
                <div class="boxheader">
                    <p class="title">Credit Card</p>
                </div>
            </div>
        </div>
    </body>
    </html>
</asp:Content>
