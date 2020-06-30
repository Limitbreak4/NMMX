Imports System.IO
Imports System.Net.Mail
Imports ClosedXML.Excel
Imports System.Data.SqlClient

Public Class frmNewAssignment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If
        '
        If Not IsPostBack Then
            'INICIALIZAR_FORM
            TryCast(Master.FindControl("lblPage"), Label).Text = "Create New Assignment"
        End If

    End Sub
    Protected Sub cmdIniciaProces_Click(sender As Object, e As EventArgs) Handles cmdIniciaProces.Click 'version FINA
        If IsNothing(lbresult.Items) Then
            lbresult.Items.Clear()
        End If
        If fu.HasFile Then
            Dim CLSfILE As New clsArchivo
            Dim FILEPATH As String = Server.MapPath("~/tmpFile/" & fu.FileName.Replace(" ", "_"))
            CLSfILE.ID_ARCHIVO = -1
            CLSfILE.FH_CARGA = FORMATEAR_FECHA(System.DateTime.Now, "C")
            CLSfILE.NOMBRE_ARCHIVO = fu.FileName
            CLSfILE.FLG_MARKETING = True
            CLSfILE.FLG_AGENCIA = False
            CLSfILE.ARCHIVO = FILE_2_BYTES(FILEPATH)
            CLSfILE.SAVE(CLSfILE)

            Dim rdrAgencia As DataSet = dsOpenDB("SELECT * FROM TAB_AGENCIA WHERE FLG_CANC = 0")
            Dim rdrCP As DataSet = dsOpenDB("SELECT * FROM TAB_REGION WHERE FLG_CANC = 0 ")
            Dim rdrEstados As DataSet = dsOpenDB("SELECT * FROM TAB_ESTADOS WHERE FLG_CANC = 0 ")
            Dim rdrCPEstado As DataSet = dsOpenDB("SELECT * FROM TAB_CP INNER JOIN TAB_ESTADOS ON TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO WHERE TAB_ESTADOS.GEO_DEFAULT <> 0")
            Dim rdrAlcaldia As DataSet = dsOpenDB("select * from tab_region inner join tab_estados on tab_region.id_estado = TAB_ESTADOS.ID_ESTADO where tab_region.FLG_CANC = 0") 'CAMBIO
            Dim dRowAgencia() As DataRow
            Dim dRowBlitz() As DataRow
            Dim rowCom() As DataRow
            Dim dRowAlcaldia() As DataRow 'CAMBIO
            If File.Exists(FILEPATH) Then
                File.Delete(FILEPATH)
            End If
            fu.PostedFile.SaveAs(FILEPATH)
            Dim m_Excel As New XLWorkbook(FILEPATH)
            'Dim lastRow As Integer = Integer.Parse(m_Excel.Worksheets(0).LastRowUsed().RowNumber())

            Dim iRow As Integer
            Dim iAck As Integer = 0
            Dim iNack As Integer = 0
            Dim iBlitz50 As Integer = 0
            Dim iBlitz100 As Integer = 0
            Dim iBlitz300 As Integer = 0
            Dim Prioridad As Integer = 0 'CAMBIO
            Dim agencia_por_alcaldia As Integer = 0 'CAMBIO

            Dim conTemp As SqlConnection = ConnProcesoLargo()



            For iRow = 1 To m_Excel.Worksheets(0).LastRowUsed().RowNumber() - 1
                'Debug.Print(iRow)
                'verifico solo còdigos postales


                If validaRenglon(m_Excel.Worksheet(1).Row(iRow + 1)) Then
                    '    [1833, 5/3/2020] Fina: entonces la regla queda:
                    '[1833, 5/3/2020] Fina: buscar ( código postal)
                    '[18:33, 5/3/2020]   Fina: If found -> asignar a su agencia (agregar al archivo de su agencia)
                    '[18:34, 5/3/2020] Fina: If Not found -> {
                    '[18:35, 5/3/2020] Fina: If estado = geo 1 -> save For blitz (50) x municipio
                    '[18:35, 5/3/2020] Fina: If estado = geo 2 -> save For blitz (100) x municipio
                    '[18:35, 5/3/2020] Fina: }
                    '[18:35, 5/3/2020] Fina: Else save For blitz (300) x municipio

                    If m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value.ToString.Length = 5 Then
                        dRowAgencia = rdrCP.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value & "'")
                    ElseIf m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value.ToString.Length = 4 Then
                        dRowAgencia = rdrCP.Tables(0).Select("CP = '0" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value & "'")
                    ElseIf m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value > 5 Then
                        ReDim dRowAgencia(0)
                    End If



