<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmNewAssignment.aspx.vb" Inherits="VisualCtrl.frmNewAssignment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <table style="width:100%;">
            <tr>
                <td style="width: 307px" class="modal-sm"><strong>UPLOAD A FILE WITH PROCESS TO VISIT</strong></td>
                <td><strong></strong></td>
                <td><strong></strong></td>
            </tr>
            <tr>
                <td style="width: 307px; height: 22px;">
                    <asp:FileUpload ID="fu" runat="server" Width="430px" />
                </td>
                <td style="height: 22px">
                    <asp:Button ID="cmdIniciaProces" runat="server" Text="Process File" BackColor="#1871BE" BorderStyle="None" ForeColor="White" />
                </td>
                <td style="height: 22px"></td>
            </tr>
            <tr>
                <td style="width: 307px" class="modal-sm">
                    &nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>




        <table style="width:100%;">
            <tr>
                <td>&nbsp;</td>
                <td style="width: 453px; vertical-align: top;">

        <asp:ListBox ID="lbresult" runat="server" Height="136px" Width="443px"></asp:ListBox>

                </td>
                <td>
                    <asp:GridView ID="grdFiles" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="ID_ASIGNACION" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                        <Columns>
                            <asp:BoundField DataField="NOMBRE_ARCHIVO" HeaderText="File" />
                            <asp:BoundField DataField="DESC_AGENCIA" HeaderText="Agency" />
                            <asp:BoundField DataField="ROWS_OK" HeaderText="OK" />
                            <asp:BoundField DataField="ROWS_NOT_OK" HeaderText="Not Ok" />
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
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 453px">&nbsp;</td>
                <td>
                    <asp:Label ID="lblNotOk" runat="server" Text="File Not OK" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 453px">&nbsp;</td>
                <td style="margin-left: 40px">
                    <asp:GridView ID="grdNAKFile" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="ID_ASIGNACION" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                        <Columns>
                            <asp:BoundField DataField="NOMBRE_ARCHIVO" HeaderText="File" />
                            <asp:BoundField HeaderText="Not Ok" DataField="ROWS_NOT_OK" />
                            <asp:ButtonField CommandName="cmdDownload" Text="Download" />
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
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 453px">&nbsp;</td>
                <td style="margin-left: 40px">
                    <asp:Label ID="lblBlitz" runat="server" Text="Blitz" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 453px">&nbsp;</td>
                <td style="margin-left: 40px">
                    <asp:GridView ID="grdBlitz" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="ID_ASIGNACION" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                        <Columns>
                            <asp:BoundField DataField="NOMBRE_ARCHIVO" HeaderText="File" />
                            <asp:BoundField HeaderText="Blitz" DataField="ROWS_OK" />
                            <asp:ButtonField CommandName="cmdDownload" Text="Download" />
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
            </tr>
        </table>

   
</asp:Content>
