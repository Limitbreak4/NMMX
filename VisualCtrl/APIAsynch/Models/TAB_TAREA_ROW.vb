Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

Partial Public Class TAB_TAREA_ROW
    <Key>
    <Column(Order:=0)>
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    Public Property id_row As Long

    <Key>
    <Column(Order:=1)>
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    Public Property num_renglon As Integer

    <Key>
    <Column(Order:=2)>
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    Public Property id_tarea As Long

    <StringLength(255)>
    Public Property llamada_api As String

    <StringLength(255)>
    Public Property resultado_api As String
End Class
