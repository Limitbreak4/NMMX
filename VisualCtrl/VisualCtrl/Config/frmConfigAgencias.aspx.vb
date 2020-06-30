Public Class frmConfigAgencias
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If
        If Session("FLG_ADMIN") = False Then
            Response.Redirect("~/Marketing/frmMarketing.aspx")
        End If
        If Not IsPostBack Then
            INICIALIZAR_FORM()
        End If
    End Sub

    Protected Sub LIMPIARbutton_Click(sender As Object, e As ImageClickEventArgs) Handles LIMPIARbutton.Click
        LIMPIAR_CAMPOS
    End Sub
    Private Sub LIMPIAR_CAMPOS()
        lblIdAgencia.Text = ""
        txtDescAgencia.Text = ""
        txtContacto.Text = ""
        chkflgCanc.Text = ""
        grdSEARCH.DataSource = Nothing
        grdSEARCH.DataBind()

    End Sub
    Private Sub INICIALIZAR_FORM()
        'HMMM REALMENTE NO TENGO NADA PARA PONER AQUÍ... PERO TENGO HAMBRE
    End Sub

    Protected Sub SALVARbutton_Click(sender As Object, e As ImageClickEventArgs) Handles SALVARbutton.Click
        Dim sSql As String = ""
        If lblIdAgencia.Text = "" Then
            sSql = "INSERT INTO TAB_AGENCIA (DESC_AGENCIA, NOMBRE_CONTACTO, MAIL_CONTACTO) VALUES("
            sSql &= "'" & txtDescAgencia.Text & "', "
            sSql &= "'" & txtContacto.Text & "', "
            sSql &= "'" & txtEmail.Text & "' "
            sSql &= ")"
        Else
            sSql &= "UPDATE TAB_AGENCIA SET "
            sSql &= "DESC_AGENCIA = '" & txtDescAgencia.Text & "', "
            sSql &= "NOMBRE_CONTACTO = '" & txtContacto.Text & "', "
            sSql &= "MAIL_CONTACTO = '" & txtEmail.Text & "', "
            sSql &= "FLG_CANC = " & IIf(chkflgCanc.Checked, 1, 0) & " "
            sSql &= "WHERE ID_AGENCIA = " & lblIdAgencia.Text
        End If
        ExecuteCmd(sSql)
    End Sub

    Protected Sub BUSCARbutton_Click(sender As Object, e As ImageClickEventArgs) Handles BUSCARbutton.Click
        Dim rdrAgencia As DataSet
        Dim sSql As String = "SELECT * FROM TAB_AGENCIA WHERE ID_AGENCIA > 0 "
        If txtDescAgencia.Text <> "" Then
            sSql &= "AND DESC_AGENCIA = '" & txtDescAgencia.Text & "' "
        End If
        If txtContacto.Text <> "" Then
            sSql &= "AND NOMBRE_CONTACTO = '" & txtContacto.Text & "', "
        End If
        If txtEmail.Text <> "" Then
            sSql &= "AND MAIL_CONTACTO = '" & txtEmail.Text & "' "
        End If

        rdrAgencia = dsOpenDB(sSql)
        grdSEARCH.DataSource = rdrAgencia.Tables(0)
        grdSEARCH.DataBind()
        mpSEARCH.Show()
        CIERRA_DATASET(rdrAgencia)
    End Sub

    Private Sub grdSEARCH_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdSEARCH.RowCommand
        Dim sIdAgencia As String = ""
        sIdAgencia = grdSEARCH.DataKeys(e.CommandArgument.ToString).Value.ToString


        CARGA_AGENCIA(sIdAgencia)
    End Sub
    Private Sub CARGA_AGENCIA(ByVal iIdAgencia As Integer)
        Dim rdrAgencia As DataSet = dsOpenDB("SELECT * FROM TAB_AGENCIA WHERE ID_AGENCIA = " & iIdAgencia)
        If rdrAgencia.Tables(0).Rows.Count > 0 Then
            lblIdAgencia.Text = rdrAgencia.Tables(0).Rows(0).Item("ID_AGENCIA")
            txtDescAgencia.Text = rdrAgencia.Tables(0).Rows(0).Item("DESC_AGENCIA")
            txtContacto.Text = rdrAgencia.Tables(0).Rows(0).Item("NOMBRE_CONTACTO")
            txtEmail.Text = rdrAgencia.Tables(0).Rows(0).Item("MAIL_CONTACTO")
            chkflgCanc.Checked = rdrAgencia.Tables(0).Rows(0).Item("FLG_CANC")
        End If
    End Sub
End Class