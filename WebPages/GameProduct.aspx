<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Base.Master" AutoEventWireup="true" CodeBehind="GameProduct.aspx.cs" Inherits="_211792H.WebPages.GameProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!DOCTYPE html>
    <html>
    <head>
        <title>Home</title>
        <link rel="stylesheet" type="text/css" href="../Assets/css/Product.css" />

    </head>
    <body>
        <div class="misc">
            <ol class="breadcrumb">
              <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
              <asp:HyperLink CssClass="breadcrumb-item special" runat="server" ID="breadcrumbGenre"></asp:HyperLink>
              <asp:HyperLink CssClass="breadcrumb-item special" runat="server" ID="breadcrumbGame"></asp:HyperLink>
              <asp:HyperLink CssClass="breadcrumb-item special" runat="server" ID="breadcrumbProduct"></asp:HyperLink>
            </ol>

        <asp:Label ID="title" runat="server" ClientIDMode="Static"></asp:Label>
        </div> 
        <div class="itemcontainer">       
            <asp:Image CssClass="productimg" runat="server" ID="prodimg"/>    
            <div class="itemdesc">
                <asp:Label runat="server" id="prodGame" ClientIDMode="Static" ></asp:Label>
                <br />
                <asp:Label runat="server" id="prodDesc" ClientIDMode="Static"></asp:Label>
                <br />
                <div class="pricecontainer">
                    <div class="discountperc">
                    </div>
                    <div style="display:inline-block; font-size:13px;">
                        <asp:Label CssClass="lblpriceclass" ID="lblPrice" runat="server"></asp:Label>
                        <asp:Label CssClass="lbldiscount" ID="lblDiscountPrice" runat="server"></asp:Label>
                    </div>                               
                </div>

                <div class="buttons">
                    <asp:LinkButton runat="server" ID="wishlistbutton" OnClick="wishlistinteract"/>
                    <asp:LinkButton OnClick="AddToCart" runat="server" CssClass="btn btn-outline-success" Text="Add To Cart"/>
                </div>
            </div>
           
        </div>

        <div class="modal" id="reviewblock" ClientIDMode="Static" runat="server">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">Add a review</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" onclick="document.getElementById('reviewblock').style.display='none';">
                  <span aria-hidden="true"></span>
                </button>
              </div>
              <div class="modal-body">
                  <div class="form-group">
                        <asp:Label ID="lbl_review" runat="server" Text="Review"></asp:Label>
                        <asp:TextBox runat="server" ID="txt_Reviewdesc" CssClass="form-control" TextMode="MultiLine" Style="width: 100%; height: 90px; border: 0.5px solid"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ValidationGroup="review" ControlToValidate="txt_Reviewdesc" ErrorMessage="Please enter something" ForeColor="Red" ></asp:RequiredFieldValidator>
                  </div>
                  <br />
                  <asp:Label runat="server" Text="Do you recommend this product"></asp:Label>
                  <br />
                  <div class="form-group" style="display:inline-block;">
                      <label class="btn btn-outline-info" id="lbl1" ClientIDMode="Static" for="recommend" runat="server">Yes</label>
                      <asp:RadioButton runat="server" GroupName="radioGrp" ClientIDMode="Static" Checked="false" CssClass="btn-check"  ID="recommend"/>

                  </div>
                  <div class="form-group" style="display:inline-block;">
                      <label class="btn btn-outline-info" id="lbl2" ClientIDMode="Static" for="notRecommend" runat="server">No</label>
                      <asp:RadioButton runat="server" GroupName="radioGrp" ClientIDMode="Static" Checked="false" CssClass="btn-check" ID="notRecommend"/> 
                  </div>

                  <div class="form-group">
                      <div class="invalid-feedback" id="invalidReview" ClientIDMode="Static" style="display:none; color:red;" runat="server">Please state your opinion on this product</div>
                  </div>
              </div>

              <div class="modal-footer">
                <asp:Button class="btn btn-outline-success" runat="server" Text="Submit Review" UseSubmitBehavior="false" ValidationGroup="review" OnClick="submitReview"/>
                <button type="button" class="btn btn-outline-danger" data-bs-dismiss="modal" onclick="document.getElementById('reviewblock').style.display='none';">Close</button>
              </div>
            </div>
          </div>
        </div>

        
        <div class="itemReviews">
            <button type="button" class="btn btn-outline-info" id="writereview" runat="server" onclick="document.getElementById('reviewblock').style.display='block';">Write A Review</button>
            <br />
            <asp:Label runat="server" ClientIDMode="Static" ID="NumberReviews"></asp:Label>
            <div id="Noreviews" ClientIDMode="Static" runat="server" style="text-align:center; margin-top:30px;">There is currently no reviews for this product.</div>
             <asp:Repeater ID="RepeaterReviews" runat="server">
                    <ItemTemplate> 
                        <div class="indiReviews" style="margin-top:10px;">
                            <div class="reviewinfo">
                                <asp:Image runat="server" CssClass="rvwprofilepic" ImageUrl='<%#Eval("Userpic").ToString()%>'/>
                                <asp:Label runat="server" CssClass="rvwname" Text='<%#Eval("UserId").ToString()%>' ></asp:Label>
                                <asp:Label runat="server" CssClass="rvwPostedTime" Text='<%#Eval("postedTime").ToString()%>'></asp:Label>
                            </div>
                            <div class="reviewdesc">
                                <asp:Label runat="server" Text='<%#Eval("Reviewdesc").ToString()%>'></asp:Label>
                            </div> 
                            </br>
                             <div class="reviewratings" id="ratingbox" ClientIDmode="Static" runat="server">
                                 <asp:Image CssClass="rvwImg" runat="server"/>
                                 <asp:Label runat="server" CssClass="rvwreccomendation" Text='<%#Eval("reccomended").ToString()%>'></asp:Label>
                            </div>
                        </div>
                    </ItemTemplate>
            </asp:Repeater>
        </div>
            
        <script>
            //Check if discounted price
            var discountperc = document.getElementsByClassName("discountperc")[0]
            var discountprice = document.getElementsByClassName("lbldiscount")[0]
            var originalprice = document.getElementsByClassName("lblpriceclass")[0];

            if (discountprice.innerText == "$") {
                discountprice.style.display = "none";
                discountperc.style.display = "none";
            }
            else {
                var string = new String(originalprice.innerText);
                originalprice.innerHTML = string.strike();
                originalprice.style.color = "grey";
                discountprice.style.display = "block";
                discountprice.style.color = "white";
                let percentage = 100 - discountprice.textContent.replace('$', '') / (originalprice.textContent.replace('$', '') / 100);
                discountperc.textContent = "-" + new String(percentage) + "%";
            }      

            //Capitalize Breadcrumb
            var breadcrumbs = document.getElementsByClassName("breadcrumb-item special");

            for (let i = 0; i < breadcrumbs.length; i++) {
                breadcrumbs[i].innerHTML = toTitleCase(breadcrumbs[i].textContent);
            }

            function toTitleCase(str) {
                return str.replace(/\w\S*/g, function (txt) {
                    return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
                });
            }

            //Checkbox changed
            var chckbox1 = document.getElementById('<%= recommend.ClientID %>');
            var lbl1 = document.getElementById("lbl1");
            var chckbox2 = document.getElementById('<%= notRecommend.ClientID %>');
            var lbl2 = document.getElementById("lbl2");
            chckbox1.onclick = function () { 
                lbl1.classList.add("btn-info");
                lbl1.classList.remove("btn-outline-info");
                
                
                lbl2.classList.remove("btn-info");
                lbl2.classList.add("btn-outline-info");
                
                
            }
 
            chckbox2.onclick = function () {
                lbl2.classList.add("btn-info");
                lbl2.classList.remove("btn-outline-info");              
                lbl1.classList.add("btn-outline-info");
                lbl1.classList.remove("btn-info");
            }

            //Format reviews

            var reviews = document.getElementsByClassName("indiReviews")

            for (let el = 0; el < reviews.length; el++) {
                var img = reviews[el].getElementsByClassName("rvwImg")[0]
                var recommend = reviews[el].getElementsByClassName("rvwreccomendation")[0]
                var profileimg = reviews[el].getElementsByClassName("rvwprofilepic")[0]
                var date = new Date(reviews[el].getElementsByClassName("rvwPostedTime")[0].innerText);
                console.log(reviews[el].getElementsByClassName("rvwPostedTime")[0].innerText);
                console.log(date);
                var today = new Date();
                var yeardiff =  today.getFullYear() - date.getFullYear() 
                var monthsdiff = today.getMonth() - date.getMonth() 
                var daydiff = today.getDay() - date.getDay() 
                console.log(String(profileimg.getAttribute('src')))
                if (String(profileimg.getAttribute('src')) == "null") {
                    profileimg.src="../../images/profilepic2.png"
                }

                if (yeardiff > 0) {
                    if (yeardiff > 1) {
                        reviews[el].getElementsByClassName("rvwPostedTime")[0].innerText = yeardiff.toString() + " years ago"
                    }
                    else {
                        reviews[el].getElementsByClassName("rvwPostedTime")[0].innerText = "This year"
                    }                   
                }

                else if (monthsdiff > 0) {
                    if (monthsdiff > 1) {
                        reviews[el].getElementsByClassName("rvwPostedTime")[0].innerText = monthsdiff.toString() + " months ago"
                    }
                    else {
                        reviews[el].getElementsByClassName("rvwPostedTime")[0].innerText = "This Month"
                    }    
                }

                else {
                    if (daydiff > 1) {
                        reviews[el].getElementsByClassName("rvwPostedTime")[0].innerText = daydiff.toString() + " days ago"
                    }
                    else {
                        reviews[el].getElementsByClassName("rvwPostedTime")[0].innerText = "Today"
                    }
                }

                if (recommend.innerText == "True") {
                    img.src = "../../images/ThumbsUp.png";
                    recommend.innerText = "Recommended"
                }
                else {
                    img.src = "../../images/ThumbsDown.png";
                    recommend.innerText = "Not Recommended"
                }

               
            }
            
        </script>
    </body>
    </html>
</asp:Content>
