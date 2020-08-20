Imports System.IO
Imports ClosedXML.Excel
Imports Newtonsoft.Json


Public Class WebForm2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim sm As ScriptManager = ScriptManager.GetCurrent(Me)
        'sm.RegisterPostBackControl(Me.cmdValidate)
        'Me.Form.Attributes.Add("enctype", "multipart/form-data")
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles cmdValidate.Click
        If fuValidation.HasFile Then
            Dim rdrRel As DataTable = dsOpenDB("SELECT * FROM TAB_ESTADOS with(nolock) inner join TAB_CP with(nolock) on TAB_ESTADOS.Id_estado_original = TAB_CP.ID_ESTADO").Tables(0)
            Dim rowCPEstado(), rowCPAlcaldia(), rowEstadoAlcaldia() As DataRow
            Dim nombrerevisado As String = fuValidation.FileName.Replace(" ", "_")

            Dim FILEPATH As String = Server.MapPath("~/tmpFile/" & nombrerevisado)
            If File.Exists(FILEPATH) Then
                File.Delete(FILEPATH)
            End If
            fuValidation.PostedFile.SaveAs(FILEPATH)
            Dim excelToValidate As New XLWorkbook(FILEPATH)

            Dim renglon As Integer
            Dim cpValido, estadoEnBase, estadoValido, regionValido, direccionValida, flagCPEstado, flagCPAlcaldia, flagEstadoAlcaldia, unError As Boolean
            Dim cpExcel, estadoExcel, regionExcel, direccionExcel As String
            Dim cpRow, estadoRow As DataRow
            Dim evaluacion As String
            Dim renglonesNotOK As Integer = 0
            Dim iAck As Integer = 0
            Dim iNack As Integer = 0
            Dim numgeo1 As Integer = 0
            Dim numgeo2 As Integer = 0
            Dim iBlitz50 As Integer = 0
            Dim iBlitz100 As Integer = 0
            Dim iBlitz300 As Integer = 0
            Dim rdrTodosCPs As DataSet = dsOpenDB("SELECT * FROM TAB_CP NOLOCK")
            Dim drowCP() As DataRow
            Dim rdrAgencia As DataSet = dsOpenDB("SELECT * FROM TAB_AGENCIA WHERE FLG_CANC = 0")
            Dim rdrCP As DataSet = dsOpenDB("SELECT * FROM TAB_REGION WHERE FLG_CANC = 0 ")
            Dim rdrEstados As DataSet = dsOpenDB("SELECT * FROM TAB_ESTADOS WHERE FLG_CANC = 0 ")
            Dim rdrAlcaldia As DataSet = dsOpenDB("select  TAB_REGION.ID_ESTADO, DESC_REGION, CP, FLG_PRIORITARIA,ID_REGION,TAB_REGION.FLG_CANC,GEO, TAB_REGION.ID_AGENCIA, DESC_ESTADO, TAB_ESTADOS.FLG_CANC, GEO_DEFAULT from tab_region inner join tab_estados on tab_region.id_estado = TAB_ESTADOS.ID_ESTADO where tab_region.FLG_CANC = 0") '
            Dim rdrAgenciaAlcaldia As DataSet = dsOpenDB("select * from TAB_ALCALDIA inner join TAB_AGENCIA on TAB_AGENCIA.ID_AGENCIA = TAB_ALCALDIA.Id_agencia")
            Dim dRowAgencia() As DataRow
            Dim dRowBlitz() As DataRow
            Dim rowCom() As DataRow
            Dim dRowAlcaldia() As DataRow 'CAMBIO
            Dim dRowEstados() As DataRow
            For renglon = 2 To excelToValidate.Worksheets(0).LastRowUsed().RowNumber()
                evaluacion = ""

                cpExcel = excelToValidate.Worksheets(0).Rows(renglon).Cells(13).Value.ToString()
                If cpExcel.Length = 4 Then
                    cpExcel = "0" & cpExcel
                End If
                estadoExcel = excelToValidate.Worksheets(0).Rows(renglon).Cells(14).Value.ToString()
                regionExcel = excelToValidate.Worksheets(0).Rows(renglon).Cells(15).Value.ToString()
                direccionExcel = excelToValidate.Worksheets(0).Rows(renglon).Cells(11).Value.ToString()
                cpValido = If(cpExcel.Length = 5, True, False)
                estadoValido = If(estadoExcel.Length > 0, True, False)
                dRowEstados = rdrEstados.Tables(0).Select("DESC_ESTADO= '" + estadoExcel + "'")
                estadoEnBase = If(dRowEstados.Length > 0, True, False)
                regionValido = If(regionExcel.Length > 0, True, False)
                direccionValida = If(direccionExcel.Length > 0, True, False)
                rowCPEstado = rdrRel.Select("CP = '" + cpExcel + "' and DESC_ESTADO = '" + estadoExcel + "'")
                rowCPAlcaldia = rdrRel.Select("CP = '" + cpExcel + "' and DESC_REGION = '" + regionExcel + "'")
                rowEstadoAlcaldia = rdrRel.Select("DESC_REGION = '" + regionExcel + "' and DESC_ESTADO = '" + estadoExcel + "'")
                flagCPEstado = If(rowCPEstado.Length > 0, True, False)
                flagCPAlcaldia = If(rowCPAlcaldia.Length > 0, True, False)
                flagEstadoAlcaldia = If(rowEstadoAlcaldia.Length > 0, True, False)
                If Not cpValido Then
                    evaluacion += "CP incorrecto. "
                    unError = True
                Else
                    drowCP = rdrTodosCPs.Tables(0).Select("CP = '" + cpExcel + "'")
                    If Not drowCP.Length > 0 Then
                        evaluacion += "CP no encontrado en base. "
                        unError = True
                    End If
                End If
                    If Not estadoValido Then
                    evaluacion += "Falta estado"
                    unError = True
                End If
                If Not estadoEnBase Then
                    evaluacion += "Estado desconocido. "
                    unError = True
                End If
                If Not regionValido Then
                    evaluacion += "Falta región. "
                    unError = True
                End If
                If Not direccionValida Then
                    evaluacion += "Direccion vacía. "
                    unError = True
                End If
                If Not (flagCPEstado) Then
                    evaluacion += "CP y Estado no corresponden. "
                    unError = True
                End If
                'If Not (flagCPAlcaldia) Then
                '    evaluacion += "CP y Alcaldia/Ciudad/Municipio no corresponden. "
                '    unError = True
                'End If
                'If Not (flagEstadoAlcaldia) Then
                '    evaluacion += "Estado y Alcaldia/Ciudad/Municipio no corresponden. "
                '    unError = True
                'End If
                If evaluacion.Length > 0 Then
                    excelToValidate.Worksheets(0).Rows(renglon).Style.Fill.BackgroundColor = XLColor.Yellow
                    excelToValidate.Worksheets(0).Rows(renglon).Cells(19).Value = evaluacion
                    renglonesNotOK += 1
                End If
                'añadir Geo o blitz
                'código de asignación


                'Info para agregar blitz a asignación
                Dim rdrAgenciaPorAlcaldia As DataSet
                Dim rowBlitzForzado As DataRow()


                Dim agenciaaux As Integer
                Dim rowaux() As DataRow
                Dim stringaux, agenciacdmx As String

                If Not unError Then
                    If validaRenglon(excelToValidate.Worksheet(1).Row(renglon)) Then

                        If excelToValidate.Worksheet(1).Row(renglon).Cell(14).Value.ToString.Length = 5 Then
                            dRowAgencia = rdrCP.Tables(0).Select("CP = '" & excelToValidate.Worksheet(1).Row(renglon).Cell(14).Value & "'")
                        ElseIf excelToValidate.Worksheet(1).Row(renglon).Cell(14).Value.ToString.Length = 4 Then
                            dRowAgencia = rdrCP.Tables(0).Select("CP = '0" & excelToValidate.Worksheet(1).Row(renglon).Cell(14).Value & "'")
                        ElseIf excelToValidate.Worksheet(1).Row(renglon).Cell(1).Value > 5 Then
                            ReDim dRowAgencia(0)
                        End If

                        If dRowAgencia.Length > 0 Then
                            excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = dRowAgencia(0).Item("GEO")
                            excelToValidate.Worksheet(1).Row(renglon).Cell(1).Value = rdrAgencia.Tables(0).Select("ID_AGENCIA = '" & dRowAgencia(0).Item("ID_AGENCIA") & "'")(0).Item("DESC_AGENCIA")
                            If dRowAgencia(0).Item("GEO") = 1 Then
                                numgeo1 += 1
                            ElseIf dRowAgencia(0).Item("GEO") = 2 Then
                                numgeo2 += 1
                            End If

                            iAck += 1
                        Else
                            'busca el cp dentro de la tabla de códigos postales y de ahi lo relaciona con su geo del estado
                            'dRowBlitz = rdrCP.Tables(0).Select("DESC_REGION = '" & m_Excel.Worksheet(1).Row(iRow + 1).Cell(11).Value & "'")
                            If excelToValidate.Worksheet(1).Row(renglon).Cell(14).Value.ToString.Length = 5 Then
                                dRowBlitz = rdrRel.Select("CP = '" & excelToValidate.Worksheet(1).Row(renglon).Cell(14).Value & "'")
                            ElseIf excelToValidate.Worksheet(1).Row(renglon).Cell(14).Value.ToString.Length = 4 Then
                                dRowBlitz = rdrRel.Select("CP = '0" & excelToValidate.Worksheet(1).Row(renglon).Cell(14).Value & "'")
                            End If

                            If dRowBlitz.Length > 0 Then
                                Select Case dRowBlitz(0).Item("GEO_DEFAULT")
                                    Case 1
                                        excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "Blitz Geo 1"
                                        agenciacdmx = dRowBlitz(0).Item("ID_AGENCIA")
                                        If agenciacdmx = "-837" Then
                                            agenciacdmx = dRowBlitz(0).Item("DESC_REGION")
                                            agenciacdmx = rdrAgenciaAlcaldia.Tables(0).Select("[name] = '" + agenciacdmx + "'")(0).Item("ID_AGENCIA")
                                        End If

                                        agenciacdmx = rdrAgencia.Tables(0).Select("ID_AGENCIA = '" + agenciacdmx + "'")(0).Item("DESC_AGENCIA")

                                        excelToValidate.Worksheet(1).Row(renglon).Cell(1).Value = agenciacdmx

                                        iBlitz50 += 1
                                    Case 2
                                        excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "Blitz Geo 2"
                                        agenciacdmx = dRowBlitz(0).Item("ID_AGENCIA")
                                        If agenciacdmx = "-837" Then
                                            agenciacdmx = dRowBlitz(0).Item("DESC_REGION")
                                            agenciacdmx = rdrAlcaldia.Tables(0).Select("DESC_REGION = '" + agenciacdmx + "'")(0).Item("ID_AGENCIA")
                                        End If

                                        agenciacdmx = rdrAgencia.Tables(0).Select("ID_AGENCIA = '" + agenciacdmx + "'")(0).Item("DESC_AGENCIA")

                                        excelToValidate.Worksheet(1).Row(renglon).Cell(1).Value = agenciacdmx

                                        iBlitz100 += 1
                                    Case Else
                                        excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "Blitz Geo 3"
                                        agenciacdmx = dRowBlitz(0).Item("ID_AGENCIA")
                                        If agenciacdmx = "-837" Then
                                            agenciacdmx = dRowBlitz(0).Item("DESC_REGION")
                                            agenciacdmx = rdrAlcaldia.Tables(0).Select("DESC_REGION = '" + agenciacdmx + "'")(0).Item("ID_AGENCIA")
                                        End If

                                        agenciacdmx = rdrAgencia.Tables(0).Select("ID_AGENCIA = '" + agenciacdmx + "'")(0).Item("DESC_AGENCIA")

                                        excelToValidate.Worksheet(1).Row(renglon).Cell(1).Value = agenciacdmx

                                        iBlitz300 += 1

                                End Select
                                'iAck += 1
                            Else
                                excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "Blitz Geo 3"
                                excelToValidate.Worksheet(1).Row(renglon).Cell(1).Value = "UNDEFINED"
                                iBlitz300 += 1
                            End If
                        End If

                        'Comment out del cambio de alcaldía
                        'Else
                        '    If excelToValidate.Worksheet(1).Row(renglon).Cell(15).Value.ToString.Length > 0 And excelToValidate.Worksheet(1).Row(renglon).Cell(16).Value.ToString.Length > 0 Then
                        '        Dim tmpEstado As String = excelToValidate.Worksheet(1).Row(renglon).Cell(15).Value.ToString
                        '        Dim tmpAlcaldia As String = excelToValidate.Worksheet(1).Row(renglon).Cell(16).Value.ToString
                        '        If tmpEstado = "Querétaro" Then
                        '            tmpEstado = "Santiago de Querétaro"
                        '        End If
                        '        dRowAlcaldia = rdrAlcaldia.Tables(0).Select("DESC_REGION = '" & tmpAlcaldia & "' AND  DESC_ESTADO = '" & tmpEstado & "'")

                        '        If dRowAlcaldia.Length > 0 Then
                        '            excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = dRowAlcaldia(0).Item("GEO")
                        '            If dRowAgencia(0).Item("GEO") = 1 Then
                        '                numgeo1 += 1
                        '            ElseIf dRowAgencia(0).Item("GEO") = 2 Then
                        '                numgeo2 += 1
                        '            End If
                        '        Else
                        '            tmpEstado = excelToValidate.Worksheet(1).Row(renglon).Cell(11).Value.ToString
                        '            dRowAlcaldia = rdrAlcaldia.Tables(0).Select("DESC_ESTADO = '" & tmpEstado & "'")
                        '            If dRowAlcaldia.Length > 0 Then
                        '                Select Case dRowAlcaldia(0).Item("GEO")
                        '                    Case 1
                        '                        excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "Blitz Geo 1"
                        '                        iBlitz50 += 1
                        '                    Case 2
                        '                        excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "Blitz Geo 2"
                        '                        iBlitz100 += 1
                        '                    Case Else
                        '                        excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "Blitz Geo 3"
                        '                        iBlitz300 += 1
                        '                End Select
                        '            Else
                        '                excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "NOT ACCEPTED"
                        '                iNack += 1

                        '            End If
                        '        End If
                        '    Else
                        '        excelToValidate.Worksheet(1).Row(renglon).Cell(18).Value = "NOT ACCEPTED"
                        '        iNack += 1
                        '    End If
                    End If
                    'EOcódigo de asignación
                Else
                    iNack += 1
                End If
                unError = False
                agenciacdmx = ""
            Next
            'salvar archivo
            excelToValidate.Save()

            Dim resultado As Evaluacion = New Evaluacion()
            resultado.totalRenglones = renglon - 2
            resultado.renglonesNOTOK = renglonesNotOK
            resultado.renglonesOk = renglon - 1 - renglonesNotOK
            hfJSONresult.Value = JsonConvert.SerializeObject(resultado)
            hfNotOK.Value = iNack
            hfTotal.Value = resultado.totalRenglones
            hfGeo1.Value = numgeo1
            hfGeo2.Value = numgeo2
            hfBlitzGeo1.Value = iBlitz50
            hfBlitzGeo2.Value = iBlitz100
            hfBlitzGeo3.Value = iBlitz300
            hlDownload.NavigateUrl = "/tmpFile/" + nombrerevisado
        Else
            Threading.Thread.Sleep(5000)
        End If

    End Sub

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
End Class