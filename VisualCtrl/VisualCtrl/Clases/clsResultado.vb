
Imports System.Data.SqlClient

Public Class clsResultado

    Public ID_ARCHIVO_DATA As Integer
    Public ID_ARCHIVO As Integer
    Public AFILIACION As String
    Public F_VISITA As String
    Public VISITA_EXITOSA As String
    Public HABIA_POP As String
    Public SE_SENALIZO As String
    Public POP_COLOCADO As String
    Public SUPRESION As String
    Public PROBLEMA_TERMINAL As String
    Public TIPO_TERMINAL As String
    Public DESCRIPCION_PROBLEMA As String
    Public OS_RESUELTO As String
    Public SENTIMIENTO_ESTABLECIMIENTO As String
    Public ACTIVACION As String
    Public NUM_AFILIACION_ALTERNO As String
    Public ESTATUS_VISITA As String
    Sub CLEAN()
        ID_ARCHIVO_DATA = -1
        ID_ARCHIVO = -1
        AFILIACION = ""
        F_VISITA = ""
        VISITA_EXITOSA = ""
        HABIA_POP = ""
        SE_SENALIZO = ""
        POP_COLOCADO = ""
        SUPRESION = ""
        PROBLEMA_TERMINAL = ""
        TIPO_TERMINAL = ""
        DESCRIPCION_PROBLEMA = ""
        OS_RESUELTO = ""
        SENTIMIENTO_ESTABLECIMIENTO = ""
        ACTIVACION = ""
        NUM_AFILIACION_ALTERNO = ""
        ESTATUS_VISITA = ""
    End Sub
    Public Sub LOAD(ByVal idRecord As String, ByRef cLoad As clsResultado)
        Dim rdrArchivoDato As DataSet
        'rdrArchivoDato = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO_DATA = '" & idRecord & "'")
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO_DATA = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idRecord
        rdrArchivoDato = dsOpenDB(comm)
        If rdrArchivoDato.Tables(0).Rows.Count > 0 Then
            cLoad.ID_ARCHIVO_DATA = rdrArchivoDato.Tables(0).Rows(0).Item("ID_ARCHIVO_DATA")
            cLoad.ID_ARCHIVO = rdrArchivoDato.Tables(0).Rows(0).Item("ID_ARCHIVO")
            cLoad.AFILIACION = rdrArchivoDato.Tables(0).Rows(0).Item("AFILIACION")
            cLoad.F_VISITA = rdrArchivoDato.Tables(0).Rows(0).Item("F_VISITA")
            cLoad.VISITA_EXITOSA = rdrArchivoDato.Tables(0).Rows(0).Item("VISITA_EXITOSA")
            cLoad.HABIA_POP = rdrArchivoDato.Tables(0).Rows(0).Item("HABIA_POP")
            cLoad.SE_SENALIZO = rdrArchivoDato.Tables(0).Rows(0).Item("SE_SENALIZO")
            cLoad.POP_COLOCADO = rdrArchivoDato.Tables(0).Rows(0).Item("POP_COLOCADO")
            cLoad.SUPRESION = rdrArchivoDato.Tables(0).Rows(0).Item("SUPRESION")
            cLoad.PROBLEMA_TERMINAL = rdrArchivoDato.Tables(0).Rows(0).Item("PROBLEMA_TERMINAL")
            cLoad.TIPO_TERMINAL = rdrArchivoDato.Tables(0).Rows(0).Item("TIPO_TERMINAL")
            cLoad.DESCRIPCION_PROBLEMA = rdrArchivoDato.Tables(0).Rows(0).Item("DESCRIPCION_PROBLEMA")
            cLoad.OS_RESUELTO = rdrArchivoDato.Tables(0).Rows(0).Item("OS_RESUELTO")
            cLoad.SENTIMIENTO_ESTABLECIMIENTO = rdrArchivoDato.Tables(0).Rows(0).Item("SENTIMIENTO_ESTABLECIMIENTO")
            cLoad.ACTIVACION = rdrArchivoDato.Tables(0).Rows(0).Item("ACTIVACION")
            cLoad.NUM_AFILIACION_ALTERNO = rdrArchivoDato.Tables(0).Rows(0).Item("NUM_AFILIACION_ALTERNO")
            cLoad.ESTATUS_VISITA = rdrArchivoDato.Tables(0).Rows(0).Item("ESTATUS_VISITA")

        End If
        CIERRA_DATASET(rdrArchivoDato)
    End Sub
    Public Sub SAVE(ByVal cSave As clsResultado)
        Dim DTInsert As String = "ARCHIVOS_DATA"
        If cSave.ID_ARCHIVO_DATA = -1 Then
            'Select Case tipoBlitz

            cSave.ID_ARCHIVO_DATA = ExecuteCmdScalar("INSERT INTO ARCHIVOS_DATA(FH_INSERT)VALUES('" & FORMATEAR_FECHA(System.DateTime.Now, "C") & "');SELECT SCOPE_IDENTITY()")
            DTInsert = "ARCHIVOS_DATA"
            'End Select
        End If
        Dim daAdapter As New SqlDataAdapter
        Dim cmdBuilder = New SqlCommandBuilder
        Dim dsDataset As New DataSet()
        Dim conSql As String = CONEXION()
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO_DATA = @PARAM1", New SqlConnection(conSql))
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = cSave.ID_ARCHIVO_DATA

        'daAdapter = New SqlDataAdapter("SELECT * FROM " & DTInsert & " WHERE ID_ARCHIVO_DATA = " & cSave.ID_ARCHIVO_DATA & "", conSql)

        daAdapter = New SqlDataAdapter(comm)
        cmdBuilder = New SqlCommandBuilder(daAdapter)
        daAdapter.Fill(dsDataset, DTInsert)
        'dsDataset.Tables(DTInsert).Rows(0)("ID_ARCHIVO") = cSave.ID_ARCHIVO
        'dsDataset.Tables(DTInsert).Rows(0)("AFILIACION") = cSave.AFILIACION
        dsDataset.Tables(DTInsert).Rows(0)("F_VISITA") = cSave.F_VISITA
        dsDataset.Tables(DTInsert).Rows(0)("VISITA_EXITOSA") = cSave.VISITA_EXITOSA
        dsDataset.Tables(DTInsert).Rows(0)("HABIA_POP") = cSave.HABIA_POP
        dsDataset.Tables(DTInsert).Rows(0)("SE_SENALIZO") = cSave.SE_SENALIZO
        dsDataset.Tables(DTInsert).Rows(0)("POP_COLOCADO") = cSave.POP_COLOCADO
        dsDataset.Tables(DTInsert).Rows(0)("SUPRESION") = cSave.SUPRESION
        dsDataset.Tables(DTInsert).Rows(0)("PROBLEMA_TERMINAL") = cSave.PROBLEMA_TERMINAL
        dsDataset.Tables(DTInsert).Rows(0)("TIPO_TERMINAL") = cSave.TIPO_TERMINAL
        dsDataset.Tables(DTInsert).Rows(0)("DESCRIPCION_PROBLEMA") = cSave.DESCRIPCION_PROBLEMA
        dsDataset.Tables(DTInsert).Rows(0)("OS_RESUELTO") = cSave.OS_RESUELTO
        dsDataset.Tables(DTInsert).Rows(0)("SENTIMIENTO_ESTABLECIMIENTO") = cSave.SENTIMIENTO_ESTABLECIMIENTO
        dsDataset.Tables(DTInsert).Rows(0)("ACTIVACION") = cSave.ACTIVACION
        dsDataset.Tables(DTInsert).Rows(0)("NUM_AFILIACION_ALTERNO") = cSave.NUM_AFILIACION_ALTERNO
        dsDataset.Tables(DTInsert).Rows(0)("ESTATUS_VISITA") = cSave.ESTATUS_VISITA

        If dsDataset.HasChanges Then
            daAdapter.Update(dsDataset, DTInsert)
        End If
    End Sub
End Class

