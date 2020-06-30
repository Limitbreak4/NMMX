<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="configMain.aspx.vb" Inherits="VisualCtrl.configMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
    </p>
    <p>
        menú inicial para la configuración</p>
    <div class="col-md-4">
        <h2 style="font-size: x-large">Usuarios</h2>
        <p>
            &nbsp;<asp:ImageButton ID="cmdConfigUsuarios" runat="server" ImageUrl="~/Img/Sistema.png" Width="50px" />
        </p>
    </div>
    
    <div class="col-md-4">
        <h2 style="font-size: x-large">Estados</h2>
        <p>
            &nbsp;<asp:ImageButton ID="cmdConfigEstados" runat="server" ImageUrl="~/Img/Sistema.png" Width="50px" />
        </p>
    </div>

    <div class="col-md-4">
        <h2 style="font-size: x-large">Municipios</h2>
        <p>
            &nbsp;<asp:ImageButton ID="cmdConfigMunicipios" runat="server" ImageUrl="~/Img/Sistema.png" Width="50px" />
        </p>
    </div>

    <div class="col-md-4">
        <h2 style="font-size: x-large">CP</h2>
        <p>
            &nbsp;<asp:ImageButton ID="cmdConfigCP" runat="server" ImageUrl="~/Img/Sistema.png" Width="50px" />
        </p>
    </div>

      <div class="col-md-4">
        <h2 style="font-size: x-large">Agencias</h2>
        <p>
            &nbsp;<asp:ImageButton ID="cmdConfigAgencias" runat="server" ImageUrl="~/Img/Sistema.png" Width="50px" />
        </p>
    </div>

        <div class="col-md-4">
        <h2 style="font-size: x-large">Correo</h2>
        <p>
            &nbsp;<asp:ImageButton ID="cmdConfMail" runat="server" ImageUrl="~/Img/Sistema.png" Width="50px" />
        </p>
    </div>

</asp:Content>
