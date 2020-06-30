<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="grafica.aspx.vb" Inherits="VisualCtrl.grafica" %>

<style>
    .axis .domain {
        display: none;
    }
</style>
<svg width="320" height="220"></svg>
<form runat="server">
    <asp:HiddenField ID="mesesJSON" runat="server" />
    
</form>

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
    var data = <%=mesesJSON.Value%>
    //    var data = [
    //    { month: "JAN", Present: "10", Placed: "15" },
    //    { month: "FEB", Present: "12", Placed: "18" },
    //    { month: "MAR", Present: "05", Placed: "20"},
    //    { month: "APR", Present: "01", Placed: "15"},
    //    { month: "MAY", Present: "02", Placed: "10"},
    //    { month: "JUN", Present: "03", Placed: "12"},
    //    { month: "JUL", Present: "15", Placed: "15"},
    //    { month: "AUG", Present: "06", Placed: "11"},
    //    { month: "SEP", Present: "10", Placed: "13"},
    //    { month: "OCT", Present: "16", Placed: "19"},
    //    { month: "NOV", Present: "19", Placed: "17"},
    //    { month: "DEC", Present: "14", Placed: "37"},
    //];


    //var data = [{
    //    "month": "VT",
    //    "Under 5 Years": 32635,
    //    "5 to 13 Years": 62538,
    //    "14 to 17 Years": 33757,
    //    "18 to 24 Years": 61679,
    //    "25 to 44 Years": 155419,
    //    "45 to 64 Years": 188593,
    //    "65 Years and Over": 86649
    //}, {
    //    "month": "VA",
    //    "Under 5 Years": 522672,
    //    "5 to 13 Years": 887525,
    //    "14 to 17 Years": 413004,
    //    "18 to 24 Years": 768475,
    //    "25 to 44 Years": 2203286,
    //    "45 to 64 Years": 2033550,
    //    "65 Years and Over": 940577
    //}, {
    //    "month": "WA",
    //    "Under 5 Years": 433119,
    //    "5 to 13 Years": 750274,
    //    "14 to 17 Years": 357782,
    //    "18 to 24 Years": 610378,
    //    "25 to 44 Years": 1850983,
    //    "45 to 64 Years": 1762811,
    //    "65 Years and Over": 783877
    //}, {
    //    "month": "WV",
    //    "Under 5 Years": 105435,
    //    "5 to 13 Years": 189649,
    //    "14 to 17 Years": 91074,
    //    "18 to 24 Years": 157989,
    //    "25 to 44 Years": 470749,
    //    "45 to 64 Years": 514505,
    //    "65 Years and Over": 285067
    //}, {
    //    "month": "WI",
    //    "Under 5 Years": 362277,
    //    "5 to 13 Years": 640286,
    //    "14 to 17 Years": 311849,
    //    "18 to 24 Years": 553914,
    //    "25 to 44 Years": 1487457,
    //    "45 to 64 Years": 1522038,
    //    "65 Years and Over": 750146
    //}, {
    //    "month": "WY",
    //    "Under 5 Years": 38253,
    //    "5 to 13 Years": 60890,
    //    "14 to 17 Years": 29314,
    //    "18 to 24 Years": 53980,
    //    "25 to 44 Years": 137338,
    //    "45 to 64 Years": 147279,
    //    "65 Years and Over": 65614
    //}];

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
    y.domain([0, 100]);
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
        .attr("font-size",11)
        .call(d3.axisBottom(x));

    g.append("g")
        .attr("class", "axis")
        .call(d3.axisLeft(y).ticks(null, "s"))
        .append("text")
        .attr("x", 2)
        .attr("y", y(y.ticks().pop()) + 0.5)
        .attr("dy", ".32em")
        .attr("fill", "#000")
        .attr("font-weight", "bold")
        .attr("text-anchor", "start")
        .text("100%");

   

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

    svg
        .append("line")
        .attr("x1", 40)
        .attr("y1", 71.5)
        .attr("x2", 300)
        .attr("y2", 72)
        .style("stroke", "red")

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

<%--<script src="../Scripts/StackedBarChart.js" ></script>--%>

