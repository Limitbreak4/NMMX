<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmLogin.aspx.vb" Inherits="VisualCtrl.frmLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
</p>

    <table style="width:100%;">
        <tr>
            <td class="text-center" style="color: #003399">Username</td>
        </tr>
        <tr>
            <td class="text-center">
                <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="text-center" style="color: #003399">Password</td>
        </tr>
        <tr>
            <td class="text-center">
                <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" AutoPostBack="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="text-center">
                        <asp:ImageButton ID="cmdLogin" runat="server" ImageUrl="~/Img/acceso.png" />
            </td>
        </tr>
    </table>

</asp:Content>
