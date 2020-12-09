Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

Partial Public Class TAB_TAREA
    <Key>
    Public Property id_tarea As Long

    Public Property id_archivo As Long

    Public Property total_lineas As Long

    Public Property lineas_procesadas As Long?
End Class
