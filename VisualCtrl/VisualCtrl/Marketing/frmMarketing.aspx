<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmMarketing.aspx.vb" Inherits="VisualCtrl.frmMarketing" %>

<%@ Import Namespace="System.Web.Script.Serialization.JavaScriptSerializer" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p></p>
    <br />
    <link rel="stylesheet" runat="server" media="screen" href="estilos.css" />
    <asp:HiddenField ID="hfLIFsProp" runat="server" />
    <asp:HiddenField ID="hfLIFsOB" runat="server" />
    <asp:HiddenField ID="hfAssignedProp" runat="server" />
    <asp:HiddenField ID="hfAssignedOB" runat="server" />
    <asp:HiddenField ID="hfNotAssignedProp" runat="server" />
    <asp:HiddenField ID="hfNotAssignedOB" runat="server" />
    <asp:HiddenField ID="hfAssignationRateProp" runat="server" />
    <asp:HiddenField ID="hfAssignationRateOB" runat="server" />

    <asp:HiddenField ID="txtLifsOB" runat="server" />
    <asp:HiddenField ID="txtLifsProp" runat="server" />
    <asp:HiddenField ID="txtAssignedOB" runat="server" />
    <asp:HiddenField ID="txtAssignedProp" runat="server" />
    <asp:HiddenField ID="txtVisitedOB" runat="server" />
    <asp:HiddenField ID="txtVisitedProp" runat="server" />
    <asp:HiddenField ID="txtCompletedOB" runat="server" />
    <asp:HiddenField ID="txtCompletedProp" runat="server" />

    <%
        txtLifsProp.Value = String.Format("{0:n0}", Integer.Parse(hfLIFsProp.Value.ToString()))
        txtLifsOB.Value = String.Format("{0:n0}", Integer.Parse(hfLIFsOB.Value.ToString()))
        txtAssignedOB.Value = String.Format("{0:n0}", Integer.Parse(hfAssignedOB.Value.ToString()))
        txtAssignedProp.Value = String.Format("{0:n0}", Integer.Parse(hfAssignedProp.Value.ToString()))
    %>

    <table style="width: 1200px">
        <tr>
            <td style="width: 200px;">
                <table style="width: 100px; vertical-align: top">
                    <tr>
                        <td class="modal-sm" style="width: 100px; border-style: none">Current Assignment</td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">
                            <asp:Button ID="Button1" runat="server" Text="Download Results" />
                        </td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">Date Created</td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">Status</td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField HeaderText="Agency" />
                                    <asp:BoundField HeaderText="Downloaded" />
                                    <asp:BoundField HeaderText="Results Uploaded" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                    </tr>
                </table>
            </td>
            <td style="width: 600px;">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList ID="dropDownCampaigns" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr style="border-style: solid; border-width: 2px">
                        <td class="primeracolumna" style="width: 70px">Portfolio</td>
                        <td class="encabezado" style="width: 50px">LIFs</td>
                        <td class="encabezado" style="width: 110px">Not Assigned</td>
                        <td class="encabezado" style="width: 80px">Assigned</td>
                        <td class="encabezado" style="width: 120px">Assignation Rate</td>
                        <td class="encabezado" style="width: 60px">Visited</td>
                        <td class="encabezado" style="width: 90px">Visits Rate</td>
                        <td class="encabezado" style="width: 80px">Completed</td>
                        <td class="encabezado" style="width: 130px">Completion Rate</td>
                    </tr>
                    <tr>
                        <td class="primeracolumna" style="width: 60px">Prop</td>
                        <td class="celda" style="border-style: solid;"><%=txtLifsProp.Value  %></td>
                        <td class="celda" style="border-style: solid;"><%=String.Format("{0:n0}", Integer.Parse(hfNotAssignedProp.Value.ToString()))  %></td>
                        <td class="celda" style="border-style: solid;"><%=txtAssignedProp.Value %></td>
                        <td class="celda" style="border-style: solid;"><%=hfAssignationRateProp.Value %>%</td>
                        <td class="celda" style="border-style: solid;">5</td>
                        <td class="celda" style="border-style: solid;">6</td>
                        <td class="celda" style="border-style: solid;">7</td>
                        <td class="celda" style="border-style: solid;">8</td>
                    </tr>
                    <tr>
                        <td class="primeracolumna" style="width: 60px">OptBlue</td>
                        <td class="celda" style="border-style: solid;"><%=txtLifsOB.Value  %></td>
                        <td class="celda" style="border-style: solid;"><%=String.Format("{0:n0}", Integer.Parse(hfNotAssignedOB.Value.ToString()))  %></td>
                        <td class="celda" style="border-style: solid;"><%=txtAssignedOB.Value %></td>
                        <td class="celda" style="border-style: solid;"><%=hfAssignationRateOB.Value %>%</td>
                        <td class="celda" style="border-style: solid;">13</td>
                        <td class="celda" style="border-style: solid;">14</td>
                        <td class="celda" style="border-style: solid;">15</td>
                        <td class="celda" style="border-style: solid;">16</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>	&nbsp;</td>
            <td>
                    <svg width="600" height="360"></svg>

            </td>
        </tr>
    </table>

    <style>
        .axis .domain {
            display: none;
        }
    </style>

    <asp:HiddenField ID="mesesJSON" runat="server" />
    <asp:HiddenField ID="totalMerchants" runat="server" />

    <script src="https://d3js.org/d3.v4.min.js"></script>
    <script>
        var svg = d3.select("svg"),
            margin = {
                top: 20,
                right: 20,
                bottom: 30,
                left: 40
            },
            width = +svg.attr("width") - margin.left - margin.right,
            height = +svg.attr("height") - margin.top - margin.bottom,
            g = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        var x = d3.scaleBand()
            .rangeRound([0, width])
            .paddingInner(0.05)
            .align(0.1);

        var y = d3.scaleLinear()
            .rangeRound([height, 0]);

        var z = d3.scaleOrdinal() //157 195 230....46 117 182
            .range(["#2e75b6", "#9cb3e6"]);

        //.range(["#98abc5", "#8a89a6", "#7b6888", "#6b486b", "#a05d56", "#d0743c", "#ff8c00"]);
        var data = <%=MesesJSON.Value%>


    // fix pre-processing
    var keys = [];
        for (key in data[0]) {
            if (key != "month")
                keys.push(key);
        }
        data.forEach(function (d) {
            d.total = 0;
            keys.forEach(function (k) {
                d.total += d[k];
            })
        });

        //data.sort(function (a, b) {
        //    return b.total - a.total;
        //});
        x.domain(data.map(function (d) {
            return d.month;
        }));
        y.domain([0, <%=totalMerchants.Value %>]);
        //    d3.max(data, function (d) {
        //    return d.total;
        ////})]).nice();
        //y.domain([0, d3.max(data, function (d) {
        //    return d.total;
        //})]).nice();
        z.domain(keys);

        g.append("g")
            .selectAll("g")
            .data(d3.stack().keys(keys)(data))
            .enter().append("g")
            .attr("fill", function (d) {
                return z(d.key);
            })
            .selectAll("rect")
            .data(function (d) {
                return d;
            })
            .enter().append("rect")
            .attr("x", function (d) {
                return x(d.data.month);
            })
            .attr("y", function (d) {
                return y(d[1]);
            })
            .attr("height", function (d) {
                return y(d[0]) - y(d[1]);
            })
            .attr("width", x.bandwidth());


        g.append("g")
            .attr("class", "axis")
            .attr("transform", "translate(0," + height + ")")
            .attr("font-size", 11)
            .call(d3.axisBottom(x));

        var mark = width / keys.length;
        var yOBLifs = height * (<%=hfLIFsOB.Value %> / 2) / <%= totalMerchants.Value %>
        var yOBasignados = height * (<%= totalMerchants.Value %> - <%=hfAssignedProp.Value %> - <%=hfAssignedOB.Value %>/2) / <%= totalMerchants.Value %>
        var yPropLifs = height * (<%=hfLIFsOB.Value %>+ <%= hfLIFsProp.Value %> / 2) / <%= totalMerchants.Value %>
        var yPropasignados = height * (<%= totalMerchants.Value %> - <%=hfAssignedProp.Value %>/2) / <%= totalMerchants.Value %>
        g.append("g")
            .attr("class", "axis")
            .call(d3.axisLeft(y).ticks(null, "s"))
            .append("text")
            .attr("x", 50)
            .attr("y", yOBLifs)
            .attr("dy", ".32em")
            .attr("fill", "#fff")
            .attr("font-weight", "bold")
            .attr("font-size", 15)
            .attr("text-anchor", "start")
            .text("<%=txtLifsOB.Value %>");

        g.append("g")
            .attr("class", "axis")
            .call(d3.axisLeft(y).ticks(null, "s"))
            .append("text")
            .attr("x", 180)
            .attr("y", yOBasignados)
            .attr("dy", ".32em")
            .attr("fill", "#fff")
            .attr("font-weight", "bold")
            .attr("font-size", 15)
            .attr("text-anchor", "start")
            .text("<%=txtAssignedOB.Value  %>");

        g.append("g")
            .attr("class", "axis")
            .call(d3.axisLeft(y).ticks(null, "s"))
            .append("text")
            .attr("x", 50)
            .attr("y", yPropLifs)
            .attr("dy", ".32em")
            .attr("fill", "#fff")
            .attr("font-weight", "bold")
            .attr("font-size", 15)
            .attr("text-anchor", "start")
            .text("<%=txtLifsProp.Value%>");

        g.append("g")
            .attr("class", "axis")
            .call(d3.axisLeft(y).ticks(null, "s"))
            .append("text")
            .attr("x", 180)
            .attr("y", yPropasignados)
            .attr("dy", ".32em")
            .attr("fill", "#fff")
            .attr("font-weight", "bold")
            .attr("font-size", 15)
            .attr("text-anchor", "start")
            .text("<%=txtAssignedProp.Value%>");


        var legend = g.append("g")
            .attr("font-family", "sans-serif")
            .attr("font-size", 10)
            .attr("text-anchor", "end")

            .selectAll("g")
            .data(keys.slice().reverse())
            .enter().append("g")
            .attr("transform", function (d, i) {
                return "translate(0," + i * 20 + ")";
            });

        //svg
        //    .append("line")
        //    .attr("x1", 40)
        //    .attr("y1", 71.5)
        //    .attr("x2", 300)
        //    .attr("y2", 72)
        //    .style("stroke", "red")

        legend.append("rect")
            .attr("x", width - 19)
            .attr("width", 19)
            .attr("height", 19)
            .attr("fill", z);

        legend.append("text")
            .attr("x", width - 24)
            .attr("y", 9.5)
            .attr("dy", "0.32em")
            .text(function (d) {
                return d;
            });
    </script>

</asp:Content>
