<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmNewAssignment.aspx.vb" Inherits="VisualCtrl.frmNewAssignment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <table style="width:100%;">
            <tr>
                <td style="width: 290px; height: 11px;" class="modal-sm"><strong>SELECT RELATED CAMPAIGN</strong></td>
                <td class="auto-style12" style="height: 11px; width: 247px;"><strong></strong></td>
                <td class="auto-style12" style="height: 11px; width: 249px;"><strong></strong></td>
            </tr>
                        <tr>
                <td style="width: 290px; height: 33px;" class="modal-sm"><asp:DropDownList ID="dropCampaigns" runat="server" Width="512px" AutoPostBack="True" Height="19px">
                </asp:DropDownList></td>
                <td style="height: 33px; width: 247px;"><strong>&nbsp;</strong><span style="font-weight: bold">INCLUDE BLITZ IN PLANNING</span></td>
                <td style="height: 33px; width: 249px;">&nbsp;</td>
            </tr>
                        <tr>
                <td style="width: 290px; height: 37px;" class="modal-sm"><strong>
                    <br />
                    UPLOAD A FILE WITH PROCESS TO VISIT<br />
                    </strong>
                    <asp:FileUpload ID="fu" runat="server" Width="430px" style="margin-top: 0" />
                            </td>
                <td style="height: 37px; width: 247px;">
                    <asp:CheckBox ID="chkGeo1" runat="server" Checked="True" Text="Include Geo 1" />
                    <br />
                    <br />
                    <asp:CheckBox ID="chkGeo2" runat="server" Checked="True" Text="Include Geo 2" />
                    <br />
                    <br />
                 <asp:CheckBox ID="chkBlitz1" runat="server" Text="Include Geo 1 blitz" />
                    <br />
                    <br />
                    <asp:CheckBox ID="chkBlitz2" runat="server" Text="Include Geo 2 blitz" />
                    <br />
                    <br />
                    <asp:CheckBox ID="chkBlitz3" runat="server" Text="Include Geo 3 blitz" />
                            </td>
                <td style="height: 37px"><strong></strong></td>
            </tr>
            <tr>
                <td style="width: 290px; height: 56px;">
                    &nbsp;</td>
                <td style="height: 56px; width: 247px;">

                    

                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="cmdIniciaProces" runat="server" Text="Process File" BackColor="#1871BE" BorderStyle="None" ForeColor="White" />

                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="cmdIniciaProces" />
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div id="Background"></div>
                        <div id="Progress">
                            Procesando, espere por favor... <br>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <script type="text/javascript">
                    window.onsubmit = function () {
                        if (Page_IsValid) {
                            var updateProgress = $find("<%= UpdateProgress1.ClientID %>");
                            window.setTimeout(function () {
                                updateProgress.set_visible(true);
                            }, 100);
                        }
                    }
                </script>
                </td>
                <td style="height: 56px; width: 249px;">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 290px" class="modal-sm">
                    &nbsp;</td>
                <td class="modal-sm" style="width: 247px">&nbsp;</td>
                <td style="width: 249px">&nbsp;</td>
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
