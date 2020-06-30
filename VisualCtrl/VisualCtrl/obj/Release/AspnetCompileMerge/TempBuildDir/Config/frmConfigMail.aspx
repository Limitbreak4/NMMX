<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frmConfigMail.aspx.vb" Inherits="VisualCtrl.frmConfigMail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    </p>
    <p>
        CONFIGURACIÓN GENERAL</p>
    <p>
        <asp:ImageButton ID="cmdLimpiar" runat="server" ImageUrl="~/Img/Borrar.PNG" ToolTip="SALVAR" />
&nbsp;&nbsp;
        <asp:ImageButton ID="cmdSalvar" runat="server" ImageUrl="~/Img/Salvar.png" ToolTip="SALVAR" />
&nbsp;&nbsp;
        <asp:ImageButton ID="cmdRegresar" runat="server" ImageUrl="~/Img/Regresar.png" ToolTip="REGRESAR" PostBackUrl="~/Config/configMain.aspx" />
    </p>
    <p>
        &nbsp;</p>
    <p>
        Configuración de Correo para Boletos y Recordatorios</p>
    <p>
        SMTP
        <asp:TextBox ID="txtSMTP" runat="server" MaxLength="100"></asp:TextBox>
&nbsp;Usuario:
        <asp:TextBox ID="txtUsuario" runat="server" MaxLength="100"></asp:TextBox>
&nbsp;Contraseña
        <asp:TextBox ID="txtPwd" runat="server" MaxLength="255"></asp:TextBox>
&nbsp;Puerto:
        <asp:TextBox ID="txtPuerto" runat="server" MaxLength="255"></asp:TextBox>
&nbsp;
        <asp:CheckBox ID="cbSSL" runat="server" Text="Usa SSL" />
    </p>
    <p>
        Nombre para mostrar:
        <asp:TextBox ID="txtNombreEnvio" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
        &nbsp;&nbsp;</p>
    <p>
        Dirección de envío:
        <asp:TextBox ID="txtDireccionEnvio" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
    </p>
    <p>
        Mensaje:</p>
    <div>
        <asp:TextBox ID="txtMensaje" runat="server" MaxLength="2000" Width="700px" Height="173px" TextMode="MultiLine"></asp:TextBox>
    </div>
    <p>
        <asp:CheckBox ID="cbIncludeAttach" runat="server" Text="Incluir Archivo Adjunto" />
    </p>
    <p>
        Variables disponibles:</p>
    <p>
        [NOMBRE_ARCHIVO] - Nombre del archivo</p>
    <p>
        [MES_GENERACION] - Mes de generación del archivo</p>
    <p>
        [TOTAL_ROWS] - Total de Merchants</p>
    <p>
        &nbsp;</p>
    </asp:Content>
