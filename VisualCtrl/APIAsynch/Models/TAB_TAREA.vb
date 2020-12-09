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

    Public Property geo1 As Byte?

    Public Property geo2 As Byte?

    Public Property blitz1 As Byte?

    Public Property blitz2 As Byte?

    Public Property blitz3 As Byte?
End Class
