Imports System.Data.SqlClient
Public Class clsAsignacion

    Public ID_ASIGNACION As Integer
    Public FH_ASIGNACION As String
    Public ID_AGENCIA As Integer
    Public FLG_CANC As Boolean
    Public ID_ARCHIVO As Integer
    Public ARCHIVO As Byte()
    Public NOMBRE_ARCHIVO As String
    Public ARCHIVO_ACK As Boolean
    Public ROWS_OK As Integer
    Public ROWS_NOT_OK As Integer
    Public TIPO_BLITZ As String

    Sub CLEAN()
        ID_ASIGNACION = -1
        FH_ASIGNACION = ""
        ID_AGENCIA = -1
        FLG_CANC = False
        ID_ARCHIVO = -1
        ARCHIVO = Nothing
        NOMBRE_ARCHIVO = ""
        ARCHIVO_ACK = False
        ROWS_OK = 0
        ROWS_NOT_OK = 0
        TIPO_BLITZ = ""
    End Sub
    Public Sub LOAD(ByVal idAsignacion As String, ByRef cLoad As clsAsignacion)
        Dim rdrAsignacion As DataSet
        'rdrAsignacion = dsOpenDB("SELECT * FROM ASIGNACIONES WHERE ID_ASIGNACION = '" & idAsignacion & "'")
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ASIGNACIONES (NOLOCK) WHERE ID_ASIGNACION = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idAsignacion

        rdrAsignacion = dsOpenDB(comm)
        If rdrAsignacion.Tables(0).Rows.Count > 0 Then
            cLoad.ID_ASIGNACION = rdrAsignacion.Tables(0).Rows(0).Item("ID_ASIGNACION")
            cLoad.FH_ASIGNACION = rdrAsignacion.Tables(0).Rows(0).Item("FH_ASIGNACION")
            cLoad.ID_AGENCIA = rdrAsignacion.Tables(0).Rows(0).Item("ID_AGENCIA")
            cLoad.FLG_CANC = rdrAsignacion.Tables(0).Rows(0).Item("FLG_CANC")
            cLoad.ID_ARCHIVO = rdrAsignacion.Tables(0).Rows(0).Item("ID_ARCHIVO")
            cLoad.ARCHIVO = rdrAsignacion.Tables(0).Rows(0).Item("ARCHIVO")
            cLoad.NOMBRE_ARCHIVO = rdrAsignacion.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO")
            cLoad.ARCHIVO_ACK = rdrAsignacion.Tables(0).Rows(0).Item("ARCHIVO_ACK")
            cLoad.ROWS_OK = rdrAsignacion.Tables(0).Rows(0).Item("ROWS_OK")
            cLoad.ROWS_NOT_OK = rdrAsignacion.Tables(0).Rows(0).Item("ROWS_NOT_OK")
            cLoad.TIPO_BLITZ = rdrAsignacion.Tables(0).Rows(0).Item("TIPO_BLITZ")
        End If
        CIERRA_DATASET(rdrAsignacion)
    End Sub
    Public Sub SAVE(ByVal cSave As clsAsignacion)

        'Today.ToString("dd-mm-aaaa")
        If cSave.ID_ASIGNACION = -1 Then
            cSave.ID_ASIGNACION = ExecuteCmdScalar("INSERT INTO ASIGNACIONES(FH_ASIGNACION,ARCHIVO_ACK,ROWS_OK,ROWS_NOT_OK,FLG_DESCARGA,VECES_DESCARGA,FLG_OUTCOME_CARGADO)
VALUES('" & FORMATEAR_FECHA(System.DateTime.Now, "S") & "','',1,0,0,0,0);SELECT SCOPE_IDENTITY()")
        Else

        End If
        Dim daAdapter As New SqlDataAdapter
        Dim cmdBuilder = New SqlCommandBuilder
        Dim dsDataset As New DataSet()
        Dim conSql As String = CONEXION()
        daAdapter = New SqlDataAdapter("SELECT * FROM ASIGNACIONES WHERE ID_ASIGNACION = " & cSave.ID_ASIGNACION & "", conSql)
        cmdBuilder = New SqlCommandBuilder(daAdapter)
        daAdapter.Fill(dsDataset, "ASIGNACIONES")

        dsDataset.Tables("ASIGNACIONES").Rows(0)("FH_ASIGNACION") = cSave.FH_ASIGNACION
        dsDataset.Tables("ASIGNACIONES").Rows(0)("ID_AGENCIA") = cSave.ID_AGENCIA
        dsDataset.Tables("ASIGNACIONES").Rows(0)("FLG_CANC") = cSave.FLG_CANC
        dsDataset.Tables("ASIGNACIONES").Rows(0)("ID_ARCHIVO") = cSave.ID_ARCHIVO
        dsDataset.Tables("ASIGNACIONES").Rows(0)("ARCHIVO") = cSave.ARCHIVO
        dsDataset.Tables("ASIGNACIONES").Rows(0)("NOMBRE_ARCHIVO") = cSave.NOMBRE_ARCHIVO
        dsDataset.Tables("ASIGNACIONES").Rows(0)("ARCHIVO_ACK") = cSave.ARCHIVO_ACK
        dsDataset.Tables("ASIGNACIONES").Rows(0)("ROWS_OK") = cSave.ROWS_OK
        dsDataset.Tables("ASIGNACIONES").Rows(0)("ROWS_NOT_OK") = cSave.ROWS_NOT_OK
        dsDataset.Tables("ASIGNACIONES").Rows(0)("TIPO_BLITZ") = cSave.TIPO_BLITZ
        If dsDataset.HasChanges Then
            daAdapter.Update(dsDataset, "ASIGNACIONES")
        End If
    End Sub
End Class
