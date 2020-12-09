<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm3.aspx.vb" Inherits="VisualCtrl.WebForm3" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        <div>
            Página Secreta de Sophie<br />
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:FileUpload ID="FileUpload1" runat="server" /><br />
                    <asp:Button ID="Button1" runat="server" Text="Upload File" /><br />
                    <asp:Label ID="Label1" runat="server"></asp:Label><br />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="Button1" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
        </div>
    </form>
</body>
</html>
