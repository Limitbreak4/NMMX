Imports System.Data.SqlClient

Public Class clsArchivo


    Public ID_ARCHIVO As Integer
    Public FH_CARGA As String
    Public NOMBRE_ARCHIVO As String
    Public ARCHIVO As Byte()
    Public FLG_MARKETING As Boolean
    Public FLG_AGENCIA As Boolean
    Public ID_CAMPAIGN As Integer





    Sub CLEAN()
        ID_ARCHIVO = -1
        FH_CARGA = ""
        NOMBRE_ARCHIVO = ""
        ARCHIVO = Nothing
        FLG_MARKETING = False
        FLG_AGENCIA = False
        ID_CAMPAIGN = 0
    End Sub
    Public Sub LOAD(ByVal idRecord As String, ByRef cLoad As clsArchivo)
        Dim rdrArchivo As DataSet
        rdrArchivo = dsOpenDB("SELECT * FROM ARCHIVOS WHERE ID_ARCHIVO = '" & idRecord & "'")
        If rdrArchivo.Tables(0).Rows.Count > 0 Then
            cLoad.ID_ARCHIVO = rdrArchivo.Tables(0).Rows(0).Item("ID_ARCHIVO")
            cLoad.FH_CARGA = rdrArchivo.Tables(0).Rows(0).Item("FH_CARGA")
            cLoad.NOMBRE_ARCHIVO = rdrArchivo.Tables(0).Rows(0).Item("NOMBRE_ARCHIVO")
            cLoad.ARCHIVO = rdrArchivo.Tables(0).Rows(0).Item("ARCHIVO")
            cLoad.FLG_MARKETING = rdrArchivo.Tables(0).Rows(0).Item("FLG_MARKETING")
            cLoad.FLG_AGENCIA = rdrArchivo.Tables(0).Rows(0).Item("FLG_AGENCIA")
            cLoad.ID_CAMPAIGN = rdrArchivo.Tables(0).Rows(0).ItemArray("ID_CAMPAIGN")
        End If
        CIERRA_DATASET(rdrArchivo)
    End Sub
    Public Sub SAVE(ByVal cSave As clsArchivo)

        If cSave.ID_ARCHIVO = -1 Then
            cSave.ID_ARCHIVO = ExecuteCmdScalar("INSERT INTO ARCHIVOS(FH_CARGA)VALUES('" & FORMATEAR_FECHA(System.DateTime.Now, "C") & "');SELECT SCOPE_IDENTITY()")
        Else

        End If
        Dim daAdapter As New SqlDataAdapter
        Dim cmdBuilder = New SqlCommandBuilder
        Dim dsDataset As New DataSet()
        Dim conSql As String = CONEXION()
        daAdapter = New SqlDataAdapter("SELECT * FROM ARCHIVOS WHERE ID_ARCHIVO = " & cSave.ID_ARCHIVO & "", conSql)
        cmdBuilder = New SqlCommandBuilder(daAdapter)
        daAdapter.Fill(dsDataset, "ARCHIVOS")

        'dsDataset.Tables("ARCHIVOS").Rows(0)("ID_ARCHIVO") = cSave.ID_ARCHIVO
        dsDataset.Tables("ARCHIVOS").Rows(0)("FH_CARGA") = cSave.FH_CARGA
        dsDataset.Tables("ARCHIVOS").Rows(0)("NOMBRE_ARCHIVO") = cSave.NOMBRE_ARCHIVO
        dsDataset.Tables("ARCHIVOS").Rows(0)("ARCHIVO") = cSave.ARCHIVO
        dsDataset.Tables("ARCHIVOS").Rows(0)("FLG_MARKETING") = cSave.FLG_MARKETING
        dsDataset.Tables("ARCHIVOS").Rows(0)("FLG_AGENCIA") = cSave.FLG_AGENCIA
        dsDataset.Tables("ARCHIVOS").Rows(0)("ID_CAMPAIGN") = cSave.ID_CAMPAIGN

        If dsDataset.HasChanges Then
            daAdapter.Update(dsDataset, "ARCHIVOS")
        End If
    End Sub
End Class
