Imports System.IO
Imports System.Net.Mail

Public Class frmPrevAssignment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If
        '
        If Not IsPostBack Then
            INICIALIZAR_FORM()
            TryCast(Master.FindControl("lblPage"), Label).Text = "Review Previous Assignments"
        End If

    End Sub
    Private Sub INICIALIZAR_FORM()
        'CARGAR_COMBO(dropDates, "SELECT DISTINCT(FH_ASIGNACION) FROM ASIGNACIONES WHERE ARCHIVO_ACK = 1 ORDER BY FH_ASIGNACION DESC", "FH_ASIGNACION", "FH_ASIGNACION", True)
        CARGAR_COMBO(dropDates, "select * from ARCHIVOS (nolock)", "ID_ARCHIVO", "FH_CARGA", "NOMBRE_ARCHIVO", True)
    End Sub

    Private Sub dropDates_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dropDates.SelectedIndexChanged
        If dropDates.Text = "" Then
            Exit Sub

        End If
        Dim rdrAssignmments As DataSet
        Dim sSql As String
        sSql = "SELECT ID_ASIGNACION, FH_ASIGNACION, ASIGNACIONES.ID_AGENCIA, TAB_AGENCIA.DESC_AGENCIA, ASIGNACIONES.FLG_CANC, ASIGNACIONES.ID_ARCHIVO, NOMBRE_ARCHIVO, ARCHIVO_ACK, ROWS_OK, ROWS_NOT_OK, FLG_DESCARGA, VECES_DESCARGA, FH_ULTIMA_DESCARGA, TIPO_BLITZ, FH_OUTCOME_CARGADO FROM ASIGNACIONES LEFT JOIN TAB_AGENCIA ON TAB_AGENCIA.ID_AGENCIA = ASIGNACIONES.ID_AGENCIA WHERE ID_ARCHIVO  = '" & dropDates.SelectedValue & "'"
        rdrAssignmments = dsOpenDB(sSql)
        grdAsignmentData.DataSource = rdrAssignmments.Tables(0)
        grdAsignmentData.DataBind()

    End Sub

    Private Sub grdAsignmentData_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdAsignmentData.RowCommand
        Dim sIdAsignacion As String = ""
        sIdAsignacion = grdAsignmentData.DataKeys(e.CommandArgument.ToString).Value.ToString

        Select Case e.CommandName.ToString
            Case "cmdShowSummary"
                'envía mail al contacto en la agencia correspondiente
                SHOW_SUMMARY(sIdAsignacion)
            Case "cmdDownload"
                DOWNLOAD_FILE(sIdAsignacion)
            Case "cmdMail"
                Dim RES As String = SEND_MAIL(sIdAsignacion)
                If RES <> "" Then
                    msg(RES)
                End If

        End Select
    End Sub

    Private Sub msg(ByVal txtIn)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" & txtIn & "');", True)
    End Sub

    Private Sub SHOW_SUMMARY(ByVal idAsignacion As Integer)
        grdSummary.DataSource = Nothing
        grdSummary.DataBind()

        Dim rdrAsignacion As DataSet = dsOpenDB("SELECT ID_ARCHIVO, ID_AGENCIA FROM ASIGNACIONES WHERE ID_ASIGNACION = " & idAsignacion)
        Dim dRow() As DataRow
        If rdrAsignacion.Tables(0).Rows.Count > 0 Then
            Dim dtSummary As New DataTable
            dtSummary.Columns.Add(New DataColumn("AGENCY", GetType(String)))
            dtSummary.Columns.Add(New DataColumn("ID_AGENCIA", GetType(String)))
            dtSummary.Columns.Add(New DataColumn("ASSIGNED_BUSINESS", GetType(String)))
            dtSummary.Columns.Add(New DataColumn("VISITED_BUSINESS", GetType(String)))
            dtSummary.Columns.Add(New DataColumn("EFECTIVENESS", GetType(String)))
            Dim dtAgencias As DataTable = rdrAsignacion.Tables(0).DefaultView.ToTable(True, "ID_AGENCIA")

            For iAgencias As Integer = 0 To dtAgencias.Rows.Count - 1
                Dim rdrData As DataSet = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & rdrAsignacion.Tables(0).Rows(0).Item("ID_ARCHIVO") & " And ID_AGENCIA = " & dtAgencias.Rows(iAgencias).Item("ID_AGENCIA"))
                Dim dtNewRow As DataRow = dtSummary.NewRow
                dtNewRow.Item("AGENCY") = VALORINTABLA("DESC_AGENCIA", "TAB_AGENCIA", "ID_AGENCIA", dtAgencias.Rows(iAgencias).Item("ID_AGENCIA"))
                dtNewRow.Item("ID_AGENCIA") = dtAgencias.Rows(iAgencias).Item("ID_AGENCIA")
                'dRow = rdrData.Tables(0).Select("")
                dtNewRow.Item("ASSIGNED_BUSINESS") = rdrData.Tables(0).Rows.Count
                Dim iVisited As Integer = 0


                dtNewRow.Item("VISITED_BUSINESS") = "0" 'VER CÓMO PRETENDE HUGO COLOCAR LA VISITA DESPUÉS DE CARGAR EL ARCHIVO
                dtNewRow.Item("EFECTIVENESS") = (iVisited * 100) / rdrData.Tables(0).Rows.Count & "%"
                dtSummary.Rows.Add(dtNewRow)
            Next
            grdSummary.DataSource = dtSummary
            grdSummary.DataBind()
        End If

        CIERRA_DATASET(rdrAsignacion)
    End Sub
    Private Sub DOWNLOAD_FILE(idAsignacion As Integer)
        Dim rdr As DataSet
        rdr = dsOpenDB("SELECT * FROM ASIGNACIONES WHERE ID_ASIGNACION = '" & idAsignacion & "'")
        If rdr.Tables(0).Rows.Count > 0 Then
            Dim binaryData() As Byte = rdr.Tables(0).Rows(0).Item("ARCHIVO")
            Response.Clear()
            Response.AddHeader("content-disposition", "attachment;filename=" & rdr.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO").ToString.Replace(" ", "_") & "")
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.OutputStream.Write(binaryData, 0, binaryData.Length)
            Response.End()
        Else

        End If
        CIERRA_DATASET(rdr)

    End Sub

    Private Sub dropDates_Load(sender As Object, e As EventArgs) Handles dropDates.Load

    End Sub
End Class