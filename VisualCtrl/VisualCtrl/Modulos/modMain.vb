Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Security
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml
Imports System.Net.Mail

Module modMain
    Public encode As New cls_EncriptacionRSA

    Public Structure edo
        Public Const Search As String = "BUSQUEDA"
        Public Const Insert As String = "INGRESO"
        Public Const Edit As String = "EDICION"
    End Structure

    Public Structure TipoLog
        Public Const Apertura As String = "APERTURA"
        Public Const Cierre As String = "CIERRE"
        Public Const Modificacion As String = "MODIFICACION"
        Public Const Ingreso As String = "INGRESO"
        Public Const Egreso As String = "EGRESO"
        Public Const Borrado As String = "BORRADO"
        Public Const Consulta As String = "CONSULTA"
        Public Const Carga As String = "CARGA"
        Public Const Envio As String = "ENVIO"
        Public Const Cancelacion As String = "CANCELACION"
        Public Const Impresion As String = "IMPRESION"
    End Structure


    Public Structure strucAsignatura
        Public idAsignatura As String
        Public ciclo As String
        Public calificacion As String
        Public idObservaciones As String
        Public idTipoAsignatura As String
        Public creditos As String
    End Structure


    Public Sub BORRAR_LOG()
        Dim sSql As String, sFECHA_LOG As String, st As New System.Diagnostics.StackTrace()
        sFECHA_LOG = FORMATEAR_FECHA(DateAdd("d", -180, Now), "C")
        sSql = "DELETE FROM LOG_SISTEMA WHERE FH_LOG < '" + sFECHA_LOG + "'"
        ExecuteCmd(sSql)
    End Sub

    Public Function dsOpenDB(inSQL As String) As DataSet
        Dim ds As New DataSet
        Dim connStr As String
        connStr = CONEXION()
        Dim connection As SqlConnection
        Dim command As SqlCommand
        Dim adapter As New SqlDataAdapter
        connection = New SqlConnection(connStr)
        Try
            connection.Open()
            command = New SqlCommand(inSQL, connection)
            command.CommandTimeout = 0
            adapter.SelectCommand = command
            adapter.SelectCommand.CommandTimeout = 0
            adapter.Fill(ds)
            adapter.Dispose()
            command.Dispose()
            connection.Close()
        Catch ex As Exception
            If Debugger.IsAttached Then
                MsgBox(ex.Message)
            End If
        Finally
            If connection.State = ConnectionState.Open Then
                connection.Close()
            End If
        End Try
        Return ds
    End Function

    Public Function CONEXION() As String
        Dim sRES As String = ""
        sRES = "server=" + AppSettings("MAIN_SERVER") & ";"
        sRES += "uid=" + AppSettings("USER_DB") + ";"
        If AppSettings("MAIN_SERVER") = "65.99.252.110" Then
            sRES += "pwd=@Acceso1#;"
        Else
            sRES += "pwd=@Acceso1#;"
        End If
        sRES += "database=" + AppSettings("DATABASE") + ";"
        sRES &= "Connection Timeout=0;"
        Return sRES
    End Function

    Public s_Clave As String = "@@yyyymiviejamulayanoesloqueerayanoesloqueerayanoesloqueera@@@1235##"

    Public Function ConnProcesoLargo() As SqlConnection
        Dim OleDbConn As New SqlClient.SqlConnection
        Try
            Dim cmdExp As SqlClient.SqlCommand
            OleDbConn.ConnectionString = CONEXION()
            OleDbConn.Open()
            Return OleDbConn
        Catch ex As Exception
            If Debugger.IsAttached Then
                MsgBox(ex.Message.ToString, vbCritical)
            End If
            Return Nothing
        End Try
    End Function


    Public Function ExecuteCmd(ByVal StrSql As String) As Boolean
        Dim ConnSql As String
        ConnSql = CONEXION()
        Dim OleDbConn As New SqlClient.SqlConnection
        Try
            Dim cmdExp As SqlClient.SqlCommand
            OleDbConn.ConnectionString = ConnSql
            cmdExp = New SqlClient.SqlCommand(StrSql, OleDbConn)
            cmdExp.CommandTimeout = 0
            OleDbConn.Open()
            cmdExp.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            If Debugger.IsAttached Then
                MsgBox(ex.Message.ToString, vbCritical)
            End If
            Return False
        Finally
            If OleDbConn.State = ConnectionState.Open Then
                OleDbConn.Close()
            End If
        End Try
    End Function

    Public Function ExecuteCmdScalar(ByVal StrSql As String) As String
        Dim ConnSql As String
        Dim res As String = ""
        ConnSql = CONEXION()
        Dim OleDbConn As New SqlClient.SqlConnection
        Try
            Dim cmdExp As SqlClient.SqlCommand
            OleDbConn.ConnectionString = ConnSql
            cmdExp = New SqlClient.SqlCommand(StrSql, OleDbConn)
            cmdExp.CommandTimeout = 0
            OleDbConn.Open()
            res = cmdExp.ExecuteScalar()
        Catch ex As Exception
            If Debugger.IsAttached Then
                MsgBox(ex.Message.ToString, vbCritical)
            End If
            Return ""
        Finally
            If OleDbConn.State = ConnectionState.Open Then
                OleDbConn.Close()
            End If
        End Try
        Return res
    End Function




    Public Function FORMATEAR_FECHA(ByVal inDATE As String, ByVal inTIPO As String) As String
        Dim sTEMP_DATE As Date, sFECHA As String = ""
        If Not IsNothing(inDATE) Then
            If inDATE <> "" And IsDate(inDATE) Then
                Try
                    Select Case UCase(inTIPO)
                        Case "L"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "ddd dd MMM yyyy"), GetType(String))
                        Case "M"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "dd/MM/yyyy HH:mm"), GetType(String))
                        Case "STR"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "ddMMyyyyHHmmss"), GetType(String))
                        Case "T"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy-MM-ddT00:00:00"), GetType(String))
                        Case "C"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy-MM-dd HH:mm"), GetType(String))
                        Case "CM"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy-MM-dd HH:mm"), GetType(String))
                        Case "S"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy-MM-dd"), GetType(String))
                        Case "I"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy-MM-dd"), GetType(String)) + " 00:00"
                            sTEMP_DATE = Convert.ChangeType(sFECHA, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy-MM-dd HH:mm"), GetType(String))
                        Case "F"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy-MM-dd"), GetType(String)) + " 23:59"
                            sTEMP_DATE = Convert.ChangeType(sFECHA, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy-MM-dd HH:mm"), GetType(String))
                        Case "MC"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "dd/MM/yyyy HH:mm"), GetType(String))
                        Case "MS"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "dd/MM/yyyy"), GetType(String))
                        Case "MSC"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "dd/MM/yyyy HH:mm"), GetType(String))
                        Case "MSE"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "dd-MM-yyyy"), GetType(String))
                        Case "AÑO"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyy"), GetType(String))
                        Case "DIA"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "dd"), GetType(String))
                        Case "MES"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "MM"), GetType(String))
                        Case "TIME", "TIMES"
                            If Len(inDATE) > 4 Then
                                If Len(inDATE) > 10 Then
                                    sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                                Else
                                    sTEMP_DATE = "21-07-1970 " + inDATE 'JEJEJEJE UNA PEQUEÑA TRAMPA PERO FUNCIONA
                                End If
                                sFECHA = Format(sTEMP_DATE, "HH:mm")
                            Else
                                sFECHA = "ERROR EN FORMATO"
                            End If
                        Case "HORAS"
                            If Len(inDATE) > 4 Then
                                If Len(inDATE) > 10 Then
                                    sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                                Else
                                    sTEMP_DATE = "21-07-1970 " + inDATE 'JEJEJEJE UNA PEQUEÑA TRAMPA PERO FUNCIONA
                                End If
                                sTEMP_DATE = Convert.ChangeType(sTEMP_DATE, GetType(DateTime))
                                sFECHA = Format(sTEMP_DATE, "HH")
                            Else
                                sFECHA = "ERROR EN FORMATO"
                            End If
                        Case "MINUTOS"
                            If Len(inDATE) > 4 Then
                                If Len(inDATE) > 10 Then
                                    sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                                Else
                                    sTEMP_DATE = "21-07-1970 " + inDATE 'JEJEJEJE UNA PEQUEÑA TRAMPA PERO FUNCIONA
                                End If
                                sTEMP_DATE = Convert.ChangeType(sTEMP_DATE, GetType(DateTime))
                                sFECHA = Format(sTEMP_DATE, "mm")
                            Else
                                sFECHA = "ERROR EN FORMATO"
                            End If
                        Case "SEGUNDOS"
                            If Len(inDATE) > 4 Then
                                If Len(inDATE) > 10 Then
                                    sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                                Else
                                    sTEMP_DATE = "21-07-1970 " + inDATE 'JEJEJEJE UNA PEQUEÑA TRAMPA PERO FUNCIONA
                                End If
                                sTEMP_DATE = Convert.ChangeType(sTEMP_DATE, GetType(DateTime))
                                sFECHA = Format(sTEMP_DATE, "ss")
                            Else
                                sFECHA = "ERROR EN FORMATO"
                            End If
                        Case "REPORTE"
                            sTEMP_DATE = Convert.ChangeType(inDATE, GetType(DateTime))
                            sFECHA = Convert.ChangeType(Format(sTEMP_DATE, "yyyyMMddHHmmss"), GetType(String))
                    End Select
                Catch ex As Exception
                    sFECHA = ""
                End Try
            End If
        End If
        Return sFECHA
    End Function

    Public Function TrUc(ByVal StrLine As String) As String
        TrUc = Trim(UCase(StrLine))
    End Function

    Public Function bCAMPO_EXISTE(ByVal TablaIn As String, ByVal CampoIn As String, ByVal ValorIn As String) As Boolean
        Dim st As New System.Diagnostics.StackTrace()
        Dim dsTEMP As DataSet, sSqlT As String, bRES As Boolean = False
        Try
            If Trim(getDATO(False, TablaIn)) <> "" And Trim(getDATO(False, CampoIn)) <> "" And Trim(getDATO(False, ValorIn)) <> "" Then
                sSqlT = "SELECT * FROM " + TablaIn + " WHERE " + CampoIn + "='" + ValorIn + "'"
                dsTEMP = dsOpenDB(sSqlT)
                If dsTEMP.Tables(0).Rows.Count > 0 Then
                    bRES = True
                End If
                CIERRA_DATASET(dsTEMP)
            End If
        Catch ex As Exception
            logFILE(st.GetFrame(0).GetMethod.Name, ex.Message)
            logFILE(st.GetFrame(1).GetMethod.Name, ex.Message)
            If Debugger.IsAttached Then
                MsgBox(ex.Message, vbCritical)
            End If
        End Try
        Return bRES
    End Function

    Public Sub CIERRA_DATASET(inDS As DataSet)
        If Not IsNothing(inDS) Then
            With inDS
                .Clear()
                .Dispose()
            End With
            inDS = Nothing
        End If
    End Sub

    Public Sub CARGAR_COMBO(ByRef ComboIn As DropDownList, ByVal sSql As String, ByVal ID_FIELD As String,
                          ByVal DESC_FIELD As String, Optional ByVal AddBlank As Boolean = False, Optional _
                          bIgnoreValue As Boolean = False)
        Dim rdr As DataSet
        rdr = dsOpenDB(sSql)
        ComboIn.Items.Clear()
        If AddBlank Then
            ComboIn.Items.Add("")
            ComboIn.Items(0).Value = ""
        End If
        For I As Integer = 0 To rdr.Tables(0).Rows.Count - 1
            If AddBlank Then
                ComboIn.Items.Add(rdr.Tables(0).Rows(I).Item(DESC_FIELD))
                'Debug.Print(rdr.Tables(0).Rows(I).Item(DESC_FIELD))
                'Debug.Print(rdr.Tables(0).Rows(I).Item(ID_FIELD))
                If Not bIgnoreValue Then
                    ComboIn.Items(I + 1).Value = rdr.Tables(0).Rows(I).Item(ID_FIELD)
                End If
            Else
                ComboIn.Items.Add(rdr.Tables(0).Rows(I).Item(DESC_FIELD))
                If Not bIgnoreValue Then
                    ComboIn.Items(I).Value = rdr.Tables(0).Rows(I).Item(ID_FIELD)
                End If
            End If
        Next
    End Sub

    Public Function VALORINTABLA(ByVal CampoOut As String, ByVal Tabla As String,
                                 ByVal CampoWhere As String, ByVal ValorWhere As String) As String
        Dim dsTEMP As DataSet, sRES As String = ""
        If CampoWhere <> "" And ValorWhere <> "" Then
            dsTEMP = dsOpenDB("SELECT " + CampoOut + " FROM " + Tabla + " WHERE " + CampoWhere + "='" + ValorWhere + "'")
        Else
            If CampoWhere <> "" Or ValorWhere <> "" Then
                Return sRES
            Else
                dsTEMP = dsOpenDB("SELECT " + CampoOut + " FROM " + Tabla)
            End If
        End If
        If dsTEMP.Tables(0).Rows.Count > 0 Then
            With dsTEMP.Tables(0).Rows(0)
                sRES = getDATO(False, .Item(CampoOut))
            End With
        End If
        CIERRA_DATASET(dsTEMP)
        Return sRES
    End Function

    Public Function bEXISTE_REGISTRO(insSql As String) As Boolean
        Dim dsEXISTE As DataSet, bEXISTE As Boolean = False, st As New System.Diagnostics.StackTrace()
        Try
            dsEXISTE = dsOpenDB(insSql)
            If dsEXISTE.Tables(0).Rows.Count > 0 Then
                bEXISTE = True
            End If
            CIERRA_DATASET(dsEXISTE)
        Catch ex As Exception
            logFILE(st.GetFrame(0).GetMethod.Name, ex.Message)
            logFILE(st.GetFrame(1).GetMethod.Name, ex.Message)
            If Debugger.IsAttached Then
                MsgBox(ex.Message, vbCritical)
                System.Diagnostics.Debug.WriteLine(ex.Message)
            End If
        End Try
        Return bEXISTE
    End Function

    Public Function QUITACOMA(inSTRING As String) As String
        Dim sRES As String = ""
        For i As Integer = 1 To Len(inSTRING)
            If Mid(inSTRING, i, 1) <> "," Then
                sRES += Mid(inSTRING, i, 1)
            End If
        Next
        Return sRES
    End Function

    Public Function getDATO(ByVal bCheckBox As Boolean, ByVal inOBJECT As Object, Optional ByVal strTextbox As Boolean = False) As String
        Dim res As String
        If strTextbox Then
            If Not IsNothing(inOBJECT) Then
                If inOBJECT = "-1" Then
                    Return ""
                Else
                    Return inOBJECT
                End If
            End If
        End If
        If bCheckBox Then
            If IsDBNull(inOBJECT) Then
                Return False
            Else
                If inOBJECT = -1 Then
                    Return True
                Else
                    Return False
                End If
            End If
        Else
            If Not IsDBNull(inOBJECT) Then
                If Not IsNothing(inOBJECT) Then
                    If inOBJECT.ToString <> "NULL" Then
                        res = inOBJECT
                    Else
                        res = ""
                    End If
                Else
                    res = ""
                End If
            Else
                res = ""
            End If
            Return res
        End If
    End Function

    Public Function VALORINTABLAMULTIWHERE(inCAMPO_OUT As String, inTABLA As String, inSTRING_WHERE As String) As String
        Dim st As New System.Diagnostics.StackTrace(), dsTEMP As DataSet, sRES As String = ""
        Try
            If Trim(getDATO(False, inSTRING_WHERE)) <> "" Then
                dsTEMP = dsOpenDB("SELECT " + inCAMPO_OUT + " FROM " + inTABLA + " " + Trim(getDATO(False, inSTRING_WHERE)))
            Else
                dsTEMP = dsOpenDB("SELECT " + inCAMPO_OUT + " FROM " + inTABLA)
            End If
            If dsTEMP.Tables(0).Rows.Count > 0 Then
                sRES = getDATO(False, dsTEMP.Tables(0).Rows(0).Item(inCAMPO_OUT))
            End If
            CIERRA_DATASET(dsTEMP)
        Catch ex As Exception
            logFILE(st.GetFrame(0).GetMethod.Name, ex.Message)
            logFILE(st.GetFrame(1).GetMethod.Name, ex.Message)
            If Debugger.IsAttached Then
                MsgBox(ex.Message)
            End If
        End Try
        Return sRES
    End Function

    Public Function CONVERT_TOBYTE_ARRAY(ByVal value As System.Drawing.Bitmap) As Byte()
        Dim bitmapBytes As Byte()
        Using stream As New System.IO.MemoryStream
            value.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg)
            bitmapBytes = stream.ToArray
        End Using
        Return bitmapBytes
    End Function

    Public Function IMAGE_2_BYTES(inIMAGEN As Image) As Byte()
        Dim sTemp As String = Path.GetTempFileName
        Dim fs As New FileStream(sTemp, FileMode.OpenOrCreate, FileAccess.ReadWrite)
        inIMAGEN.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg)
        fs.Position = 0
        '
        Dim imgLength As Integer = CInt(fs.Length)
        Dim bytes(0 To imgLength - 1) As Byte
        fs.Read(bytes, 0, imgLength)
        fs.Close()
        Return bytes
    End Function

    Public Function FILE_2_BYTES(inFILEPath As String) As Byte()
        Dim buffer() As Byte
        Dim res As String = ""
        Try
            Using fstream As FileStream = System.IO.File.OpenRead(inFILEPath)
                ReDim buffer(CInt(fstream.Length - 1))
                fstream.Read(buffer, 0, buffer.Length)
            End Using
            Dim stringValue As String = Convert.ToBase64String(buffer)
            res = stringValue
            Return buffer
        Catch ex As Exception
            logFILE("ERROR:", ex.Message.ToString)
            Return Nothing
        End Try

    End Function


    Public Function BYTES_2_IMAGE(ByVal bytes() As Byte) As Image
        Dim st As New System.Diagnostics.StackTrace()
        If bytes Is Nothing Then Return Nothing
        '
        Dim ms As New MemoryStream(bytes)
        Dim bm As Bitmap = Nothing
        Try
            bm = New Bitmap(ms)
        Catch ex As Exception
            logFILE(st.GetFrame(0).GetMethod.Name, ex.Message)
            logFILE(st.GetFrame(1).GetMethod.Name, ex.Message)
            If Debugger.IsAttached Then
                MsgBox(ex.Message, vbCritical)
                System.Diagnostics.Debug.WriteLine(ex.Message)
            End If
        End Try
        Return bm
    End Function

    Public Function GET_BASE_64(inFILENAME As String) As String
        Dim buffer() As Byte
        Dim res As String = ""
        Try
            Using fstream As FileStream = System.IO.File.OpenRead(inFILENAME)
                'ReDim buffer(CInt(fstream.Length - 1))
                ReDim buffer(CInt(fstream.Length))
                fstream.Read(buffer, 0, buffer.Length)
            End Using
            Dim stringValue As String = Convert.ToBase64String(buffer)
            res = stringValue
        Catch ex As Exception
            If Debugger.IsAttached Then
                MsgBox(ex.Message.ToString, vbCritical)
            End If
        End Try
        Return res
    End Function

    Public Function FORMAT_NUM(inNUMERO As String) As String
        Dim sRES As String = "0.00"
        If Trim(inNUMERO) = "." Then
            sRES = "0.00"
        Else
            If inNUMERO <> "" Then
                If IsNumeric(inNUMERO) Then
                    sRES = FormatNumber(inNUMERO, 2, vbFalse)
                Else
                    sRES = "0.00"
                End If
            Else
                sRES = "0.00"
            End If
        End If
        Return sRES
    End Function

    Public Function bEMAIL_VALIDO(inEMAIL As String) As Boolean
        Try
            Dim vEmailAddress As New System.Net.Mail.MailAddress(inEMAIL)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Sub logFILE(inTIPO As String, inVALOR As String)
        Dim sFile As String
        sFile = HttpContext.Current.Request.MapPath("~/Log/" + FORMATEAR_FECHA(Now, "S") + ".log")

        Dim objStreamWriter As StreamWriter
        objStreamWriter = New StreamWriter(sFile, True)

        objStreamWriter.WriteLine(FORMATEAR_FECHA(Now, "C") + " " + inTIPO + ": " + inVALOR)
        objStreamWriter.Close()

    End Sub

    Public Function WRAP_TEXT(ByVal Text As String, ByVal LineLength As Integer) As List(Of String)

        Dim ReturnValue As New List(Of String)

        Text = Trim(Text)


        Dim Words As String() = Text.Split(" ")

        If Words.Length = 1 And Words(0).Length > LineLength Then

            ' Text is just one big word that is longer than one line
            ' Split it mercilessly
            Dim lines As Integer = (Int(Text.Length / LineLength) + 1)
            Text = Text.PadRight(lines * LineLength)
            For i As Integer = 0 To lines - 1
                Dim SliceStart As Integer = i * LineLength
                ReturnValue.Add(Text.Substring(SliceStart, LineLength))
            Next
        Else
            Dim CurrentLine As New System.Text.StringBuilder
            For Each Word As String In Words
                ' will this word fit on the current line?
                If CurrentLine.Length + Word.Length < LineLength Then
                    CurrentLine.Append(Word & " ")
                Else
                    ' is the word too long for one line
                    If Word.Length > LineLength Then
                        Dim Slice As String =
                        Word.Substring(0, LineLength - CurrentLine.Length)
                        CurrentLine.Append(Slice)
                        ReturnValue.Add(CurrentLine.ToString)
                        CurrentLine = New System.Text.StringBuilder()

                        Word = Word.Substring(Slice.Length,
                      Word.Length - Slice.Length)

                        Dim RemainingSlices As Integer =
                      Int(Word.Length / LineLength) + 1
                        For LineNumber As Integer = 1 To RemainingSlices
                            If LineNumber = RemainingSlices Then
                                CurrentLine.Append(Word & " ")
                            Else


                                Slice = Word.Substring(0,
                                LineLength)
                                CurrentLine.Append(Slice)

                                ReturnValue.Add(CurrentLine.ToString)
                                CurrentLine = New System.Text.StringBuilder

                                Word = Word.Substring(Slice.Length, Word.Length - Slice.Length)
                            End If
                        Next
                    Else

                        ReturnValue.Add(CurrentLine.ToString)
                        CurrentLine = New System.Text.StringBuilder(Word & " ")
                    End If
                End If
            Next

            If CurrentLine.Length > 0 Then
                ReturnValue.Add(CurrentLine.ToString)
            End If
        End If
        Return ReturnValue
    End Function

    Public Function ES_SABADO(inFECHA As String) As Boolean
        If inFECHA <> "" Then
            If Weekday(CDate(inFECHA)) = 7 Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function ES_DOMINGO(inFECHA As String) As Boolean
        If inFECHA <> "" Then
            If Weekday(CDate(inFECHA)) = 1 Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function DIA_HABIL_SIN_FINES(inFECHA As String) As String
        Dim sFECHA As String = inFECHA
        If inFECHA <> "" Then
            If Weekday(CDate(inFECHA)) = 7 Then
                sFECHA = FORMATEAR_FECHA(DateAdd("D", 2, CDate(inFECHA)), "S")
            ElseIf Weekday(CDate(inFECHA)) = 1 Then
                sFECHA = FORMATEAR_FECHA(DateAdd("D", 1, CDate(inFECHA)), "S")
            End If
        End If
        Return sFECHA
    End Function

    Public Function BARRAS_128(ByVal tcString As String) As String
        Dim lcStart, lcStop, lcRet, lcCheck
        Dim lnLong, lnI, lnCheckSum, lnAsc, aa

        lcStart = Chr(104 + 32)
        lcStop = Chr(106 + 32)
        lnCheckSum = Asc(lcStart) - 32
        lcRet = tcString
        lnLong = Len(lcRet)
        For lnI = 1 To lnLong
            lnAsc = Asc(Mid(lcRet, lnI, 1)) - 32
            If Not lnAsc >= 0 And lnAsc <= 99 Then
                lcRet = Replace(lcRet, lnI, 1, AscW(Chr(32)))
                lnAsc = Asc(Mid(lcRet, lnI, 1)) - 32
            End If
            lnCheckSum = lnCheckSum + (lnAsc * lnI)
        Next
        aa = lnCheckSum Mod 103
        aa = aa + 32
        lcCheck = Chr(aa)
        lcRet = lcStart + lcRet + lcCheck + lcStop
        '*--- Esto es para cambiar los espacios y caracteres invalidos
        lcRet = Replace(lcRet, Chr(32), Chr(232))
        lcRet = Replace(lcRet, Chr(127), Chr(192))
        lcRet = Replace(lcRet, Chr(128), Chr(193))
        '*---
        BARRAS_128 = lcRet
    End Function

    Public Function NUM_MUESTRAS_SOLICITUD(inNUM_SOLICITUD) As String
        Dim sRES As String = "", dsNUM_MUESTRAS As DataSet
        If IsNumeric(inNUM_SOLICITUD) Then
            dsNUM_MUESTRAS = dsOpenDB("SELECT COUNT(*) AS TOTAL FROM SOLICITUDES_DETALLES WHERE NUM_SOLICITUD = " & inNUM_SOLICITUD)
            sRES = dsNUM_MUESTRAS.Tables(0).Rows(0).Item("TOTAL").ToString
            CIERRA_DATASET(dsNUM_MUESTRAS)
        End If
        Return sRES
    End Function

    Public Function LETRAS(ByVal numero As String) As String
        '********Declara variables de tipo cadena************
        Dim palabras, entero, dec, flag As String

        palabras = ""
        entero = ""
        dec = ""

        '********Declara variables de tipo entero***********
        Dim num, x, y As Integer

        flag = "N"

        '**********Número Negativo***********
        If Mid(numero, 1, 1) = "-" Then
            numero = Mid(numero, 2, numero.ToString.Length - 1).ToString
            palabras = "menos "
        End If

        '**********Si tiene ceros a la izquierda*************
        For x = 1 To numero.ToString.Length
            If Mid(numero, 1, 1) = "0" Then
                numero = Trim(Mid(numero, 2, numero.ToString.Length).ToString)
                If Trim(numero.ToString.Length) = 0 Then palabras = ""
            Else
                Exit For
            End If
        Next

        '*********Dividir parte entera y decimal************
        For y = 1 To Len(numero)
            If Mid(numero, y, 1) = "." Then
                flag = "S"
            Else
                If flag = "N" Then
                    entero = entero + Mid(numero, y, 1)
                Else
                    dec = dec + Mid(numero, y, 1)
                End If
            End If
        Next y

        If Len(dec) = 1 Then dec = dec & "0"

        '**********proceso de conversión***********
        flag = "N"

        If Val(numero) <= 999999999 Then
            For y = Len(entero) To 1 Step -1
                num = Len(entero) - (y - 1)
                Select Case y
                    Case 3, 6, 9
                        '**********Asigna las palabras para las centenas***********
                        Select Case Mid(entero, num, 1)
                            Case "1"
                                If Mid(entero, num + 1, 1) = "0" And Mid(entero, num + 2, 1) = "0" Then
                                    palabras = palabras & "cien "
                                Else
                                    palabras = palabras & "ciento "
                                End If
                            Case "2"
                                palabras = palabras & "doscientos "
                            Case "3"
                                palabras = palabras & "trescientos "
                            Case "4"
                                palabras = palabras & "cuatrocientos "
                            Case "5"
                                palabras = palabras & "quinientos "
                            Case "6"
                                palabras = palabras & "seiscientos "
                            Case "7"
                                palabras = palabras & "setecientos "
                            Case "8"
                                palabras = palabras & "ochocientos "
                            Case "9"
                                palabras = palabras & "novecientos "
                        End Select
                    Case 2, 5, 8
                        '*********Asigna las palabras para las decenas************
                        Select Case Mid(entero, num, 1)
                            Case "1"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    flag = "S"
                                    palabras = palabras & "diez "
                                End If
                                If Mid(entero, num + 1, 1) = "1" Then
                                    flag = "S"
                                    palabras = palabras & "once "
                                End If
                                If Mid(entero, num + 1, 1) = "2" Then
                                    flag = "S"
                                    palabras = palabras & "doce "
                                End If
                                If Mid(entero, num + 1, 1) = "3" Then
                                    flag = "S"
                                    palabras = palabras & "trece "
                                End If
                                If Mid(entero, num + 1, 1) = "4" Then
                                    flag = "S"
                                    palabras = palabras & "catorce "
                                End If
                                If Mid(entero, num + 1, 1) = "5" Then
                                    flag = "S"
                                    palabras = palabras & "quince "
                                End If
                                If Mid(entero, num + 1, 1) > "5" Then
                                    flag = "N"
                                    palabras = palabras & "dieci"
                                End If
                            Case "2"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "veinte "
                                    flag = "S"
                                Else
                                    palabras = palabras & "veinti"
                                    flag = "N"
                                End If
                            Case "3"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "treinta "
                                    flag = "S"
                                Else
                                    palabras = palabras & "treinta y "
                                    flag = "N"
                                End If
                            Case "4"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "cuarenta "
                                    flag = "S"
                                Else
                                    palabras = palabras & "cuarenta y "
                                    flag = "N"
                                End If
                            Case "5"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "cincuenta "
                                    flag = "S"
                                Else
                                    palabras = palabras & "cincuenta y "
                                    flag = "N"
                                End If
                            Case "6"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "sesenta "
                                    flag = "S"
                                Else
                                    palabras = palabras & "sesenta y "
                                    flag = "N"
                                End If
                            Case "7"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "setenta "
                                    flag = "S"
                                Else
                                    palabras = palabras & "setenta y "
                                    flag = "N"
                                End If
                            Case "8"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "ochenta "
                                    flag = "S"
                                Else
                                    palabras = palabras & "ochenta y "
                                    flag = "N"
                                End If
                            Case "9"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "noventa "
                                    flag = "S"
                                Else
                                    palabras = palabras & "noventa y "
                                    flag = "N"
                                End If
                        End Select
                    Case 1, 4, 7
                        '*********Asigna las palabras para las unidades*********
                        Select Case Mid(entero, num, 1)
                            Case "1"
                                If flag = "N" Then
                                    If y = 1 Then
                                        palabras = palabras & "uno "
                                    Else
                                        palabras = palabras & "un "
                                    End If
                                End If
                            Case "2"
                                If flag = "N" Then palabras = palabras & "dos "
                            Case "3"
                                If flag = "N" Then palabras = palabras & "tres "
                            Case "4"
                                If flag = "N" Then palabras = palabras & "cuatro "
                            Case "5"
                                If flag = "N" Then palabras = palabras & "cinco "
                            Case "6"
                                If flag = "N" Then palabras = palabras & "seis "
                            Case "7"
                                If flag = "N" Then palabras = palabras & "siete "
                            Case "8"
                                If flag = "N" Then palabras = palabras & "ocho "
                            Case "9"
                                If flag = "N" Then palabras = palabras & "nueve "
                        End Select
                End Select

                '***********Asigna la palabra mil***************
                If y = 4 Then
                    If Mid(entero, 6, 1) <> "0" Or Mid(entero, 5, 1) <> "0" Or Mid(entero, 4, 1) <> "0" Or
                    (Mid(entero, 6, 1) = "0" And Mid(entero, 5, 1) = "0" And Mid(entero, 4, 1) = "0" And
                    Len(entero) <= 6) Then palabras = palabras & "mil "
                End If

                '**********Asigna la palabra millón*************
                If y = 7 Then
                    If Len(entero) = 7 And Mid(entero, 1, 1) = "1" Then
                        palabras = palabras & "millón "
                    Else
                        palabras = palabras & "millones "
                    End If
                End If
            Next y

            '**********Une la parte entera y la parte decimal*************
            If dec <> "" Then
                LETRAS = palabras
            Else
                LETRAS = palabras
            End If
        Else
            LETRAS = ""
        End If
    End Function

    Public Function ConvertToByteArray(ByVal value As System.Drawing.Bitmap) As Byte()
        Dim bitmapBytes As Byte()
        Using stream As New System.IO.MemoryStream
            value.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg)
            bitmapBytes = stream.ToArray
        End Using
        Return bitmapBytes
    End Function

    Public Function vigenciaCert(ByVal certPath As String) As String
        Dim res As String = ""
        Try
            Dim objCert As X509Certificate2 = New X509Certificate2(HttpContext.Current.Server.MapPath("~/Certificados/") & certPath.ToString)
            res = objCert.NotAfter.ToString()
            Return res
        Catch ex As Exception
            Return res
        End Try

    End Function

    Public Function GetCadenaOriginal(xmlMS As MemoryStream) As String
        Dim xmlDoc As New XmlDocument
        xmlDoc.Load(xmlMS)
        Dim t As System.Xml.XmlNode

        Dim res As String = ""
        Dim sCadena As String = "||"
        ' nodo dec
        t = xmlDoc.GetElementsByTagName("Dec")(0)
        sCadena &= "2.0|" 'Version()
        sCadena &= t.Attributes("tipoCertificado").Value & "|" 'tipoCertificado()
        ' nodo ipes
        t = xmlDoc.GetElementsByTagName("Ipes")(0)
        sCadena &= t.Attributes("idNombreInstitucion").Value & "|" 'idnombreinstitucion()
        sCadena &= t.Attributes("idCampus").Value & "|" 'idCampus()
        sCadena &= t.Attributes("idEntidadFederativa").Value & "|" 'idEntidadFederativa()
        'nodo responsable
        t = xmlDoc.GetElementsByTagName("Responsable")(0)
        sCadena &= t.Attributes("curp").Value & "|" 'curp()
        sCadena &= t.Attributes("idCargo").Value & "|" 'idcargo()
        'nodo rvoe
        t = xmlDoc.GetElementsByTagName("Rvoe")(0)
        sCadena &= t.Attributes("numero").Value & "|" 'numero()
        sCadena &= t.Attributes("fechaExpedicion").Value & "|" 'fechaExpedicion()

        'nodo carrera
        t = xmlDoc.GetElementsByTagName("Carrera")(0)
        sCadena &= t.Attributes("idCarrera").Value & "|" 'idCarrera()
        sCadena &= t.Attributes("idTipoPeriodo").Value & "|" 'idTipoPeriodo()
        sCadena &= t.Attributes("clavePlan").Value & "|" 'clavePlan()
        sCadena &= t.Attributes("idNivelEstudios").Value & "|" 'idNivelEstudios()
        sCadena &= t.Attributes("calificacionMinima").Value & "|" 'calificacionMinima()
        sCadena &= t.Attributes("calificacionMaxima").Value & "|" 'calificacionMaxima()
        sCadena &= t.Attributes("calificacionMinimaAprobatoria").Value & "|" 'calificacionMinimaAprobatoria()

        'nodo alumno
        t = xmlDoc.GetElementsByTagName("Alumno")(0)
        sCadena &= t.Attributes("numeroControl").Value & "|" 'numeroControl()
        sCadena &= t.Attributes("curp").Value & "|" 'Curp()
        sCadena &= t.Attributes("nombre").Value & "|" 'nombre()
        sCadena &= t.Attributes("primerApellido").Value & "|" 'primerAellido()
        sCadena &= t.Attributes("segundoApellido").Value & "|" 'segundoApellido()
        sCadena &= t.Attributes("idGenero").Value & "|" 'idGenero()
        sCadena &= t.Attributes("fechaNacimiento").Value & "|" 'fechaNacimiento()
        sCadena &= "|" 'foto()
        sCadena &= "|" 'firmaAutografa()

        'nodo expedicion
        t = xmlDoc.GetElementsByTagName("Expedicion")(0)
        sCadena &= t.Attributes("idTipoCertificacion").Value & "|" 'idTipoCeertificacion()
        sCadena &= t.Attributes("fecha").Value & "|" 'fecha()
        sCadena &= t.Attributes("idLugarExpedicion").Value & "|" 'idLugarExpedicion()

        'nodo asignaturas
        t = xmlDoc.GetElementsByTagName("Asignaturas")(0)
        sCadena &= t.Attributes("total").Value & "|" 'total()
        sCadena &= t.Attributes("asignadas").Value & "|" 'asignadas()
        sCadena &= t.Attributes("promedio").Value & "|" 'promedio()
        sCadena &= t.Attributes("totalCreditos").Value & "|" 'totalcreditos()
        sCadena &= t.Attributes("creditosObtenidos").Value & "|" 'creditosObtenidos()

        'nodo asignatura - ciclo
        Dim nodeArr As System.Xml.XmlNodeList
        nodeArr = xmlDoc.GetElementsByTagName("Asignatura")
        For i As Integer = 0 To nodeArr.Count - 1
            sCadena &= nodeArr(i).Attributes("idAsignatura").Value & "|" 'idAsignatura()
            sCadena &= nodeArr(i).Attributes("ciclo").Value & "|" 'ciclo()
            sCadena &= nodeArr(i).Attributes("calificacion").Value & "|" 'calificacion()
            sCadena &= nodeArr(i).Attributes("idTipoAsignatura").Value & "|" 'idTipoAsignatura()
            sCadena &= nodeArr(i).Attributes("creditos").Value & "|" 'creditos()
        Next

        Return sCadena & "|"
    End Function





    Public Function formatCalificacion(ByVal strCal As String) As String
        Dim res As String
        Dim ilValue As Double

        If CDbl(strCal) = 10 Then
            res = FormatNumber(strCal, 0)
        Else

            ilValue = Format(Val(strCal), "###0.00")
            If ilValue > Val(strCal) Then
                strCal = ilValue - 0.01
            Else
                strCal = ilValue
            End If
            res = FormatNumber(strCal, 2)
        End If

        Return res
    End Function

    Public Function GetCadenaOriginalTitulo(xmlMS As MemoryStream) As String

        Dim xmlDoc As New XmlDocument
        xmlDoc.Load(xmlMS)
        Dim t As System.Xml.XmlNode

        Dim res As String = ""
        Dim sCadena As String = "||"
        ' nodo TituloElectronico
        t = xmlDoc.GetElementsByTagName("TituloElectronico")(0)
        sCadena &= t.Attributes("version").Value & "|"
        sCadena &= t.Attributes("folioControl").Value & "|" 'folioControl()

        ' nodo FirmaResponsable
        Dim nodeArr As System.Xml.XmlNodeList
        nodeArr = xmlDoc.GetElementsByTagName("FirmaResponsable")
        For i As Integer = 0 To nodeArr.Count - 1
            sCadena &= nodeArr(i).Attributes("curp").Value & "|" 'curp()
            sCadena &= nodeArr(i).Attributes("idCargo").Value & "|" 'idCargo()
            sCadena &= nodeArr(i).Attributes("cargo").Value & "|" 'cargo()
            sCadena &= "|" 'abrTitulo()
        Next

        'nodo Institucion
        t = xmlDoc.GetElementsByTagName("Institucion")(0)
        sCadena &= t.Attributes("cveInstitucion").Value & "|" 'cveInstitucion()
        sCadena &= t.Attributes("nombreInstitucion").Value & "|" 'nombreInstitucion()

        'nodo Carrera
        t = xmlDoc.GetElementsByTagName("Carrera")(0)
        sCadena &= t.Attributes("cveCarrera").Value & "|" 'cveCarrera()
        sCadena &= t.Attributes("nombreCarrera").Value & "|" 'nombreCarrera()
        sCadena &= t.Attributes("fechaInicio").Value & "|" 'fechaInicio()
        sCadena &= t.Attributes("fechaTerminacion").Value & "|" 'fechaTerminacion()
        sCadena &= t.Attributes("idAutorizacionReconocimiento").Value & "|" 'idAutorizacionReconocimiento()
        sCadena &= t.Attributes("autorizacionReconocimiento").Value & "|" 'autorizacionReconocimiento()
        sCadena &= "|" 'numeroRvoe()

        'nodo Profesionista
        t = xmlDoc.GetElementsByTagName("Profesionista")(0)
        sCadena &= t.Attributes("curp").Value & "|" 'curp()
        sCadena &= t.Attributes("nombre").Value & "|" 'nombre()
        sCadena &= t.Attributes("primerApellido").Value & "|" 'primerApellido()
        sCadena &= t.Attributes("segundoApellido").Value & "|" 'segundoApellido()
        sCadena &= t.Attributes("correoElectronico").Value & "|" 'correoElectronico()

        'nodo Expedicion
        t = xmlDoc.GetElementsByTagName("Expedicion")(0)
        sCadena &= t.Attributes("fechaExpedicion").Value & "|" 'fechaExpedicion()
        sCadena &= t.Attributes("idModalidadTitulacion").Value & "|" 'idModalidadTitulacion()
        sCadena &= t.Attributes("modalidadTitulacion").Value & "|" 'modalidadTitulacion()
        sCadena &= t.Attributes("fechaExamenProfesional").Value & "|" 'fechaExamenProfesional()
        sCadena &= "|" 'fechaExencionExamenProfesional()
        sCadena &= t.Attributes("cumplioServicioSocial").Value & "|" 'cumplioServicioSocial()
        sCadena &= t.Attributes("idFundamentoLegalServicioSocial").Value & "|" 'idFundamentoLegalServicioSocial()
        sCadena &= t.Attributes("fundamentoLegalServicioSocial").Value & "|" 'fundamentoLegalServicioSocial()
        sCadena &= t.Attributes("idEntidadFederativa").Value & "|" 'idEntidadFederativa()
        sCadena &= t.Attributes("entidadFederativa").Value & "|" 'entidadFederativa()

        'nodo Antecedente
        t = xmlDoc.GetElementsByTagName("Antecedente")(0)
        sCadena &= t.Attributes("institucionProcedencia").Value & "|" 'idTipoCeertificacion()
        sCadena &= t.Attributes("idTipoEstudioAntecedente").Value & "|" 'idTipoEstudioAntecedente()
        sCadena &= t.Attributes("tipoEstudioAntecedente").Value & "|" 'tipoEstudioAntecedente()
        sCadena &= t.Attributes("idEntidadFederativa").Value & "|" 'idEntidadFederativa()
        sCadena &= t.Attributes("entidadFederativa").Value & "|" 'entidadFederativa()
        sCadena &= t.Attributes("fechaInicio").Value & "|" 'fechaInicio()
        sCadena &= t.Attributes("fechaTerminacion").Value & "|" 'fechaTerminacio n()
        sCadena &= "|" 'noCedula()

        Return sCadena & "||"
    End Function

    Public Sub INCDOWNLOADS(ByVal idAsignacion)
        Dim rdrDownload As DataSet = dsOpenDB("SELECT FLG_DESCARGA, VECES_DESCARGA, FH_PRIMERA_DESCARGA, FH_ULTIMA_DESCARGA, ID_USUARIO_PRIMER_DESCARGA, ID_USUARIO_ULTIMA_DESCARGA FROM ASIGNACIONES WHERE ID_ASIGNACION = " & idAsignacion)


        Dim sSql As String = "UPDATE ASIGNACIONES SET "
        sSql &= "FLG_DESCARGA = 1, "
        sSql &= "VECES_DESCARGA = VECES_DESCARGA + 1 , "
        If getDATO(False, rdrDownload.Tables(0).Rows(0).Item("FH_PRIMERA_DESCARGA")) = "" Then
            sSql &= "FH_PRIMERA_DESCARGA = '" & FORMATEAR_FECHA(Now, "C") & "', "
        End If
        If getDATO(False, rdrDownload.Tables(0).Rows(0).Item("ID_USUARIO_PRIMER_DESCARGA")) = "" Then
            sSql &= "ID_USUARIO_PRIMER_DESCARGA = '" & HttpContext.Current.Session("idUsuario") & "', "
        End If

        sSql &= "FH_ULTIMA_DESCARGA = '" & FORMATEAR_FECHA(Now, "C") & "', "
        sSql &= "ID_USUARIO_ULTIMA_DESCARGA = '" & HttpContext.Current.Session("idUsuario") & "' "

        sSql &= "WHERE ID_ASIGNACION = " & idAsignacion
        ExecuteCmd(sSql)
    End Sub
    Public Function SEND_MAIL(ByVal idAsignacion As Integer) As String
        Dim rdrMailSettings As DataSet = dsOpenDB("SELECT * FROM MAILSETTINGS")
        Dim rdrAsignacion As DataSet = dsOpenDB("SELECT * FROM ASIGNACIONES INNER JOIN TAB_AGENCIA ON TAB_AGENCIA.ID_AGENCIA = ASIGNACIONES.ID_AGENCIA WHERE ID_ASIGNACION = " & idAsignacion)
        Dim mUser As String
        Dim mpass As String
        Dim mserver As String
        Dim mPort As String
        Dim mNombreEnvio As String
        Dim mDirEnvio As String
        Dim mSecureConn As Boolean
        Dim mMsg As String = ""
        Dim flgIncAttach As Boolean
        If rdrMailSettings.Tables(0).Rows.Count > 0 Then
            mUser = rdrMailSettings.Tables(0).Rows(0).Item("USUARIO")
            mpass = encode.desencriptar128BitRijndael(rdrMailSettings.Tables(0).Rows(0).Item("PWD"), s_Clave)
            mserver = rdrMailSettings.Tables(0).Rows(0).Item("SMTP")
            mPort = rdrMailSettings.Tables(0).Rows(0).Item("PUERTO")
            mNombreEnvio = rdrMailSettings.Tables(0).Rows(0).Item("NOMBRE_ENVIO")
            mDirEnvio = rdrMailSettings.Tables(0).Rows(0).Item("DIR_ENVIO")
            mSecureConn = rdrMailSettings.Tables(0).Rows(0).Item("SECURE_CONN")
            mMsg = getDATO(False, rdrMailSettings.Tables(0).Rows(0).Item("MENSAJE"))
            flgIncAttach = rdrMailSettings.Tables(0).Rows(0).Item("FLG_INC_ATTACH")
        End If


        Dim Message As New System.Net.Mail.MailMessage
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
        Message.From = New MailAddress(mDirEnvio)
