<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Agency.Master" CodeBehind="frmDownloadAssignment.aspx.vb" Inherits="VisualCtrl.frmDownloadAssignment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    <table style="width:100%;">
        <tr>
            <td><span style="font-weight: bold">DESCARGAR ASIGNACIONES PREVIAS</span></td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                &nbsp;</td>
            <td style="vertical-align: top">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                &nbsp;</td>
            <td style="vertical-align: top">
                &nbsp;</td>
        </tr>
        </table>
</p>
<p>
    <asp:GridView ID="grdAsignmentData" runat="server" AutoGenerateColumns="False" Width="95%" DataKeyNames="ID_ASIGNACION" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
        <Columns>
            <asp:ButtonField DataTextField="FH_ASIGNACION" HeaderText="Fecha de Creación" CommandName="cmdShowSummary" >
            <ItemStyle Width="200px" />
            </asp:ButtonField>
            <asp:ButtonField CommandName="cmdDownload" DataTextField="NOMBRE_ARCHIVO" HeaderText="File" Text="Botón" />
            <asp:CheckBoxField DataField="FLG_DESCARGA" HeaderText="Descargado">
            <ItemStyle Width="50px" />
            </asp:CheckBoxField>
            <asp:BoundField DataField="FH_PRIMERA_DESCARGA" HeaderText="Fecha">
            <ItemStyle Width="150px" />
            </asp:BoundField>
            <asp:BoundField DataField="FH_ULTIMA_DESCARGA" HeaderText="Última Descarga">
            <ItemStyle Width="150px" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#007DBB" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#00547E" />
    </asp:GridView>
</p>
<p>
    &nbsp;</p>
</asp:Content>
