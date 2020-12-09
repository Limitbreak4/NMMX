Imports System.Data.SqlClient
Public Class frmMarketing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If

        TryCast(Master.FindControl("lblPage"), Label).Text = "Marketing Main Page"
        'Add CSS files
        If dropDownCampaigns.Items.Count = 0 Then

            Dim rdrArchivos As DataSet = dsOpenDB(New SqlCommand("SELECT 
            ID_ARCHIVO,FH_CARGA,nombre_archivo,FH_carga + '-' + nombre_archivo as ids,archivos.ID_CAMPAIGN,NOMBRE_CAMPAIGN 
            FROM ARCHIVOS with(nolock) inner join TAB_CAMPAIGNS with (nolock) 
            on ARCHIVOS.ID_CAMPAIGN = TAB_CAMPAIGNS.ID_CAMPAIGN order by FH_CARGA desc"))
            Dim campañas() As DataRow = rdrArchivos.Tables(0).Select("1 = 1")
            Dim i As Integer
            For i = 0 To campañas.Count - 1
                Try
                    dropDownCampaigns.Items.Add(campañas(i).Item("ids"))
                    dropDownCampaigns.Items(i).Value = campañas(i).Item("ID_ARCHIVO")
                Catch
                End Try
            Next
            fillGraphInfo(campañas(0).Item("ID_ARCHIVO"))
        End If


    End Sub

    Private Sub dropDownCampaigns_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dropDownCampaigns.SelectedIndexChanged
        fillGraphInfo(dropDownCampaigns.SelectedValue)
    End Sub

    Private Sub fillGraphInfo(index As Integer)
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM archivos_data (nolock) where id_archivo = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = index

        Dim rdrMerchants As DataSet = dsOpenDB(comm)
        Dim countPropDataRow() As DataRow = rdrMerchants.Tables(0).Select("PORTAFOLIO = 'Prop'")
        Dim countProp As Integer = countPropDataRow.Count
        countPropDataRow = rdrMerchants.Tables(0).Select("PORTAFOLIO = 'PROP' and id_agencia <> -1")
        Dim PropAsignados As Integer = countPropDataRow.Count
        Dim PropNoAsignados = countProp - PropAsignados
        Dim tempProp As Double = If(countProp > 0, PropAsignados / countProp * 100, 0)
        Dim assignationRateProp As String = If(tempProp > 0, tempProp.ToString().Substring(0, tempProp.ToString().IndexOf(".") + 3), 0)

        Dim countOBDataRow() As DataRow = rdrMerchants.Tables(0).Select("PORTAFOLIO = 'OB'")
        Dim countOB As Integer = countOBDataRow.Count
        countPropDataRow = rdrMerchants.Tables(0).Select("PORTAFOLIO = 'OB' and id_agencia <> -1")
        Dim OBAsignados As Integer = countPropDataRow.Count
        Dim OBNoAsignados = countOB - OBAsignados
        Dim tempOB As Double = If(countOB > 0, OBAsignados / countOB * 100, 0)
        Dim assignationRateOB As String = If(tempOB > 0, tempOB.ToString().Substring(0, tempOB.ToString().IndexOf(".") + 3), 0)

        totalMerchants.Value = countProp + countOB

        hfLIFsProp.Value = countProp
        hfLIFsOB.Value = countOB
        hfAssignedOB.Value = OBAsignados
        hfAssignedProp.Value = PropAsignados
        hfNotAssignedOB.Value = OBNoAsignados
        hfNotAssignedProp.Value = PropNoAsignados
        hfAssignationRateOB.Value = assignationRateOB
        hfAssignationRateProp.Value = assignationRateProp



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






        Dim resestemes As String = "[{ ""letter"": ""OB"", ""frequency"": """ + obt.ToString() + """ },
            { ""letter"": ""Prop"", ""frequency"": """ + propt.ToString() + """ }]"

        Dim mesesPresent(0 To 3) As Integer
        Dim mesesPlaced(0 To 3) As Integer
        mesesPresent(0) = countProp
        mesesPresent(1) = PropAsignados
        mesesPresent(2) = 15
        mesesPresent(3) = 15


        mesesPlaced(0) = countOB
        mesesPlaced(1) = OBAsignados
        mesesPlaced(2) = 15
        mesesPlaced(3) = 15




        Dim json As String = "[
        { month: ""LIFs"", Prop: """ + mesesPresent(0).ToString() + """, OB: """ + mesesPlaced(0).ToString() + """ },
        { month: ""Assigned"", Prop: """ + mesesPresent(1).ToString() + """, OB: """ + mesesPlaced(1).ToString() + """ },
        { month: ""Visited"", Prop: """ + mesesPresent(2).ToString() + """, OB: """ + mesesPlaced(2).ToString() + """},
        { month: ""Completed"", Prop: """ + mesesPresent(3).ToString() + """, OB: """ + mesesPlaced(3).ToString() + """},

    ]"

        mesesJSON.Value = json
    End Sub
End Class