<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmMarketing.aspx.vb" Inherits="VisualCtrl.frmMarketing" %>
<%@ Import Namespace="System.Web.Script.Serialization.JavaScriptSerializer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p></p>
    <br />
    <table class="nav-justified" style=" vertical-align: top; border-style: none">
        <tr>
            <td class="modal-sm" style="width: 100px; height: 250px; vertical-align: top; border-width: 1px; border-style: none">
                <table style="width: 100px; vertical-align: top">
                    <tr>
                        <td class="modal-sm" style="width: 100px; border-style: none">Current Assignment</td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px;vertical-align: top; border-width: 1px; border-style: none">
                            <asp:Button ID="Button1" runat="server" Text="Download Results" />
                        </td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">Date Created</td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">Status</td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField HeaderText="Agency" />
                                    <asp:BoundField HeaderText="Downloaded" />
                                    <asp:BoundField HeaderText="Results Uploaded" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="modal-sm" style="width: 100px; vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                    </tr>
                </table>
            </td>
            <td style="height: 250px; width: 330px; align-content: center;vertical-align: top; border-width: 0px; border-style: none;" class="img-responsive">
                <table style="align-content: center; vertical-align: top">
                    <tr>
                        <td>
                            <table style="width: 330px; vertical-align: top; border-width: 0px; border-style: none">
                                <tr>
                                    <td class="titulo" style="border-style: none">COMPLETED VISITS</td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="text-align: center; width: 330px; vertical-align: top; border-width: 1px; border-style: none">
                                <tr>
                                    <td>

                                        <asp:HiddenField ID="TotalVisits" runat="server" />
                                        <%=TotalVisits.Value %></td>
                                    <td>
                                        <asp:HiddenField ID="OBTotalVisits" runat="server" />
                                        <%=OBTotalVisits.Value %></td>
                                    <td>
                                        <asp:HiddenField ID="PropTotalVisits" runat="server" />
                                        <%=PropTotalVisits.Value %></td>
                                </tr>
                                <tr>
                                    <td>Total
                                        l
                                        <br />
                                        Visits</td>
                                    <td>OB Total
                                        <br />
                                        Visits</td>
                                    <td>Prop Total
                                        <br /Visits</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div id="chart">
                    <svg></svg>
                    <div>
                    </div>

                </div>

            </td>
            <td style="height: 330px; width: 100px;vertical-align: top; border-width: 1px; border-style: none"></td>
            <td style="height: 330px; width: 330px;vertical-align: top; border-width: 1px; border-style: none">
                <table class="tabla2" style="vertical-align: top; vertical-align: top; border-width: 0px; border-style: none">
                    <tr>

                        <td>
                            <table style="width: 330px; vertical-align: top; border-width: 0px; border-style: none">
                                <tr>
                                    <td class="titulo" style="border-style: none">POP COVERAGE</td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="tabla2" style="width: 330px; vertical-align: top; vertical-align: top; border-width: 1px; border-style: none">
                                <tr>
                                    <td class="tercio" style="vertical-align: top; border-width: 1px; border-style: none">
                                        <asp:HiddenField ID="TotalPopCoverage" runat="server" />
                                        <%=TotalPopCoverage.Value %>%
                                    </td>
                                    <td class="tercio" style="vertical-align: top; border-width: 1px; border-style: none">
                                        <asp:HiddenField ID="OrganicPop" runat="server" />
                                        <%=OrganicPop.Value %>%
                                    </td>
                                    <td class="tercio" style="vertical-align: top; border-width: 1px; border-style: none">
                                        <asp:HiddenField ID="PlacedPop" runat="server" />
                                        <%=PlacedPop.Value %>%
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="tabla2" style="width: 330px; vertical-align: top; vertical-align: top; border-width: 1px; border-style: none">
                                <tr>
                                    <td class="labels" style="vertical-align: top; border-width: 1px; border-style: none">Total POP Coverage</td>
                                    <td class="labels" style="vertical-align: top; border-width: 1px; border-style: none">Organic<br />
                                        POP</td>
                                    <td class="labels" style="vertical-align: top; border-width: 1px; border-style: none">Placed<br />
                                        POP</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                    </tr>
                </table>

                <table class="borderless" style="width: 330px; vertical-align: top;vertical-align: top; border-width: 1px; border-style: none">
                    <tr>
                        <td class="columna" style="vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                        <td class="colborde" style="vertical-align: top; border-width: 1px; border-style: none">OB</td>
                        <td class="espacio" style="vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                        <td class="columna" style="vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                        <td class="colborde" style="vertical-align: top; border-width: 1px; border-style: none">PROP</td>
                    </tr>
                    <tr>
                        <td class="labels" style="font-weight: bold;vertical-align: top; border-width: 1px; border-style: none">Total<br />
                            Coverage</td>
                        <td class="arriba" style="vertical-align: top; border-width: 1px; border-style: none">
                            <asp:HiddenField ID="OBtotal" runat="server" />
                            <%=OBtotal.Value %>%
                        </td>
                        <td class="espacio" style="vertical-align: top; border-width: 1px; border-style: none">&nbsp;</td>
                        <td class="labels" style="font-weight: bold; vertical-align: top; border-width: 1px; border-style: none">Total<br />
                            Coverage</td>
                        <td class="arriba" style="vertical-align: top; border-width: 1px; border-style: none">
                            <asp:HiddenField ID="PropTotal" runat="server" />
                            <%=PropTotal.Value %>%
                        </td>
                    </tr>
                    <tr>
                        <td class="labels" style="vertical-align: top; border-width: 1px; border-style: none;">Organic</td>
                        <td class="enmedio" style="vertical-align: top; border-width: 1px; border-style: none">
                            <asp:HiddenField ID="OBorganic" runat="server" />
                            <%=OBorganic.Value %>%
                        </td>
                        <td class="espacio" style="vertical-align: top; border-width: 1px; border-style: none"></td>
                        <td class="labels" style="vertical-align: top; border-width: 1px; border-style: none">Organic</td>
                        <td class="enmedio" style="vertical-align: top; border-width: 1px; border-style: none">
                            <asp:HiddenField ID="Proporganic" runat="server" />
                            <%=Proporganic.Value %>%
                        </td>
                    </tr>
                    <tr>
                        <td class="labels" style="vertical-align: top; border-width: 1px; border-style: none">Placed</td>
                        <td class="abajo" style="vertical-align: top; border-width: 1px; border-style: none">
                            <asp:HiddenField ID="OBplaced" runat="server" />
                            <%=OBplaced.Value %>%
                        </td>
                        <td class="espacio" style="vertical-align: top; border-width: 1px; border-style: none"></td>
                        <td class="labels" style="vertical-align: top; border-width: 1px; border-style: none">Placed</td>
                        <td class="abajo" style="vertical-align: top; border-width: 1px; border-style: none">
                            <asp:HiddenField ID="Propplaced" runat="server" />
                            <%=Propplaced.Value %>%
                        </td>
                    </tr>
                </table>
                </td>
        </tr>
        <tr>
            <td class="modal-sm" style="width: 100px;vertical-align: top; border-width: 1px; border-style: none"></td>
            <td style="width: 330px;vertical-align: top; border-width: 1px; border-style: none" class="modal-sm">
                <table style="vertical-align: top;vertical-align: top; border-width: 1px; border-style: none">
                    <tr>
                        <td class="titulo" style="vertical-align: top; width:330px;  border-width: 1px; border-style: none">DOWNSTREAM</td>
                    </tr>
                    <tr>
                        <td>
                            <div id="visitsperyear" style="width:330px; vertical-align: top; border-width: 1px; border-style: none">
                                <svg></svg>
                            </div>
                        </td>
                    </tr>
                </table>

            </td>
            <td style="width: 100px; vertical-align: top; border-width: 1px; border-style: none"></td>
            <td style="width: 330px; vertical-align: top; border-width: 1px; border-style: none">
                <table style="vertical-align: top; vertical-align: top; border-width: 1px; border-style: none">
                    <tr class="titulo" style="width: 330px; vertical-align: top; border-width: 1px; border-style: none">
                        <td>POP COVERAGE
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>

                                <object data="./grafica.aspx" height="300" style="width: 330px">
                                    <embed src="./grafica.aspx" width="330" height="400"/>
                                    Error: Embedded data could not be displayed.
                                </object>
                            </div>
                        </td>
                    </tr>
                </table>


            </td>
        </tr>
    </table>
    <br />
    <p id="demo"></p>
    <br />
    <asp:HiddenField ID="CompletedJSON" runat="server" />
    <asp:HiddenField ID="DownstreamJSON" runat="server" />
    <asp:HiddenField ID="MesesJSON" runat="server" />
    <script src="https://d3js.org/d3.v3.min.js" charset="utf-8"></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/d3/4.2.2/d3.min.js'></script>
    <script src="../Scripts/barChart.js"></script>
    <script src="../Scripts/barMes.js"></script>
    <script>
        //var estemes = [
        //    { "letter": "Prop", "frequency": "1500" },
        //    { "letter": "OB", "frequency": "2700" }
        //];
        var estemes = <%=CompletedJSON.Value%>
        BarMes.initialize(estemes, "chart", 225, 100);
        console.log(estemes);
        //var visits = [
        //    { "letter": "Jan", "frequency": "512" },
        //    { "letter": "Feb", "frequency": "2880" },
        //    { "letter": "Mar", "frequency": "1322" },
        //    { "letter": "Apr", "frequency": "4021" },
        //    { "letter": "May", "frequency": "3555" },
        //    { "letter": "Jun", "frequency": "2880" },
        //    { "letter": "Jul", "frequency": "1322" },
        //    { "letter": "Aug", "frequency": "4021" },
        //    { "letter": "Sep", "frequency": "3555" },
        //    { "letter": "Oct", "frequency": "2880" },
        //    { "letter": "Nov", "frequency": "1322" },
        //    { "letter": "Dec", "frequency": "1024" }
        //];
        var visits = <%=DownstreamJSON.Value%>
        BarChart.initialize(visits, "visitsperyear", 225, 100);
    </script>

</asp:Content>