#Enable Warning BC42104 ' Variable is used before it has been assigned a value

        Dim strContacto As String = ""

        Dim rdrContacto As DataSet = dsOpenDB("SELECT * FROM TAB_USUARIOS WHERE ID_AGENCIA = " & rdrAsignacion.Tables(0).Rows(0).Item("ID_AGENCIA") & " AND EMAIL <> '' AND FLG_CANC = 0 AND FLG_CONTACTO_AGENCIA = 1 ")
        If rdrContacto.Tables(0).Rows.Count > 0 Then

        Else
            'msg("NO CONTACTS ASSIGNED FOR THIS AGENCY, PLEASE CONFIGURE A USER AS THE MAIN CONTACT FOR THIS AGENCY.")
            Return "NO CONTACTS ASSIGNED FOR THIS AGENCY, PLEASE CONFIGURE A USER AS THE MAIN CONTACT FOR THIS AGENCY."
            Exit Function
        End If

        For iContacto As Integer = 0 To rdrContacto.Tables(0).Rows.Count - 1
            Dim arrMail() As String = rdrContacto.Tables(0).Rows(iContacto).Item("EMAIL").ToString.Split(";")
            For iMail As Integer = 0 To arrMail.Length - 1
                If Debugger.IsAttached Then
                    Message.To.Add(New MailAddress(Trim("jesusefrain@gmail.com")))
                    Message.To.Add(New MailAddress(Trim("adelabra@gmail.com")))
                Else
                    Message.To.Add(New MailAddress(Trim(arrMail(iMail))))
                End If

            Next
            'Message.To.Add(New MailAddress(rdrContacto.Tables(0).Rows(iContacto).Item("EMAIL")))
        Next
        CIERRA_DATASET(rdrContacto)
        Message.Subject = "Assginment document"



        If flgIncAttach Then
            Dim binaryXLS() As Byte = rdrAsignacion.Tables(0).Rows(0).Item("ARCHIVO")
            Dim msAttach As New MemoryStream(binaryXLS)

            Message.Attachments.Add(New Attachment(msAttach, rdrAsignacion.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO").ToString.Replace(" ", "_") & "", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
        End If

        '---attachments


        Dim smail As String
        smail = rdrAsignacion.Tables(0).Rows(0).Item("NOMBRE_CONTACTO") & " " & ControlChars.Cr & ControlChars.Cr
        smail = smail & ControlChars.Cr
        mMsg = mMsg.Replace("[NOMBRE_ARCHIVO]", rdrAsignacion.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO"))
        mMsg = mMsg.Replace("[MES_GENERACION]", MonthName(CInt(rdrAsignacion.Tables(0).Rows(0).Item("FH_ASIGNACION").ToString.Substring(5, 2))))
        mMsg = mMsg.Replace("[TOTAL_ROWS]", rdrAsignacion.Tables(0).Rows(0).Item("ROWS_OK"))
        smail = smail & mMsg
        Message.Body = smail

#Disable Warning BC42104 ' Variable is used before it has been assigned a value
        Dim client As New SmtpClient(mserver)
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
        client.Port = mPort
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
        client.EnableSsl = mSecureConn
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
        client.Credentials = New System.Net.NetworkCredential(mUser, mpass)
#Enable Warning BC42104 ' Variable is used before it has been assigned a value
#Enable Warning BC42104 ' Variable is used before it has been assigned a value

        Try
            client.Send(Message)

            ' msg("MAIL SENT TO AGENCY CONTACT")
            Return ""
        Catch ex As Exception
            If Debugger.IsAttached Then
                MsgBox(ex)
            End If
            Return "Error en el mensaje, verifique que la dirección electrónica del remitente sea la correcta."
        Finally
            CIERRA_DATASET(rdrAsignacion)
            CIERRA_DATASET(rdrMailSettings)
        End Try

    End Function
End Module