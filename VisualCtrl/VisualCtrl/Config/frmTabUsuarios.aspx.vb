Imports System.Data.SqlClient
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
            INICIALIZAR_FORM()
        End If
    End Sub
    Private Sub INICIALIZAR_FORM()
        'CARGAR_COMBO(dropRol, "*", "TAB_ROLES", "FLG_CANC = 0 ORDER BY DESC_ROLE", "ID_ROLE", "", "DESC_ROLE", True)
        'CARGAR_COMBO(dropAgencia, "*", "TAB_AGENCIA", "FLG_CANC = 0 ORDER BY DESC_AGENCIA", "ID_AGENCIA", "", "DESC_AGENCIA", True)
        CARGAR_COMBO(dropRol, New SqlCommand("SELECT * FROM TAB_ROLES (NOLOCK) WHERE FLG_CANC = 0 ORDER BY DESC_ROLE"), "ID_ROLE", "", "DESC_ROLE", True)
        CARGAR_COMBO(dropAgencia, New SqlCommand("SELECT * FROM TAB_AGENCIA (NOLOCK) WHERE FLG_CANC = 0 ORDER BY DESC_AGENCIA"), "ID_AGENCIA", "", "DESC_AGENCIA", True)
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
            msg("New USER ACCEPTED")
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
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM TAB_USUARIOS (NOLOCK) WHERE ID_USUARIO = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.VarChar).Value = txtidUsuario.Text

        If bEXISTE_REGISTRO(comm) Then
            cargaUsuario(txtidUsuario.Text)
        End If
    End Sub

    Protected Sub BUSCARbutton_Click(sender As Object, e As ImageClickEventArgs) Handles BUSCARbutton.Click
        Dim rdrUsuarios As DataSet
        Dim sel As String = "SELECT * FROM TAB_USUARIOS INNER JOIN TAB_ROLES ON TAB_ROLES.ID_ROLE = TAB_USUARIOS.ID_ROLE WHERE ID_USUARIO IS NOT NULL "
        If txtNombreCompleto.Text <> "" Then
            sel &= "AND NOMBRE LIKE @PARAM1 "
        End If
        If txtTelefono.Text <> "" Then
            sel &= "AND TELEFONO = @PARAM2 "
        End If
        If txtEmail.Text <> "" Then
            sel &= "AND EMAIL = @PARAM3 "
        End If
        If chkflgCanc.Checked Then
            sel &= "AND FLG_CANC = @PARAM4 "
        End If
        If chkAdmin.Checked Then
            sel &= "AND FLG_ADMIN = @PARAM5 "
        End If

        If chkContacto.Checked Then
            sel &= "AND FLG_CONTACTO_AGENCIA = @PARAM6 "
        End If

        Dim comm As SqlCommand = New SqlCommand(sel)


        If txtNombreCompleto.Text <> "" Then
            comm.Parameters.Add("@PARAM1", SqlDbType.VarChar).Value = "%" & txtNombreCompleto.Text & "%"
        End If
        If txtTelefono.Text <> "" Then
            comm.Parameters.Add("@PARAM2", SqlDbType.VarChar).Value = txtTelefono.Text
        End If
        If txtEmail.Text <> "" Then
            comm.Parameters.Add("@PARAM3", SqlDbType.VarChar).Value = txtEmail.Text
        End If
        If chkflgCanc.Checked Then
            comm.Parameters.Add("@PARAM4", SqlDbType.Bit).Value = chkflgCanc.Checked
        End If
        If chkAdmin.Checked Then
            comm.Parameters.Add("@PARAM5", SqlDbType.Bit).Value = chkAdmin.Checked
        End If
        If chkContacto.Checked Then
            comm.Parameters.Add("@PARAM1", SqlDbType.Bit).Value = chkContacto.Checked
        End If

        rdrUsuarios = dsOpenDB(comm)
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

    Protected Sub dropAgencia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dropAgencia.SelectedIndexChanged

    End Sub
End Class