Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Description
Imports APIAsynch

Namespace Controllers
    Public Class TAB_TAREA_ROWController
        Inherits System.Web.Http.ApiController

        Private db As New NMMX

        ' GET: api/TAB_TAREA_ROW
        Function GetTAB_TAREA_ROW() As IQueryable(Of TAB_TAREA_ROW)
            Return db.TAB_TAREA_ROW
        End Function

        ' GET: api/TAB_TAREA_ROW/5
        <ResponseType(GetType(TAB_TAREA_ROW))>
        Function GetTAB_TAREA_ROW(ByVal id As Long) As IHttpActionResult
            Dim tAB_TAREA_ROW As TAB_TAREA_ROW = db.TAB_TAREA_ROW.Find(id)
            If IsNothing(tAB_TAREA_ROW) Then
                Return NotFound()
            End If

            Return Ok(tAB_TAREA_ROW)
        End Function

        ' PUT: api/TAB_TAREA_ROW/5
        <ResponseType(GetType(Void))>
        Function PutTAB_TAREA_ROW(ByVal id As Long, ByVal tAB_TAREA_ROW As TAB_TAREA_ROW) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = tAB_TAREA_ROW.id_row Then
                Return BadRequest()
            End If

            db.Entry(tAB_TAREA_ROW).State = EntityState.Modified

            Try
                db.SaveChanges()
            Catch ex As DbUpdateConcurrencyException
                If Not (TAB_TAREA_ROWExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/TAB_TAREA_ROW
        <ResponseType(GetType(TAB_TAREA_ROW))>
        Function PostTAB_TAREA_ROW(ByVal tAB_TAREA_ROW As TAB_TAREA_ROW) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.TAB_TAREA_ROW.Add(tAB_TAREA_ROW)

            Try
                db.SaveChanges()
            Catch ex As DbUpdateException
                If (TAB_TAREA_ROWExists(tAB_TAREA_ROW.id_row)) Then
                    Return Conflict()
                Else
                    Throw
                End If
            End Try

            Return CreatedAtRoute("DefaultApi", New With {.id = tAB_TAREA_ROW.id_row}, tAB_TAREA_ROW)
        End Function

        ' DELETE: api/TAB_TAREA_ROW/5
        <ResponseType(GetType(TAB_TAREA_ROW))>
        Function DeleteTAB_TAREA_ROW(ByVal id As Long) As IHttpActionResult
            Dim tAB_TAREA_ROW As TAB_TAREA_ROW = db.TAB_TAREA_ROW.Find(id)
            If IsNothing(tAB_TAREA_ROW) Then
                Return NotFound()
            End If

            db.TAB_TAREA_ROW.Remove(tAB_TAREA_ROW)
            db.SaveChanges()

            Return Ok(tAB_TAREA_ROW)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function TAB_TAREA_ROWExists(ByVal id As Long) As Boolean
            Return db.TAB_TAREA_ROW.Count(Function(e) e.id_row = id) > 0
        End Function
    End Class
End Namespace