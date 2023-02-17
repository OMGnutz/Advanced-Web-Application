<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin2.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="_211792H.WebPages.Admin.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!DOCTYPE html>
    <html>
    <head>
        <title</title>
        <link rel="stylesheet" type="text/css" href="../../Assets/css/DashBoard.css" />
        <script type="text/javascript" src="https://www.google.com/jsapi"></script>
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    </head>
    <body>
        <div id="contents">
            <div id="topcontent">
                <div class="databox">
                    <div class="databoxheader" style="background-color:#5bc0de">
                        <p class="dataTitle">Total users</p>
                    </div>
                    <asp:Label runat="server" CssClass="ttlLabel" ID="ttlusers"></asp:Label>
                </div>

                <div class="databox">
                    <div class="databoxheader" style="background-color:#7a8288">
                        <p class="dataTitle">Total Orders</p>
                    </div>
                    <asp:Label runat="server" CssClass="ttlLabel" ID="ttlOrders" Text="0"></asp:Label>
                </div>

                <div class="databox">
                    <div class="databoxheader" style="background-color:#e9ecef">
                        <p class="dataTitle">Total Products</p>
                    </div>
                    <asp:Label runat="server" CssClass="ttlLabel" ID="ttlProducts"></asp:Label>
                </div>

                <div class="databox">
                    <div class="databoxheader" style="background-color:#62c462;">
                        <p class="dataTitle">Total Sales</p>
                    </div>
                    <asp:Label runat="server" CssClass="ttlLabel" ID="ttlSales"></asp:Label>
                </div>

            </div>

            <div id="charts">
                <asp:Literal ID="lt" runat="server"></asp:Literal>
                <div id="chart_div" style="margin-left:auto; margin-right:auto; margin-top:30px; width:700px;"></div>
                <div id="chart_div2" style="margin-left:auto; margin-right:auto; margin-top:150px; width:700px;"></div>
            </div>
        </div>

        <script type="text/javascript">
            google.load("visualization", "1", { packages: ["corechart"] });
            google.setOnLoadCallback(drawChart);
            function drawChart() {
                var options = {
                    title: 'Most Popular Products',
                    width: 700,
                    height: 500,
                    bar: { groupWidth: "95%" },
                    legend: { position: "none" },
                    isStacked: true
                };
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetChartData",
                    data: '{}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        var data = google.visualization.arrayToDataTable(r.d);
                        var chart = new google.visualization.BarChart($("#chart_div")[0]);
                        chart.draw(data, options);
                    },
                    failure: function (r) {
                        alert(r.d);
                    },
                    error: function (r) {
                        alert(r.d);
                    }
                });
            }

            google.setOnLoadCallback(drawChart2);
            function drawChart2() {
                var options = {
                    title: 'Most Popular Genre',
                    width: 700,
                    height: 500,
                    bar: { groupWidth: "95%" },
                    legend: { position: "none" },
                    isStacked: true
                };
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetChartData2",
                    data: '{}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        var data = google.visualization.arrayToDataTable(r.d);
                        var chart = new google.visualization.BarChart($("#chart_div2")[0]);
                        chart.draw(data, options);
                    },
                    failure: function (r) {
                        alert(r.d);
                    },
                    error: function (r) {
                        alert(r.d);
                    }
                });
            }
        </script>
    </body>
    </html>
</asp:Content>
