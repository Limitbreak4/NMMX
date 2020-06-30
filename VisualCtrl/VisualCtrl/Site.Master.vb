Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If getDATO(False, Session("idUsuario")) = "" Then
            'Menu1.Visible = False
        Else
            Menu1.Visible = True
            If Session("FLG_ADMIN") = False Then
                Menu1.Items.RemoveAt(5)
            Else
                If Menu1.Items.Count < 6 Then
                    Dim newMenuItem As New MenuItem() With {
                                               .Text = "Settings",
                                               .Value = "Settings",
                                               .NavigateUrl = "~/Config/configMain.aspx"
                                               }
                    Menu1.Items.AddAt(5, newMenuItem)
                End If

            End If
        End If
    End Sub

    Protected Sub Menu1_MenuItemClick(sender As Object, e As MenuEventArgs) Handles Menu1.MenuItemClick
        lblPage.Text = Menu1.SelectedItem.Text
    End Sub

    'Protected Sub cmdAcceso_Click(sender As Object, e As ImageClickEventArgs) Handles cmdAcceso.Click
    '    Response.Redirect("~/frmLogin.aspx")
    'End Sub
End Class