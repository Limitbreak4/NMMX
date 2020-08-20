﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebForm1.aspx.vb" Inherits="VisualCtrl.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      
    <select class="progressSelector" onchange="moveProgressBar(value)">
        <option value="started" selected>Started</option>
        <option value="inProgress">In Progress</option>
        <option value="completed">Completed</option>
    </select>

  

    <div class="progress"></div>

    <script src='https://cdnjs.cloudflare.com/ajax/libs/d3/3.5.5/d3.min.js'></script>

<%--    <script>


        var svg = d3.select('.progress')
            .append('svg')
            .attr('height', 100)
            .attr('width', 500);

        var states = ['started', 'inProgress', 'completed'],
            segmentWidth = 100,
            currentState = 'started';

        var colorScale = d3.scale.ordinal()
            .domain(states)
            .range(['yellow', 'orange', 'green']);

        svg.append('rect')
            .attr('class', 'bg-rect')
            .attr('rx', 10)
            .attr('ry', 10)
            .attr('fill', 'gray')
            .attr('height', 15)
            .attr('width', function () {
                return segmentWidth * states.length;
            })
            .attr('x', 0);

        var progress = svg.append('rect')
            .attr('class', 'progress-rect')
            .attr('fill', function () {
                return colorScale(currentState);
            })
            .attr('height', 15)
            .attr('width', 0)
            .attr('rx', 10)
            .attr('ry', 10)
            .attr('x', 0);

        progress.transition()
            .duration(1000)
            .attr('width', function () {
                var index = states.indexOf(currentState);
                return (index + 1) * segmentWidth;
            });

        function moveProgressBar(state) {
            progress.transition()
                .duration(1000)
                .attr('fill', function () {
                    return colorScale(state);
                })
                .attr('width', function () {
                    var index = states.indexOf(state);
                    return (index + 1) * segmentWidth;
                });
        }


    </script>--%>

    <script>
        var width = 960,
            height = 500; // adjust for height of input bar div

        var color = d3.scale.category20();

        // draw and append the container
        var svg = d3.select("body").append("svg")
            .attr("width", width).attr("height", height);

        // set the thickness of the inner and outer radii
        var min = Math.min(width, height);
        var oRadius = min / 2 * 0.9;
        var iRadius = min / 2 * 0.85;

        // construct default pie laoyut
        var pie = d3.layout.pie().value(function (d) {
            return d;
        }).sort(null);

        // construct arc generator
        var arc = d3.svg.arc()
            .outerRadius(oRadius)
            .innerRadius(iRadius);

        // creates the pie chart container
        var g = svg.append('g')
            .attr('transform', 'translate(300,250)')

        g.append('text').attr('text-anchor', 'middle').text("<%=HiddenField1.Value.ToString() %>")
        // generate random data
        var data = [<%=Integer.Parse(HiddenField1.Value.ToString())%>, <%=Integer.Parse(HiddenField2.Value.ToString())%>];

        // enter data and draw pie chart
        var path = g.datum(data).selectAll("path")
            .data(pie)
            .enter().append("path")
            .attr("class", "piechart")
            .attr("fill", function (d, i) { return color(i); })
            .attr("d", arc)
            .each(function (d) {
                this._current = d;
            })

        function makeData(size) {
            //console.log("inicio makedata")
            //return d3.range(size).map(function (item) {
            //    console.log("inicio function extraña" + item)
            //    return  38;
            //});
            var var1 = <%=Integer.Parse(HiddenField1.Value.ToString())%>;

            var var2 = <%=Integer.Parse(HiddenField2.Value.ToString())%>;
            var var3 = [var1, var2];
            return var3
        };

        function render() {
            console.log("inicio render")
            // generate new random data
            //data = makeData(2);
            // add transition to new path
            data = makeData(2);

            g.datum(data).selectAll("path").data(pie).transition().duration(1000).attrTween("d", arcTween)

            // add any new paths
            g.datum(data).selectAll("path")
                .data(pie)
                .enter().append("path")
                .attr("class", "piechart")
                .attr("fill", function (d, i) { return color(i); })
                .attr("d", arc)
                .each(function (d) { this._current = d; })

            // remove data not being used
            g.datum(data).selectAll("path")
                .data(pie).exit().remove();

            var dummy = <%=HiddenField1.Value.ToString()  %>;
            console.log("fin render")
        }

        render();



        // Store the displayed angles in _current.
        // Then, interpolate from _current to the new angles.
        // During the transition, _current is updated in-place by d3.interpolate.
        function arcTween(a) {
            console.log("inicio funcion arc")
            var i = d3.interpolate(this._current, a);
            this._current = i(0);
            return function (t) {
                console.log("inicio function extraña t")
                return arc(i(t));
            };
        }


    </script>

    <asp:HiddenField ID="HiddenField1" runat="server" />

    <asp:Button ID="Button1" runat="server" Text="Button" />

    <asp:HiddenField ID="HiddenField2" runat="server" />

</asp:Content>
