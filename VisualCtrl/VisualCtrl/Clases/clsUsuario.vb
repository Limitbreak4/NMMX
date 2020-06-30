Imports System.Data.SqlClient
Public Class clsUsuario
    Public idUsuario As String
    Public nombreCompleto As String
    Public telefono As String
    Public email As String
    Public pwd As String
    Public idRol As Integer
    Public idAgencia As Integer
    Public flgAdmin As Boolean
    Public flgCanc As Boolean
    Public FLG_CONTACTO_AGENCIA As Boolean


    Sub CLEAN()
        idUsuario = ""
        nombreCompleto = ""
        telefono = ""
        email = ""
        pwd = ""
        idRol = -1
        idAgencia = -1
        flgAdmin = False
        flgCanc = False
        FLG_CONTACTO_AGENCIA = False
    End Sub
    Public Sub LOAD(ByVal idUsuario As String, ByRef cLoad As clsUsuario)
        Dim rdrUsuario As DataSet
        rdrUsuario = dsOpenDB("SELECT * FROM TAB_USUARIOS WHERE ID_USUARIO = '" & idUsuario & "'")
        If rdrUsuario.Tables(0).Rows.Count > 0 Then
            cLoad.idUsuario = rdrUsuario.Tables(0).Rows(0).Item("ID_USUARIO")
            cLoad.nombreCompleto = rdrUsuario.Tables(0).Rows(0).Item("NOMBRE")
            cLoad.telefono = rdrUsuario.Tables(0).Rows(0).Item("TELEFONO")
            cLoad.email = rdrUsuario.Tables(0).Rows(0).Item("EMAIL")
            cLoad.pwd = encode.desencriptar128BitRijndael(rdrUsuario.Tables(0).Rows(0).Item("PWD"), s_Clave)
            cLoad.idRol = rdrUsuario.Tables(0).Rows(0).Item("ID_ROLE")
            If Not IsDBNull(rdrUsuario.Tables(0).Rows(0).Item("ID_AGENCIA")) Then
                cLoad.idAgencia = rdrUsuario.Tables(0).Rows(0).Item("ID_AGENCIA")
            Else
                cLoad.idAgencia = -1
            End If
            cLoad.flgAdmin = rdrUsuario.Tables(0).Rows(0).Item("FLG_ADMIN")
            cLoad.flgCanc = rdrUsuario.Tables(0).Rows(0).Item("FLG_CANC")
            cLoad.FLG_CONTACTO_AGENCIA = rdrUsuario.Tables(0).Rows(0).Item("FLG_CONTACTO_AGENCIA")
        End If
        CIERRA_DATASET(rdrUsuario)
    End Sub
    Public Sub SAVE(ByVal cSave As clsUsuario)

        If Not bEXISTE_REGISTRO("SELECT * FROM TAB_USUARIOS WHERE ID_USUARIO = '" & cSave.idUsuario & "'") Then
            ExecuteCmd("INSERT INTO TAB_USUARIOS (ID_USUARIO) VALUES ('" & cSave.idUsuario & "')")
        Else

        End If
        Dim daAdapter As New SqlDataAdapter
        Dim cmdBuilder = New SqlCommandBuilder
        Dim dsDataset As New DataSet()
        Dim conSql As String = CONEXION()
        daAdapter = New SqlDataAdapter("SELECT * FROM TAB_USUARIOS WHERE ID_USUARIO = '" & cSave.idUsuario & "'", conSql)
        cmdBuilder = New SqlCommandBuilder(daAdapter)
        daAdapter.Fill(dsDataset, "TAB_USUARIOS")

        dsDataset.Tables("TAB_USUARIOS").Rows(0)("ID_USUARIO") = cSave.idUsuario
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("NOMBRE") = cSave.nombreCompleto
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("TELEFONO") = cSave.telefono
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("EMAIL") = cSave.email
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("PWD") = encode.encriptar128BitRijndael(cSave.pwd, s_Clave)
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("ID_ROLE") = cSave.idRol
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("ID_AGENCIA") = cSave.idAgencia
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("FLG_ADMIN") = cSave.flgAdmin
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("FLG_CANC") = cSave.flgCanc
        dsDataset.Tables("TAB_USUARIOS").Rows(0)("FLG_CONTACTO_AGENCIA") = cSave.FLG_CONTACTO_AGENCIA


        If dsDataset.HasChanges Then
            daAdapter.Update(dsDataset, "TAB_USUARIOS")
        End If
    End Sub
End Class
