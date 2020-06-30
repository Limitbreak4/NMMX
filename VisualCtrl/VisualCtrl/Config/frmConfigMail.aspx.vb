Public Class frmConfigMail
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
    Private Sub INICIALIZAR_FORM()
        Dim RDRMAILSETTINGS As DataSet = dsOpenDB("SELECT * FROM MAILSETTINGS")
        If RDRMAILSETTINGS.Tables(0).Rows.Count > 0 Then
            txtSMTP.Text = RDRMAILSETTINGS.Tables(0).Rows(0).Item("SMTP")
            txtUsuario.Text = RDRMAILSETTINGS.Tables(0).Rows(0).Item("USUARIO")
            txtPwd.Text = encode.desencriptar128BitRijndael(RDRMAILSETTINGS.Tables(0).Rows(0).Item("PWD"), s_Clave)
            txtPuerto.Text = RDRMAILSETTINGS.Tables(0).Rows(0).Item("PUERTO")
            txtNombreEnvio.Text = RDRMAILSETTINGS.Tables(0).Rows(0).Item("NOMBRE_ENVIO")
            txtDireccionEnvio.Text = RDRMAILSETTINGS.Tables(0).Rows(0).Item("DIR_ENVIO")
            txtMensaje.Text = RDRMAILSETTINGS.Tables(0).Rows(0).Item("MENSAJE")
            cbSSL.Checked = RDRMAILSETTINGS.Tables(0).Rows(0).Item("SECURE_CONN")
            cbIncludeAttach.Checked = RDRMAILSETTINGS.Tables(0).Rows(0).Item("FLG_INC_ATTACH")
        End If
        CIERRA_DATASET(RDRMAILSETTINGS)
    End Sub
    Protected Sub cmdSalvar_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSalvar.Click
        Dim RDRMAILSETTINGS As DataSet = dsOpenDB("SELECT * FROM MAILSETTINGS")
        Dim sSql As String
        If RDRMAILSETTINGS.Tables(0).Rows.Count > 0 Then
            sSql = "UPDATE MAILSETTINGS SET "
            sSql &= "SMTP = '" & txtSMTP.Text & "', "
            sSql &= "USUARIO = '" & txtUsuario.Text & "', "
            sSql &= "PWD = '" & encode.encriptar128BitRijndael(txtPwd.Text, s_Clave) & "', "
            sSql &= "SECURE_CONN = " & IIf(cbSSL.Checked, 1, 0) & ", "
            sSql &= "PUERTO = '" & txtPuerto.Text & "', "
            sSql &= "DIR_ENVIO = '" & txtDireccionEnvio.Text & "', "
            sSql &= "NOMBRE_ENVIO = '" & txtNombreEnvio.Text & "', "
            sSql &= "MENSAJE = '" & txtMensaje.Text & "', "
            sSql &= "FLG_INC_ATTACH = " & IIf(cbIncludeAttach.Checked, 1, 0) & " "
            ExecuteCmd(sSql)
        Else
            sSql = "INSERT INTO MAILSETTINGS(SMTP, USUARIO, PWD, SECURE_CONN, PUERTO, DIR_ENVIO, NOMBRE_ENVIO, MENSAJE, FLG_INC_ATTACH) VALUES("
            sSql &= "'" & txtSMTP.Text & "', "
            sSql &= "'" & txtUsuario.Text & "', "
            sSql &= "'" & encode.encriptar128BitRijndael(txtPwd.Text, s_Clave) & "', "
            sSql &= IIf(cbSSL.Checked, 1, 0) & ", "
            sSql &= "'" & txtPuerto.Text & "', "
            sSql &= "'" & txtDireccionEnvio.Text & "', "
            sSql &= "'" & txtNombreEnvio.Text & "', "
            sSql &= "'" & txtMensaje.Text & "', "
            sSql &= IIf(cbIncludeAttach.Checked, 1, 0) & ", "
            sSql &= ")"
            ExecuteCmd(sSql)
        End If
        msg("CONFIGURACION ALMACENADA")
    End Sub

    Private Sub msg(ByVal txtIn)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" & txtIn & "');", True)
    End Sub
End Class