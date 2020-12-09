Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss
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
        LIMPIAR_CAMPOS()
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
        'Dim sSql As String = ""
        Dim comm As SqlCommand
        If lblIdAgencia.Text = "" Then
            'sSql = "INSERT INTO TAB_AGENCIA (DESC_AGENCIA, NOMBRE_CONTACTO, MAIL_CONTACTO) VALUES("
            'sSql &= "'" & txtDescAgencia.Text & "', "
            'sSql &= "'" & txtContacto.Text & "', "
            'sSql &= "'" & txtEmail.Text & "' "
            'sSql &= ")"
            comm = New SqlCommand("INSERT INTO TAB_AGENCIA (DESC_AGENCIA, NOMBRE_CONTACTO, MAIL_CONTACTO) VALUES (@PARAM1, @PARAM2, @PARAM3)")
            comm.Parameters.Add("@PARAM1", SqlDbType.VarChar).Value = txtDescAgencia.Text
            comm.Parameters.Add("@PARAM2", SqlDbType.VarChar).Value = txtContacto.Text
            comm.Parameters.Add("@PARAM3", SqlDbType.VarChar).Value = txtEmail.Text
        Else
            'sSql &= "UPDATE TAB_AGENCIA SET "
            'sSql &= "DESC_AGENCIA = '" & txtDescAgencia.Text & "', "
            'sSql &= "NOMBRE_CONTACTO = '" & txtContacto.Text & "', "
            'sSql &= "MAIL_CONTACTO = '" & txtEmail.Text & "', "
            'sSql &= "FLG_CANC = " & IIf(chkflgCanc.Checked, 1, 0) & " "
            'sSql &= "WHERE ID_AGENCIA = " & lblIdAgencia.Text
            comm = New SqlCommand("UPDATE TAB_AGENCIA SET DESC_AGENCIA = @PARAM1, NOMBRE_CONTACTO = @PARAM2, MAIL_CONTACTO = @PARAM3,FLG_CANC = @PARAM4 WHERE ID_AGENCIA = PARAM5")
            comm.Parameters.Add("@PARAM1", SqlDbType.VarChar).Value = txtDescAgencia.Text
            comm.Parameters.Add("@PARAM2", SqlDbType.VarChar).Value = txtContacto.Text
            comm.Parameters.Add("@PARAM3", SqlDbType.VarChar).Value = txtEmail.Text
            comm.Parameters.Add("@PARAM4", SqlDbType.Bit).Value = IIf(chkflgCanc.Checked, 1, 0)
            comm.Parameters.Add("@PARAM5", SqlDbType.BigInt).Value = lblIdAgencia.Text


        End If
        ExecuteCmd(comm)
    End Sub

    Protected Sub BUSCARbutton_Click(sender As Object, e As ImageClickEventArgs) Handles BUSCARbutton.Click
        Dim rdrAgencia As DataSet
        'Dim sSql As String = "SELECT * FROM TAB_AGENCIA WHERE ID_AGENCIA > 0 "
        Dim sSql As String = "ID_AGENCIA > 0 "
        If txtDescAgencia.Text <> "" Then
            'sSql &= "AND DESC_AGENCIA = '" & txtDescAgencia.Text & "' "
            sSql &= " AND DESC_AGENCIA = @PARAM1 "
        End If
        If txtContacto.Text <> "" Then
            'sSql &= "AND NOMBRE_CONTACTO = '" & txtContacto.Text & "', "
            sSql &= " AND NOMBRE_CONTACTO = @PARAM2"
        End If
        If txtEmail.Text <> "" Then
            'sSql &= "AND MAIL_CONTACTO = '" & txtEmail.Text & "' "
            sSql &= " AND MAIL_CONTACTO = @PARAM3"
        End If
        sSql = AntiXssEncoder.HtmlEncode(sSql, False)
        Dim comm As SqlCommand = New SqlCommand(sSql)
        If txtDescAgencia.Text <> "" Then
            'sSql &= "AND DESC_AGENCIA = '" & txtDescAgencia.Text & "' "
            comm.Parameters.Add("@PARAM1", SqlDbType.VarChar).Value = txtDescAgencia.Text
        End If
        If txtContacto.Text <> "" Then
            'sSql &= "AND NOMBRE_CONTACTO = '" & txtContacto.Text & "', "
            comm.Parameters.Add("@PARAM2", SqlDbType.VarChar).Value = txtContacto.Text
        End If
        If txtEmail.Text <> "" Then
            'sSql &= "AND MAIL_CONTACTO = '" & txtEmail.Text & "' "
            comm.Parameters.Add("@PARAM3", SqlDbType.VarChar).Value = txtEmail.Text
        End If

        rdrAgencia = dsOpenDB(comm)
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
        Dim comm As SqlCommand = New SqlCommand("SELECT * FROM TAB_AGENCIA WHERE ID_AGENCIA = @PARAM1")
        comm.Parameters.Add("@PARAM1", SqlDbType.BigInt).Value = iIdAgencia
        'Dim rdrAgencia As DataSet = dsOpenDB("*", "TAB_AGENCIA", "ID_AGENCIA = " & iIdAgencia)
        Dim rdrAgencia As DataSet = dsOpenDB(comm)
        If rdrAgencia.Tables(0).Rows.Count > 0 Then
            lblIdAgencia.Text = rdrAgencia.Tables(0).Rows(0).Item("ID_AGENCIA")
            txtDescAgencia.Text = rdrAgencia.Tables(0).Rows(0).Item("DESC_AGENCIA")
            txtContacto.Text = rdrAgencia.Tables(0).Rows(0).Item("NOMBRE_CONTACTO")
            txtEmail.Text = rdrAgencia.Tables(0).Rows(0).Item("MAIL_CONTACTO")
            chkflgCanc.Checked = rdrAgencia.Tables(0).Rows(0).Item("FLG_CANC")
        End If
    End Sub
End Class