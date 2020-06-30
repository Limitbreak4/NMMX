Public Class AgencyMaster
    Inherits MasterPage
    Protected Sub Menu1_MenuItemClick(sender As Object, e As MenuEventArgs) Handles Menu1.MenuItemClick
        'lblPage.Text = Menu1.SelectedItem.Text
        'lblUsuario.Text = Session("NombreCompleto")
    End Sub

    'Protected Sub cmdAcceso_Click(sender As Object, e As ImageClickEventArgs) Handles cmdAcceso.Click
    '    Response.Redirect("~/frmLogin.aspx")
    'End Sub
End Class