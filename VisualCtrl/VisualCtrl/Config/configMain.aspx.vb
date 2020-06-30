Public Class configMain
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("idUsuario") = "" Then
            Response.Redirect("~/Default.aspx")
        End If
        If Session("FLG_ADMIN") = False Then
            Response.Redirect("~/Marketing/frmMarketing.aspx")
        End If
        If Not IsPostBack Then
            'INICIALIZAR_FORM()
        End If

    End Sub

    Protected Sub cmdConfigGeneral_Click(sender As Object, e As ImageClickEventArgs) Handles cmdConfigUsuarios.Click
        Response.Redirect("~/Config/frmTabUsuarios.aspx")
    End Sub

    Protected Sub cmdConfMail_Click(sender As Object, e As ImageClickEventArgs) Handles cmdConfMail.Click
        Response.Redirect("~/Config/frmConfigMail.aspx")
    End Sub

    Protected Sub cmdConfigAgencias_Click(sender As Object, e As ImageClickEventArgs) Handles cmdConfigAgencias.Click
        Response.Redirect("~/Config/frmConfigAgencias.aspx")
    End Sub
End Class