<%--<script type="text/javascript">

    // Setup svg using Bostock's margin convention

    //var margin = {top: 20, right: 160, bottom: 35, left: 30};

    //var width = 960 - margin.left - margin.right,
    //    height = 500 - margin.top - margin.bottom;

    //var svg = d3.select("body")
    //  .append("svg")
    //  .attr("width", width + margin.left + margin.right)
    //  .attr("height", height + margin.top + margin.bottom)
    //  .append("g")
    //  .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    ///* Data in strings like it would be if imported from a csv */

    //var info = [
    //    { month: "JAN", redDelicious: "10", mcintosh: "15" },
    //    { month: "FEB", redDelicious: "12", mcintosh: "18" },
    //    { month: "MAR", redDelicious: "05", mcintosh: "20"},
    //    { month: "APR", redDelicious: "01", mcintosh: "15"},
    //    { month: "MAY", redDelicious: "02", mcintosh: "10"},
    //    { month: "JUN", redDelicious: "03", mcintosh: "12"},
    //    { month: "JUL", redDelicious: "04", mcintosh: "15"},
    //    { month: "AUG", redDelicious: "06", mcintosh: "11"},
    //    { month: "SEP", redDelicious: "10", mcintosh: "13"},
    //    { month: "OCT", redDelicious: "16", mcintosh: "19"},
    //    { month: "NOV", redDelicious: "19", mcintosh: "17"},
    //    { month: "DEC", redDelicious: "14", mcintosh: "37"},
    //];

    //var info = [
    //    { year: "2006", redDelicious: "10", mcintosh: "15" },
    //    { year: "2007", redDelicious: "12", mcintosh: "18" },
    //    { year: "2008", redDelicious: "05", mcintosh: "20" },
    //    { year: "2009", redDelicious: "01", mcintosh: "15" },
    //    { year: "2333", redDelicious: "02", mcintosh: "10" },
    //    { year: "2011", redDelicious: "03", mcintosh: "12" },
    //    { year: "2012", redDelicious: "04", mcintosh: "15" },
    //    { year: "2013", redDelicious: "06", mcintosh: "11" },
    //    { year: "2014", redDelicious: "10", mcintosh: "13" },
    //    { year: "2015", redDelicious: "16", mcintosh: "19" },
    //    { year: "2016", redDelicious: "19", mcintosh: "17" },
    //    { year: "2017", redDelicious: "14", mcintosh: "37" },
    //];

    //var parse = d3.time.format("%Y").parse;


    //// Transpose the data into layers
    //var dataset = d3.layout.stack()(["redDelicious", "mcintosh", "oranges", "pears"].map(function(fruit) {
    //  return data.map(function(d) {
    //    return {x: parse(d.year), y: +d[fruit]};
    //  });
    //}));


    //// Set x, y and colors
    //var x = d3.scale.ordinal()
    //  .domain(dataset[0].map(function(d) { return d.x; }))
    //  .rangeRoundBands([10, width-10], 0.02);

    //var y = d3.scale.linear()
    //  .domain([0, d3.max(dataset, function(d) {  return d3.max(d, function(d) { return d.y0 + d.y; });  })])
    //  .range([height, 0]);

    //var colors = ["b33040", "#d25c4d", "#f2b447", "#d9d574"];


    //// Define and draw axes
    //var yAxis = d3.svg.axis()
    //  .scale(y)
    //  .orient("left")
    //  .ticks(5)
    //  .tickSize(-width, 0, 0)
    //  .tickFormat( function(d) { return d } );

    //var xAxis = d3.svg.axis()
    //  .scale(x)
    //  .orient("bottom")
    //  .tickFormat(d3.time.format("%Y"));

    //svg.append("g")
    //  .attr("class", "y axis")
    //  .call(yAxis);

    //svg.append("g")
    //  .attr("class", "x axis")
    //  .attr("transform", "translate(0," + height + ")")
    //  .call(xAxis);


    //// Create groups for each series, rects for each segment 
    //var groups = svg.selectAll("g.cost")
    //  .data(dataset)
    //  .enter().append("g")
    //  .attr("class", "cost")
    //  .style("fill", function(d, i) { return colors[i]; });

    //var rect = groups.selectAll("rect")
    //  .data(function(d) { return d; })
    //  .enter()
    //  .append("rect")
    //  .attr("x", function(d) { return x(d.x); })
    //  .attr("y", function(d) { return y(d.y0 + d.y); })
    //  .attr("height", function(d) { return y(d.y0) - y(d.y0 + d.y); })
    //  .attr("width", x.rangeBand())
    //  .on("mouseover", function() { tooltip.style("display", null); })
    //  .on("mouseout", function() { tooltip.style("display", "none"); })
    //  .on("mousemove", function(d) {
    //    var xPosition = d3.mouse(this)[0] - 15;
    //    var yPosition = d3.mouse(this)[1] - 25;
    //    tooltip.attr("transform", "translate(" + xPosition + "," + yPosition + ")");
    //    tooltip.select("text").text(d.y);
    //  });


    //// Draw legend
    //var legend = svg.selectAll(".legend")
    //  .data(colors)
    //  .enter().append("g")
    //  .attr("class", "legend")
    //  .attr("transform", function(d, i) { return "translate(30," + i * 19 + ")"; });

    //legend.append("rect")
    //  .attr("x", width - 18)
    //  .attr("width", 18)
    //  .attr("height", 18)
    //  .style("fill", function(d, i) {return colors.slice().reverse()[i];});

    //legend.append("text")
    //  .attr("x", width + 5)
    //  .attr("y", 9)
    //  .attr("dy", ".35em")
    //  .style("text-anchor", "start")
    //  .text(function(d, i) { 
    //    switch (i) {
    //      case 0: return "Anjou pears";
    //      case 1: return "Naval oranges";
    //      case 2: return "McIntosh apples";
    //      case 3: return "Red Delicious apples";
    //    }
    //  });


    //// Prep the tooltip bits, initial display is hidden
    //var tooltip = svg.append("g")
    //  .attr("class", "tooltip")
    //  .style("display", "none");

    //tooltip.append("rect")
    //  .attr("width", 30)
    //  .attr("height", 20)
    //  .attr("fill", "white")
    //  .style("opacity", 0.5);

    //tooltip.append("text")
    //  .attr("x", 15)
    //  .attr("dy", "1.2em")
    //  .style("text-anchor", "middle")
    //  .attr("font-size", "12px")
    //  .attr("font-weight", "bold");
   //StackedBarChart.initialize(info, "chart");

</script>--%>

