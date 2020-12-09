<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Agency.Master" CodeBehind="frmCargarAsingacion.aspx.vb" Inherits="VisualCtrl.frmCargarAsingacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <table class="nav-justified">
        <tr>
            <td><span style="font-weight: bold">CARGAR ARCHIVOS DE RESULTADOS</span></td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top" class="auto-style12">
                &nbsp;</td>
            <td style="vertical-align: top" class="auto-style12">
                </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                &nbsp;<asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="Button1" runat="server" Text="Cargar resultados" />
&nbsp;</td>
            <td style="vertical-align: top">
                &nbsp;</td>
        </tr>
        </table>
</p>
<p>
    <%--<asp:GridView ID="grdAsignmentData" runat="server" AutoGenerateColumns="False" Width="95%" DataKeyNames="ID_ASIGNACION" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
        <Columns>
            <asp:BoundField DataField="FH_ASIGNACION" HeaderText="Fecha de Creación">
            <ItemStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="NOMBRE_ARCHIVO" HeaderText="Archivo">
            <ItemStyle Width="400px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:FileUpload ID="fuOutcome" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:ButtonField ButtonType="Button" Text="Cargar" />
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
    </asp:GridView>--%>
</p>
<p>
    &nbsp;</p>
</asp:Content>
