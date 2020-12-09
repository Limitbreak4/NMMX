Public Class WebForm3
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (FileUpload1.HasFile) Then
            Dim arch As String = FileUpload1.FileName
            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/UploadedFiles/" + arch))
            Label1.ForeColor = System.Drawing.Color.Green
            Label1.Text = "Uploaded Fiel: " + arch
        Else
            Label1.ForeColor = System.Drawing.Color.Red
            Label1.Text = "No file to upload"
        End If
    End Sub
End Class