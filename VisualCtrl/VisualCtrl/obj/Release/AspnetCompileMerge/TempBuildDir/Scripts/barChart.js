var BarChart = {
	initialize: function (bars, area,w,h) {
		data = bars;
		data.forEach(function (d) {
			d.frequency = +d.frequency;
		});

		var svg = d3.select("#"+area+" svg");
		var margin = { top: 20, right: 30, bottom: 20, left: 30 };
		var width = 720 - margin.left - margin.right;
		var height = 320 - margin.top - margin.bottom;
		var width = w;
		var height = h;
		var xScale = d3.scaleBand().rangeRound([0, width]).padding(0.1);
		var yScale = d3.scaleLinear().rangeRound([height, 0]);

		var g = svg
			.attr("width", width + margin.left + margin.right)
			.attr("height", height + margin.top + margin.bottom)
			.append("g")
			.attr("transform", "translate(" + margin.left + "," + margin.top + ")");

		xScale.domain(data.map(function (d) { return d.letter; }));
		yScale.domain([0, d3.max(data, function (d) { return d.frequency; })]);

		g.append("g")
			.attr("class", "axis axis--x")
			.attr("transform", "translate(0," + height + ")")
			.call(d3.axisBottom(xScale));

		g.append("g")
			.attr("class", "axis axis--y")
			.call(d3.axisLeft(yScale).ticks(10, "s"))
			.append("text")
			.attr("class", "axis--label")
			.attr("transform", "rotate(-90)")
			.attr("y", 20)
			.attr("dy", "0.71em")
			.attr("text-anchor", "end")
			.text("");

		g.selectAll(".bar")
			.data(data)
			.enter()
			.append("rect")
			.attr("class", "bar")
			.attr("x", function (d) { return xScale(d.letter); })
			.attr("y", function (d) { return yScale(d.frequency); })
			.attr("width", xScale.bandwidth())
			.attr("height", function (d) { return height - yScale(d.frequency); });

		d3.select("input").on("change", change);

		function change() {
			var xScale0 = xScale.domain(data.sort(this.checked
				? function (a, b) { return b.frequency - a.frequency; }
				: function (a, b) { return d3.ascending(a.letter, b.letter); })
				.map(function (d) { return d.letter; }))
				.copy();

			svg.selectAll(".bar")
				.sort(function (a, b) { return xScale0(a.letter) - xScale0(b.letter); });

			var transition = svg.transition().duration(750);
			var delay = function (d, i) { return i * 50; };

			transition.selectAll(".bar")
				.delay(delay)
				.attr("x", function (d) { return xScale0(d.letter); });

			transition.select(".axis--x")
				.call(d3.axisBottom(xScale))
				.selectAll("g")
				.delay(delay);
		}


	}

}


function data() {
	var raw_data = [
		{ "letter": "Jan", "frequency": "3555" },
		{ "letter": "Feb", "frequency": "2880" },
		{ "letter": "Mar", "frequency": "1322" },
		{ "letter": "Apr", "frequency": "4021" },
		{ "letter": "May", "frequency": "3555" },
		{ "letter": "Jun", "frequency": "2880" },
		{ "letter": "Jul", "frequency": "1322" },
		{ "letter": "Aug", "frequency": "4021" },
		{ "letter": "Sep", "frequency": "3555" },
		{ "letter": "Oct", "frequency": "2880" },
		{ "letter": "Nov", "frequency": "1322" },
		{ "letter": "Dec", "frequency": "4021" }
	]
	return raw_data;
}
