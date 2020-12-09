Imports System.IO
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss

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

        CARGAR_COMBO(dropDates, New SqlCommand("SELECT * FROM ARCHIVOS (nolock)"), "ID_ARCHIVO", "FH_CARGA", "NOMBRE_ARCHIVO", True)

        'CARGAR_COMBO(dropDates, "*", "ARCHIVOS (nolock)", "", "ID_ARCHIVO", "FH_CARGA", "NOMBRE_ARCHIVO", True)
    End Sub

    Private Sub dropDates_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dropDates.SelectedIndexChanged
        If dropDates.Text = "" Then
            Exit Sub

        End If
        Dim rdrAssignmments As DataSet
        Dim sel As String = "SELECT ID_ASIGNACION, FH_ASIGNACION, ASIGNACIONES.ID_AGENCIA, TAB_AGENCIA.DESC_AGENCIA, ASIGNACIONES.FLG_CANC, ASIGNACIONES.ID_ARCHIVO, NOMBRE_ARCHIVO, ARCHIVO_ACK, ROWS_OK, ROWS_NOT_OK, FLG_DESCARGA, VECES_DESCARGA, FH_ULTIMA_DESCARGA, TIPO_BLITZ, FH_OUTCOME_CARGADO "
        sel += "FROM ASIGNACIONES LEFT JOIN TAB_AGENCIA ON TAB_AGENCIA.ID_AGENCIA = ASIGNACIONES.ID_AGENCIA "
        sel += "WHERE ID_ARCHIVO  = @PARAM1 "
        Dim comm As SqlCommand = New SqlCommand(sel)
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = dropDates.SelectedValue


        'sSql = "SELECT ID_ASIGNACION, FH_ASIGNACION, ASIGNACIONES.ID_AGENCIA, TAB_AGENCIA.DESC_AGENCIA, ASIGNACIONES.FLG_CANC, ASIGNACIONES.ID_ARCHIVO, NOMBRE_ARCHIVO, ARCHIVO_ACK, ROWS_OK, ROWS_NOT_OK, FLG_DESCARGA, VECES_DESCARGA, FH_ULTIMA_DESCARGA, TIPO_BLITZ, FH_OUTCOME_CARGADO FROM ASIGNACIONES LEFT JOIN TAB_AGENCIA ON TAB_AGENCIA.ID_AGENCIA = ASIGNACIONES.ID_AGENCIA WHERE ID_ARCHIVO  = '" & dropDates.SelectedValue & "'"
        rdrAssignmments = dsOpenDB(comm)
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
        idAsignacion = AntiXssEncoder.HtmlEncode(idAsignacion, False)
        grdSummary.DataSource = Nothing
        grdSummary.DataBind()
        Dim comm As SqlCommand = New SqlCommand("SELECT ID_ARCHIVO, ID_AGENCIA FROM ASIGNACIONES (NOLOCK) WHERE ID_ASIGNACION = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idAsignacion


        Dim rdrAsignacion As DataSet = dsOpenDB(comm)
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
                'Dim rdrData As DataSet = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & rdrAsignacion.Tables(0).Rows(0).Item("ID_ARCHIVO") & " And ID_AGENCIA = " & dtAgencias.Rows(iAgencias).Item("ID_AGENCIA"))
                comm = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1 And ID_AGENCIA = @PARAM2")
                comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = rdrAsignacion.Tables(0).Rows(0).Item("ID_ARCHIVO")
                comm.Parameters.Add("@PARAM2", SqlDbType.BigInt).Value = dtAgencias.Rows(iAgencias).Item("ID_AGENCIA")
                Dim rdrData As DataSet = dsOpenDB(comm)
                Dim dtNewRow As DataRow = dtSummary.NewRow
                comm = New SqlCommand("SELECT DESC_AGENCIA FROM TAB_AGENCIA (NOLOCK) WHERE ID_AGENCIA = @PARAM1")
                comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = dtAgencias.Rows(iAgencias).Item("ID_AGENCIA")

                dtNewRow.Item("AGENCY") = VALORINTABLA("DESC_AGENCIA", "TAB_AGENCIA", "ID_AGENCIA", dtAgencias.Rows(iAgencias).Item("ID_AGENCIA"), comm)
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
        'rdr = dsOpenDB("SELECT * FROM ASIGNACIONES WHERE ID_ASIGNACION = '" & idAsignacion & "'")
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ASIGNACIONES (NOLOCK) WHERE ID_ASIGNACION = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idAsignacion
        rdr = dsOpenDB(comm)
        If rdr.Tables(0).Rows.Count > 0 Then
            Dim binaryData() As Byte = rdr.Tables(0).Rows(0).Item("ARCHIVO")
            Response.Clear()
            Response.AddHeader("content-disposition", "attachment;filename=" & AntiXssEncoder.HtmlEncode(rdr.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO").ToString.Replace(" ", "_"), False) & "")
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