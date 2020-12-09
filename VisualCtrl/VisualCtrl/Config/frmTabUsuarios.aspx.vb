Public Class frmTabUsuarios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If
        If Session("FLG_ADMIN") = False Then
            Response.Redirect("~/Marketing/frmMarketing.aspx")
        End If

        If Not IsPostBack Then
            INICIALIZAR_FORM
        End If
    End Sub
    Private Sub INICIALIZAR_FORM()
        'CARGAR_COMBO(dropRol, "SELECT * FROM TAB_ROLES WHERE FLG_CANC = 0 ORDER BY DESC_ROLE", "ID_ROLE", "DESC_ROLE")
        CARGAR_COMBO(dropAgencia, "SELECT * FROM TAB_AGENCIA WHERE FLG_CANC = 0 ORDER BY DESC_AGENCIA", "ID_AGENCIA", "DESC_AGENCIA", True)
    End Sub
    Protected Sub LIMPIARbutton_Click(sender As Object, e As ImageClickEventArgs) Handles LIMPIARbutton.Click
        LIMPIAR_CAMPOS()
    End Sub

    Private Sub LIMPIAR_CAMPOS()
        txtidUsuario.Text = ""
        txtidUsuario.Enabled = True
        txtNombreCompleto.Text = ""
        txtPassword.Text = ""
        txtEmail.Text = ""
        txtTelefono.Text = ""
        dropRol.SelectedIndex = -1
        dropAgencia.SelectedIndex = -1
        chkAdmin.Checked = False
        chkflgCanc.Checked = False
        chkContacto.Checked = False
        Session("sPwd") = ""
        grdSEARCH.DataSource = Nothing
        grdSEARCH.DataBind()

        SetFocus(txtidUsuario)
    End Sub

    Protected Sub SALVARbutton_Click(sender As Object, e As ImageClickEventArgs) Handles SALVARbutton.Click
        If ValidaObligatorios() Then
            Dim cUsuario As New clsUsuario
            cUsuario.CLEAN()
            cUsuario.idUsuario = txtidUsuario.Text
            cUsuario.nombreCompleto = txtNombreCompleto.Text
            cUsuario.telefono = txtTelefono.Text
            cUsuario.email = txtEmail.Text
            If txtPassword.Text = "" Then
                If Session("sPwd") = "" Then
                    msg("LA CONTRASEÑA NO PUEDE ESTAR VACÍA")
                Else
                    cUsuario.pwd = Session("sPwd")
                End If
            Else
                cUsuario.pwd = txtPassword.Text
            End If
            cUsuario.idRol = dropRol.SelectedValue
            If dropAgencia.SelectedValue <> "" Then
                cUsuario.idAgencia = dropAgencia.SelectedValue
            End If
            cUsuario.flgAdmin = chkAdmin.Checked
            cUsuario.flgCanc = chkflgCanc.Checked
            cUsuario.FLG_CONTACTO_AGENCIA = chkContacto.Checked
            cUsuario.SAVE(cUsuario)
            msg("NEW USER ACCEPTED")
        End If
    End Sub
    Private Function ValidaObligatorios() As Boolean
        Dim res As Boolean = False

        If txtidUsuario.Text <> "" Then
            res = True
        End If
        Return res
    End Function

    Private Sub msg(ByVal txtIn)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" & txtIn & "');", True)
    End Sub

    Protected Sub txtidUsuario_TextChanged(sender As Object, e As EventArgs) Handles txtidUsuario.TextChanged
        If bEXISTE_REGISTRO("SELECT * FROM TAB_USUARIOS WHERE ID_USUARIO = '" & txtidUsuario.Text & "'") Then
            cargaUsuario(txtidUsuario.Text)
        End If
    End Sub

    Protected Sub BUSCARbutton_Click(sender As Object, e As ImageClickEventArgs) Handles BUSCARbutton.Click
        Dim rdrUsuarios As DataSet
        Dim sSql As String = "SELECT * FROM TAB_USUARIOS "
        sSql &= "INNER JOIN TAB_ROLES ON TAB_ROLES.ID_ROLE = TAB_USUARIOS.ID_ROLE "
        sSql &= "WHERE ID_USUARIO IS NOT NULL "
        If txtNombreCompleto.Text <> "" Then
            sSql &= "AND NOMBRE LIKE '%" & txtNombreCompleto.Text.Replace(" ", "%") & "%'"
        End If
        If txtTelefono.Text <> "" Then
            sSql &= "AND TELEFONO = '" & txtTelefono.Text & "' "
        End If
        If txtEmail.Text <> "" Then
            sSql &= "AND EMAIL = '" & txtEmail.Text & "' "
        End If
        If chkflgCanc.Checked Then
            sSql &= "AND FLG_CANC = " & chkflgCanc.Checked & " "
        End If
        If chkAdmin.Checked Then
            sSql &= "AND FLG_ADMIN = " & chkAdmin.Checked & " "
        End If

        If chkContacto.Checked Then
            sSql &= "AND FLG_CONTACTO_AGENCIA = " & chkContacto.Checked & " "
        End If

        rdrUsuarios = dsOpenDB(sSql)
        grdSEARCH.DataSource = rdrUsuarios.Tables(0)
        grdSEARCH.DataBind()
        mpSEARCH.Show()
    End Sub

    Private Sub grdSEARCH_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdSEARCH.RowCommand
        Dim idUsuario As String = ""
        idUsuario = grdSEARCH.DataKeys(e.CommandArgument.ToString).Value.ToString
        cargaUsuario(idUsuario)
    End Sub
    Private Sub cargaUsuario(ByVal idUsuario As String)
        Dim cUsuario As New clsUsuario
        cUsuario.LOAD(idUsuario, cUsuario)
        txtidUsuario.Text = cUsuario.idUsuario
        txtNombreCompleto.Text = cUsuario.nombreCompleto
        txtTelefono.Text = cUsuario.telefono
        txtPassword.Text = cUsuario.pwd

        Session("sPwd") = cUsuario.pwd
        txtEmail.Text = cUsuario.email

        dropRol.SelectedValue = cUsuario.idRol
        If cUsuario.idAgencia <> -1 Then
            dropAgencia.SelectedValue = cUsuario.idAgencia
        Else
            dropAgencia.SelectedIndex = -1
        End If
        chkAdmin.Checked = cUsuario.flgAdmin
        chkflgCanc.Checked = cUsuario.flgCanc
        chkContacto.Checked = cUsuario.FLG_CONTACTO_AGENCIA
    End Sub
End Class