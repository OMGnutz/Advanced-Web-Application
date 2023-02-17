<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Base.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="_211792H.WebPages.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!DOCTYPE html>
    <html>
    <head>
        <title>Home</title>
        <link rel="stylesheet" type="text/css" href="../Assets/css/Home.css" />
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>

    </head>
    <body>

        <div class="sidecontainer">
            <div>
                <a style="font-family: 'Oswald', sans-serif;">BROWSE CATEGORIES</a>
                <a href="/WebPages/Category.aspx?Category=top sellers" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">Top Sellers</a>
                <a href="/WebPages/Category.aspx?Category=new releases" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">New Releases</a>
                <a href="/WebPages/Category.aspx?Category=best reviews" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">Best Reviews</a>
                <a href="/WebPages/Category.aspx?Category=special offers" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">Special Offers</a>
                <a style="font-family: 'Oswald', sans-serif;"></a>
                <a style="font-family: 'Oswald', sans-serif;">BROWSE BY GENRE</a>
                <a href="/WebPages/Genre.aspx?Genre=fps" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">First Person Shooter</a>
                <a href="#" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">Third Person Shooter</a>
                <a href="/WebPages/Genre.aspx?Genre=mmo" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">Massively Multiplayer Online</a>
                <a href="/WebPages/Genre.aspx?Genre=rpg" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">RPG</a>
                <a href="#" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">Sports</a>
                <a href="/WebPages/Genre.aspx?Genre=horror" style="font-family: 'Oswald', sans-serif; font-size: 12.5px">Horror</a>
                <a style="font-family: 'Oswald', sans-serif;"></a>
                <div>
                <a style="font-family: 'Oswald', sans-serif;">RECENTLY VIEWED</a>
                </div>
            </div>
        </div>

        <div id="itemContainer">

            <div class="carousel slide" data-bs-ride="carousel" id="CarouselSpecialOffers" style="margin-top:100px; display:none;">
            <p class="carouselheader">Special Offers!</p>
            <div class="carousel-indicators" id="SOindicator">
                <button type="button" data-bs-target="#CarouselSpecialOffers" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                <button type="button" data-bs-target="#CarouselSpecialOffers" data-bs-slide-to="1" aria-label="Slide 2"></button>
                <button type="button" data-bs-target="#CarouselSpecialOffers" data-bs-slide-to="2" aria-label="Slide 3" id="SOindi3"></button>
            </div>
            <div class="carousel-inner">
                <div class="carousel-item active">
                    <div class="row">
                        <asp:Repeater ID="RepeaterSpecialOffer1" runat="server">
                            <ItemTemplate>
                                <div class="col">
                                    <div class="imagepopover" data-bs-trigger="hover" data-bs-toggle="popover" data-bs-placement="right" 
                                        data-bs-content="<b><%#Eval("Name")%>  (<%#Eval("GameOrigin")%>)</b><br>Released: <%#DateTime.Parse(Eval("ReleaseDate").ToString()).ToString("dd MMMM, yyyy")%><br><br><%#Eval("Description")%> <hr>Overall Reviews:<br><span class='reviews'><%#Eval("OverallReviews")%></span>" 
                                        data-bs-html="true">
                                        <asp:ImageButton PostBackUrl='<%# ResolveClientUrl("GameProduct.aspx?Id=" + Eval("Id"))%>' runat="server" ImageUrl='<%#Eval("Image") %>' CssClass="img-fluid" />
                                    </div>
                                    <div class="carouselitemlabel">
                                        <asp:Label CssClass="productname" ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                        <br />
                                        <asp:Label CssClass="lblpriceclass" ID="lblPrice" runat="server" Text='<%# "$" + Eval("Price") %>' ></asp:Label>
                                        <asp:Label CssClass="lbldiscount" runat="server" Text='<%# "$" + Eval("DiscountedPrice") %>'></asp:Label>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div> 
                </div>

                <div class="carousel-item" id="SO2" >
                    <div class="row">
                         <asp:Repeater ID="RepeaterSpecialOffer2" runat="server">
                            <ItemTemplate>
                                <div class="col">
                                    <div class="imagepopover" data-bs-trigger="hover" data-bs-toggle="popover" data-bs-placement="right" 
                                        data-bs-content="<b><%#Eval("Name")%>  (<%#Eval("GameOrigin")%>)</b><br>Released: <%#DateTime.Parse(Eval("ReleaseDate").ToString()).ToString("dd MMMM, yyyy")%><br><br><%#Eval("Description")%> <hr>Overall Reviews:<br><span class='reviews'><%#Eval("OverallReviews")%></span>" 
                                        data-bs-html="true">
                                        <asp:ImageButton PostBackUrl='<%# ResolveClientUrl("GameProduct.aspx?Id=" + Eval("Id"))%>' runat="server" ImageUrl='<%#Eval("Image") %>' CssClass="img-fluid" />
                                    </div>
                                    <div class="carouselitemlabel">
                                        <asp:Label CssClass="productname" ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                        <br />
                                        <asp:Label CssClass="lblpriceclass" ID="lblPrice" runat="server" Text='<%# "$" + Eval("Price") %>' ></asp:Label>
                                        <asp:Label CssClass="lbldiscount" runat="server" Text='<%# "$" + Eval("DiscountedPrice") %>'></asp:Label>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

                <div class="carousel-item" id="SO3">
                    <div class="row">
                         <asp:Repeater ID="RepeaterSpecialOffer3" runat="server">
                            <ItemTemplate>
                                <div class="col">
                                    <div class="imagepopover" data-bs-trigger="hover" data-bs-toggle="popover" data-bs-placement="right" 
                                        data-bs-content="<b><%#Eval("Name")%>  (<%#Eval("GameOrigin")%>)</b><br>Released: <%#DateTime.Parse(Eval("ReleaseDate").ToString()).ToString("dd MMMM, yyyy")%><br><br><%#Eval("Description")%> <hr>Overall Reviews:<br><span class='reviews'><%#Eval("OverallReviews")%></span>" 
                                        data-bs-html="true">
                                        <asp:ImageButton PostBackUrl='<%# ResolveClientUrl("GameProduct.aspx?Id=" + Eval("Id"))%>' runat="server" ImageUrl='<%#Eval("Image") %>' CssClass="img-fluid" />
                                    </div>
                                    <div class="carouselitemlabel">
                                        <asp:Label CssClass="productname" ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                        <br />
                                        <asp:Label CssClass="lblpriceclass" ID="lblPrice" runat="server" Text='<%# "$" + Eval("Price") %>' ></asp:Label>
                                        <asp:Label CssClass="lbldiscount" runat="server" Text='<%# "$" + Eval("DiscountedPrice") %>'></asp:Label>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
           
          <button class="carousel-control-prev" type="button" data-bs-target="#CarouselSpecialOffers" data-bs-slide="prev" id="SOprev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
          </button>
          <button class="carousel-control-next" type="button" data-bs-target="#CarouselSpecialOffers" data-bs-slide="next" id="SOnext">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
          </button>
        </div>


        <div class="carousel slide" id="carouselgenre">
            <p class="carouselheader">Browse By Genre</p>
            <div class="carousel-indicators">
                <button type="button" data-bs-target="#carouselgenre" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                <button type="button" data-bs-target="#carouselgenre" data-bs-slide-to="1" aria-label="Slide 2"></button>
                <button type="button" data-bs-target="#carouselgenre" data-bs-slide-to="2" aria-label="Slide 3"></button>
            </div>

            <div class="carousel-inner">
                <div class="carousel-item active">
                    <div class="row">
                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="~/WebPages/Genre.aspx?Genre=fps" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/fpsbackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="First Person Shooter"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="javascript:void(0);" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/tpsbackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Third Person Shooter"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="~/WebPages/Genre.aspx?Genre=rpg" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/rpgbackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Role Playing"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="~/WebPages/Genre.aspx?Genre=mmo" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/mmobackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Massively Multiplayer"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="carousel-item">
                    <div class="row">
                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="~/WebPages/Genre.aspx?Genre=horror" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/horrorbackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Horror"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="javascript:void(0);" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/sportsbackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Sports"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="javascript:void(0);" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/animebackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Anime"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="javascript:void(0);" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/fightingbackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Fighting"></asp:Label>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="carousel-item">
                    <div class="row">
                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="javascript:void(0);" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/visualnovelbackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Visual Novel"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="javascript:void(0);" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/strategybackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Strategy"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="javascript:void(0);" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/racingbackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="Racing"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col">
                            <div class="imagepopover">
                                <asp:ImageButton PostBackUrl="javascript:void(0);" runat="server" CssClass="carouselgenreimg img-fluid" ImageUrl="../images/citybackground.png"/>
                                <div class="carouselitemlabel">
                                    <asp:Label runat="server" Text="City"></asp:Label>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>

            <button class="carousel-control-prev" type="button" data-bs-target="#carouselgenre" data-bs-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#carouselgenre" data-bs-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
            </button>


        </div>


            <div id="hotreleases">
                <span>New Releases</span>
            </div>
    </div>

        <script>
            var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
            var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
                return new bootstrap.Popover(popoverTriggerEl)
            })


            var prices = document.getElementsByClassName("lblpriceclass")
            for (let el = 0; el < prices.length; el++) {         
                var originalprice = prices[el];
                var string = new String(originalprice.innerText);
                originalprice.innerHTML = string.strike();
                originalprice.style.color = "grey";
                }
        </script>

        <script src="../Scripts/jquery-3.4.1.min.js"></script>
        <script type="text/javascript">
            
            $.ajax({
                url: '/CodeBehindComms/GetRows',
                type: "GET",
                dataType: "JSON",
                success: function (data) {
                    if (parseInt(data) > 2) {
                        document.getElementById("CarouselSpecialOffers").style.display = "block";
                    }
                    if (parseInt(data) < 6) {
                        document.getElementById("SO2").remove();
                        document.getElementById("SOindicator").remove();
                        document.getElementById("SOnext").remove();
                        document.getElementById("SOprev").remove();

                    }

                    if (parseInt(data) < 9) {
                        document.getElementById("SO3").remove();
                        document.getElementById("SOindi3").remove();
                    }
                }
            }); 

            var reviews = document.getElementsByClassName("reviews");
            for (let i = 0; i < reviews.length; i++) {
                
            }

        </script>




    </body>
    </html>
</asp:Content>
