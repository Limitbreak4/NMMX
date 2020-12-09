Imports System.Data.SqlClient

Public Class clsRecord

    Public ID_ARCHIVO_DATA As Integer
    Public ID_ARCHIVO As Integer
    Public INDUSTRIA As String
    Public SUBINDUSTRIA As String
    Public REPRESENTANTELEGAL As String
    Public COMERCIOSABIA As String
    Public NUEVOTELEFONO As String
    Public EMAIL As String
    Public COMENTARIOS As String
    Public COL8 As String
    Public COL9 As String
    Public COL10 As String
    Public COL11 As String
    Public COL12 As String
    Public COL13 As String
    Public COL14 As String
    Public COL15 As String
    Public COMODIN As String
    Public ASIGNADO As String
    Public FILE As String
    Public BASE As String
    Public BANCO As String
    Public AFILIACION As String
    Public NOMBRE_ESTABLECIMIENTO As String
    Public CALLE_NUMERO As String
    Public COLONIA As String
    Public CP As String
    Public ESTADO_MUNICIPIO As String 'REGIÓN
    'Public ALCALDIA As String 'REGIÓN
    Public CIUDAD As String
    Public TELEFONO As String
    Public CANAL As String
    Public COBERTURA As String
    Public ID_REGION As Integer
    Public FLG_RECORD_OK As Boolean
    Public FH_INSERT As String
    Public ID_AGENCIA As Integer
    Public TIPO_BLITZ As String
    Public GEO As Integer
    Public MOTIVO_RECHAZO As String
    Public PORTAFOLIO As String 'cambioPortafolio
    Public PARTNER As String
    Public ID_CAMPAIGN As Integer



    Sub CLEAN()
        ID_ARCHIVO_DATA = -1
        ID_ARCHIVO = -1
        INDUSTRIA = ""
        SUBINDUSTRIA = ""
        REPRESENTANTELEGAL = ""
        COMERCIOSABIA = ""
        NUEVOTELEFONO = ""
        EMAIL = ""
        COMENTARIOS = ""
        COMODIN = ""
        COL9 = "" ' Colonia
        COL10 = "" 'C.P.
        COL11 = "" 'Estado o Municipio
        COL12 = "" ' CiudAD
        COL13 = "" 'Telefono
        COL14 = "" 'CanaL
        COL15 = "" 'COBERTURA
        ASIGNADO = ""
        FILE = ""
        BASE = ""
        BANCO = ""
        AFILIACION = ""
        NOMBRE_ESTABLECIMIENTO = ""
        CALLE_NUMERO = ""
        COLONIA = ""
        CP = ""
        ESTADO_MUNICIPIO = "" 'REGIÓN
        'ALCALDIA = ""
        CIUDAD = ""
        TELEFONO = ""
        CANAL = ""
        COBERTURA = ""
        ID_REGION = -1
        FLG_RECORD_OK = False
        FH_INSERT = ""
        ID_AGENCIA = -1
        TIPO_BLITZ = ""
        GEO = -1
        MOTIVO_RECHAZO = ""
        ' COL17  = "" 
        ' COL18  = ""
        ' COL19  = ""
        ' COL20  = ""
        ' COL21 As Integer
        ' COL22  = ""
        PORTAFOLIO = "" 'cambioPortafolio
        PARTNER = ""
        ID_CAMPAIGN = -1
    End Sub
    Public Sub LOAD(ByVal idRecord As String, ByRef cLoad As clsRecord)
        Dim rdrArchivoDato As DataSet
        'rdrArchivoDato = dsOpenDB("SELECT * FROM ARCHIVOS_DATA WHERE ID_ARCHIVO_DATA = '" & idRecord & "'")
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM ARCHIVOS_DATA (NOLOCK) WHERE ID_ARCHIVO_DATA = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = idRecord
        rdrArchivoDato = dsOpenDB(comm)
        If rdrArchivoDato.Tables(0).Rows.Count > 0 Then
            cLoad.ID_ARCHIVO_DATA = rdrArchivoDato.Tables(0).Rows(0).Item("ID_ARCHIVO_DATA")
            cLoad.ID_ARCHIVO = rdrArchivoDato.Tables(0).Rows(0).Item("ID_ARCHIVO")
            cLoad.INDUSTRIA = rdrArchivoDato.Tables(0).Rows(0).Item("INDUSTRIA")
            cLoad.SUBINDUSTRIA = rdrArchivoDato.Tables(0).Rows(0).Item("SUBINDUSTRIA")
            cLoad.REPRESENTANTELEGAL = rdrArchivoDato.Tables(0).Rows(0).Item("REPRESENTANTELEGAL")
            cLoad.COMERCIOSABIA = rdrArchivoDato.Tables(0).Rows(0).Item("COMERCIOSABIA")
            cLoad.NUEVOTELEFONO = rdrArchivoDato.Tables(0).Rows(0).Item("NUEVOTELEFONO")
            cLoad.EMAIL = rdrArchivoDato.Tables(0).Rows(0).Item("EMAIL")
            cLoad.COMENTARIOS = rdrArchivoDato.Tables(0).Rows(0).Item("COMENTARIOS")
            cLoad.COMODIN = rdrArchivoDato.Tables(0).Rows(0).Item("COMODIN")
            cLoad.COL8 = rdrArchivoDato.Tables(0).Rows(0).Item("COL8")
            cLoad.COL9 = rdrArchivoDato.Tables(0).Rows(0).Item("COL9")
            cLoad.COL10 = rdrArchivoDato.Tables(0).Rows(0).Item("COL10")
            cLoad.COL11 = rdrArchivoDato.Tables(0).Rows(0).Item("COL11")
            cLoad.COL12 = rdrArchivoDato.Tables(0).Rows(0).Item("COL12")
            cLoad.COL13 = rdrArchivoDato.Tables(0).Rows(0).Item("COL13")
            cLoad.COL14 = rdrArchivoDato.Tables(0).Rows(0).Item("COL14")
            cLoad.COL15 = rdrArchivoDato.Tables(0).Rows(0).Item("COL15")

            cLoad.ASIGNADO = rdrArchivoDato.Tables(0).Rows(0).Item("ASIGNADO")
            cLoad.FILE = rdrArchivoDato.Tables(0).Rows(0).Item("FILE")
            cLoad.BASE = rdrArchivoDato.Tables(0).Rows(0).Item("BASE")
            cLoad.BANCO = rdrArchivoDato.Tables(0).Rows(0).Item("BANCO")
            cLoad.AFILIACION = rdrArchivoDato.Tables(0).Rows(0).Item("AFILIACION")
            cLoad.NOMBRE_ESTABLECIMIENTO = rdrArchivoDato.Tables(0).Rows(0).Item("NOMBRE_ESTABLECIMIENTO")
            cLoad.CALLE_NUMERO = rdrArchivoDato.Tables(0).Rows(0).Item("CALLE_NUMERO")
            cLoad.COLONIA = rdrArchivoDato.Tables(0).Rows(0).Item("COLONIA")
            cLoad.CP = rdrArchivoDato.Tables(0).Rows(0).Item("CP")
            cLoad.ESTADO_MUNICIPIO = rdrArchivoDato.Tables(0).Rows(0).Item("ESTADO_MUNICIPIO")
            'cLoad.ALCALDIA = rdrArchivoDato.Tables(0).Rows(0).Item("ALCALDIA")
            cLoad.CIUDAD = rdrArchivoDato.Tables(0).Rows(0).Item("CIUDAD")
            cLoad.TELEFONO = rdrArchivoDato.Tables(0).Rows(0).Item("TELEFONO")
            cLoad.CANAL = rdrArchivoDato.Tables(0).Rows(0).Item("CANAL")
            cLoad.COBERTURA = rdrArchivoDato.Tables(0).Rows(0).Item("COBERTURA")
            cLoad.ID_REGION = rdrArchivoDato.Tables(0).Rows(0).Item("ID_REGION")
            cLoad.FLG_RECORD_OK = rdrArchivoDato.Tables(0).Rows(0).Item("FLG_RECORD_OK")
            cLoad.FH_INSERT = rdrArchivoDato.Tables(0).Rows(0).Item("FH_INSERT")
            cLoad.ID_AGENCIA = rdrArchivoDato.Tables(0).Rows(0).Item("ID_AGENCIA")
            cLoad.TIPO_BLITZ = rdrArchivoDato.Tables(0).Rows(0).Item("TIPO_BLITZ")
            cLoad.GEO = rdrArchivoDato.Tables(0).Rows(0).Item("GEO")
            cLoad.MOTIVO_RECHAZO = rdrArchivoDato.Tables(0).Rows(0).Item("MOTIVO_RECHAZO")
            'cLoad.col17 = rdrArchivoDato.Tables(0).Rows(0).Item("SELLER_AUTH_FIRST_NAME")
            'cLoad.col18 = rdrArchivoDato.Tables(0).Rows(0).Item("SELLER_AUTH_LAST_NAME")
            'cLoad.COBERTURA = rdrArchivoDato.Tables(0).Rows(0).Item("COBERTURA")
            'cLoad.PRIORIDAD = rdrArchivoDato.Tables(0).Rows(0).Item("PRIORIDAD")
            cLoad.PORTAFOLIO = rdrArchivoDato.Tables(0).Rows(0).Item("PORTAFOLIO") 'cambioPortafolio
            cLoad.PARTNER = rdrArchivoDato.Tables(0).Rows(0).Item("PARTNER")
            cLoad.PARTNER = rdrArchivoDato.Tables(0).Rows(0).Item("ID_CAMPAIGN")
        End If
        CIERRA_DATASET(rdrArchivoDato)
    End Sub
    Public Sub SAVE(ByVal cSave As clsRecord, Optional ByRef existingConnection As SqlConnection = Nothing)
        Dim DTInsert As String = ""
        If cSave.ID_ARCHIVO_DATA = -1 Then
            'Select Case tipoBlitz

            cSave.ID_ARCHIVO_DATA = ExecuteCmdScalar("INSERT INTO ARCHIVOS_DATA(FH_INSERT)VALUES('" & FORMATEAR_FECHA(System.DateTime.Now, "C") & "');SELECT SCOPE_IDENTITY()", existingConnection)
            DTInsert = "ARCHIVOS_DATA"
            'End Select
        End If
        Dim daAdapter As SqlDataAdapter
        Dim cmdBuilder = New SqlCommandBuilder
        Dim dsDataset As New DataSet()
        Dim conSql As String = CONEXION()
        If IsNothing(existingConnection) Then
            daAdapter = New SqlDataAdapter("SELECT * FROM " & DTInsert & " WHERE ID_ARCHIVO_DATA = " & cSave.ID_ARCHIVO_DATA & "", conSql)
        Else
            daAdapter = New SqlDataAdapter("SELECT * FROM " & DTInsert & " WHERE ID_ARCHIVO_DATA = " & cSave.ID_ARCHIVO_DATA & "", existingConnection)
        End If

        cmdBuilder = New SqlCommandBuilder(daAdapter)
        daAdapter.Fill(dsDataset, DTInsert)
        dsDataset.Tables(DTInsert).Rows(0)("ID_ARCHIVO") = cSave.ID_ARCHIVO
        dsDataset.Tables(DTInsert).Rows(0)("INDUSTRIA") = cSave.INDUSTRIA
        dsDataset.Tables(DTInsert).Rows(0)("SUBINDUSTRIA") = cSave.SUBINDUSTRIA
        dsDataset.Tables(DTInsert).Rows(0)("REPRESENTANTELEGAL") = cSave.REPRESENTANTELEGAL
        dsDataset.Tables(DTInsert).Rows(0)("COMERCIOSABIA") = cSave.COMERCIOSABIA
        dsDataset.Tables(DTInsert).Rows(0)("NUEVOTELEFONO") = cSave.NUEVOTELEFONO
        dsDataset.Tables(DTInsert).Rows(0)("EMAIL") = cSave.EMAIL
        dsDataset.Tables(DTInsert).Rows(0)("COMENTARIOS") = cSave.COMENTARIOS
        dsDataset.Tables(DTInsert).Rows(0)("COMODIN") = cSave.COMODIN
        dsDataset.Tables(DTInsert).Rows(0)("COL8") = cSave.COL8
        dsDataset.Tables(DTInsert).Rows(0)("COL9") = cSave.COL9
        dsDataset.Tables(DTInsert).Rows(0)("COL10") = cSave.COL10
        dsDataset.Tables(DTInsert).Rows(0)("COL11") = cSave.COL11
        dsDataset.Tables(DTInsert).Rows(0)("COL12") = cSave.COL12
        dsDataset.Tables(DTInsert).Rows(0)("COL13") = cSave.COL13
        dsDataset.Tables(DTInsert).Rows(0)("COL14") = cSave.COL14
        dsDataset.Tables(DTInsert).Rows(0)("COL15") = cSave.COL15

        dsDataset.Tables(DTInsert).Rows(0).Item("ASIGNADO") = cSave.ASIGNADO
        dsDataset.Tables(DTInsert).Rows(0).Item("FILE") = cSave.FILE
        dsDataset.Tables(DTInsert).Rows(0).Item("BASE") = cSave.BASE
        dsDataset.Tables(DTInsert).Rows(0).Item("BANCO") = cSave.BANCO
        dsDataset.Tables(DTInsert).Rows(0).Item("AFILIACION") = cSave.AFILIACION
        dsDataset.Tables(DTInsert).Rows(0).Item("NOMBRE_ESTABLECIMIENTO") = cSave.NOMBRE_ESTABLECIMIENTO
        dsDataset.Tables(DTInsert).Rows(0).Item("CALLE_NUMERO") = cSave.CALLE_NUMERO
        dsDataset.Tables(DTInsert).Rows(0).Item("COLONIA") = cSave.COLONIA
        dsDataset.Tables(DTInsert).Rows(0).Item("CP") = cSave.CP
        dsDataset.Tables(DTInsert).Rows(0).Item("ESTADO_MUNICIPIO") = cSave.ESTADO_MUNICIPIO
        'dsDataset.Tables(DTInsert).Rows(0).Item("ALCALDIA") = cSave.ALCALDIA
        dsDataset.Tables(DTInsert).Rows(0).Item("CIUDAD") = cSave.CIUDAD
        dsDataset.Tables(DTInsert).Rows(0).Item("TELEFONO") = cSave.TELEFONO
        dsDataset.Tables(DTInsert).Rows(0).Item("CANAL") = cSave.CANAL
        dsDataset.Tables(DTInsert).Rows(0).Item("COBERTURA") = cSave.COBERTURA
        dsDataset.Tables(DTInsert).Rows(0).Item("ID_REGION") = cSave.ID_REGION
        dsDataset.Tables(DTInsert).Rows(0)("FH_INSERT") = FORMATEAR_FECHA(Now, "C")
        dsDataset.Tables(DTInsert).Rows(0)("ID_AGENCIA") = cSave.ID_AGENCIA
        dsDataset.Tables(DTInsert).Rows(0)("FLG_RECORD_OK") = cSave.FLG_RECORD_OK
        dsDataset.Tables(DTInsert).Rows(0)("TIPO_BLITZ") = cSave.TIPO_BLITZ
        dsDataset.Tables(DTInsert).Rows(0)("GEO") = cSave.GEO
        dsDataset.Tables(DTInsert).Rows(0)("PORTAFOLIO") = cSave.PORTAFOLIO 'cambioPortafolio
        dsDataset.Tables(DTInsert).Rows(0)("PARTNER") = cSave.PARTNER
        'dsDataset.Tables(DTInsert).Rows(0)("SELLER_AUTH_FIRST_NAME") = cSave.col17
        'dsDataset.Tables(DTInsert).Rows(0)("SELLER_AUTH_LAST_NAME") = cSave.SELLER_AUTH_LAST_NAME
        'dsDataset.Tables(DTInsert).Rows(0)("COBERTURA") = cSave.COBERTURA
        'dsDataset.Tables(DTInsert).Rows(0)("PRIORIDAD") = cSave.PRIORIDAD

        dsDataset.Tables(DTInsert).Rows(0)("ID_CAMPAIGN") = cSave.ID_CAMPAIGN


        If dsDataset.HasChanges Then
            daAdapter.Update(dsDataset, DTInsert)
        End If
    End Sub
End Class
