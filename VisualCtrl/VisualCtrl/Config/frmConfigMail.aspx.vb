Imports System.Data.SqlClient
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

        'Dim RDRMAILSETTINGS As DataSet = dsOpenDB("*", "MAILSETTINGS", "")
        Dim RDRMAILSETTINGS As DataSet = dsOpenDB(New SqlCommand("SELECT * FROM MAILSETTINGS (NOLOCK)"))
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
        Dim RDRMAILSETTINGS As DataSet = dsOpenDB(New SqlCommand("SELECT * FROM MAILSETTINGS (NOLOCK)"))
        Dim sSql As String
        Dim comm As SqlCommand
        If RDRMAILSETTINGS.Tables(0).Rows.Count > 0 Then
            'sSql = "UPDATE MAILSETTINGS SET "
            'sSql &= "SMTP = '" & txtSMTP.Text & "', "
            'sSql &= "USUARIO = '" & txtUsuario.Text & "', "
            'sSql &= "PWD = '" & encode.encriptar128BitRijndael(txtPwd.Text, s_Clave) & "', "
            'sSql &= "SECURE_CONN = " & IIf(cbSSL.Checked, 1, 0) & ", "
            'sSql &= "PUERTO = '" & txtPuerto.Text & "', "
            'sSql &= "DIR_ENVIO = '" & txtDireccionEnvio.Text & "', "
            'sSql &= "NOMBRE_ENVIO = '" & txtNombreEnvio.Text & "', "
            'sSql &= "MENSAJE = '" & txtMensaje.Text & "', "
            'sSql &= "FLG_INC_ATTACH = " & IIf(cbIncludeAttach.Checked, 1, 0) & " "
            sSql = "UPDATE MAILSETTINGS SET "
            sSql &= "SMTP = @PARAM1, "
            sSql &= "USUARIO = @PARAM2, "
            sSql &= "PWD = @PARAM3, "
            sSql &= "SECURE_CONN = @PARAM4, "
            sSql &= "PUERTO = @PARAM5, "
            sSql &= "DIR_ENVIO = @PARAM6, "
            sSql &= "NOMBRE_ENVIO = @PARAM7, "
            sSql &= "MENSAJE = @PARAM8, "
            sSql &= "FLG_INC_ATTACH = @PARAM9"
            comm = New SqlCommand(sSql)
            comm.Parameters.Add("@PARAM1", SqlDbType.VarChar).Value = txtSMTP.Text
            comm.Parameters.Add("@PARAM2", SqlDbType.VarChar).Value = txtUsuario.Text
            comm.Parameters.Add("@PARAM3", SqlDbType.VarChar).Value = encode.encriptar128BitRijndael(txtPwd.Text, s_Clave)
            comm.Parameters.Add("@PARAM4", SqlDbType.Bit).Value = IIf(cbSSL.Checked, 1, 0)
            comm.Parameters.Add("@PARAM5", SqlDbType.VarChar).Value = txtPuerto.Text
            comm.Parameters.Add("@PARAM6", SqlDbType.VarChar).Value = txtDireccionEnvio.Text
            comm.Parameters.Add("@PARAM7", SqlDbType.VarChar).Value = txtNombreEnvio.Text
            comm.Parameters.Add("@PARAM8", SqlDbType.VarChar).Value = txtMensaje.Text
            comm.Parameters.Add("@PARAM9", SqlDbType.Bit).Value = IIf(cbIncludeAttach.Checked, 1, 0)
            ExecuteCmd(comm)
        Else
            comm = New SqlCommand("INSERT INTO MAILSETTINGS (SMTP, USUARIO, PWD, SECURE_CONN, PUERTO, DIR_ENVIO, NOMBRE_ENVIO, MENSAJE, FLG_INC_ATTACH) 
VALUES(@PARAM1, @PARAM2, @PARAM3, @PARAM4, @PARAM5, @PARAM6, @PARAM7, @PARAM8, @PARAM9)")
            'sSql &= "'" & txtSMTP.Text & "', "
            'sSql &= "'" & txtUsuario.Text & "', "
            'sSql &= "'" & encode.encriptar128BitRijndael(txtPwd.Text, s_Clave) & "', "
            'sSql &= IIf(cbSSL.Checked, 1, 0) & ", "
            'sSql &= "'" & txtPuerto.Text & "', "
            'sSql &= "'" & txtDireccionEnvio.Text & "', "
            'sSql &= "'" & txtNombreEnvio.Text & "', "
            'sSql &= "'" & txtMensaje.Text & "', "
            'sSql &= IIf(cbIncludeAttach.Checked, 1, 0) & ", "
            'sSql &= ")"
            comm.Parameters.Add("@PARAM1", SqlDbType.VarChar).Value = txtSMTP.Text
            comm.Parameters.Add("@PARAM2", SqlDbType.VarChar).Value = txtUsuario.Text
            comm.Parameters.Add("@PARAM3", SqlDbType.VarChar).Value = encode.encriptar128BitRijndael(txtPwd.Text, s_Clave)
            comm.Parameters.Add("@PARAM4", SqlDbType.Bit).Value = IIf(cbSSL.Checked, 1, 0)
            comm.Parameters.Add("@PARAM5", SqlDbType.VarChar).Value = txtPuerto.Text
            comm.Parameters.Add("@PARAM6", SqlDbType.VarChar).Value = txtDireccionEnvio.Text
            comm.Parameters.Add("@PARAM7", SqlDbType.VarChar).Value = txtNombreEnvio.Text
            comm.Parameters.Add("@PARAM8", SqlDbType.VarChar).Value = txtMensaje.Text
            comm.Parameters.Add("@PARAM9", SqlDbType.Bit).Value = IIf(cbIncludeAttach.Checked, 1, 0)
            ExecuteCmd(comm)
        End If
        msg("CONFIGURACION ALMACENADA")
    End Sub

    Private Sub msg(ByVal txtIn)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" & txtIn & "');", True)
    End Sub
End Class