#Disable Warning BC42104 ' Variable is used before it has been assigned a value
                    If dRowAgencia.Length > 0 Then
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, dRowAgencia(0).Item("ID_AGENCIA"), dRowAgencia(0).Item("ID_REGION"), dRowAgencia(0).Item("GEO"), True, "", "", conTemp)
                        iAck += 1
                    Else
                        'busca el cp dentro de la tabla de códigos postales y de ahi lo relaciona con su geo del estado
                        'dRowBlitz = rdrCP.Tables(0).Select("DESC_REGION = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value & "'")
                        dRowBlitz = rdrCPEstado.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value & "'")
                        If dRowBlitz.Length > 0 Then
                            Select Case dRowBlitz(0).Item("GEO_DEFAULT")
                                Case 1
                                    addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 1, True, "BLITZ50", "", conTemp)
                                    iBlitz50 += 1
                                Case 2
                                    addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 2, True, "BLITZ100", "", conTemp)
                                    iBlitz100 += 1
                                Case Else
                                    addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 3, True, "BLITZ300", "", conTemp)
                                    iBlitz300 += 1
                            End Select
                            'iAck += 1
                        Else
                            '    rowCom = rdrEstados.Tables(0).Select("DESC_ESTADO = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value & "'")
                            '    If rowCom.Length > 0 Then
                            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 3, True, "BLITZ300", "", conTemp)
                            iBlitz300 += 1
                            'iAck += 1
                            '    Else
                            '        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -1, False, "", "CIUDAD/ESTADO NO ENCONTRADA EN DB")
                            '        iNack += 1
                            '    End If
                        End If

                    End If
                    'Comment out del cambio de alcaldía
                Else

                    If m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value.ToString.Length > 0 And m_Excel.Worksheet(1).Row(iRow + 1).Cell(12).Value.ToString.Length > 0 Then
                        'CAMBIO busca agencia pero usando estado y municipio/alcaldía/ciudad

                        'Dim tmpEstado As String = m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value.ToString.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                        'tmpEstado = tmpEstado.Replace("Á", "a").Replace("É", "e").Replace("Í", "i").Replace("Ó", "o").Replace("Ú", "u")
                        'tmpEstado = tmpEstado.ToLower()
                        'Dim tmpAlcaldia As String = m_Excel.Worksheet(1).Row(iRow + 1).Cell(12).Value.ToString.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                        'tmpAlcaldia = tmpAlcaldia.Replace("Á", "a").Replace("É", "e").Replace("Í", "i").Replace("Ó", "o").Replace("Ú", "u")
                        'tmpAlcaldia = tmpAlcaldia.ToLower()
                        'rdrAlcaldia.CaseSensitive = False
                        Dim tmpEstado As String = m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value.ToString
                        Dim tmpAlcaldia As String = m_Excel.Worksheet(1).Row(iRow + 1).Cell(12).Value.ToString
                        If tmpEstado = "Querétaro" Then
                            tmpEstado = "Santiago de Querétaro"
                        End If
                        dRowAlcaldia = rdrAlcaldia.Tables(0).Select("DESC_REGION = '" & tmpAlcaldia & "' AND  DESC_ESTADO = '" & tmpEstado & "'")

                        If dRowAlcaldia.Length > 0 Then
                            'Si tiene Estado y Ciudad/Alcaldía/Municipio bien, se mete a la asignación. Si falta alguno, se mete a blitz
                            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, dRowAlcaldia(0).Item("ID_AGENCIA"), dRowAlcaldia(0).Item("ID_REGION"), dRowAlcaldia(0).Item("GEO"), True, "", "", conTemp)
                        Else
                            tmpEstado = m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value.ToString
                            dRowAlcaldia = rdrAlcaldia.Tables(0).Select("DESC_ESTADO = '" & tmpEstado & "'")
                            If dRowAlcaldia.Length > 0 Then
                                Select Case dRowAlcaldia(0).Item("GEO")
                                    Case 1
                                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 1, True, "BLITZ50", "", conTemp)
                                        iBlitz50 += 1
                                    Case 2
                                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 2, True, "BLITZ100", "", conTemp)
                                        iBlitz100 += 1
                                    Case Else
                                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 3, True, "BLITZ300", "", conTemp)
                                        iBlitz300 += 1
                                End Select
                            Else
                                addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -1, False, "", "", conTemp)
                                iNack += 1

                            End If
                        End If
                    Else
                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -1, False, "", "", conTemp)
                        iNack += 1
                    End If

                End If
            Next
            CREA_FILES_AGENCIA_ACK(CLSfILE.ID_ARCHIVO)
            DESPLIEGA_FILES_GRID(CLSfILE.ID_ARCHIVO)
            msg("FILE COMPLETED TO PROCESS")
            lbresult.Items.Insert(0, iRow - 1 & " ROWS READ")
            lbresult.Items.Insert(1, iAck & " ROWS OK")
            lbresult.Items.Insert(2, iNack & " ROWS NOT OK")
            lbresult.Items.Insert(3, iBlitz50 & " ROWS ASSIGNED TO GEO 1 BLITZ")
            lbresult.Items.Insert(4, iBlitz100 & " ROWS ASSIGNED TO GEO 2 BLITZ")
            lbresult.Items.Insert(5, iBlitz300 & " ROWS ASSIGNED TO GEO 3 BLITZ")
            conTemp.Close()


        Else
            msg("NO FILE TO PROCESS")
        End If
    End Sub
    'Protected Sub cmdIniciaProces_Click(sender As Object, e As EventArgs) Handles cmdIniciaProces.Click 'versión ADLV
    '    lbresult.Items.Clear()

    '    If fu.HasFile Then
    '        Dim CLSfILE As New clsArchivo
    '        Dim FILEPATH As String = Server.MapPath("~/tmpFile/" & fu.FileName.Replace(" ", "_"))
    '        CLSfILE.ID_ARCHIVO = -1
    '        CLSfILE.FH_CARGA = FORMATEAR_FECHA(System.DateTime.Now, "C")
    '        CLSfILE.NOMBRE_ARCHIVO = fu.FileName
    '        CLSfILE.FLG_MARKETING = True
    '        CLSfILE.FLG_AGENCIA = False
    '        CLSfILE.ARCHIVO = FILE_2_BYTES(FILEPATH)
    '        CLSfILE.SAVE(CLSfILE)

    '        Dim rdrAgencia As DataSet = dsOpenDB("SELECT * FROM TAB_AGENCIA WHERE FLG_CANC = 0")
    '        Dim rdrCP As DataSet = dsOpenDB("SELECT * FROM TAB_REGION WHERE FLG_CANC = 0 ")
    '        Dim rdrEstados As DataSet = dsOpenDB("SELECT * FROM TAB_ESTADOS WHERE FLG_CANC = 0 ")
    '        Dim rdrCPEstado As DataSet = dsOpenDB("SELECT * FROM TAB_CP INNER JOIN TAB_ESTADOS ON TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO WHERE TAB_ESTADOS.GEO_DEFAULT <> 0")
    '        Dim dRowAgencia() As DataRow
    '        Dim dRowBlitz() As DataRow
    '        Dim rowCom() As DataRow
    '        If File.Exists(FILEPATH) Then
    '            File.Delete(FILEPATH)
    '        End If
    '        fu.PostedFile.SaveAs(FILEPATH)
    '        Dim m_Excel As New XLWorkbook(FILEPATH)
    '        'Dim lastRow As Integer = Integer.Parse(m_Excel.Worksheets(0).LastRowUsed().RowNumber())

    '        Dim iRow As Integer
    '        Dim iAck As Integer = 0
    '        Dim iNack As Integer = 0
    '        Dim iBlitz50 As Integer = 0
    '        Dim iBlitz100 As Integer = 0
    '        Dim iBlitz300 As Integer = 0
    '        For iRow = 2 To m_Excel.Worksheets(0).LastRowUsed().RowNumber() - 1
    '            'Debug.Print(iRow)
    '            'verifico solo còdigos postales 
    '            If validaRenglon(m_Excel.Worksheet(1).Row(iRow + 1)) Then
    '                '    [1833, 5/3/2020] Fina: entonces la regla queda:
    '                '[1833, 5/3/2020] Fina: buscar ( código postal)
    '                '[18:33, 5/3/2020]   Fina: If found -> asignar a su agencia (agregar al archivo de su agencia)
    '                '[18:34, 5/3/2020] Fina: If Not found -> {
    '                '[18:35, 5/3/2020] Fina: If estado = geo 1 -> save For blitz (50) x municipio
    '                '[18:35, 5/3/2020] Fina: If estado = geo 2 -> save For blitz (100) x municipio
    '                '[18:35, 5/3/2020] Fina: }
    '                '[18:35, 5/3/2020] Fina: Else save For blitz (300) x municipio
    '                dRowAgencia = rdrCP.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value & "'")
    '                If dRowAgencia.Length > 0 Then
    '                    addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, dRowAgencia(0).Item("ID_AGENCIA"), dRowAgencia(0).Item("ID_REGION"), dRowAgencia(0).Item("GEO"), True)
    '                Else
    '                    'busca el cp dentro de la tabla de códigos postales y de ahi lo relaciona con su geo del estado
    '                    'dRowBlitz = rdrCP.Tables(0).Select("DESC_REGION = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value & "'")
    '                    dRowBlitz = rdrCPEstado.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value & "'")
    '                    If dRowBlitz.Length > 0 Then
    '                        Select Case dRowBlitz(0).Item("GEO_DEFAULT")
    '                            Case 1
    '                                addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 1, True, "BLITZ50")
    '                                iBlitz50 += 1
    '                            Case 2
    '                                addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 2, True, "BLITZ100")
    '                                iBlitz100 += 1
    '                            Case Else
    '                                addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 3, True, "BLITZ300")
    '                                iBlitz300 += 1
    '                        End Select
    '                        iAck += 1
    '                    Else
    '                        '    rowCom = rdrEstados.Tables(0).Select("DESC_ESTADO = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value & "'")
    '                        '    If rowCom.Length > 0 Then
    '                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 3, True, "BLITZ300")
    '                        iBlitz300 += 1
    '                        '    Else
    '                        '        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -1, False, "", "CIUDAD/ESTADO NO ENCONTRADA EN DB")
    '                        '        iNack += 1
    '                        '    End If
    '                    End If

    '                End If
    '                    Else
    '                addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -1, False)
    '                iNack += 1
    '            End If


    '        Next
    '        CREA_FILES_AGENCIA_ACK(CLSfILE.ID_ARCHIVO)
    '        DESPLIEGA_FILES_GRID(CLSfILE.ID_ARCHIVO)
    '        msg("FILE COMPLETED TO PROCESS")
    '        lbresult.Items.Insert(0, iRow & " ROWS READ")
    '        lbresult.Items.Insert(1, iAck & " ROWS OK")
    '        lbresult.Items.Insert(2, iNack & " ROWS NOT OK")
    '        lbresult.Items.Insert(3, iBlitz50 & " ROWS ASSIGNED TO GEO 1 BLITZ")
    '        lbresult.Items.Insert(4, iBlitz100 & " ROWS ASSIGNED TO GEO 2 BLITZ")
    '        lbresult.Items.Insert(5, iBlitz300 & " ROWS ASSIGNED TO GEO 3 BLITZ")



    '    Else
    '        msg("NO FILE TO PROCESS")
    '    End If
    'End Sub

    Private Function validaRenglon(ByVal rowExcel As IXLRow) As Boolean 'VERSION FINA
        Dim res As Boolean = True
        If Not rowExcel.Cell(10).Value.ToString.Length = 5 Then
            If Not rowExcel.Cell(10).Value.ToString.Length = 4 Then
                res = False
            End If
        End If


        If Not IsNumeric(rowExcel.Cell(10).Value.ToString) Then
            res = False
        End If

        'If rowExcel.Cell(11).Value.ToString = "" Then
        '    res = False
        'End If


        'If rowExcel.Cell(9).Value.ToString = "" Then
        '    res = False
        'End If

        If rowExcel.Cell(8).Value.ToString = "" Then
            res = False
        End If
        Return res
    End Function

    'Private Function validaRenglon(ByVal rowExcel As IXLRow) As Boolean 'versión ADLV
    '    Dim res As Boolean = True
    '    If Not rowExcel.Cell(10).Value.ToString.Length = 5 Then
    '        res = False
    '    End If

    '    If Not IsNumeric(rowExcel.Cell(10).Value.ToString) Then
    '        res = False
    '    End If

    '    If rowExcel.Cell(11).Value.ToString = "" Then
    '        res = False
    '    End If

    '    If rowExcel.Cell(12).Value.ToString = "" Then
    '        res = False
    '    End If

    '    'If rowExcel.Cell(9).Value.ToString = "" Then
    '    '    res = False
    '    'End If

    '    If rowExcel.Cell(8).Value.ToString = "" Then
    '        res = False
    '    End If
    '    Return res
    'End Function

    Private Sub msg(ByVal txtIn)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" & txtIn & "');", True)
    End Sub

    Private Sub DESPLIEGA_FILES_GRID(ByVal idArchivo As String)
        Dim rdrAsignaciones As DataSet = dsOpenDB("SELECT ROWS_OK,ROWS_NOT_OK, DESC_AGENCIA, NOMBRE_ARCHIVO, ID_ASIGNACION FROM ASIGNACIONES INNER JOIN TAB_AGENCIA ON TAB_AGENCIA.ID_AGENCIA = ASIGNACIONES.ID_AGENCIA WHERE ARCHIVO_ACK = 1 AND ID_ARCHIVO = " & idArchivo)
        grdFiles.DataSource = rdrAsignaciones.Tables(0)
        grdFiles.DataBind()

        Dim rdrAsignacionesNACK As DataSet = dsOpenDB("SELECT ROWS_NOT_OK, NOMBRE_ARCHIVO, ID_ASIGNACION FROM ASIGNACIONES WHERE ARCHIVO_ACK = 0 AND ID_ARCHIVO = " & idArchivo)
        If rdrAsignacionesNACK.Tables(0).Rows.Count > 0 Then
            lblNotOk.Visible = True
        End If
        grdNAKFile.DataSource = rdrAsignacionesNACK.Tables(0)
        grdNAKFile.DataBind()


        Dim rdrBlitz As DataSet = dsOpenDB("SELECT ROWS_OK,ROWS_NOT_OK, NOMBRE_ARCHIVO, ID_ASIGNACION, TIPO_BLITZ FROM ASIGNACIONES WHERE ARCHIVO_ACK = 1 AND TIPO_BLITZ  <> '' AND ID_ARCHIVO = " & idArchivo)
        If rdrBlitz.Tables(0).Rows.Count > 0 Then
            lblBlitz.Visible = True
        End If
        grdBlitz.DataSource = rdrBlitz.Tables(0)
        grdBlitz.DataBind()


    End Sub
    Private Sub addRecordOk(ByVal rowExcel As IXLRow, ByVal idArchivo As Integer, ByVal idAgencia As Integer, ByVal idRegion As Integer, ByVal GEO As Integer, ByVal bRecordOk As Boolean, Optional ByVal tipoBlitz As String = "", Optional ByVal motivoRechazo As String = "", Optional ByRef existingConnection As SqlConnection = Nothing)
        Dim cRecord As New clsRecord
        cRecord.ID_ARCHIVO_DATA = -1
        cRecord.ID_ARCHIVO = idArchivo
        cRecord.COL1 = ""
        cRecord.COL2 = ""
        cRecord.COL3 = ""
        cRecord.COL4 = ""
        cRecord.COL5 = ""
        cRecord.COL6 = ""
        cRecord.COL7 = ""
        cRecord.COL8 = ""
        cRecord.COL9 = ""
        cRecord.COL10 = ""
        cRecord.COL11 = ""
        cRecord.COL12 = ""
        cRecord.COL13 = ""
        cRecord.COL14 = ""
        cRecord.COL15 = ""
        cRecord.COL16 = ""

        cRecord.ID_AGENCIA = idAgencia
        cRecord.FILE = rowExcel.Cell(2).Value.ToString
        cRecord.PORTAFOLIO = rowExcel.Cell(3).Value.ToString 'cambioPortafolio
        cRecord.PARTNER = rowExcel.Cell(4).Value.ToString 'PARTNER
        cRecord.CANAL = rowExcel.Cell(5).Value.ToString
        cRecord.AFILIACION = rowExcel.Cell(6).Value.ToString
        cRecord.NOMBRE_ESTABLECIMIENTO = rowExcel.Cell(7).Value.ToString
        cRecord.CALLE_NUMERO = rowExcel.Cell(8).Value.ToString
        cRecord.COLONIA = rowExcel.Cell(9).Value.ToString
        cRecord.CP = rowExcel.Cell(10).Value.ToString
        cRecord.ESTADO_MUNICIPIO = rowExcel.Cell(11).Value.ToString
        cRecord.CIUDAD = rowExcel.Cell(12).Value.ToString
        cRecord.TELEFONO = rowExcel.Cell(13).Value.ToString
        cRecord.COBERTURA = IIf(rowExcel.Cell(16).Value.ToString.ToUpper = "COBERTURA", "Y", "N") 'rowExcel.Cell(15).Value.ToString


        'cRecord.ASIGNADO = rowExcel.Cell(1).Value.ToString

        'cRecord.FILE = rowExcel.Cell(2).Value.ToString
        'cRecord.ID_AGENCIA = idAgencia
        'cRecord.PORTAFOLIO = rowExcel.Cell(3).Value.ToString 'cambioPortafolio
        'cRecord.BASE = rowExcel.Cell(4).Value.ToString
        'cRecord.BANCO = rowExcel.Cell(5).Value.ToString
        'cRecord.AFILIACION = rowExcel.Cell(6).Value.ToString
        'cRecord.NOMBRE_ESTABLECIMIENTO = rowExcel.Cell(7).Value.ToString
        'cRecord.CALLE_NUMERO = rowExcel.Cell(8).Value.ToString
        'cRecord.COLONIA = rowExcel.Cell(9).Value.ToString
        'cRecord.CP = rowExcel.Cell(10).Value.ToString
        'cRecord.ESTADO_MUNICIPIO = rowExcel.Cell(11).Value.ToString
        ''cRecord.alcaldia = rowExcel.Cell(12).Value.ToString
        'cRecord.CIUDAD = rowExcel.Cell(12).Value.ToString
        'cRecord.TELEFONO = rowExcel.Cell(13).Value.ToString
        'cRecord.CANAL = rowExcel.Cell(4).Value.ToString
        'cRecord.COBERTURA = IIf(rowExcel.Cell(16).Value.ToString.ToUpper = "COBERTURA", "Y", "N") 'rowExcel.Cell(15).Value.ToString
        'cRecord.ID_REGION = idRegion
        cRecord.FLG_RECORD_OK = bRecordOk
        cRecord.FH_INSERT = FORMATEAR_FECHA(System.DateTime.Now, "C")

        cRecord.TIPO_BLITZ = tipoBlitz
        cRecord.GEO = GEO



        cRecord.MOTIVO_RECHAZO = motivoRechazo
        cRecord.SAVE(cRecord, existingConnection)

    End Sub
    Private Sub addRecordNok()

    End Sub
    Private Sub CREA_FILES_AGENCIA_ACK(ByVal idArchivo As Integer)
        Dim RDRACK As DataSet
        RDRACK = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = ''")

        Dim distinctDT As DataTable = RDRACK.Tables(0).DefaultView.ToTable(True, "ID_AGENCIA")
        Dim drAck() As DataRow
        'CREA EL REGISTRO DE ASIGNACIÓN

        For I As Integer = 0 To distinctDT.Rows.Count - 1




            drAck = RDRACK.Tables(0).Select("ID_ARCHIVO = " & idArchivo & " AND ID_AGENCIA = " & distinctDT.Rows(I).Item("ID_AGENCIA"))
            If drAck.Length > 0 Then
                Dim baseruta As String = Server.MapPath("~/tmpFile/plantilla_ack.xlsx")
                Dim strAgencia As String = VALORINTABLA("DESC_AGENCIA", "TAB_AGENCIA", "ID_AGENCIA", distinctDT.Rows(I).Item("ID_AGENCIA"))

                Dim PathCopia As String = Server.MapPath("~/tmpFile/" & strAgencia.Replace(" ", "_") & ".xlsx")
                If System.IO.File.Exists(PathCopia) Then
                    File.Delete(PathCopia)
                End If
                If System.IO.File.Exists(baseruta) Then
                    File.Copy(baseruta, PathCopia)
                End If


                Dim m_Excel As New XLWorkbook(PathCopia)
                Dim COUNTROWSOK As Integer = 0

                For iRow As Integer = 0 To drAck.Length - 1
                    If drAck(iRow).Item("FLG_RECORD_OK") Then
                        COUNTROWSOK += 1

                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(1).Value = strAgencia 'AGENCIA
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(2).Value = drAck(iRow).Item("FILE") 'ARCHIVO
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(3).Value = drAck(iRow).Item("PORTAFOLIO") 'PORTAFOLIO
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(4).Value = drAck(iRow).Item("PARTNER") 'PARTNER
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(5).Value = drAck(iRow).Item("CANAL") 'CANAL
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(6).Value = drAck(iRow).Item("AFILIACION") 'AFILIACIÓN
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(7).Value = drAck(iRow).Item("NOMBRE_ESTABLECIMIENTO") 'NOMBRE_ESTABLECIMIENTO
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(8).Value = drAck(iRow).Item("CALLE_NUMERO") 'CALLEY NUMERO
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(9).Value = drAck(iRow).Item("COLONIA") 'COLONIA
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(10).Value = drAck(iRow).Item("CP") 'CP
                        If drAck(iRow).Item("ID_REGION") <> -1 Then
                            'desc_regioN
                            If VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", drAck(iRow).Item("ID_REGION"))) <> "" Then
                                m_Excel.Worksheet(1).Row(iRow + 2).Cell(11).Value = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", drAck(iRow).Item("ID_REGION")))
                            Else
                                m_Excel.Worksheet(1).Row(iRow + 2).Cell(11).Value = drAck(iRow).Item("ESTADO_MUNICIPIO")
                            End If
                        Else
                            'lo que venga de texto
                            m_Excel.Worksheet(1).Row(iRow + 2).Cell(11).Value = drAck(iRow).Item("ESTADO_MUNICIPIO")
                        End If
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(12).Value = drAck(iRow).Item("CIUDAD") 'CIUDAD_MPIO_ALCALDIA
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(13).Value = drAck(iRow).Item("TELEFONO") 'TELEFONO
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(14).Value = drAck(iRow).Item("GEO") 'COBERTURA_GEO


                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(1).Value = "S"
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(2).Value = drAck(iRow).Item("FILE")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(3).Value = strAgencia
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(4).Value = drAck(iRow).Item("BASE")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(5).Value = drAck(iRow).Item("BANCO")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(6).Value = drAck(iRow).Item("AFILIACION")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(7).Value = drAck(iRow).Item("NOMBRE_ESTABLECIMIENTO")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(8).Value = drAck(iRow).Item("CALLE_NUMERO")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(9).Value = drAck(iRow).Item("COLONIA")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(10).Value = drAck(iRow).Item("CP")
                        'If drAck(iRow).Item("ID_REGION") <> -1 Then
                        '    'desc_regioN
                        '    m_Excel.Worksheet(1).Row(iRow + 3).Cell(11).Value = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", drAck(iRow).Item("ID_REGION")))
                        'Else
                        '    'lo que venga de texto
                        '    m_Excel.Worksheet(1).Row(iRow + 3).Cell(11).Value = drAck(iRow).Item("ESTADO_MUNICIPIO")
                        'End If

                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(12).Value = drAck(iRow).Item("ALCALDIA")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(13).Value = drAck(iRow).Item("CIUDAD")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(14).Value = drAck(iRow).Item("TELEFONO")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(15).Value = drAck(iRow).Item("CANAL")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(16).Value = drAck(iRow).Item("COBERTURA")
                        'm_Excel.Worksheet(1).Row(iRow + 3).Cell(17).Value = drAck(iRow).Item("GEO")

                        ''m_Excel.Worksheet(1).Row(iRow + 3).Cell(20).SetDataValidation().List("1,3", True)
                        ''m_Excel.Worksheet(1).Row(iRow + 3).Cell(21).SetDataValidation().List("1,3", True)
                        ''m_Excel.Worksheet(1).Row(iRow + 3).Cell(22).SetDataValidation().List("1,3", True)
                        ''m_Excel.Worksheet(1).Row(iRow + 3).Cell(23).SetDataValidation().List("1,3", True)
                        ''m_Excel.Worksheet(1).Row(iRow + 3).Cell(24).SetDataValidation().List("1,3", True)
                        ''m_Excel.Worksheet(1).Row(iRow + 3).Cell(25).SetDataValidation().List("1,3", True)
                        ''m_Excel.Worksheet(1).Row(iRow + 3).Cell(26).SetDataValidation().List("1,3", True)

                    End If
                Next
                m_Excel.Save()
                Dim cAsignacion As New clsAsignacion
                cAsignacion.ID_ASIGNACION = -1
                cAsignacion.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
                cAsignacion.ID_AGENCIA = distinctDT.Rows(I).Item("ID_AGENCIA")
                cAsignacion.FLG_CANC = False
                cAsignacion.ID_ARCHIVO = idArchivo
                cAsignacion.ARCHIVO = FILE_2_BYTES(PathCopia)
                cAsignacion.NOMBRE_ARCHIVO = strAgencia & ".xlsx"
                cAsignacion.ARCHIVO_ACK = True
                cAsignacion.ROWS_OK = COUNTROWSOK
                cAsignacion.TIPO_BLITZ = ""
                cAsignacion.SAVE(cAsignacion)
                'If iRowNack > 0 Then
                '   
                'End If
            End If
        Next

        Dim pathNACK As String = Server.MapPath("~/tmpFile/" & idArchivo & "NOT_OK.xlsx")
        Dim baserutaNACK As String = Server.MapPath("~/tmpFile/plantilla_NACK.xlsx")
        If System.IO.File.Exists(pathNACK) Then
            File.Delete(pathNACK)
        End If
        If System.IO.File.Exists(baserutaNACK) Then
            File.Copy(baserutaNACK, pathNACK)
        End If

        Dim m_ExcelNACK As New XLWorkbook(pathNACK)

        Dim rdrNack As DataSet = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 0")
        Dim COUNTNACK As Integer = 0
        If rdrNack.Tables(0).Rows.Count - 1 > 0 Then
            For IrOWnACK As Integer = 0 To rdrNack.Tables(0).Rows.Count - 1
                COUNTNACK += 1

                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(1).Value = "" 'AGENCIA
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(2).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("FILE") 'ARCHIVO
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(3).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("PORTAFOLIO") 'PORTAFOLIO
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(4).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("PARTNER") 'PARTNER
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(5).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CANAL") 'CANAL
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(6).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("AFILIACION") 'AFILIACIÓN
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(7).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("NOMBRE_ESTABLECIMIENTO") 'NOMBRE_ESTABLECIMIENTO
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(8).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CALLE_NUMERO") 'CALLEY NUMERO
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(9).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("COLONIA") 'COLONIA
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(10).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CP") 'CP
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(11).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("ESTADO_MUNICIPIO")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(12).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CIUDAD") 'CIUDAD_MPIO_ALCALDIA
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(13).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("TELEFONO") 'TELEFONO
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(14).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("GEO") 'COBERTURA_GEO


                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(1).Value = ""
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(2).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("FILE")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(3).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("PORTAFOLIO")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(4).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("BASE")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(5).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("BANCO")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(6).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("AFILIACION")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(7).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("NOMBRE_ESTABLECIMIENTO")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(8).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CALLE_NUMERO")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(9).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("COLONIA")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(10).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CP")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(11).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("ESTADO_MUNICIPIO")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(12).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("ALCALDIA")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(13).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CIUDAD")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(14).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("TELEFONO")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(15).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CANAL")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(16).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("COBERTURA")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 3).Cell(17).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("GEO")
            Next
            m_ExcelNACK.Save()
            Dim cAsignacionNack As New clsAsignacion
            cAsignacionNack.ID_ASIGNACION = -1
            cAsignacionNack.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
            cAsignacionNack.ID_AGENCIA = -1
            cAsignacionNack.FLG_CANC = False
            cAsignacionNack.ID_ARCHIVO = idArchivo
            cAsignacionNack.ARCHIVO = FILE_2_BYTES(pathNACK)
            cAsignacionNack.NOMBRE_ARCHIVO = idArchivo & "NOT_OK.xlsx"
            cAsignacionNack.ARCHIVO_ACK = False
            cAsignacionNack.ROWS_NOT_OK = COUNTNACK
            cAsignacionNack.SAVE(cAsignacionNack)
        End If


        'CREA LOS FILES PARA LOS BLITZ DE ÉSTA ASIGNACIÓN BLITZ 50
        Dim RDRACK_50 As DataSet
        RDRACK_50 = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ50'")
        Dim baseruta_50 As String = Server.MapPath("~/tmpFile/plantilla_ack.xlsx")
        Dim PathCopia_50 As String = Server.MapPath("~/tmpFile/" & "BLITZ_50.xlsx")
        If System.IO.File.Exists(PathCopia_50) Then
            File.Delete(PathCopia_50)
        End If
        If System.IO.File.Exists(baseruta_50) Then
            File.Copy(baseruta_50, PathCopia_50)
        End If
        Dim m_Excel_50 As New XLWorkbook(PathCopia_50)
        Dim iCount50 As Integer
        For iCount50 = 0 To RDRACK_50.Tables(0).Rows.Count - 1
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(1).Value = ""
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(2).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("FILE") 'ARCHIVO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(3).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("PORTAFOLIO") 'PORTAFOLIO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(4).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("PARTNER") 'PARTNER
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(5).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("CANAL") 'CANAL
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(6).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("AFILIACION") 'AFILIACIÓN
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(7).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("NOMBRE_ESTABLECIMIENTO") 'NOMBRE_ESTABLECIMIENTO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(8).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("CALLE_NUMERO") 'CALLEY NUMERO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(9).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("COLONIA") 'COLONIA
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(10).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("CP") 'CP
            If RDRACK_50.Tables(0).Rows(iCount50).Item("ID_REGION") <> -1 Then
                'desc_regioN
                If VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_50.Tables(0).Rows(iCount50).Item("ID_REGION"))) <> "" Then
                    m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(11).Value = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_50.Tables(0).Rows(iCount50).Item("ID_REGION")))
                Else
                    m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(11).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("ESTADO_MUNICIPIO")
                End If
            Else
                'lo que venga de texto
                m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(11).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("ESTADO_MUNICIPIO")
            End If
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(12).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("CIUDAD") 'CIUDAD_MPIO_ALCALDIA
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(13).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("TELEFONO") 'TELEFONO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(14).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("GEO") 'COBERTURA_GEO
        Next
        m_Excel_50.Save()
        Dim cAsignacion_50 As New clsAsignacion
        cAsignacion_50.ID_ASIGNACION = -1
        cAsignacion_50.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
        cAsignacion_50.ID_AGENCIA = -1 'distinctDT.Rows(I).Item("ID_AGENCIA")  
        cAsignacion_50.FLG_CANC = False
        cAsignacion_50.ID_ARCHIVO = idArchivo
        cAsignacion_50.ARCHIVO = FILE_2_BYTES(PathCopia_50)
        cAsignacion_50.NOMBRE_ARCHIVO = "BLITZ_50.xlsx"
        cAsignacion_50.ARCHIVO_ACK = True
        cAsignacion_50.ROWS_OK = iCount50
        cAsignacion_50.TIPO_BLITZ = "BLITZ50"
        cAsignacion_50.SAVE(cAsignacion_50)



        'CREA LOS FILES PARA LOS BLITZ DE ÉSTA ASIGNACIÓN BLITZ 100
        Dim RDRACK_100 As DataSet
        RDRACK_100 = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ100'")
        Dim baseruta_100 As String = Server.MapPath("~/tmpFile/plantilla_ack.xlsx")
        Dim PathCopia_100 As String = Server.MapPath("~/tmpFile/" & "BLITZ_100.xlsx")
        If System.IO.File.Exists(PathCopia_100) Then
            File.Delete(PathCopia_100)
        End If
        If System.IO.File.Exists(baseruta_100) Then
            File.Copy(baseruta_100, PathCopia_100)
        End If
        Dim m_Excel_100 As New XLWorkbook(PathCopia_100)
        Dim iCount_100 As Integer
        For iCount_100 = 0 To RDRACK_100.Tables(0).Rows.Count - 1
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(1).Value = ""
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(2).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("FILE") 'ARCHIVO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(3).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("PORTAFOLIO") 'PORTAFOLIO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(4).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("PARTNER") 'PARTNER
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(5).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("CANAL") 'CANAL
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(6).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("AFILIACION") 'AFILIACIÓN
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(7).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("NOMBRE_ESTABLECIMIENTO") 'NOMBRE_ESTABLECIMIENTO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(8).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("CALLE_NUMERO") 'CALLEY NUMERO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(9).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("COLONIA") 'COLONIA
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(10).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("CP") 'CP
            If RDRACK_100.Tables(0).Rows(iCount_100).Item("ID_REGION") <> -1 Then
                'desc_regioN
                If VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_100.Tables(0).Rows(iCount_100).Item("ID_REGION"))) <> "" Then
                    m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(11).Value = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_100.Tables(0).Rows(iCount_100).Item("ID_REGION")))
                Else
                    m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(11).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("ESTADO_MUNICIPIO")
                End If
            Else
                'lo que venga de texto
                m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(11).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("ESTADO_MUNICIPIO")
            End If
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(12).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("CIUDAD") 'CIUDAD_MPIO_ALCALDIA
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(13).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("TELEFONO") 'TELEFONO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(14).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("GEO") 'COBERTURA_GEO
        Next
        m_Excel_100.Save()
        Dim cAsignacion_100 As New clsAsignacion
        cAsignacion_100.ID_ASIGNACION = -1
        cAsignacion_100.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
        cAsignacion_100.ID_AGENCIA = -1 'distinctDT.Rows(I).Item("ID_AGENCIA")  
        cAsignacion_100.FLG_CANC = False
        cAsignacion_100.ID_ARCHIVO = idArchivo
        cAsignacion_100.ARCHIVO = FILE_2_BYTES(PathCopia_100)
        cAsignacion_100.NOMBRE_ARCHIVO = "BLITZ_100.xlsx"
        cAsignacion_100.ARCHIVO_ACK = True
        cAsignacion_100.ROWS_OK = iCount_100
        cAsignacion_100.TIPO_BLITZ = "BLITZ100"
        cAsignacion_100.SAVE(cAsignacion_100)



        'CREA LOS FILES PARA LOS BLITZ DE ÉSTA ASIGNACIÓN BLITZ 300
        Dim RDRACK_300 As DataSet
        RDRACK_300 = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ300'")
        Dim baseruta_300 As String = Server.MapPath("~/tmpFile/plantilla_ack.xlsx")
        Dim PathCopia_300 As String = Server.MapPath("~/tmpFile/" & "BLITZ_300.xlsx")
        If System.IO.File.Exists(PathCopia_300) Then
            File.Delete(PathCopia_300)
        End If
        If System.IO.File.Exists(baseruta_300) Then
            File.Copy(baseruta_300, PathCopia_300)
        End If
        Dim m_Excel_300 As New XLWorkbook(PathCopia_300)
        Dim iCount_300 As Integer
        For iCount_300 = 0 To RDRACK_300.Tables(0).Rows.Count - 1
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(1).Value = ""
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(2).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("FILE") 'ARCHIVO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(3).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("PORTAFOLIO") 'PORTAFOLIO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(4).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("PARTNER") 'PARTNER
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(5).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("CANAL") 'CANAL
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(6).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("AFILIACION") 'AFILIACIÓN
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(7).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("NOMBRE_ESTABLECIMIENTO") 'NOMBRE_ESTABLECIMIENTO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(8).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("CALLE_NUMERO") 'CALLEY NUMERO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(9).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("COLONIA") 'COLONIA
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(10).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("CP") 'CP
            If RDRACK_300.Tables(0).Rows(iCount_300).Item("ID_REGION") <> -1 Then
                'desc_regioN
                If VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_300.Tables(0).Rows(iCount_300).Item("ID_REGION"))) <> "" Then
                    m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(11).Value = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_300.Tables(0).Rows(iCount_300).Item("ID_REGION")))
                Else
                    m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(11).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("ESTADO_MUNICIPIO")
                End If
            Else
                'lo que venga de texto
                m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(11).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("ESTADO_MUNICIPIO")
            End If
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(12).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("CIUDAD") 'CIUDAD_MPIO_ALCALDIA
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(13).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("TELEFONO") 'TELEFONO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(14).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("GEO") 'COBERTURA_GEO
        Next
        m_Excel_300.Save()
        Dim cAsignacion_300 As New clsAsignacion
        cAsignacion_300.ID_ASIGNACION = -1
        cAsignacion_300.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
        cAsignacion_300.ID_AGENCIA = -1 'distinctDT.Rows(I).Item("ID_AGENCIA")  
        cAsignacion_300.FLG_CANC = False
        cAsignacion_300.ID_ARCHIVO = idArchivo
        cAsignacion_300.ARCHIVO = FILE_2_BYTES(PathCopia_300)
        cAsignacion_300.NOMBRE_ARCHIVO = "BLITZ_300.xlsx"
        cAsignacion_300.ARCHIVO_ACK = True
        cAsignacion_300.ROWS_OK = iCount_300
        cAsignacion_300.TIPO_BLITZ = "BLITZ300"
        cAsignacion_300.SAVE(cAsignacion_300)


        CIERRA_DATASET(RDRACK)
        CIERRA_DATASET(RDRACK_50)
        CIERRA_DATASET(RDRACK_100)
        CIERRA_DATASET(RDRACK_300)

    End Sub

    Private Sub CUENTA_NACK(ByVal IdArchivo As Integer)
        Dim RDRACK As DataSet
        RDRACK = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & IdArchivo)

        Dim distinctDT As DataTable = RDRACK.Tables(0).DefaultView.ToTable(True, "ID_AGENCIA")
        Dim drAck() As DataRow
        'CREA EL REGISTRO DE ASIGNACIÓN

        For I As Integer = 0 To distinctDT.Rows.Count - 1




            drAck = RDRACK.Tables(0).Select("ID_ARCHIVO = " & IdArchivo & " AND ID_AGENCIA = " & distinctDT.Rows(I).Item("ID_AGENCIA"))
            If drAck.Length > 0 Then
                Dim baseruta As String = Server.MapPath("~/tmpFile/plantilla_ack.xlsx")
                Dim strAgencia As String = VALORINTABLA("DESC_AGENCIA", "TAB_AGENCIA", "ID_AGENCIA", distinctDT.Rows(I).Item("ID_AGENCIA"))
                Dim PathCopia As String = Server.MapPath("~/tmpFile/" & strAgencia.Replace(" ", "_") & ".xlsx")
                If System.IO.File.Exists(PathCopia) Then
                    File.Delete(PathCopia)
                End If
                If System.IO.File.Exists(baseruta) Then
                    File.Copy(baseruta, PathCopia)
                End If

                Dim m_Excel As New XLWorkbook(PathCopia)
                For iRow As Integer = 0 To drAck.Length - 1
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(1).Value = "S"
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(2).Value = "QUÉ VA AQUÍ?"
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(3).Value = strAgencia
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(4).Value = drAck(iRow).Item("BASE")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(5).Value = drAck(iRow).Item("BANCO")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(6).Value = drAck(iRow).Item("SE_NUMBER")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(7).Value = drAck(iRow).Item("SELLER_ID_LEGAL_NAME")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(8).Value = drAck(iRow).Item("ADDRESS_LINE1")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(9).Value = drAck(iRow).Item("ADDRESS_LINE2")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(10).Value = drAck(iRow).Item("CP")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(11).Value = drAck(iRow).Item("ID_MUNICIPIO")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(12).Value = drAck(iRow).Item("ID_CIUDAD")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(13).Value = drAck(iRow).Item("SELLER_BUSINESS_HOME_NUMBER")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(14).Value = "QUÉ VA AQUÍ?"
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(15).Value = drAck(iRow).Item("COBERTURA")
                    m_Excel.Worksheet(1).Row(iRow + 3).Cell(16).Value = "1"


                Next
                m_Excel.Save()
                Dim cAsignacion As New clsAsignacion
                cAsignacion.ID_ASIGNACION = -1
                cAsignacion.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
                cAsignacion.ID_AGENCIA = distinctDT.Rows(I).Item("ID_AGENCIA")
                cAsignacion.FLG_CANC = False
                cAsignacion.ID_ARCHIVO = IdArchivo
                cAsignacion.ARCHIVO = FILE_2_BYTES(PathCopia)
                cAsignacion.NOMBRE_ARCHIVO = strAgencia & ".xlsx"
                cAsignacion.SAVE(cAsignacion)
            End If
        Next
        CIERRA_DATASET(RDRACK)
    End Sub
    Private Sub CREA_FILES_AGENCIA_NACK()

    End Sub

    Private Sub grdFiles_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdFiles.RowCommand
        Dim sIdAsignacion As String = ""
        sIdAsignacion = grdFiles.DataKeys(e.CommandArgument.ToString).Value.ToString

        Select Case e.CommandName.ToString
            Case "cmdMail"
                'envía mail al contacto en la agencia correspondiente
                Dim RES As String = SEND_MAIL(sIdAsignacion)
                If RES <> "" Then
                    msg(RES)
                End If
            Case "cmdDownload"
                DOWNLOAD_FILE(sIdAsignacion)


        End Select
    End Sub


    '    [1833, 5/3/2020] Fina: entonces la regla queda:
    '[1833, 5/3/2020] Fina: buscar ( código postal)
    '[18:33, 5/3/2020] Fina: If found -> asignar a su agencia (agregar al archivo de su agencia)
    '[18:34, 5/3/2020] Fina: If Not found -> {
    '[18:35, 5/3/2020] Fina: If estado = geo 1 -> save For blitz (50) x municipio
    '[18:35, 5/3/2020] Fina: If estado = geo 2 -> save For blitz (100) x municipio
    '[18:35, 5/3/2020] Fina: }
    '[18:35, 5/3/2020] Fina: Else save For blitz (300) x municipio



    Private Sub DOWNLOAD_FILE(idAsignacion)
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

    Private Sub grdNAKFile_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdNAKFile.RowCommand
        Dim sIdAsignacion As String = ""
        sIdAsignacion = grdNAKFile.DataKeys(e.CommandArgument.ToString).Value.ToString
        DOWNLOAD_FILE(sIdAsignacion)
    End Sub
    Private Sub testrows()





        Dim baseruta As String = Server.MapPath("~/tmpFile/plantilla_ack.xlsx")

        Dim PathCopia As String = Server.MapPath("~/tmpFile/test.xlsx")
        If System.IO.File.Exists(PathCopia) Then
            File.Delete(PathCopia)
        End If
        If System.IO.File.Exists(baseruta) Then
            File.Copy(baseruta, PathCopia)
        End If


        Dim m_Excel As New XLWorkbook(PathCopia)
        Dim COUNTROWSOK As Integer = 0

        For iRow As Integer = 0 To 10
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(1).Value = "1"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(2).Value = "2"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(3).Value = "3"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(4).Value = "4"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(5).Value = "5"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(6).Value = "6"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(7).Value = "7"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(8).Value = "8"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(9).Value = "9"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(10).Value = "10"
            m_Excel.Worksheet(1).Row(iRow + 3).Cell(11).SetDataValidation().List(m_Excel.Worksheet(2).Range("A2:A4"), True)
            'm_Excel.Worksheet(1).Row(iRow + 3).Cell(12).SetDataValidation().List("prueba 1, prueba 2, prueba 3", True)
            m_Excel.Worksheet(2).Hide()
        Next
        m_Excel.Save()
        msg("finalizado")

    End Sub

    Private Sub grdBlitz_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdBlitz.RowCommand
        Dim sIdAsignacion As String = ""
        sIdAsignacion = grdBlitz.DataKeys(e.CommandArgument.ToString).Value.ToString

        Select Case e.CommandName.ToString
            Case "cmdMail"
                'envía mail al contacto en la agencia correspondiente
                Dim RES As String = SEND_MAIL(sIdAsignacion)
                If RES <> "" Then
                    msg(RES)
                End If
            Case "cmdDownload"
                DOWNLOAD_FILE(sIdAsignacion)


        End Select
    End Sub

    'Protected Sub cmdIniciaProces0_Click(sender As Object, e As EventArgs) Handles cmdIniciaProces0.Click
    '    testrows()

    'End Sub
End Class