Public Class frmMarketing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        TryCast(Master.FindControl("lblPage"), Label).Text = "Marketing Main Page"
        'Add CSS files

        Dim css1 As New System.Web.UI.HtmlControls.HtmlLink()
        css1.Href = "BarChart.css"
        css1.Attributes.Item("rel") = "stylesheet"
        css1.Attributes.Item("type") = "text/css"
        Master.Page.Header.Controls.Add(css1)
        Dim css2 As New System.Web.UI.HtmlControls.HtmlLink()
        css2.Href = "PopCoverage.css"
        css2.Attributes.Item("rel") = "stylesheet"
        css2.Attributes.Item("type") = "text/css"
        Master.Page.Header.Controls.Add(css2)
        Dim css3 As New System.Web.UI.HtmlControls.HtmlLink()
        css3.Href = "visitas.css"
        css3.Attributes.Item("rel") = "stylesheet"
        css3.Attributes.Item("type") = "text/css"
        Master.Page.Header.Controls.Add(css3)


        Dim obt As Integer
        Dim propt As Integer
        obt = 1500
        propt = 1200

        OBTotalVisits.Value = obt
        PropTotalVisits.Value = propt
        TotalVisits.Value = (propt + obt)


        Dim orgpop, plapop As Integer
        orgpop = 17
        plapop = 35
        OrganicPop.Value = orgpop
        PlacedPop.Value = plapop
        TotalPopCoverage.Value = orgpop + plapop

        Dim obtot, oborg, obpla, proptot, proporg, proppla As Integer
        obpla = 38
        oborg = 22
        obtot = obpla + oborg
        proppla = 41
        proporg = 12
        proptot = proptot + proppla
        OBtotal.Value = obtot
        OBplaced.Value = obpla
        OBorganic.Value = oborg
        PropTotal.Value = proptot
        Proporganic.Value = proporg
        Propplaced.Value = proppla

        Dim resestemes As String = "[{ ""letter"": ""OB"", ""frequency"": """ + obt.ToString() + """ },
            { ""letter"": ""Prop"", ""frequency"": """ + propt.ToString() + """ }]"

        CompletedJSON.Value = resestemes

        Dim visits(0 To 11) As Integer

        visits(0) = 700
        visits(1) = 1500
        visits(2) = 1200
        visits(3) = 2000
        visits(4) = 1700
        visits(5) = 2500
        visits(6) = 2200
        visits(7) = 3000
        visits(8) = 2700
        visits(9) = 3500
        visits(10) = 3200
        visits(11) = 4000


        Dim visitsdata = "[
            { ""letter"": ""Jan"", ""frequency"": """ + visits(0).ToString() + """ },
            { ""letter"": ""Feb"", ""frequency"": """ + visits(1).ToString() + """ },
            { ""letter"": ""Mar"", ""frequency"": """ + visits(2).ToString() + """ },
            { ""letter"": ""Apr"", ""frequency"": """ + visits(3).ToString() + """ },
            { ""letter"": ""May"", ""frequency"": """ + visits(4).ToString() + """ },
            { ""letter"": ""Jun"", ""frequency"": """ + visits(5).ToString() + """ },
            { ""letter"": ""Jul"", ""frequency"": """ + visits(6).ToString() + """ },
            { ""letter"": ""Aug"", ""frequency"": """ + visits(7).ToString() + """ },
            { ""letter"": ""Sep"", ""frequency"": """ + visits(8).ToString() + """ },
            { ""letter"": ""Oct"", ""frequency"": """ + visits(9).ToString() + """ },
            { ""letter"": ""Nov"", ""frequency"": """ + visits(10).ToString() + """ },
            { ""letter"": ""Dec"", ""frequency"": """ + visits(11).ToString() + """ }]"

        DownstreamJSON.Value = visitsdata

    End Sub

End Class