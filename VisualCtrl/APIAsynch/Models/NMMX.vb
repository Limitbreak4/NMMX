Imports System
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq

Partial Public Class NMMX
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=NMMX")
    End Sub

    Public Overridable Property TAB_TAREA As DbSet(Of TAB_TAREA)
    Public Overridable Property TAB_TAREA_ROW As DbSet(Of TAB_TAREA_ROW)

    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        modelBuilder.Entity(Of TAB_TAREA_ROW)() _
            .Property(Function(e) e.llamada_api) _
            .IsUnicode(False)

        modelBuilder.Entity(Of TAB_TAREA_ROW)() _
            .Property(Function(e) e.resultado_api) _
            .IsUnicode(False)
    End Sub
End Class
