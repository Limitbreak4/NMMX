<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmPrevAssignment.aspx.vb" Inherits="VisualCtrl.frmPrevAssignment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<p>
    <table style="width:100%;">
        <tr>
            <td><strong>Previous </strong>A<span style="font-weight: bold">ssignments</span></td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <asp:DropDownList ID="dropDates" runat="server" Width="200px" AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td style="vertical-align: top">
                &nbsp;</td>
        </tr>
        </table>
</p>
<p>
    <strong>Download Status</strong></p>
<p>
    <asp:GridView ID="grdAsignmentData" runat="server" AutoGenerateColumns="False" Width="95%" DataKeyNames="ID_ASIGNACION" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
        <Columns>
            <asp:ButtonField DataTextField="FH_ASIGNACION" HeaderText="Date Created" CommandName="cmdShowSummary" />
            <asp:BoundField DataField="DESC_AGENCIA" HeaderText="Agency" />
            <asp:ButtonField CommandName="cmdDownload" DataTextField="NOMBRE_ARCHIVO" HeaderText="File" Text="Botón" />
            <asp:CheckBoxField DataField="FLG_DESCARGA" HeaderText="Downloaded">
            <ItemStyle Width="50px" />
            </asp:CheckBoxField>
            <asp:BoundField DataField="FH_ULTIMA_DESCARGA" HeaderText="Last Downloaded" />
            <asp:BoundField HeaderText="Results Uploaded" />
            <asp:ButtonField CommandName="cmdDownload" Text="Download" />
            <asp:ButtonField CommandName="cmdMail" Text="Mail" />
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
    <strong>Summary</strong></p>
<p>
    <table style="width:100%;">
        <tr>
            <td style="width: 551px">
                <asp:GridView ID="grdSummary" runat="server" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                    <Columns>
                        <asp:BoundField DataField="AGENCY" HeaderText="Agency" />
                        <asp:BoundField DataField="ASSIGNED_BUSINESS" HeaderText="Assigned Business" />
                        <asp:BoundField DataField="VISITED_BUSINESS" HeaderText="Visited Business" />
                        <asp:BoundField DataField="EFECTIVENESS" HeaderText="Efectiveness" />
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
            </td>
            <td>
                <asp:Image ID="imgGraph" runat="server" Height="162px" Width="370px" />
            </td>
        </tr>
        <tr>
            <td style="width: 551px">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</p>
</asp:Content>
