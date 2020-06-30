Public Class frmReadyBlitz
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If
        '
        If Not IsPostBack Then
            INICIALIZAR_FORM
            TryCast(Master.FindControl("lblPage"), Label).Text = "Ready for Blitz"
        End If
    End Sub

    Private Sub INICIALIZAR_FORM()
        Dim rdrMex50 As DataSet
        Dim rdrMex100 As DataSet
        Dim rdrMex300 As DataSet
        'MÉXICO 50
        Dim sSql As String = "SELECT ESTADO_MUNICIPIO, TIPO_BLITZ, COUNT(*)  AS TOTAL FROM ARCHIVOS_DATA "
        sSql &= "LEFT JOIN TAB_CP ON TAB_CP.CP = ARCHIVOS_DATA.CP "
        sSql &= "INNER JOIN TAB_ESTADOS ON TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO "
        sSql &= "WHERE TIPO_BLITZ = 'BLITZ50' "
        sSql &= "AND DESC_ESTADO IN ('Ciudad de México', 'México') "
        sSql &= "AND ID_AGENCIA = -1 "
        sSql &= "GROUP BY ESTADO_MUNICIPIO, TIPO_BLITZ "
        sSql &= "HAVING COUNT(*) BETWEEN 50 AND 99 "
        sSql &= "ORDER BY TIPO_BLITZ, ESTADO_MUNICIPIO "
        rdrMex50 = dsOpenDB(sSql)
        grdMex50.DataSource = rdrMex50.Tables(0)
        grdMex50.DataBind()


        'MÉXICO 100
        sSql = "SELECT ESTADO_MUNICIPIO, TIPO_BLITZ, COUNT(*)  AS TOTAL FROM ARCHIVOS_DATA "
        sSql &= "LEFT JOIN TAB_CP ON TAB_CP.CP = ARCHIVOS_DATA.CP "
        sSql &= "INNER JOIN TAB_ESTADOS ON TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO "
        sSql &= "WHERE TIPO_BLITZ = 'BLITZ100' "
        sSql &= "AND DESC_ESTADO IN ('Ciudad de México', 'México') "
        sSql &= "AND ID_AGENCIA = -1 "
        sSql &= "GROUP BY ESTADO_MUNICIPIO, TIPO_BLITZ "
        sSql &= "HAVING COUNT(*) BETWEEN 100 AND 299 "
        sSql &= "ORDER BY TIPO_BLITZ, ESTADO_MUNICIPIO "
        rdrMex100 = dsOpenDB(sSql)
        grdMex100.DataSource = rdrMex100.Tables(0)
        grdMex100.DataBind()

        'MÉXICO 300
        sSql = "SELECT ESTADO_MUNICIPIO, TIPO_BLITZ, COUNT(*)  AS TOTAL FROM ARCHIVOS_DATA "
        sSql &= "LEFT JOIN TAB_CP ON TAB_CP.CP = ARCHIVOS_DATA.CP "
        sSql &= "INNER JOIN TAB_ESTADOS ON TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO "
        sSql &= "WHERE TIPO_BLITZ = 'BLITZ300' "
        sSql &= "AND DESC_ESTADO IN ('Ciudad de México', 'México') "
        sSql &= "AND ID_AGENCIA = -1 "
        sSql &= "GROUP BY ESTADO_MUNICIPIO, TIPO_BLITZ "
        sSql &= "HAVING COUNT(*) >= 300 "
        sSql &= "ORDER BY TIPO_BLITZ, ESTADO_MUNICIPIO "
        rdrMex300 = dsOpenDB(sSql)
        grdMex300.DataSource = rdrMex300.Tables(0)
        grdMex300.DataBind()


        Dim rdrSta50 As DataSet
        Dim rdrSta100 As DataSet
        Dim rdrSta300 As DataSet

        'ESTADOS 50
        sSql = "SELECT DESC_ESTADO, TIPO_BLITZ, COUNT(*)  AS TOTAL FROM ARCHIVOS_DATA "
        sSql &= "LEFT JOIN TAB_CP ON TAB_CP.CP = ARCHIVOS_DATA.CP "
        sSql &= "INNER JOIN TAB_ESTADOS ON TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO "
        sSql &= "WHERE TIPO_BLITZ = 'BLITZ50' "
        sSql &= "AND DESC_ESTADO NOT IN ('Ciudad de México', 'México') "
        sSql &= "AND ID_AGENCIA = -1 "
        sSql &= "GROUP BY DESC_ESTADO, TIPO_BLITZ "
        sSql &= "HAVING COUNT(*) BETWEEN 50 AND 99 "
        sSql &= "ORDER BY TIPO_BLITZ, DESC_ESTADO "
        rdrSta50 = dsOpenDB(sSql)
        grdStates50.DataSource = rdrSta50.Tables(0)
        grdStates50.DataBind()


        'ESTADOS 100
        sSql = "SELECT DESC_ESTADO, TIPO_BLITZ, COUNT(*)  AS TOTAL FROM ARCHIVOS_DATA "
        sSql &= "LEFT JOIN TAB_CP ON TAB_CP.CP = ARCHIVOS_DATA.CP "
        sSql &= "INNER JOIN TAB_ESTADOS ON TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO "
        sSql &= "WHERE TIPO_BLITZ = 'BLITZ100' "
        sSql &= "AND DESC_ESTADO NOT IN ('Ciudad de México', 'México') "
        sSql &= "AND ID_AGENCIA = -1 "
        sSql &= "GROUP BY DESC_ESTADO, TIPO_BLITZ "
        sSql &= "HAVING COUNT(*) BETWEEN 100 AND 299 "
        sSql &= "ORDER BY TIPO_BLITZ, DESC_ESTADO "
        rdrSta100 = dsOpenDB(sSql)
        grdStates100.DataSource = rdrSta100.Tables(0)
        grdStates100.DataBind()

        'ESTADOS 300
        sSql = "SELECT DESC_ESTADO, TIPO_BLITZ, COUNT(*)  AS TOTAL FROM ARCHIVOS_DATA "
        sSql &= "LEFT JOIN TAB_CP ON TAB_CP.CP = ARCHIVOS_DATA.CP "
        sSql &= "INNER JOIN TAB_ESTADOS ON TAB_ESTADOS.ID_ESTADO = TAB_CP.ID_ESTADO "
        sSql &= "WHERE TIPO_BLITZ = 'BLITZ300' "
        sSql &= "AND DESC_ESTADO  NOT IN ('Ciudad de México', 'México') "
        sSql &= "AND ID_AGENCIA = -1 "
        sSql &= "GROUP BY DESC_ESTADO, TIPO_BLITZ "
        sSql &= "HAVING COUNT(*) >= 300 "
        sSql &= "ORDER BY TIPO_BLITZ, DESC_ESTADO "
        rdrSta300 = dsOpenDB(sSql)
        grdStates300.DataSource = rdrSta300.Tables(0)
        grdStates300.DataBind()

    End Sub

End Class