<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="status.aspx.vb" Inherits="VisualCtrl.status" %>


<%
    Dim respuesta As String
    respuesta = "{name: " + Session("current") + ", age: 31, city: New York}"
%>

<% =respuesta.ToString() %>