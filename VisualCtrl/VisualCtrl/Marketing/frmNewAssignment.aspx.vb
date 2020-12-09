Imports System.IO
Imports System.Net.Mail
Imports ClosedXML.Excel
Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss
Imports Newtonsoft.Json
Imports System.Net
Imports System.Text
Imports System.Configuration.ConfigurationManager

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

            CARGAR_COMBO(dropCampaigns, New SqlCommand("SELECT * FROM TAB_CAMPAIGNS"), "ID_CAMPAIGN", "", "NOMBRE_CAMPAIGN", True, False)
        End If

    End Sub

    Private Function validaEncabezado(encabezado As IXLRow) As Boolean
        Dim baseplantilla As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/plantilla_ack.xlsx"), False)
        Dim plantilla As New XLWorkbook(baseplantilla)
        Dim header As IXLRow = plantilla.Worksheet(1).Row(1)
        Dim res As Boolean = True
        Dim indice As Integer
        For indice = 1 To 36
            If encabezado.Cell(indice).Equals(header.Cell(indice)) Then
                res = False
            End If
        Next
        Return res
    End Function



    Protected Sub cmdIniciaProces_Click(sender As Object, e As EventArgs) Handles cmdIniciaProces.Click 'version FINA
        Dim resString As String = "nothing"

        If IsNothing(lbresult.Items) Then
            lbresult.Items.Clear()
        End If
        If fu.HasFile And validaInjection(fu.FileName) And dropCampaigns.SelectedIndex <> 0 Then
            Dim idcamapaña As Integer = Integer.Parse(dropCampaigns.SelectedValue)
            Dim CLSfILE As New clsArchivo
            Dim FILEPATH As String = ""

            FILEPATH = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/" & fu.FileName.Replace(" ", "_")), False)

            CLSfILE.ID_ARCHIVO = -1
            CLSfILE.FH_CARGA = FORMATEAR_FECHA(System.DateTime.Now, "C")
            CLSfILE.NOMBRE_ARCHIVO = fu.FileName
            CLSfILE.FLG_MARKETING = True
            CLSfILE.FLG_AGENCIA = False
            CLSfILE.ARCHIVO = FILE_2_BYTES(FILEPATH)
            CLSfILE.ID_CAMPAIGN = dropCampaigns.SelectedItem.Value
            CLSfILE.SAVE(CLSfILE)

            If File.Exists(FILEPATH) Then
                File.Delete(FILEPATH)
            End If
            fu.PostedFile.SaveAs(FILEPATH)
            Dim m_Excel As New XLWorkbook(FILEPATH)

            'LLAMAR API REST PARA ENCOLAR EL TRABAJO
            Dim webClient As New WebClient()
            Dim resByte As Byte()

            Dim reqString() As Byte
            Dim dictData As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            dictData.Add("id_archivo", CLSfILE.ID_ARCHIVO)
            dictData.Add("total_lineas", m_Excel.Worksheets(0).LastRowUsed().RowNumber() - 1)
            dictData.Add("lineas_procesadas", 0)

            webClient.Headers("content-type") = "application/json"
            reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented))
            resByte = webClient.UploadData(AppSettings("APIURL") + "/TAB_TAREA/", "post", reqString)
            'resByte = webClient.DownloadData(AppSettings("APIURL") + "/values/")

            resString = Encoding.Default.GetString(resByte)
            Console.WriteLine(resString)
            webClient.Dispose()



            'terminar y controlar todo en la página


            Dim sel As String
            Dim tabs As String
            Dim cond As String

            'Dim rdrAgencia As DataSet = dsOpenDB("Select * FROM TAB_AGENCIA nolock WHERE FLG_CANC = 0")
            Dim rdrAgencia As DataSet = dsOpenDB(New SqlCommand("Select * FROM TAB_AGENCIA nolock WHERE FLG_CANC = 0"))
            'Dim rdrCP As DataSet = dsOpenDB("Select * FROM TAB_REGION nolock WHERE FLG_CANC = 0 ")
            Dim rdrCP As DataSet = dsOpenDB(New SqlCommand("Select * FROM TAB_REGION nolock WHERE FLG_CANC = 0 "))

            'Dim rdrEstados As DataSet = dsOpenDB("Select * FROM TAB_ESTADOS nolock WHERE FLG_CANC = 0 ")
            Dim rdrEstados As DataSet = dsOpenDB(New SqlCommand("Select * FROM TAB_ESTADOS nolock WHERE FLG_CANC = 0 "))

            'Dim rdrCPEstado As DataSet = dsOpenDB("Select * FROM TAB_CP INNER JOIN TAB_ESTADOS On TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO WHERE TAB_ESTADOS.GEO_DEFAULT <> 0")
            Dim rdrCPEstado As DataSet = dsOpenDB(New SqlCommand("Select * FROM TAB_CP INNER JOIN TAB_ESTADOS On TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO WHERE TAB_ESTADOS.GEO_DEFAULT <> 0"))

            'Dim rdrAlcaldia As DataSet = dsOpenDB("Select  TAB_REGION.ID_ESTADO, DESC_REGION, CP, FLG_PRIORITARIA, ID_REGION, TAB_REGION.FLG_CANC, GEO, TAB_REGION.ID_AGENCIA, DESC_ESTADO, TAB_ESTADOS.FLG_CANC, GEO_DEFAULT from tab_region inner join tab_estados On tab_region.id_estado = TAB_ESTADOS.ID_ESTADO where tab_region.FLG_CANC = 0") 'CAMBIO
            Dim rdrAlcaldia As DataSet = dsOpenDB(New SqlCommand("Select  TAB_REGION.ID_ESTADO, DESC_REGION, CP, FLG_PRIORITARIA, ID_REGION, TAB_REGION.FLG_CANC, GEO, TAB_REGION.ID_AGENCIA, DESC_ESTADO, TAB_ESTADOS.FLG_CANC, GEO_DEFAULT from tab_region inner join tab_estados On tab_region.id_estado = TAB_ESTADOS.ID_ESTADO where tab_region.FLG_CANC = 0")) 'CAMBIO

            'Dim rdrEveryCP As DataSet = dsOpenDB("Select * from TAB_CP nolock")
            Dim rdrEveryCP As DataSet = dsOpenDB(New SqlCommand("Select * from TAB_CP nolock"))


            Dim dRowAgencia() As DataRow
            Dim dRowBlitz() As DataRow
            Dim rowCom() As DataRow
            Dim dRowAlcaldia() As DataRow
            'Dim rdrRel As DataTable = dsOpenDB("Select * FROM TAB_ESTADOS With(nolock) inner join TAB_CP With(nolock) On TAB_ESTADOS.Id_estado_original = TAB_CP.ID_ESTADO").Tables(0)
            Dim rdrRel As DataTable = dsOpenDB(New SqlCommand("Select * FROM TAB_ESTADOS With(nolock) inner join TAB_CP With(nolock) On TAB_ESTADOS.Id_estado_original = TAB_CP.ID_ESTADO")).Tables(0)


            'Dim lastRow As Integer = Integer.Parse(m_Excel.Worksheets(0).LastRowUsed().RowNumber())

            'Info para agregar blitz a asignación
            Dim rdrAgenciaPorAlcaldia As DataSet
            Dim rowBlitzForzado As DataRow()
            If chkBlitz1.Checked Or chkBlitz2.Checked Or chkBlitz3.Checked Then
                'rdrAgenciaPorAlcaldia = dsOpenDB("Select * FROM TAB_ALCALDIA")
                rdrAgenciaPorAlcaldia = dsOpenDB(New SqlCommand("Select * FROM TAB_ALCALDIA (NOLOCK)"))
            End If

            If validaEncabezado(m_Excel.Worksheet(1).Row(1)) Then

                Dim iRow As Integer
                Dim iAck As Integer = 0
                Dim iGeo1 As Integer = 0
                Dim iGeo2 As Integer = 0
                Dim iNack As Integer = 0
                Dim iBlitz50 As Integer = 0
                Dim iBlitz100 As Integer = 0
                Dim iBlitz300 As Integer = 0
                Dim Prioridad As Integer = 0 'CAMBIO
                Dim agencia_por_alcaldia As Integer = 0 'CAMBIO
                Dim alcaldiatemporal As String

                Dim conTemp As SqlConnection = ConnProcesoLargo()

                Dim cpestado As Boolean
                Dim rowcpestado() As DataRow
                Dim cptemp As String

                Session("total") = m_Excel.Worksheets(0).LastRowUsed().RowNumber()
                For iRow = 1 To m_Excel.Worksheets(0).LastRowUsed().RowNumber() - 1
                    Session("current") = iRow
                    'Debug.Print(iRow)
                    'verifico solo còdigos postales
                    Dim agenciaaux As Integer
                    Dim rowaux() As DataRow
                    Dim stringaux As String

                    If m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString.Length = 5 Then
                        cptemp = m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value
                    ElseIf m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString.Length = 4 Then
                        cptemp = "0" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value
                    End If




                    rowcpestado = rdrRel.Select("CP = '" + cptemp + "' and DESC_ESTADO = '" + m_Excel.Worksheet(1).Row(iRow + 1).Cell(15).Value + "'")

                    cpestado = If(rowcpestado.Length > 0, True, False)

                    Dim renglon As Boolean = validaRenglon(m_Excel.Worksheet(1).Row(iRow + 1))

                    Dim region As Boolean = If(m_Excel.Worksheet(1).Row(iRow + 1).Cell(16).Value.ToString = "", False, True)

                    If renglon And cpestado And region Then
                        '    [1833, 5/3/2020] Fina: entonces la regla queda:
                        '[1833, 5/3/2020] Fina: buscar ( código postal)
                        '[18:33, 5/3/2020]   Fina: If found -> asignar a su agencia (agregar al archivo de su agencia)
                        '[18:34, 5/3/2020] Fina: If Not found -> {
                        '[18:35, 5/3/2020] Fina: If estado = geo 1 -> save For blitz (50) x municipio
                        '[18:35, 5/3/2020] Fina: If estado = geo 2 -> save For blitz (100) x municipio
                        '[18:35, 5/3/2020] Fina: }
                        '[18:35, 5/3/2020] Fina: Else save For blitz (300) x municipio

                        dRowAgencia = rdrCP.Tables(0).Select("CP = '" & cptemp & "'")


                        If dRowAgencia.Length > 0 Then
                            Select Case dRowAgencia(0).Item("GEO")
                                Case 1
                                    If chkGeo1.Checked Then
                                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, dRowAgencia(0).Item("ID_AGENCIA"), dRowAgencia(0).Item("ID_REGION"), dRowAgencia(0).Item("GEO"), True, idcamapaña, "", "", conTemp)
                                        iGeo1 += 1
                                    Else
                                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -10, dRowAgencia(0).Item("ID_REGION"), dRowAgencia(0).Item("GEO"), True, idcamapaña, "GEO 1", "", conTemp)
                                        iGeo1 += 1
                                    End If
                                Case 2
                                    If chkGeo2.Checked Then
                                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, dRowAgencia(0).Item("ID_AGENCIA"), dRowAgencia(0).Item("ID_REGION"), dRowAgencia(0).Item("GEO"), True, idcamapaña, "", "", conTemp)
                                        iGeo2 += 1
                                    Else
                                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -20, dRowAgencia(0).Item("ID_REGION"), dRowAgencia(0).Item("GEO"), True, idcamapaña, "GEO 2", "", conTemp)
                                        iGeo2 += 1
                                    End If
                                Case Else
                                    addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, dRowAgencia(0).Item("ID_AGENCIA"), dRowAgencia(0).Item("ID_REGION"), dRowAgencia(0).Item("GEO"), True, idcamapaña, "", "", conTemp)
                                    iAck += 1
                            End Select
                        Else
                            'busca el cp dentro de la tabla de códigos postales y de ahi lo relaciona con su geo del estado
                            'dRowBlitz = rdrCP.Tables(0).Select("DESC_REGION = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value & "'")
                            If m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString.Length = 5 Then
                                dRowBlitz = rdrCPEstado.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value & "'")
                            ElseIf m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString.Length = 4 Then
                                dRowBlitz = rdrCPEstado.Tables(0).Select("CP = '0" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value & "'")
                            End If

                            If dRowBlitz.Length > 0 Then
                                Select Case dRowBlitz(0).Item("GEO_DEFAULT")
                                    Case 1
                                        If Not chkBlitz1.Checked Then
                                            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -1, True, idcamapaña, "BLITZ50", "", conTemp)
                                        Else
                                            'METER BLITZ A LA ASIGNACION
                                            'Revisar si NO es CDMX
                                            agenciaaux = dRowBlitz(0).Item("ID_AGENCIA")
                                            If agenciaaux < 0 Then
                                                rowaux = rdrEveryCP.Tables(0).Select("CP = " & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString)
                                                alcaldiatemporal = rowaux(0).Item("DESC_REGION")
                                                rowaux = rdrAgenciaPorAlcaldia.Tables(0).Select("[NAME] = '" & alcaldiatemporal & "'")
                                                agenciaaux = rowaux(0).Item("ID_AGENCIA")
                                            End If
                                            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, agenciaaux, -1, dRowBlitz(0).Item("GEO_DEFAULT"), True, idcamapaña, "INCL_BGEO1", "", conTemp)
                                        End If
                                        iBlitz50 += 1
                                    Case 2
                                        If Not chkBlitz2.Checked Then
                                            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -2, True, idcamapaña, "BLITZ100", "", conTemp)
                                        Else
                                            'METER BLITZ A LA ASIGNACION
                                            'Revisar si NO es CDMX
                                            agenciaaux = dRowBlitz(0).Item("ID_AGENCIA")
                                            If agenciaaux < 0 Then
                                                rowaux = rdrEveryCP.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString)
                                                alcaldiatemporal = rowaux(0).Item("DESC_REGION")
                                                rowaux = rdrAgenciaPorAlcaldia.Tables(0).Select("[NAME] = '" & alcaldiatemporal & "'")
                                                agenciaaux = rowaux(0).Item("ID_AGENCIA")
                                            End If
                                            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, agenciaaux, -1, dRowBlitz(0).Item("GEO_DEFAULT"), True, idcamapaña, "INCL_BGEO2", "", conTemp)
                                        End If
                                        iBlitz100 += 1
                                    Case Else
                                        If Not chkBlitz3.Checked Then
                                            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -3, True, idcamapaña, "BLITZ300", "", conTemp)
                                        Else
                                            'METER BLITZ A LA ASIGNACION
                                            'Revisar si NO es CDMX
                                            agenciaaux = dRowBlitz(0).Item("ID_AGENCIA")
                                            If agenciaaux < 0 Then
                                                rowaux = rdrEveryCP.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString & "'")
                                                alcaldiatemporal = rowaux(0).Item("DESC_REGION")
                                                rowaux = rdrAgenciaPorAlcaldia.Tables(0).Select("[NAME] = '" & alcaldiatemporal & "'")
                                                agenciaaux = rowaux(0).Item("ID_AGENCIA")
                                            End If
                                            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, agenciaaux, -1, dRowBlitz(0).Item("GEO_DEFAULT"), True, idcamapaña, "INCL_BGEO3", "", conTemp)
                                        End If
                                        iBlitz300 += 1

                                End Select
                                'iAck += 1
                            Else
                                '    rowCom = rdrEstados.Tables(0).Select("DESC_ESTADO = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(10).Value & "'")
                                '    If rowCom.Length > 0 Then
                                m_Excel.Worksheet(1).Row(iRow + 1).Cell(18).Value = "No se puede asignar"
                                addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 3, True, idcamapaña, "BLITZ300", "", conTemp)
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
                        Dim rechazo As String = "Problemas: "
                        If m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString = "" Then
                            rechazo += "Falta CP. "
                        End If
                        If m_Excel.Worksheet(1).Row(iRow + 1).Cell(15).Value.ToString = "" Then
                            rechazo += "Falta Estado. "
                        End If
                        If m_Excel.Worksheet(1).Row(iRow + 1).Cell(12).Value.ToString = "" Then
                            rechazo += "Falta Dirección. "
                        End If
                        If Not region Then
                            rechazo += "Falta Ciudad/Alcaldía/Municipio. "
                        End If
                        If (rdrEveryCP.Tables(0).Select("CP = '" + m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString + "'")).Length < 1 Then
                            rechazo += "CP no existe. "
                        End If
                        If (rdrEstados.Tables(0).Select("DESC_ESTADO = '" + m_Excel.Worksheet(1).Row(iRow + 1).Cell(15).Value.ToString + "'")).Length < 1 Then
                            rechazo += "Estado no existe. "
                        End If
                        If Not cpestado Then
                            rechazo += "CP y Estado no corresponden"
                        End If
                        m_Excel.Worksheet(1).Row(iRow + 1).Cell(19).Value = rechazo
                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -837, False, idcamapaña, "", "", conTemp)
                        iNack += 1
                    End If
                    'If m_Excel.Worksheet(1).Row(iRow + 1).Cell(15).Value.ToString.Length > 0 And m_Excel.Worksheet(1).Row(iRow + 1).Cell(16).Value.ToString.Length > 0 Then
                    '    'CAMBIO busca agencia pero usando estado y municipio/alcaldía/ciudad

                    '    'Dim tmpEstado As String = m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value.ToString.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                    '    'tmpEstado = tmpEstado.Replace("Á", "a").Replace("É", "e").Replace("Í", "i").Replace("Ó", "o").Replace("Ú", "u")
                    '    'tmpEstado = tmpEstado.ToLower()
                    '    'Dim tmpAlcaldia As String = m_Excel.Worksheet(1).Row(iRow + 1).Cell(12).Value.ToString.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                    '    'tmpAlcaldia = tmpAlcaldia.Replace("Á", "a").Replace("É", "e").Replace("Í", "i").Replace("Ó", "o").Replace("Ú", "u")
                    '    'tmpAlcaldia = tmpAlcaldia.ToLower()
                    '    'rdrAlcaldia.CaseSensitive = False
                    '    Dim tmpEstado As String = m_Excel.Worksheet(1).Row(iRow + 1).Cell(15).Value.ToString
                    '    Dim tmpAlcaldia As String = m_Excel.Worksheet(1).Row(iRow + 1).Cell(16).Value.ToString
                    '    If tmpEstado = "Querétaro" Then
                    '        tmpEstado = "Santiago de Querétaro"
                    '    End If
                    '    dRowAlcaldia = rdrAlcaldia.Tables(0).Select("DESC_REGION = '" & tmpAlcaldia & "' AND  DESC_ESTADO = '" & tmpEstado & "'")

                    '    If dRowAlcaldia.Length > 0 Then
                    '        'Si tiene Estado y Ciudad/Alcaldía/Municipio bien, se mete a la asignación. Si falta alguno, se mete a blitz
                    '        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, dRowAlcaldia(0).Item("ID_AGENCIA"), dRowAlcaldia(0).Item("ID_REGION"), dRowAlcaldia(0).Item("GEO"), True, idcamapaña, "", "", conTemp)
                    '    Else
                    '        tmpEstado = m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value.ToString
                    '        dRowAlcaldia = rdrAlcaldia.Tables(0).Select("DESC_ESTADO = '" & tmpEstado & "'")
                    '        If dRowAlcaldia.Length > 0 Then
                    '            Select Case dRowAlcaldia(0).Item("GEO")
                    '                Case 1
                    '                    'addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 1, True, "BLITZ50", idcamapaña, "", conTemp)
                    '                    If Not chkBlitz1.Checked Then
                    '                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -1, True, idcamapaña, "BLITZ50", "", conTemp)
                    '                    Else
                    '                        'METER BLITZ A LA ASIGNACION
                    '                        'Revisar si NO es CDMX
                    '                        agenciaaux = dRowBlitz(0).Item("ID_AGENCIA")
                    '                        If agenciaaux < 0 Then
                    '                            rowaux = rdrEveryCP.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString & "'")
                    '                            alcaldiatemporal = rowaux(0).Item("DESC_REGION")
                    '                            rowaux = rdrAgenciaPorAlcaldia.Tables(0).Select("[NAME] = '" & alcaldiatemporal & "'")
                    '                            agenciaaux = rowaux(0).Item("ID_AGENCIA")
                    '                        End If
                    '                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, agenciaaux, -1, dRowBlitz(0).Item("GEO_DEFAULT"), True, idcamapaña, "INCL_BGEO1", "", conTemp)
                    '                    End If
                    '                    iBlitz50 += 1
                    '                Case 2
                    '                    'addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 2, True, "BLITZ100", idcamapaña, "", conTemp)
                    '                    If Not chkBlitz2.Checked Then
                    '                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -2, True, idcamapaña, "BLITZ100", "", conTemp)
                    '                    Else
                    '                        'METER BLITZ A LA ASIGNACION
                    '                        'Revisar si NO es CDMX
                    '                        agenciaaux = dRowBlitz(0).Item("ID_AGENCIA")
                    '                        If agenciaaux < 0 Then
                    '                            rowaux = rdrEveryCP.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString & "'")
                    '                            alcaldiatemporal = rowaux(0).Item("DESC_REGION")
                    '                            rowaux = rdrAgenciaPorAlcaldia.Tables(0).Select("[NAME] = '" & alcaldiatemporal & "'")
                    '                            agenciaaux = rowaux(0).Item("ID_AGENCIA")
                    '                        End If
                    '                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, agenciaaux, -1, dRowBlitz(0).Item("GEO_DEFAULT"), True, idcamapaña, "INCL_BGEO2", "", conTemp)
                    '                    End If
                    '                    iBlitz100 += 1
                    '                Case Else
                    '                    'addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, 3, True, "BLITZ300", idcamapaña, "", conTemp)
                    '                    If Not chkBlitz3.Checked Then
                    '                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -3, True, idcamapaña, "BLITZ300", "", conTemp)
                    '                    Else
                    '                        'METER BLITZ A LA ASIGNACION
                    '                        'Revisar si NO es CDMX
                    '                        agenciaaux = dRowBlitz(0).Item("ID_AGENCIA")
                    '                        If agenciaaux < 0 Then
                    '                            rowaux = rdrEveryCP.Tables(0).Select("CP = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(14).Value.ToString & "'")
                    '                            alcaldiatemporal = rowaux(0).Item("DESC_REGION")
                    '                            rowaux = rdrAgenciaPorAlcaldia.Tables(0).Select("[NAME] = '" & alcaldiatemporal & "'")
                    '                            agenciaaux = rowaux(0).Item("ID_AGENCIA")
                    '                        End If
                    '                        addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, agenciaaux, -1, dRowBlitz(0).Item("GEO_DEFAULT"), True, idcamapaña, "INCL_BGEO3", "", conTemp)
                    '                    End If
                    '                    iBlitz300 += 1
                    '            End Select
                    '        Else
                    '            addRecordOk(m_Excel.Worksheet(1).Row(iRow + 1), CLSfILE.ID_ARCHIVO, -1, -1, -1, False, idcamapaña, "", "", conTemp)
                    '            iNack += 1

                    '        End If
                    '    End If
                    'Else



                Next
                CREA_FILES_AGENCIA_ACK(CLSfILE.ID_ARCHIVO, chkGeo1.Checked, chkGeo2.Checked)
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
                msg("Incorrect format: File headers don't match")
            End If
        Else
            msg("No file to process available")
        End If
        Console.WriteLine("**********" + resString + "**********")

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
        If Not rowExcel.Cell(14).Value.ToString.Length = 5 Then
            If Not rowExcel.Cell(14).Value.ToString.Length = 4 Then
                res = False
            End If
        End If


        If Not IsNumeric(rowExcel.Cell(14).Value.ToString) Then
            res = False
        End If


        If rowExcel.Cell(12).Value.ToString = "" Then
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
        Dim comm As SqlCommand = New SqlCommand("SELECT ROWS_OK,ROWS_NOT_OK, DESC_AGENCIA, NOMBRE_ARCHIVO, ID_ASIGNACION FROM ASIGNACIONES INNER JOIN TAB_AGENCIA ON TAB_AGENCIA.ID_AGENCIA = ASIGNACIONES.ID_AGENCIA WHERE ARCHIVO_ACK = 1 AND ID_ARCHIVO = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo

        Dim rdrAsignaciones As DataSet = dsOpenDB(comm)
        grdFiles.DataSource = rdrAsignaciones.Tables(0)
        grdFiles.DataBind()

        Dim comm2 As SqlCommand = New SqlCommand("SELECT ROWS_NOT_OK, NOMBRE_ARCHIVO, ID_ASIGNACION FROM ASIGNACIONES (NOLOCK) WHERE ARCHIVO_ACK = 0 AND ID_ARCHIVO = @PARAM1")
        comm2.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo
        Dim rdrAsignacionesNACK As DataSet = dsOpenDB(comm2)

        If rdrAsignacionesNACK.Tables(0).Rows.Count > 0 Then
            lblNotOk.Visible = True
        End If
        grdNAKFile.DataSource = rdrAsignacionesNACK.Tables(0)
        grdNAKFile.DataBind()

        Dim comm3 As SqlCommand = New SqlCommand("SELECT ROWS_OK,ROWS_NOT_OK, NOMBRE_ARCHIVO, ID_ASIGNACION, TIPO_BLITZ FROM ASIGNACIONES WHERE ARCHIVO_ACK = 1 AND TIPO_BLITZ  <> '' AND ID_ARCHIVO = @PARAM1")
        comm3.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo

        Dim rdrBlitz As DataSet = dsOpenDB(comm3)
        If rdrBlitz.Tables(0).Rows.Count > 0 Then
            lblBlitz.Visible = True
        End If
        grdBlitz.DataSource = rdrBlitz.Tables(0)
        grdBlitz.DataBind()


    End Sub
    Private Sub addRecordOk(ByVal rowExcel As IXLRow, ByVal idArchivo As Integer, ByVal idAgencia As Integer, ByVal idRegion As Integer, ByVal GEO As Integer, ByVal bRecordOk As Boolean, ByVal idcampaña As Integer, Optional ByVal tipoBlitz As String = "", Optional ByVal motivoRechazo As String = "", Optional ByRef existingConnection As SqlConnection = Nothing)
        Dim cRecord As New clsRecord
        cRecord.ID_ARCHIVO_DATA = -1
        cRecord.ID_ARCHIVO = idArchivo
        cRecord.ID_AGENCIA = idAgencia
        cRecord.GEO = GEO
        cRecord.COL8 = rowExcel.Cell(19).Value.ToString
        cRecord.COL9 = ""
        cRecord.COL10 = ""
        cRecord.COL11 = ""
        cRecord.COL12 = ""
        cRecord.COL13 = ""
        cRecord.COL14 = ""
        cRecord.COL15 = ""
        cRecord.INDUSTRIA = rowExcel.Cell(2).Value.ToString
        cRecord.SUBINDUSTRIA = rowExcel.Cell(3).Value.ToString
        cRecord.PORTAFOLIO = rowExcel.Cell(4).Value.ToString 'cambioPortafolio
        cRecord.PARTNER = rowExcel.Cell(5).Value.ToString 'PARTNER
        cRecord.CANAL = rowExcel.Cell(6).Value.ToString
        cRecord.BASE = rowExcel.Cell(7).Value.ToString
        cRecord.COMODIN = rowExcel.Cell(8).Value.ToString
        cRecord.AFILIACION = rowExcel.Cell(9).Value.ToString
        cRecord.NOMBRE_ESTABLECIMIENTO = rowExcel.Cell(10).Value.ToString
        cRecord.REPRESENTANTELEGAL = rowExcel.Cell(11).Value.ToString
        cRecord.CALLE_NUMERO = rowExcel.Cell(12).Value.ToString
        cRecord.COLONIA = rowExcel.Cell(13).Value.ToString
        cRecord.CP = rowExcel.Cell(14).Value.ToString
        cRecord.ESTADO_MUNICIPIO = rowExcel.Cell(15).Value.ToString
        cRecord.CIUDAD = rowExcel.Cell(16).Value.ToString
        cRecord.TELEFONO = rowExcel.Cell(17).Value.ToString
        cRecord.COBERTURA = IIf(rowExcel.Cell(1).Value.ToString.ToUpper = "COBERTURA", "Y", "N") 'rowExcel.Cell(15).Value.ToString
        cRecord.ID_CAMPAIGN = idcampaña

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

        cRecord.MOTIVO_RECHAZO = motivoRechazo
        cRecord.SAVE(cRecord, existingConnection)

    End Sub
    Private Sub addRecordNok()

    End Sub
    Private Sub CREA_FILES_AGENCIA_ACK(ByVal idArchivo As Integer, ByVal geo1 As Boolean, ByVal geo2 As Boolean)
        Dim RDRACK As DataSet
        'RDRACK = dsOpenDB("*", "ARCHIVOS_DATA", "ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND ID_AGENCIA > 0")
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1 AND FLG_RECORD_OK = 1 AND ID_AGENCIA > 0")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo

        RDRACK = dsOpenDB(comm)

        comm = New SqlCommand("SELECT * FROM ARCHIVOS (nolock) WHERE ID_ARCHIVO = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo

        Dim original As DataSet = dsOpenDB(comm)
        Dim distinctDT As DataTable = RDRACK.Tables(0).DefaultView.ToTable(True, "ID_AGENCIA")
        Dim drAck() As DataRow
        'CREA EL REGISTRO DE ASIGNACIÓN

        Dim nombrearchivo As String = original.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO")
        If Not validaInjection(nombrearchivo) Then
            nombrearchivo = "Antihacking.xls"
        End If

        nombrearchivo = nombrearchivo.Substring(0, nombrearchivo.IndexOf(".xls"))
        For I As Integer = 0 To distinctDT.Rows.Count - 1

            drAck = RDRACK.Tables(0).Select("ID_ARCHIVO = " & idArchivo & " AND ID_AGENCIA = " & distinctDT.Rows(I).Item("ID_AGENCIA"))
            If drAck.Length > 0 Then
                Dim baseruta As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/plantilla_ack.xlsx"), False)
                comm = New SqlCommand("SELECT DESC_AGENCIA FROM TAB_AGENCIA (NOLOCK) WHERE ID_AGENCIA = @PARAM1")
                comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = distinctDT.Rows(I).Item("ID_AGENCIA")

                Dim strAgencia As String = VALORINTABLA("DESC_AGENCIA", "TAB_AGENCIA", "ID_AGENCIA", distinctDT.Rows(I).Item("ID_AGENCIA"), comm)
                Dim agencia As String = strAgencia
                strAgencia = nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + strAgencia
                Dim PathCopia As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/" & strAgencia.Replace(" ", "_") & ".xlsx"), False)
                If System.IO.File.Exists(PathCopia) Then
                    File.Delete(PathCopia)
                End If
                If System.IO.File.Exists(baseruta) Then
                    File.Copy(baseruta, PathCopia)
                End If


                Dim m_Excel As New XLWorkbook(PathCopia)

                Dim COUNTROWSOK As Integer = 0
                Dim iRow As Integer
                For iRow = 0 To drAck.Length - 1
                    If drAck(iRow).Item("FLG_RECORD_OK") Then
                        COUNTROWSOK += 1
                        '20-jul-2020: originalmente era así, pero cambiaron el formato
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(1).Value = strAgencia 'AGENCIA
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(2).Value = drAck(iRow).Item("FILE") 'ARCHIVO
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(3).Value = drAck(iRow).Item("PORTAFOLIO") 'PORTAFOLIO
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(4).Value = drAck(iRow).Item("PARTNER") 'PARTNER
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(5).Value = drAck(iRow).Item("CANAL") 'CANAL
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(6).Value = drAck(iRow).Item("AFILIACION") 'AFILIACIÓN
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(7).Value = drAck(iRow).Item("NOMBRE_ESTABLECIMIENTO") 'NOMBRE_ESTABLECIMIENTO
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(8).Value = drAck(iRow).Item("CALLE_NUMERO") 'CALLEY NUMERO
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(9).Value = drAck(iRow).Item("COLONIA") 'COLONIA
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(10).Value = drAck(iRow).Item("CP") 'CP
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(1).Value = agencia
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(2).Value = drAck(iRow).Item("INDUSTRIA")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(3).Value = drAck(iRow).Item("SUBINDUSTRIA")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(4).Value = drAck(iRow).Item("PORTAFOLIO")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(5).Value = drAck(iRow).Item("PARTNER")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(6).Value = drAck(iRow).Item("CANAL")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(7).Value = drAck(iRow).Item("BASE")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(8).Value = drAck(iRow).Item("COMODIN")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(9).Value = drAck(iRow).Item("AFILIACION")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(10).Value = drAck(iRow).Item("NOMBRE_ESTABLECIMIENTO")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(11).Value = drAck(iRow).Item("REPRESENTANTELEGAL")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(12).Value = drAck(iRow).Item("CALLE_NUMERO")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(13).Value = drAck(iRow).Item("COLONIA")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(14).Value = drAck(iRow).Item("CP")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(15).Value = drAck(iRow).Item("ESTADO_MUNICIPIO")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(16).Value = drAck(iRow).Item("CIUDAD")
                        m_Excel.Worksheet(1).Row(iRow + 2).Cell(17).Value = drAck(iRow).Item("TELEFONO")
                        If (drAck(iRow).Item("TIPO_BLITZ").Equals("")) Then
                            m_Excel.Worksheet(1).Row(iRow + 2).Cell(18).Value = drAck(iRow).Item("GEO")
                        Else
                            m_Excel.Worksheet(1).Row(iRow + 2).Cell(18).Value = "Blitz Geo " & drAck(iRow).Item("GEO")
                        End If


                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(20).SetDataValidation.List("VALUES!$H$10:$H$11")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(21).SetDataValidation.List("VALUES!$B$2:$B$5")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(22).SetDataValidation.List("VALUES!$E$2:$E$3")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(23).SetDataValidation.List("VALUES!$E$2:$E$3")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(24).SetDataValidation.List("VALUES!$C$2:$C$4")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(25).SetDataValidation.List("VALUES!$D$2:$D$6")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(26).SetDataValidation.List("VALUES!$F$2:$F$3")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(27).SetDataValidation.List("VALUES!$G$2:$G$6")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(28).SetDataValidation.List("VALUES!$D$10:$D$15")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(29).SetDataValidation.List("VALUES!$F$10")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(30).SetDataValidation.List("VALUES!$B$10:$B$14")
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(35).SetDataValidation.List("VALUES!$C$10:$C$11")




                        'If drAck(iRow).Item("ID_REGION") <> -1 Then
                        '    'desc_regioN
                        '    If VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", drAck(iRow).Item("ID_REGION"))) <> "" Then
                        '        m_Excel.Worksheet(1).Row(iRow + 2).Cell(15).Value = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", drAck(iRow).Item("ID_REGION")))
                        '    Else
                        '        m_Excel.Worksheet(1).Row(iRow + 2).Cell(15).Value = drAck(iRow).Item("ESTADO_MUNICIPIO")
                        '    End If
                        'Else
                        '    'lo que venga de texto
                        '    m_Excel.Worksheet(1).Row(iRow + 2).Cell(15).Value = drAck(iRow).Item("ESTADO_MUNICIPIO")
                        'End If
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(12).Value = drAck(iRow).Item("CIUDAD") 'CIUDAD_MPIO_ALCALDIA
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(13).Value = drAck(iRow).Item("TELEFONO") 'TELEFONO
                        'm_Excel.Worksheet(1).Row(iRow + 2).Cell(14).Value = drAck(iRow).Item("GEO") 'COBERTURA_GEO


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
                iRow += 1
                m_Excel.Worksheet(1).Range("T2:T" + iRow.ToString).SetDataValidation.List("VALUES!$H$10:$H$11")
                m_Excel.Worksheet(1).Range("U2:U" + iRow.ToString).SetDataValidation.List("VALUES!$B$2:$B$5")
                m_Excel.Worksheet(1).Range("V2:V" + iRow.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
                m_Excel.Worksheet(1).Range("W2:W" + iRow.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
                m_Excel.Worksheet(1).Range("X2:X" + iRow.ToString).SetDataValidation.List("VALUES!$C$2:$C$4")
                m_Excel.Worksheet(1).Range("Y2:Y" + iRow.ToString).SetDataValidation.List("VALUES!$D$2:$D$6")
                m_Excel.Worksheet(1).Range("Z2:Z" + iRow.ToString).SetDataValidation.List("VALUES!$F$2:$F$3")
                m_Excel.Worksheet(1).Range("AA2:AA" + iRow.ToString).SetDataValidation.List("VALUES!$G$2:$G$6")
                m_Excel.Worksheet(1).Range("AB2:AB" + iRow.ToString).SetDataValidation.List("VALUES!$D$10:$D$15")
                m_Excel.Worksheet(1).Range("AC2:AC" + iRow.ToString).SetDataValidation.List("VALUES!$F$10")
                m_Excel.Worksheet(1).Range("AD2:AD" + iRow.ToString).SetDataValidation.List("VALUES!$B$10:$B$14")
                m_Excel.Worksheet(1).Range("AI2:AI" + iRow.ToString).SetDataValidation.List("VALUES!$C$10:$C$11")
                m_Excel.Save()
                iRow += -1
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
        Dim pathNACK As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/" & nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "NOT_OK.xlsx"), False)
        Dim baserutaNACK As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/plantilla_NACK.xlsx"), False)
        If System.IO.File.Exists(pathNACK) Then
            File.Delete(pathNACK)
        End If
        If System.IO.File.Exists(baserutaNACK) Then
            File.Copy(baserutaNACK, pathNACK)
        End If

        Dim m_ExcelNACK As New XLWorkbook(pathNACK)
        comm = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1 AND FLG_RECORD_OK = 0")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo
        Dim rdrNack As DataSet = dsOpenDB(comm)
        Dim COUNTNACK As Integer = 0

        If rdrNack.Tables(0).Rows.Count - 1 > 0 Then
            Dim IrOWnACK As Integer = 0
            For IrOWnACK = 0 To rdrNack.Tables(0).Rows.Count - 1
                COUNTNACK += 1

                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(1).Value = ""
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(2).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("INDUSTRIA")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(3).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("SUBINDUSTRIA")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(4).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("PORTAFOLIO")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(5).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("PARTNER")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(6).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CANAL")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(7).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("BASE")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(8).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("COMODIN")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(9).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("AFILIACION")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(10).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("NOMBRE_ESTABLECIMIENTO")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(11).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("REPRESENTANTELEGAL")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(12).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CALLE_NUMERO")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(13).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("COLONIA")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(14).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CP")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(15).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("ESTADO_MUNICIPIO")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(16).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("CIUDAD")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(17).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("TELEFONO")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(18).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("GEO")
                m_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(20).Value = rdrNack.Tables(0).Rows(IrOWnACK).Item("COL8")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(20).SetDataValidation.List("VALUES!$H$10:$H$11")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(21).SetDataValidation.List("VALUES!$B$2:$B$5")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(22).SetDataValidation.List("VALUES!$E$2:$E$3")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(23).SetDataValidation.List("VALUES!$E$2:$E$3")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(24).SetDataValidation.List("VALUES!$C$2:$C$4")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(25).SetDataValidation.List("VALUES!$D$2:$D$6")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(26).SetDataValidation.List("VALUES!$F$2:$F$3")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(27).SetDataValidation.List("VALUES!$G$2:$G$6")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(28).SetDataValidation.List("VALUES!$D$10:$D$15")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(29).SetDataValidation.List("VALUES!$F$10")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(30).SetDataValidation.List("VALUES!$B$10:$B$14")
                'm_ExcelNACK.Worksheet(1).Row(IrOWnACK + 2).Cell(35).SetDataValidation.List("VALUES!$C$10:$C$11")

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
            IrOWnACK += 1
            m_ExcelNACK.Worksheet(1).Range("T2:T" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$H$10:$H$11")
            m_ExcelNACK.Worksheet(1).Range("U2:U" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$B$2:$B$5")
            m_ExcelNACK.Worksheet(1).Range("V2:V" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
            m_ExcelNACK.Worksheet(1).Range("W2:W" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
            m_ExcelNACK.Worksheet(1).Range("X2:X" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$C$2:$C$4")
            m_ExcelNACK.Worksheet(1).Range("Y2:Y" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$D$2:$D$6")
            m_ExcelNACK.Worksheet(1).Range("Z2:Z" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$F$2:$F$3")
            m_ExcelNACK.Worksheet(1).Range("AA2:AA" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$G$2:$G$6")
            m_ExcelNACK.Worksheet(1).Range("AB2:AB" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$D$10:$D$15")
            m_ExcelNACK.Worksheet(1).Range("AC2:AC" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$F$10")
            m_ExcelNACK.Worksheet(1).Range("AD2:AD" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$B$10:$B$14")
            m_ExcelNACK.Worksheet(1).Range("AI2:AI" + IrOWnACK.ToString).SetDataValidation.List("VALUES!$C$10:$C$11")
            IrOWnACK += -1
            m_ExcelNACK.Save()
            Dim cAsignacionNack As New clsAsignacion
            cAsignacionNack.ID_ASIGNACION = -1
            cAsignacionNack.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
            cAsignacionNack.ID_AGENCIA = -1
            cAsignacionNack.FLG_CANC = False
            cAsignacionNack.ID_ARCHIVO = idArchivo
            cAsignacionNack.ARCHIVO = FILE_2_BYTES(pathNACK)
            cAsignacionNack.NOMBRE_ARCHIVO = nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "NOT_OK.xlsx"
            cAsignacionNack.ARCHIVO_ACK = False
            cAsignacionNack.ROWS_NOT_OK = COUNTNACK
            cAsignacionNack.SAVE(cAsignacionNack)
        End If


        If Not geo1 Then
            Dim RDRACK_GEO1 As DataSet
            comm = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1 AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'GEO 1'")
            comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo

            RDRACK_GEO1 = dsOpenDB(comm)
            Dim baseruta_geo1 As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/plantilla_ack.xlsx"), False)
            Dim PathCopia_geo1 As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/" & nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "GEO 1 EXCLUIDO.xlsx"), False)
            If System.IO.File.Exists(PathCopia_geo1) Then
                File.Delete(PathCopia_geo1)
            End If
            If System.IO.File.Exists(baseruta_geo1) Then
                File.Copy(baseruta_geo1, PathCopia_geo1)
            End If
            Dim m_Excel_geo1 As New XLWorkbook(PathCopia_geo1)
            Dim iCountGeo1 As Integer
            For iCountGeo1 = 0 To RDRACK_GEO1.Tables(0).Rows.Count - 1
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(1).Value = ""
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(2).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("INDUSTRIA") 'ARCHIVO
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(3).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("SUBINDUSTRIA") 'PORTAFOLIO
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(4).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("PORTAFOLIO") 'PARTNER
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(5).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("PARTNER") 'CANAL
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(6).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("CANAL") 'AFILIACIÓN
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(7).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("BASE") 'NOMBRE_ESTABLECIMIENTO
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(8).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("COMODIN") 'CALLEY NUMERO
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(9).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("AFILIACION") 'COLONIA
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(10).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("NOMBRE_ESTABLECIMIENTO") 'CP
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(11).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("REPRESENTANTELEGAL") 'CP
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(12).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("CALLE_NUMERO") 'CP
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(13).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("COLONIA") 'CP
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(14).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("CP") 'CP
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(15).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("ESTADO_MUNICIPIO") 'CP
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(16).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("CIUDAD") 'CP
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(17).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("TELEFONO") 'CP
                m_Excel_geo1.Worksheet(1).Row(iCountGeo1 + 2).Cell(18).Value = RDRACK_GEO1.Tables(0).Rows(iCountGeo1).Item("GEO") 'CP
            Next
            iCountGeo1 += 1
            m_Excel_geo1.Worksheet(1).Range("T2:T" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$H$10:$H$11")
            m_Excel_geo1.Worksheet(1).Range("U2:U" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$B$2:$B$5")
            m_Excel_geo1.Worksheet(1).Range("V2:V" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
            m_Excel_geo1.Worksheet(1).Range("W2:W" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
            m_Excel_geo1.Worksheet(1).Range("X2:X" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$C$2:$C$4")
            m_Excel_geo1.Worksheet(1).Range("Y2:Y" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$D$2:$D$6")
            m_Excel_geo1.Worksheet(1).Range("Z2:Z" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$F$2:$F$3")
            m_Excel_geo1.Worksheet(1).Range("AA2:AA" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$G$2:$G$6")
            m_Excel_geo1.Worksheet(1).Range("AB2:AB" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$D$10:$D$15")
            m_Excel_geo1.Worksheet(1).Range("AC2:AC" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$F$10")
            m_Excel_geo1.Worksheet(1).Range("AD2:AD" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$B$10:$B$14")
            m_Excel_geo1.Worksheet(1).Range("AI2:AI" + iCountGeo1.ToString).SetDataValidation.List("VALUES!$C$10:$C$11")
            iCountGeo1 += -1
            m_Excel_geo1.Save()
            Dim cAsignacion_geo1 As New clsAsignacion
            cAsignacion_geo1.ID_ASIGNACION = -1
            cAsignacion_geo1.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
            cAsignacion_geo1.ID_AGENCIA = -1 'distinctDT.Rows(I).Item("ID_AGENCIA")  
            cAsignacion_geo1.FLG_CANC = False
            cAsignacion_geo1.ID_ARCHIVO = idArchivo
            cAsignacion_geo1.ARCHIVO = FILE_2_BYTES(PathCopia_geo1)
            cAsignacion_geo1.NOMBRE_ARCHIVO = nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "GEO 1 EXCLUIDO.xlsx"
            cAsignacion_geo1.ARCHIVO_ACK = True
            cAsignacion_geo1.ROWS_OK = iCountGeo1
            cAsignacion_geo1.TIPO_BLITZ = "GEO 1"
            cAsignacion_geo1.SAVE(cAsignacion_geo1)
        End If


        If Not geo2 Then
            Dim RDRACK_GEO2 As DataSet
            'RDRACK_GEO2 = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'GEO 2'")
            comm = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1 AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'GEO 2'")
            comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo

            RDRACK_GEO2 = dsOpenDB(comm)
            Dim baseruta_geo2 As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/plantilla_ack.xlsx"), False)
            Dim PathCopia_geo2 As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/" & nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "GEO 2 EXCLUIDO.xlsx"), False)
            If System.IO.File.Exists(PathCopia_geo2) Then
                File.Delete(PathCopia_geo2)
            End If
            If System.IO.File.Exists(baseruta_geo2) Then
                File.Copy(baseruta_geo2, PathCopia_geo2)
            End If
            Dim m_Excel_geo2 As New XLWorkbook(PathCopia_geo2)
            Dim iCountGeo2 As Integer
            For iCountGeo2 = 0 To RDRACK_GEO2.Tables(0).Rows.Count - 1
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(1).Value = ""
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(2).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("INDUSTRIA") 'ARCHIVO
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(3).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("SUBINDUSTRIA") 'PORTAFOLIO
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(4).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("PORTAFOLIO") 'PARTNER
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(5).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("PARTNER") 'CANAL
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(6).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("CANAL") 'AFILIACIÓN
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(7).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("BASE") 'NOMBRE_ESTABLECIMIENTO
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(8).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("COMODIN") 'CALLEY NUMERO
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(9).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("AFILIACION") 'COLONIA
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(10).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("NOMBRE_ESTABLECIMIENTO") 'CP
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(11).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("REPRESENTANTELEGAL") 'CP
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(12).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("CALLE_NUMERO") 'CP
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(13).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("COLONIA") 'CP
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(14).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("CP") 'CP
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(15).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("ESTADO_MUNICIPIO") 'CP
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(16).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("CIUDAD") 'CP
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(17).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("TELEFONO") 'CP
                m_Excel_geo2.Worksheet(1).Row(iCountGeo2 + 2).Cell(18).Value = RDRACK_GEO2.Tables(0).Rows(iCountGeo2).Item("GEO") 'CP
            Next
            iCountGeo2 += 1
            m_Excel_geo2.Worksheet(1).Range("T2:T" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$H$10:$H$11")
            m_Excel_geo2.Worksheet(1).Range("U2:U" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$B$2:$B$5")
            m_Excel_geo2.Worksheet(1).Range("V2:V" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
            m_Excel_geo2.Worksheet(1).Range("W2:W" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
            m_Excel_geo2.Worksheet(1).Range("X2:X" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$C$2:$C$4")
            m_Excel_geo2.Worksheet(1).Range("Y2:Y" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$D$2:$D$6")
            m_Excel_geo2.Worksheet(1).Range("Z2:Z" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$F$2:$F$3")
            m_Excel_geo2.Worksheet(1).Range("AA2:AA" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$G$2:$G$6")
            m_Excel_geo2.Worksheet(1).Range("AB2:AB" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$D$10:$D$15")
            m_Excel_geo2.Worksheet(1).Range("AC2:AC" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$F$10")
            m_Excel_geo2.Worksheet(1).Range("AD2:AD" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$B$10:$B$14")
            m_Excel_geo2.Worksheet(1).Range("AI2:AI" + iCountGeo2.ToString).SetDataValidation.List("VALUES!$C$10:$C$11")
            iCountGeo2 += -1
            m_Excel_geo2.Save()
            Dim cAsignacion_geo2 As New clsAsignacion
            cAsignacion_geo2.ID_ASIGNACION = -1
            cAsignacion_geo2.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
            cAsignacion_geo2.ID_AGENCIA = -1 'distinctDT.Rows(I).Item("ID_AGENCIA")  
            cAsignacion_geo2.FLG_CANC = False
            cAsignacion_geo2.ID_ARCHIVO = idArchivo
            cAsignacion_geo2.ARCHIVO = FILE_2_BYTES(PathCopia_geo2)
            cAsignacion_geo2.NOMBRE_ARCHIVO = nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "GEO 2 EXCLUIDO.xlsx"
            cAsignacion_geo2.ARCHIVO_ACK = True
            cAsignacion_geo2.ROWS_OK = iCountGeo2
            cAsignacion_geo2.TIPO_BLITZ = "GEO 2"
            cAsignacion_geo2.SAVE(cAsignacion_geo2)
        End If


        'CREA LOS FILES PARA LOS BLITZ DE ÉSTA ASIGNACIÓN BLITZ 50
        Dim RDRACK_50 As DataSet
        'RDRACK_50 = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ50'")
        comm = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1 AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ50'")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo

        RDRACK_50 = dsOpenDB(comm)
        Dim baseruta_50 As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/plantilla_ack.xlsx"), False)
        Dim PathCopia_50 As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/" & nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "BLITZ_50.xlsx"), False)
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
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(2).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("INDUSTRIA") 'ARCHIVO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(3).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("SUBINDUSTRIA") 'PORTAFOLIO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(4).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("PORTAFOLIO") 'PARTNER
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(5).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("PARTNER") 'CANAL
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(6).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("CANAL") 'AFILIACIÓN
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(7).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("BASE") 'NOMBRE_ESTABLECIMIENTO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(8).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("COMODIN") 'CALLEY NUMERO
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(9).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("AFILIACION") 'COLONIA
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(10).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("NOMBRE_ESTABLECIMIENTO") 'CP
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(11).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("REPRESENTANTELEGAL") 'CP
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(12).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("CALLE_NUMERO") 'CP
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(13).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("COLONIA") 'CP
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(14).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("CP") 'CP
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(15).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("ESTADO_MUNICIPIO") 'CP
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(16).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("ALCALDIA") 'CP
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(17).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("TELEFONO") 'CP
            m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(18).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("GEO") 'CP
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(20).SetDataValidation.List("VALUES!$H$10:$H$11")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(21).SetDataValidation.List("VALUES!$B$2:$B$5")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(22).SetDataValidation.List("VALUES!$E$2:$E$3")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(23).SetDataValidation.List("VALUES!$E$2:$E$3")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(24).SetDataValidation.List("VALUES!$C$2:$C$4")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(25).SetDataValidation.List("VALUES!$D$2:$D$6")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(26).SetDataValidation.List("VALUES!$F$2:$F$3")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(27).SetDataValidation.List("VALUES!$G$2:$G$6")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(28).SetDataValidation.List("VALUES!$D$10:$D$15")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(29).SetDataValidation.List("VALUES!$F$10")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(30).SetDataValidation.List("VALUES!$B$10:$B$14")
            'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(35).SetDataValidation.List("VALUES!$C$10:$C$11")

            If RDRACK_50.Tables(0).Rows(iCount50).Item("ID_REGION") <> -1 Then
                'desc_regioN
                comm = New SqlCommand("SELECT ID_ESTADO FROM TAB_REGION (NOLOCK) WHERE ID_REGION = @PARAM1")
                comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("ID_REGION")
                Dim val As String = VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_50.Tables(0).Rows(iCount50).Item("ID_REGION"), comm)
                'If VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_50.Tables(0).Rows(iCount50).Item("ID_REGION"))) <> "" Then
                Dim comm2 As SqlCommand = New SqlCommand("SELECT DESC_ESTADO FROM TAB_ESTADOS (NOLOCK) WHERE ID_ESTADO = @PARAM1")
                comm2.Parameters.Add("@PARAM2", SqlDbType.BigInt).Value = val
                Dim valor = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", val, comm2)
                If valor <> "" Then
                    'm_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(15).Value = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_50.Tables(0).Rows(iCount50).Item("ID_REGION")))
                    m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(15).Value = valor
                Else
                    m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(15).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("ESTADO_MUNICIPIO")
                End If
            Else
                'lo que venga de texto
                m_Excel_50.Worksheet(1).Row(iCount50 + 2).Cell(15).Value = RDRACK_50.Tables(0).Rows(iCount50).Item("ESTADO_MUNICIPIO")
            End If

        Next
        iCount50 += 1
        m_Excel_50.Worksheet(1).Range("T2:T" + iCount50.ToString).SetDataValidation.List("VALUES!$H$10:$H$11")
        m_Excel_50.Worksheet(1).Range("U2:U" + iCount50.ToString).SetDataValidation.List("VALUES!$B$2:$B$5")
        m_Excel_50.Worksheet(1).Range("V2:V" + iCount50.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
        m_Excel_50.Worksheet(1).Range("W2:W" + iCount50.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
        m_Excel_50.Worksheet(1).Range("X2:X" + iCount50.ToString).SetDataValidation.List("VALUES!$C$2:$C$4")
        m_Excel_50.Worksheet(1).Range("Y2:Y" + iCount50.ToString).SetDataValidation.List("VALUES!$D$2:$D$6")
        m_Excel_50.Worksheet(1).Range("Z2:Z" + iCount50.ToString).SetDataValidation.List("VALUES!$F$2:$F$3")
        m_Excel_50.Worksheet(1).Range("AA2:AA" + iCount50.ToString).SetDataValidation.List("VALUES!$G$2:$G$6")
        m_Excel_50.Worksheet(1).Range("AB2:AB" + iCount50.ToString).SetDataValidation.List("VALUES!$D$10:$D$15")
        m_Excel_50.Worksheet(1).Range("AC2:AC" + iCount50.ToString).SetDataValidation.List("VALUES!$F$10")
        m_Excel_50.Worksheet(1).Range("AD2:AD" + iCount50.ToString).SetDataValidation.List("VALUES!$B$10:$B$14")
        m_Excel_50.Worksheet(1).Range("AI2:AI" + iCount50.ToString).SetDataValidation.List("VALUES!$C$10:$C$11")
        iCount50 += -1
        m_Excel_50.Save()
        Dim cAsignacion_50 As New clsAsignacion
        cAsignacion_50.ID_ASIGNACION = -1
        cAsignacion_50.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
        cAsignacion_50.ID_AGENCIA = -1 'distinctDT.Rows(I).Item("ID_AGENCIA")  
        cAsignacion_50.FLG_CANC = False
        cAsignacion_50.ID_ARCHIVO = idArchivo
        cAsignacion_50.ARCHIVO = FILE_2_BYTES(PathCopia_50)
        cAsignacion_50.NOMBRE_ARCHIVO = nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "BLITZ_50.xlsx"
        cAsignacion_50.ARCHIVO_ACK = True
        cAsignacion_50.ROWS_OK = iCount50
        cAsignacion_50.TIPO_BLITZ = "BLITZ50"
        cAsignacion_50.SAVE(cAsignacion_50)



        'CREA LOS FILES PARA LOS BLITZ DE ÉSTA ASIGNACIÓN BLITZ 100
        Dim RDRACK_100 As DataSet
        'RDRACK_100 = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ100'")
        comm = New SqlCommand("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = @PARAM1 AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ100'")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo
        RDRACK_100 = dsOpenDB(comm)
        Dim baseruta_100 As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/plantilla_ack.xlsx"), False)
        Dim PathCopia_100 As String = AntiXssEncoder.HtmlEncode(Server.MapPath("~/tmpFile/" & nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "BLITZ_100.xlsx"), False)

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
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(2).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("INDUSTRIA") 'ARCHIVO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(3).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("SUBINDUSTRIA") 'PORTAFOLIO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(4).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("PORTAFOLIO") 'PARTNER
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(5).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("PARTNER") 'CANAL
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(6).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("CANAL") 'AFILIACIÓN
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(7).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("BASE") 'NOMBRE_ESTABLECIMIENTO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(8).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("COMODIN") 'CALLEY NUMERO
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(9).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("AFILIACION") 'COLONIA
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(10).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("NOMBRE_ESTABLECIMIENTO") 'CP
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(11).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("REPRESENTANTELEGAL") 'CP
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(12).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("CALLE_NUMERO") 'CP
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(13).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("COLONIA") 'CP
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(14).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("CP") 'CP
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(15).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("ESTADO_MUNICIPIO") 'CP
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(16).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("CIUDAD") 'CP
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(17).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("TELEFONO") 'CP
            m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(18).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("GEO") 'CP
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(20).SetDataValidation.List("VALUES!$H$10:$H$11")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(21).SetDataValidation.List("VALUES!$B$2:$B$5")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(22).SetDataValidation.List("VALUES!$E$2:$E$3")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(23).SetDataValidation.List("VALUES!$E$2:$E$3")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(24).SetDataValidation.List("VALUES!$C$2:$C$4")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(25).SetDataValidation.List("VALUES!$D$2:$D$6")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(26).SetDataValidation.List("VALUES!$F$2:$F$3")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(27).SetDataValidation.List("VALUES!$G$2:$G$6")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(28).SetDataValidation.List("VALUES!$D$10:$D$15")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(29).SetDataValidation.List("VALUES!$F$10")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(30).SetDataValidation.List("VALUES!$B$10:$B$14")
            'm_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(35).SetDataValidation.List("VALUES!$C$10:$C$11")
            If RDRACK_100.Tables(0).Rows(iCount_100).Item("ID_REGION") <> -1 Then
                comm = New SqlCommand("SELECT ID_ESTADO FROM TAB_REGION (NOLOCK) WHERE ID_REGION = @PARAM1")
                comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("ID_REGION")

                Dim val100 As String = VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_100.Tables(0).Rows(iCount_100).Item("ID_REGION"), comm)
                'desc_regioN
                Dim comm100 As SqlCommand = New SqlCommand("SELECT DESC_ESTADO FROM TAB_ESTADOS (NOLOCK) WHERE ID_ESTADO = @PARAM1")
                comm100.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = val100
                Dim valcomp As String = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", val100, comm100)
                If valcomp <> "" Then
                    m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(15).Value = valcomp
                Else
                    m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(15).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("ESTADO_MUNICIPIO")
                End If
            Else
                'lo que venga de texto
                m_Excel_100.Worksheet(1).Row(iCount_100 + 2).Cell(15).Value = RDRACK_100.Tables(0).Rows(iCount_100).Item("ESTADO_MUNICIPIO")
            End If
        Next
        iCount_100 += 1
        m_Excel_100.Worksheet(1).Range("T2:T" + iCount_100.ToString).SetDataValidation.List("VALUES!$H$10:$H$11")
        m_Excel_100.Worksheet(1).Range("U2:U" + iCount_100.ToString).SetDataValidation.List("VALUES!$B$2:$B$5")
        m_Excel_100.Worksheet(1).Range("V2:V" + iCount_100.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
        m_Excel_100.Worksheet(1).Range("W2:W" + iCount_100.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
        m_Excel_100.Worksheet(1).Range("X2:X" + iCount_100.ToString).SetDataValidation.List("VALUES!$C$2:$C$4")
        m_Excel_100.Worksheet(1).Range("Y2:Y" + iCount_100.ToString).SetDataValidation.List("VALUES!$D$2:$D$6")
        m_Excel_100.Worksheet(1).Range("Z2:Z" + iCount_100.ToString).SetDataValidation.List("VALUES!$F$2:$F$3")
        m_Excel_100.Worksheet(1).Range("AA2:AA" + iCount_100.ToString).SetDataValidation.List("VALUES!$G$2:$G$6")
        m_Excel_100.Worksheet(1).Range("AB2:AB" + iCount_100.ToString).SetDataValidation.List("VALUES!$D$10:$D$15")
        m_Excel_100.Worksheet(1).Range("AC2:AC" + iCount_100.ToString).SetDataValidation.List("VALUES!$F$10")
        m_Excel_100.Worksheet(1).Range("AD2:AD" + iCount_100.ToString).SetDataValidation.List("VALUES!$B$10:$B$14")
        m_Excel_100.Worksheet(1).Range("AI2:AI" + iCount_100.ToString).SetDataValidation.List("VALUES!$C$10:$C$11")
        iCount_100 += -1
        m_Excel_100.Save()
        Dim cAsignacion_100 As New clsAsignacion
        cAsignacion_100.ID_ASIGNACION = -1
        cAsignacion_100.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
        cAsignacion_100.ID_AGENCIA = -1 'distinctDT.Rows(I).Item("ID_AGENCIA")  
        cAsignacion_100.FLG_CANC = False
        cAsignacion_100.ID_ARCHIVO = idArchivo
        cAsignacion_100.ARCHIVO = FILE_2_BYTES(PathCopia_100)
        cAsignacion_100.NOMBRE_ARCHIVO = nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "BLITZ_100.xlsx"
        cAsignacion_100.ARCHIVO_ACK = True
        cAsignacion_100.ROWS_OK = iCount_100
        cAsignacion_100.TIPO_BLITZ = "BLITZ100"
        cAsignacion_100.SAVE(cAsignacion_100)



        'CREA LOS FILES PARA LOS BLITZ DE ÉSTA ASIGNACIÓN BLITZ 300
        Dim RDRACK_300 As DataSet
        'RDRACK_300 = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & idArchivo & " AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ300'")
        comm = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1 AND FLG_RECORD_OK = 1 AND TIPO_BLITZ = 'BLITZ300'")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idArchivo


        RDRACK_300 = dsOpenDB(comm)
        Dim baseruta_300 As String = Server.MapPath("~/tmpFile/plantilla_ack.xlsx")
        Dim PathCopia_300 As String = Server.MapPath("~/tmpFile/" & nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "BLITZ_300.xlsx")
        baseruta_300 = AntiXssEncoder.HtmlEncode(baseruta_300, False)
        PathCopia_300 = AntiXssEncoder.HtmlEncode(PathCopia_300, False)
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
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(2).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("INDUSTRIA") 'ARCHIVO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(3).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("SUBINDUSTRIA") 'PORTAFOLIO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(4).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("PORTAFOLIO") 'PARTNER
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(5).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("PARTNER") 'CANAL
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(6).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("CANAL") 'AFILIACIÓN
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(7).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("BASE") 'NOMBRE_ESTABLECIMIENTO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(8).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("COMODIN") 'CALLEY NUMERO
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(9).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("AFILIACION") 'COLONIA
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(10).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("NOMBRE_ESTABLECIMIENTO") 'CP
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(11).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("REPRESENTANTELEGAL") 'CP
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(12).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("CALLE_NUMERO") 'CP
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(13).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("COLONIA") 'CP
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(14).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("CP") 'CP
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(15).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("ESTADO_MUNICIPIO") 'CP
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(16).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("CIUDAD") 'CP
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(17).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("TELEFONO") 'CP
            m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(18).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("GEO") 'CP
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(20).SetDataValidation.List("VALUES!$H$10:$H$11")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(21).SetDataValidation.List("VALUES!$B$2:$B$5")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(22).SetDataValidation.List("VALUES!$E$2:$E$3")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(23).SetDataValidation.List("VALUES!$E$2:$E$3")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(24).SetDataValidation.List("VALUES!$C$2:$C$4")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(25).SetDataValidation.List("VALUES!$D$2:$D$6")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(26).SetDataValidation.List("VALUES!$F$2:$F$3")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(27).SetDataValidation.List("VALUES!$G$2:$G$6")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(28).SetDataValidation.List("VALUES!$D$10:$D$15")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(29).SetDataValidation.List("VALUES!$F$10")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(30).SetDataValidation.List("VALUES!$B$10:$B$14")
            'm_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(35).SetDataValidation.List("VALUES!$C$10:$C$11")
            If RDRACK_300.Tables(0).Rows(iCount_300).Item("ID_REGION") <> -1 Then
                'desc_regioN
                comm = New SqlCommand("SELECT * FROM ID_ESTADO FROM TAB_REGION (NOLOCK) WHERE ID_REGION = @PARAM1")
                comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("ID_REGION")
                Dim val300 As String = VALORINTABLA("ID_ESTADO", "TAB_REGION", "ID_REGION", RDRACK_300.Tables(0).Rows(iCount_300).Item("ID_REGION"), comm)
                Dim comm300 As SqlCommand = New SqlCommand("SELECT DESC_ESTADO FROM TAB_ESTADOS (NOLOCK) WHERE ID_ESTADO = @PARAM1")
                comm300.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = val300
                Dim valcomp300 As String = VALORINTABLA("DESC_ESTADO", "TAB_ESTADOS", "ID_ESTADO", val300, comm300)
                If valcomp300 <> "" Then
                    m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(15).Value = valcomp300
                Else
                    m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(15).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("ESTADO_MUNICIPIO")
                End If
            Else
                'lo que venga de texto
                m_Excel_300.Worksheet(1).Row(iCount_300 + 2).Cell(15).Value = RDRACK_300.Tables(0).Rows(iCount_300).Item("ESTADO_MUNICIPIO")
            End If

        Next
        iCount_300 += 1
        m_Excel_300.Worksheet(1).Range("T2:T" + iCount_300.ToString).SetDataValidation.List("VALUES!$H$10:$H$11")
        m_Excel_300.Worksheet(1).Range("U2:U" + iCount_300.ToString).SetDataValidation.List("VALUES!$B$2:$B$5")
        m_Excel_300.Worksheet(1).Range("V2:V" + iCount_300.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
        m_Excel_300.Worksheet(1).Range("W2:W" + iCount_300.ToString).SetDataValidation.List("VALUES!$E$2:$E$3")
        m_Excel_300.Worksheet(1).Range("X2:X" + iCount_300.ToString).SetDataValidation.List("VALUES!$C$2:$C$4")
        m_Excel_300.Worksheet(1).Range("Y2:Y" + iCount_300.ToString).SetDataValidation.List("VALUES!$D$2:$D$6")
        m_Excel_300.Worksheet(1).Range("Z2:Z" + iCount_300.ToString).SetDataValidation.List("VALUES!$F$2:$F$3")
        m_Excel_300.Worksheet(1).Range("AA2:AA" + iCount_300.ToString).SetDataValidation.List("VALUES!$G$2:$G$6")
        m_Excel_300.Worksheet(1).Range("AB2:AB" + iCount_300.ToString).SetDataValidation.List("VALUES!$D$10:$D$15")
        m_Excel_300.Worksheet(1).Range("AC2:AC" + iCount_300.ToString).SetDataValidation.List("VALUES!$F$10")
        m_Excel_300.Worksheet(1).Range("AD2:AD" + iCount_300.ToString).SetDataValidation.List("VALUES!$B$10:$B$14")
        m_Excel_300.Worksheet(1).Range("AI2:AI" + iCount_300.ToString).SetDataValidation.List("VALUES!$C$10:$C$11")
        iCount_300 += -1
        m_Excel_300.Save()
        Dim cAsignacion_300 As New clsAsignacion
        cAsignacion_300.ID_ASIGNACION = -1
        cAsignacion_300.FH_ASIGNACION = FORMATEAR_FECHA(Today, "S")
        cAsignacion_300.ID_AGENCIA = -1 'distinctDT.Rows(I).Item("ID_AGENCIA")  
        cAsignacion_300.FLG_CANC = False
        cAsignacion_300.ID_ARCHIVO = idArchivo
        cAsignacion_300.ARCHIVO = FILE_2_BYTES(PathCopia_300)
        cAsignacion_300.NOMBRE_ARCHIVO = nombrearchivo + " - " + Date.Now.ToString("yyyyMMdd") + " - " + "BLITZ_300.xlsx"
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
        IdArchivo = AntiXssEncoder.HtmlEncode(IdArchivo, False)
        Dim RDRACK As DataSet
        'RDRACK = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO = " & IdArchivo)
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = IdArchivo

        RDRACK = dsOpenDB(comm)

        Dim distinctDT As DataTable = RDRACK.Tables(0).DefaultView.ToTable(True, "ID_AGENCIA")
        Dim drAck() As DataRow
        'CREA EL REGISTRO DE ASIGNACIÓN

        For I As Integer = 0 To distinctDT.Rows.Count - 1




            drAck = RDRACK.Tables(0).Select("ID_ARCHIVO = " & IdArchivo & " AND ID_AGENCIA = " & distinctDT.Rows(I).Item("ID_AGENCIA"))
            If drAck.Length > 0 Then
                Dim baseruta As String = Server.MapPath("~/tmpFile/plantilla_ack.xlsx")
                comm = New SqlCommand("SELECT DESC_AGENCIA FROM TAB_AGENCIA (NOLOCK) WHERE ID_AGENCIA = @PARAM1")
                comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = distinctDT.Rows(I).Item("ID_AGENCIA")

                Dim strAgencia As String = AntiXssEncoder.HtmlEncode(VALORINTABLA("DESC_AGENCIA", "TAB_AGENCIA", "ID_AGENCIA", distinctDT.Rows(I).Item("ID_AGENCIA"), comm), False)
                strAgencia = AntiXssEncoder.HtmlEncode(strAgencia, False)
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
        'rdr = dsOpenDB("SELECT * FROM ASIGNACIONES WHERE ID_ASIGNACION = '" & idAsignacion & "'")
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ASIGNACIONES (NOLOCK) WHERE ID_ASIGNACION = @PARAM1 ")
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

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles chkBlitz1.CheckedChanged

    End Sub



    Protected Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles chkGeo1.CheckedChanged

    End Sub

    'Protected Sub cmdIniciaProces0_Click(sender As Object, e As EventArgs) Handles cmdIniciaProces0.Click
    '    testrows()

    'End Sub
End Class