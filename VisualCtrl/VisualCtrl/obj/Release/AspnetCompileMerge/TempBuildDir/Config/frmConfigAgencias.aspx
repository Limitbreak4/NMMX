﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmConfigAgencias.aspx.vb" Inherits="VisualCtrl.frmConfigAgencias" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="js/jquery.ui.core.js" type="text/javascript"></script>
    <script src="js/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="js/jquery.ui.button.js" type="text/javascript"></script>
    <script src="js/jquery.ui.position.js" type="text/javascript"></script>
    <script src="js/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="js/jquery.ui.combobox.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        if (history.forward(1)) { history.replace(history.forward(1)); }
    </script>

    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            border: 3px solid black;
            background-color: #FFFFFF;
            padding-top: 10px;
            padding-left: 10px;
        }
        .modal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
        .modalLOADING
        {
            position: fixed;
            top: 45%;
            left: 45%;
        }
        .auto-style15 {
            width: 418px;
        }
        </style>

    <div align="left">
        <h4>
            CONFIGURACION DE AGENCIAS
        </h4>
    </div>
    <div align="left" >
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="LIMPIARbutton" runat="server" Height="32px" ImageUrl="~/Img/Borrar.png"
                        ToolTip="LIMPIAR" Width="32px" />
                </td>
                <td>
                    <asp:Image ID="image0" runat="server" Width="32px" ImageUrl="~/Img/blank.png" />
                </td>
                <td>
                    <asp:ImageButton ID="SALVARbutton" runat="server" Height="32px" ImageUrl="~/Img/Salvar.png"
                        ToolTip="SALVAR" Width="32px" />
                </td>
                <td>
                    <asp:Image ID="image1" runat="server" Width="32px" ImageUrl="~/Img/blank.png" />
                </td>
                <td>
                    <asp:ImageButton ID="BUSCARbutton" runat="server" Height="32px" ImageUrl="~/Img/Buscar.png"
                        ToolTip="BUSCAR" Width="32px" />
                </td>
                <td>
                    <asp:Image ID="image2" runat="server" Width="32px" ImageUrl="~/Img/blank.png" />
                </td>
                <td>
                    <asp:ImageButton ID="REGRESARbutton" runat="server" Height="32px" ImageUrl="~/Img/Regresar.png"
                        ToolTip="REGRESAR" Width="32px" PostBackUrl="~/frmConfiguracion.aspx" />
                </td>
            </tr>
        </table>
    </div>
    <div align="center">
        <table>
            <tr>
                <td align="left">
                    Agencia</td>
                <td align="left" class="auto-style15">
                    <asp:TextBox ID="txtDescAgencia" runat="server" Width="350px" MaxLength="100"></asp:TextBox>
                </td>
                <td align="left">
                    &nbsp;</td>
                <td align="left">
                    <asp:Label ID="lblIdAgencia" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    Contacto</td>
                <td align="left" class="auto-style15">
                    <asp:TextBox ID="txtContacto" runat="server" Width="300px" MaxLength="255"></asp:TextBox>
                </td>
                <td align="left">
                    Email</td>
                <td align="left">
                    <asp:TextBox ID="txtEmail" runat="server" Width="350px" MaxLength="255"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td align="left">
                    &nbsp;</td>
                <td align="left" class="auto-style15">
                    <asp:CheckBox ID="chkflgCanc" runat="server" Text="Cancelado" />
                </td>
                <td align="left">
                    &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="left">
                    :</td>
                <td align="left" class="auto-style15">
                    &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="left">
                    &nbsp;</td>
                <td align="left" class="auto-style15">
                    NOTA: EL CONTACTO DE LA AGENCIA PASA A SER EL&nbsp; DE CONFIGURACIÓN DE USUARIOS</td>
                <td align="left">
                    &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
            </tr>
        </table>
    </div>

    <!-- ModalPopupExtender SEARCH -->
    <asp:ImageButton ID="btnHIDE_SEARCH" runat="server" ImageUrl="~/Img/blank.png" Height="16px"
        Width="18px" />
    <asp:ModalPopupExtender ID="mpSEARCH" runat="server" 
        PopupControlID="panelSEARCH" TargetControlID="btnHIDE_SEARCH"
        CancelControlID="btnCLOSE_SEARCH" 
        BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>
    <asp:Panel ID="panelSEARCH" runat="server" CssClass="modalPopup" align="center" Style="display:visible"
        Width="96%">
        <table style="width: 100%;" align="center">
            <tr align="center">
                <td width="100%" align="center" valign="top" >
                    <asp:ImageButton ID="btnCLOSE_SEARCH" runat="server" Height="36px" ImageUrl="~/Img/Regresar.png"
                        ToolTip="REGRESAR" />
                </td>
            </tr>
            <tr>
                <td align="center" class="style37" valign="top" width="90%">
                    <strong>AGENCIA</strong>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top" width="100%" >
                    <div style="max-height: 500px; overflow: scroll;" align="center">
                        <asp:GridView ID="grdSEARCH" runat="server" 
                            AutoGenerateColumns="False" Width="100%"
                            DataKeyNames="ID_AGENCIA" CellPadding="3" ForeColor="Black" 
                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                            GridLines="Vertical" EmptyDataText="SIN USUARIOS" 
                            ShowHeaderWhenEmpty="True">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <Columns>
                                <asp:ButtonField DataTextField="DESC_AGENCIA" HeaderText="Agencia" 
                                    Text="Botón">
                                </asp:ButtonField>
                                <asp:BoundField DataField="NOMBRE_CONTACTO" HeaderText="Contacto">
                                </asp:BoundField>
                                <asp:BoundField DataField="MAIL_CONTACTO" HeaderText="Email" />
                                <asp:CheckBoxField DataField="FLG_CANC" HeaderText="Cancelado" />
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <!-- ModalPopupExtender SEARCH -->


</asp:Content>