Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss
Public Class frmDownloadAssignment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If
        If Session("ID_ROLE") <> 2 Then
            Response.Redirect("~/Default.aspx")
        End If

        If Not IsPostBack Then
            INICIALIZAR_FORM()
            TryCast(Master.FindControl("lblPage"), Label).Text = AntiXssEncoder.HtmlEncode("Descarga de Asignaciones Previas", False)
            TryCast(Master.FindControl("lblUsuario"), Label).Text = AntiXssEncoder.HtmlEncode("NombreCompleto", False)
        End If
    End Sub

    Private Sub INICIALIZAR_FORM()
        Dim rdrAsignaciones As DataSet
        Dim sel As String = "SELECT ID_ASIGNACION, FH_ASIGNACION, FH_PRIMERA_DESCARGA, FLG_DESCARGA, ID_USUARIO_PRIMER_DESCARGA, FH_ULTIMA_DESCARGA, NOMBRE_ARCHIVO"
        sel += " FROM ASIGNACIONES "
        sel += " WHERE FLG_CANC = 0 AND ARCHIVO_ACK = 1 AND FLG_DESCARGA = 1 AND ID_AGENCIA = @PARAM1 "
        Dim comm As SqlCommand = New SqlCommand(sel)
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = Session("ID_AGENCIA")
        rdrAsignaciones = dsOpenDB(comm)
        grdAsignmentData.DataSource = rdrAsignaciones.Tables(0)
        grdAsignmentData.DataBind()
    End Sub

    Private Sub grdAsignmentData_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdAsignmentData.RowCommand
        Dim sIdAsignacion As String = ""
        sIdAsignacion = grdAsignmentData.DataKeys(e.CommandArgument.ToString).Value.ToString
        DOWNLOAD_FILE(sIdAsignacion)

    End Sub

    Private Sub DOWNLOAD_FILE(idAsignacion As Integer)
        Dim rdr As DataSet
        idAsignacion = AntiXssEncoder.HtmlEncode(idAsignacion, False)
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ASIGNACIONES (NOLOCK) WHERE ID_ASIGNACION = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idAsignacion
        rdr = dsOpenDB(comm)
        If rdr.Tables(0).Rows.Count > 0 Then
            Dim binaryData() As Byte = rdr.Tables(0).Rows(0).Item("ARCHIVO")
            Dim nombrearchivo As String = rdr.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO").ToString.Replace(" ", "_")
            nombrearchivo = AntiXssEncoder.UrlEncode(nombrearchivo)
            INCDOWNLOADS(idAsignacion)
            Response.Clear()
            Response.AddHeader("content-disposition", "attachment;filename=" & nombrearchivo & "")
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.OutputStream.Write(binaryData, 0, binaryData.Length)
            Response.End()
        Else

        End If
        CIERRA_DATASET(rdr)

    End Sub

End Class