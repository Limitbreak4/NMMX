<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmDataValidaton.aspx.vb" Inherits="VisualCtrl.WebForm2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table class="nav-justified" style="height: 111px">
        <tr>
            <td style="width: 262px; height: 39px"></td>
            <td class="auto-style12" style="height: 39px; font-weight: bold; width: 570px;">Select file to validate</td>
            <td style="width: 387px; height: 39px"></td>
        </tr>
        <tr>
            <td style="width: 262px; height: 39px"></td>
            <td class="auto-style12" style="height: 39px; ali; width: 570px;">






                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                    <ContentTemplate>
                        <br />
                        <asp:FileUpload ID="fuValidation" runat="server" Height="27px" Width="491px" />
                        <br />
                        <asp:Button ID="cmdValidate" runat="server" Text="Validate" OnClick="Button1_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="cmdValidate" />
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
            <td style="width: 387px; height: 39px"></td>
        </tr>
        <tr>
            <td style="width: 262px; height: 232px;"></td>
            <td style="height: 232px; width: 570px;">
                <table class="auto-style15">
                    <tr>
                        <td class="auto-style18">File Name</td>
                        <td class="auto-style17">&nbsp;</td>
                        <td class="auto-style16">Evaluated rows</td>
                        <td><%=hfTotal.Value.ToString() %></td>
                    </tr>
                    <tr>
                        <td class="auto-style18"></td>
                        <td class="auto-style17">&nbsp;</td>
                        <td class="auto-style16">Geo 1</td>
                        <td><%=hfGeo1.Value.ToString() %></td>
                    </tr>
                    <tr>
                        <td class="auto-style18">
                            <asp:HyperLink ID="hlDownload" runat="server">[Download marked version]</asp:HyperLink>
                        </td>
                        <td class="auto-style17">&nbsp;</td>
                        <td class="auto-style16">Geo 2</td>
                        <td><%=hfGeo2.Value.ToString() %></td>
                    </tr>
                    <tr>
                        <td class="auto-style18">&nbsp;</td>
                        <td class="auto-style17">&nbsp;</td>
                        <td class="auto-style16">Blitz geo 1</td>
                        <td><%=hfBlitzGeo1.Value.ToString() %></td>
                    </tr>
                    <tr>
                        <td class="auto-style18">&nbsp;</td>
                        <td class="auto-style17">&nbsp;</td>
                        <td class="auto-style16">Blitz geo 2</td>
                        <td><%=hfBlitzGeo2.Value.ToString() %></td>
                    </tr>
                    <tr>
                        <td class="auto-style18">&nbsp;</td>
                        <td class="auto-style17">&nbsp;</td>
                        <td class="auto-style16">Blitz geo 3</td>
                        <td><%=hfBlitzGeo3.Value.ToString() %></td>
                    </tr>
                    <tr>
                        <td class="auto-style18">&nbsp;</td>
                        <td class="auto-style17">&nbsp;</td>
                        <td class="auto-style16">Rejected</td>
                        <td><%=hfNotOK.Value.ToString() %></td>
                    </tr>
                </table>
                <asp:HiddenField ID="hfJSONresult" runat="server" />
                <asp:HiddenField ID="hfTotal" runat="server" />
                <asp:HiddenField ID="hfNotOK" runat="server" />
                
                <asp:HiddenField ID="hfGeo1" runat="server" />
                <asp:HiddenField ID="hfGeo2" runat="server" />
                <asp:HiddenField ID="hfBlitzGeo1" runat="server" />
                <asp:HiddenField ID="hfBlitzGeo2" runat="server" />
                <asp:HiddenField ID="hfBlitzGeo3" runat="server" />
                <asp:HiddenField ID="hfFileName" runat="server" />

            </td>
            <td style="width: 387px; height: 232px;"></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="StylePlaceholder">
    <style type="text/css">
        .auto-style15 {
            width: 98%;
        }
        .auto-style16 {
            width: 174px;
        }
        .auto-style17 {
            width: 44px;
        }
        .auto-style18 {
            width: 192px;
        }
    </style>
</asp:Content>

