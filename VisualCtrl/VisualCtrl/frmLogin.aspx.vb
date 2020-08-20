Imports System.Configuration.ConfigurationManager

Public Class frmLogin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.Title = AppSettings("NOMBRE_APLICACION")
            USUARIO_CLEAN()
            LIMPIAR_CAMPOS()
        End If
    End Sub



    Private Sub USUARIO_CLEAN()
        'ConfigSistema.LOAD()
        Session("idUsuario") = ""
        Session("NombreCompleto") = ""
        Session("FLG_ADMIN") = False
    End Sub

    Private Sub LIMPIAR_CAMPOS()
        txtUsuario.Text = ""
        txtPwd.Text = ""
        SetFocus(txtUsuario)
    End Sub


    Protected Sub cmdLogin_Click(sender As Object, e As ImageClickEventArgs) Handles cmdLogin.Click
        Dim objCifrar As New cls_EncriptacionRSA, dsUSUARIO As DataSet
        Dim sSql As String
        If Debugger.IsAttached Then
            If Trim(txtUsuario.Text) = "" And Trim(txtPwd.Text) = "" Then
                txtUsuario.Text = "ADMIN"
                txtPwd.Text = UCase(Chr(64) + Chr(77) + Chr(97) + Chr(114) + Chr(97) + Chr(108) + Chr(106) + Chr(111) + Chr(48))
            End If
        End If
        If txtUsuario.Text <> "" Then
            If txtPwd.Text <> "" Then
                'If TrUc(txtUsuario.Text) = "ADMIN" And
                '(TrUc(txtPwd.Text) = UCase(Chr(64) + Chr(77) + Chr(97) + Chr(114) + Chr(97) + Chr(108) + Chr(106) + Chr(111) + Chr(48)) _
                ' Or TrUc(txtPwd.Text) = "@ACCESO1#") Then
                '    Session("idUsuario") = "ADMIN"
                '    Session("NombreCompleto") = "ADMINISTRADOR DEL SISTEMA"
                '    Session("ID_ROLE") = 1
                '    Session("FLG_ADMIN") = True
                '    Session("ID_AGENCIA") = getDATO(False, 1)
                '    If Session("ID_ROLE") = 1 Then
                '        Response.Redirect("~/Marketing/frmMarketing.aspx")
                '    ElseIf Session("ID_ROLE") = 2 Then
                '        Response.Redirect("~/Agency/frmAgency.aspx")
                '    End If
                'Else
                If bCAMPO_EXISTE("TAB_USUARIOS", "ID_USUARIO", TrUc(txtUsuario.Text)) = True Then
                        sSql = "SELECT * FROM  TAB_USUARIOS WHERE ID_USUARIO ='" + TrUc(txtUsuario.Text) + "'"
                        dsUSUARIO = dsOpenDB(sSql)
                        If dsUSUARIO.Tables(0).Rows.Count > 0 Then
                            With dsUSUARIO.Tables(0).Rows(0)
                                If .Item("FLG_CANC") = 0 Then
                                    If getDATO(False, .Item("PWD")) <> "" Then
                                        If TrUc(objCifrar.desencriptar128BitRijndael(getDATO(False, .Item("PWD")), s_Clave)) = TrUc(txtPwd.Text) Then
                                            Session("idUsuario") = TrUc(txtUsuario.Text)
                                            Session("NombreCompleto") = .Item("NOMBRE")
                                            Session("ID_ROLE") = .Item("ID_ROLE")
                                            Session("FLG_ADMIN") = .Item("FLG_ADMIN")
                                            Session("ID_AGENCIA") = getDATO(False, .Item("ID_AGENCIA"))
                                            CIERRA_DATASET(dsUSUARIO)
                                            If Session("ID_ROLE") = 1 Then
                                                Response.Redirect("~/Marketing/frmMarketing.aspx")
                                            ElseIf Session("ID_ROLE") = 2 Then
                                                Response.Redirect("~/Agency/frmAgency.aspx")
                                            End If
                                        Else
                                            objCifrar = Nothing
                                            msg("INCORRECT PASSWORD, PLEASE CHECK.")
                                            LIMPIAR_CAMPOS()
                                        End If
                                    Else
                                        msg("NO PASSWORD HAS BEEN SET FOR THIS USER, PLEASE CONTACT YOUR ADMINISTRATOR")
                                        LIMPIAR_CAMPOS()
                                    End If
                                Else
                                    msg("USER DISABLED, PLEASE CONTACT YOUR ADMINISTRATOR")
                                    LIMPIAR_CAMPOS()
                                End If
                            End With
                        End If
                        CIERRA_DATASET(dsUSUARIO)
                    Else
                        msg("USERT NOT FOUND")
                        LIMPIAR_CAMPOS()
                    End If
                'End If
            Else
                msg("PASSWORD FIELD CAN'T BE EMPTY")
                LIMPIAR_CAMPOS()
            End If
        Else
            msg("USER FIELD CAN'T BE EMPTY")
            LIMPIAR_CAMPOS()
        End If
    End Sub

    Private Sub msg(ByVal txtIn)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" & txtIn & "');", True)
    End Sub

    Protected Sub txtPwd_TextChanged(sender As Object, e As EventArgs) Handles txtPwd.TextChanged
        cmdLogin_Click(sender, Nothing)
    End Sub
End Class