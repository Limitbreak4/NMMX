Imports System.IO
Imports ClosedXML.Excel
Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss

Public Class frmCargarAsingacion
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
            TryCast(Master.FindControl("lblUsuario"), Label).Text = AntiXssEncoder.HtmlEncode(Session("NombreCompleto").ToString.ToUpperInvariant, False)
        End If
    End Sub

    Private Sub INICIALIZAR_FORM()
        Dim rdrAsignaciones As DataSet
        Dim sel As String = "SELECT ID_ASIGNACION, FH_ASIGNACION, FLG_DESCARGA, ID_USUARIO_PRIMER_DESCARGA, FH_ULTIMA_DESCARGA, NOMBRE_ARCHIVO "
        sel += " FROM ASIGNACIONES "
        sel += " WHERE FLG_CANC = 0 AND ARCHIVO_ACK = 1 AND FLG_OUTCOME_CARGADO = 0 AND FLG_DESCARGA = 1 AND ID_AGENCIA = @PARAM1"
        Dim comm As SqlCommand = New SqlCommand(sel)
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = Session("ID_AGENCIA")
        rdrAsignaciones = dsOpenDB(comm)
        'grdAsignmentData.DataSource = rdrAsignaciones.Tables(0)
        'grdAsignmentData.DataBind()
    End Sub

    'Private Sub grdAsignmentData_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdAsignmentData.RowCommand
    '    Dim sIdAsignacion As String = ""
    '    sIdAsignacion = grdAsignmentData.DataKeys(e.CommandArgument.ToString).Value.ToString
    '    Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
    '    'Dim rdrAsignacion As DataSet = dsOpenDB("SELECT * FROM ASIGNACIONES WHERE ID_ASIGNACION = " & sIdAsignacion)
    '    Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ASIGNACIONES (NOLOCK) WHERE ID_ASIGNACION = @PARAM1")
    '    comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = sIdAsignacion
    '    Dim rdrAsignacion As DataSet = dsOpenDB(comm)

    '    Dim sSql As String

    '    Dim fuObj As FileUpload

    '    fuObj = TryCast(grdAsignmentData.Rows(rowIndex).Cells(2).FindControl("fuOutcome"), FileUpload)
    '    If fuObj.HasFile Then
    '        '1.- abre el archivo y verifica cuál es la última columna usada
    '        Dim tmpFileName As String = FORMATEAR_FECHA(Now, "STR")
    '        Dim baseruta As String = Server.MapPath("~/tmpFile/" & tmpFileName & ".xlsx")
    '        If System.IO.File.Exists(baseruta) Then
    '            File.Delete(baseruta)
    '        End If
    '        fuObj.SaveAs(baseruta)
    '        Dim m_Excel As New XLWorkbook(baseruta)
    '        Dim COUNTROWSOK As Integer = 0
    '        'obtener el id del archivo de acuerdo a la asignación correspondiente
    '        'llenar un dataset de los archivos_data que corresponden a ese archivo y a la agencia correspondiente
    '        'sSql = "SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & rdrAsignacion.Tables(0).Rows(0).Item("ID_ARCHIVO") & " "
    '        'sSql &= "AND ID_AGENCIA = " & rdrAsignacion.Tables(0).Rows(0).Item("ID_AGENCIA")
    '        comm = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1 AND ID_AGENCIA = @PARAM2")
    '        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = rdrAsignacion.Tables(0).Rows(0).Item("ID_ARCHIVO")
    '        comm.Parameters.Add("@PARAM2", SqlDbType.BigInt).Value = rdrAsignacion.Tables(0).Rows(0).Item("ID_AGENCIA")

    '        Dim rdrArchivosData As DataSet = dsOpenDB(comm)
    '        Dim dtArchivosData() As DataRow

    '        Dim cResult As New clsResultado
    '        For iRow = 3 To m_Excel.Worksheets(0).LastRowUsed().RowNumber() - 1
    '            'FILTRO POR NUMERO DE AFILIACIÓN QUE ES EL CAMPO ÚNICO, HAY QUE VERIFICAR QUE SI SEA EL CMAPO.
    '            dtArchivosData = rdrArchivosData.Tables(0).Select("AFILIACION = '" & m_Excel.Worksheet(1).Row(iRow).Cell(6).Value & "'")
    '        If dtArchivosData.Length > 0 Then
    '                cResult.CLEAN()
    '                cResult.ID_ARCHIVO = rdrAsignacion.Tables(0).Rows(0).Item("ID_ARCHIVO")
    '                cResult.ID_ARCHIVO_DATA = dtArchivosData(0).Item("ID_ARCHIVO_DATA")
    '                cResult.F_VISITA = FORMATEAR_FECHA(m_Excel.Worksheet(1).Row(iRow).Cell(18).Value, "S")
    '                cResult.VISITA_EXITOSA = m_Excel.Worksheet(1).Row(iRow).Cell(19).Value 'visita exitosa
    '                cResult.HABIA_POP = m_Excel.Worksheet(1).Row(iRow).Cell(20).Value   'había pop?
    '                cResult.SE_SENALIZO = m_Excel.Worksheet(1).Row(iRow).Cell(21).Value 'se señalizó?
    '                cResult.POP_COLOCADO = m_Excel.Worksheet(1).Row(iRow).Cell(22).Value   'si no se pudo colocar el pop, por qué?
    '                cResult.SUPRESION = m_Excel.Worksheet(1).Row(iRow).Cell(23).Value 'hubo supresión
    '                cResult.PROBLEMA_TERMINAL = m_Excel.Worksheet(1).Row(iRow).Cell(24).Value     'probelmas con la terminal?
    '                cResult.TIPO_TERMINAL = m_Excel.Worksheet(1).Row(iRow).Cell(25).Value         'tipo de terminal
    '                cResult.DESCRIPCION_PROBLEMA = m_Excel.Worksheet(1).Row(iRow).Cell(26).Value 'descripción de problema
    '                cResult.OS_RESUELTO = m_Excel.Worksheet(1).Row(iRow).Cell(27).Value '# de orden de servicio resuelto
    '                cResult.SENTIMIENTO_ESTABLECIMIENTO = m_Excel.Worksheet(1).Row(iRow).Cell(28).Value 'sentimiento_establecimiento
    '                cResult.ACTIVACION = m_Excel.Worksheet(1).Row(iRow).Cell(29).Value 'activación (columna AE)???
    '                cResult.NUM_AFILIACION_ALTERNO = m_Excel.Worksheet(1).Row(iRow).Cell(30).Value 'comercio transacciona con otro número?
    '                cResult.ESTATUS_VISITA = m_Excel.Worksheet(1).Row(iRow).Cell(31).Value ''estatus de visita
    '                cResult.SAVE(cResult)
    '            End If
    '        Next
    '        'sSql = "UPDATE ASIGNACIONES SET FLG_OUTCOME_CARGADO = 1, "
    '        'sSql &= "FH_OUTCOME_CARGADO = '" & FORMATEAR_FECHA(Now, "C") & "', "
    '        'sSql &= "ID_USUARIO_OUTCOME = '" & Session("idUsuario") & "' "
    '        'sSql &= "WHERE ID_ASIGNACION = " & sIdAsignacion
    '        sSql = "UPDATE ASIGNACIONES SET FLG_OUTCOME_CARGADO = 1, "
    '        sSql &= "FH_OUTCOME_CARGADO = @PARAM1 "
    '        sSql &= "ID_USUARIO_OUTCOME = @PARAM2 "
    '        sSql &= "WHERE ID_ASIGNACION = @PARAM3 "
    '        comm = New SqlCommand(sSql)
    '        comm.Parameters.Add("@PARAM", SqlDbType.VarChar).Value = FORMATEAR_FECHA(Now, "C")

    '        comm.Parameters.Add("@PARAM", SqlDbType.VarChar).Value = Session("idUsuario")
    '        comm.Parameters.Add("@PARAM", SqlDbType.BigInt).Value = sIdAsignacion

    '        ExecuteCmd(comm)
    '        Dim mensajestring As String = AntiXss.AntiXssEncoder.HtmlEncode("CARGA DE RESULTADOS FINALIZADA", False)
    '        msg(mensajestring)
    '        INICIALIZAR_FORM()
    '    Else
    '        msg("SELECCIONE UN ARCHIVO")
    '    End If
    'End Sub
    Private Sub msg(ByVal txtIn)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" & txtIn & "');", True)
    End Sub
    Private Sub DOWNLOAD_FILE(idAsignacion As Integer)
        Dim rdr As DataSet
        'rdr = dsOpenDB("SELECT * FROM ASIGNACIONES WHERE ID_ASIGNACION = '" & idAsignacion & "'")
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ASIGNACIONES WHERE ID_ASIGNACION = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idAsignacion
        rdr = dsOpenDB(comm)
        If rdr.Tables(0).Rows.Count > 0 Then
            Dim archivostring = rdr.Tables(0).Rows(0).Item("ARCHIVO")
            Dim binaryData() As Byte = Convert.FromBase64String(archivostring)
            'Dim binaryData() As Byte = rdr.Tables(0).Rows(0).Item("ARCHIVO")
            Dim nombrearchivo As String = AntiXssEncoder.HtmlEncode(rdr.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO").ToString.Replace(" ", "_"), False)
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

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim fileEnconded As String = AntiXssEncoder.HtmlEncode("Respuesta_" & FileUpload1.FileName, False)

        If FileUpload1.HasFile And validaInjection(fileEnconded) And (fileEnconded.EndsWith("xlsx") Or fileEnconded.EndsWith("xls")) Then
            FileUpload1.SaveAs(Server.MapPath("~/tmpFile/" & fileEnconded))
            Dim FILEPATH As String = Server.MapPath("~/tmpFile/" & fileEnconded)
            Dim fileRespuesta As New clsArchivo
            fileRespuesta.ID_ARCHIVO = -1
            fileRespuesta.FH_CARGA = FORMATEAR_FECHA(System.DateTime.Now, "C")
            fileRespuesta.NOMBRE_ARCHIVO = fileEnconded
            fileRespuesta.FLG_MARKETING = False
            fileRespuesta.FLG_AGENCIA = True
            fileRespuesta.ARCHIVO = FILE_2_BYTES(FILEPATH)
            fileRespuesta.ID_CAMPAIGN = -1
            fileRespuesta.SAVE(fileRespuesta)

            Dim ex_Resp As New XLWorkbook(FILEPATH)



        End If
    End Sub
End Class