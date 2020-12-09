Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            HiddenField1.Value = 0
            HiddenField2.Value = 1500
        End If

        Debug.Print("entré al load")
    End Sub

    Protected Sub HiddenField1_ValueChanged(sender As Object, e As EventArgs) Handles HiddenField1.ValueChanged

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim porcentaje As Double = HiddenField1.Value
        Dim i As Integer
        For i = 0 To 1000000
            porcentaje = porcentaje + 1
            HiddenField1.Value = porcentaje
            Debug.Print(porcentaje)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "render();", True)
            Session("total") = "1000000"
            Session("current") = i.ToString()
        Next


    End Sub
End Class