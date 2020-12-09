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
    Public Class TAB_TAREAController
        Inherits System.Web.Http.ApiController

        Private db As New NMMX

        ' GET: api/TAB_TAREA
        Function GetTAB_TAREA() As IQueryable(Of TAB_TAREA)
            Return db.TAB_TAREA
        End Function

        ' GET: api/TAB_TAREA/5
        <ResponseType(GetType(TAB_TAREA))>
        Function GetTAB_TAREA(ByVal id As Long) As IHttpActionResult
            Dim tAB_TAREA As TAB_TAREA = db.TAB_TAREA.Find(id)
            If IsNothing(tAB_TAREA) Then
                Return NotFound()
            End If

            Return Ok(tAB_TAREA)
        End Function

        ' PUT: api/TAB_TAREA/5
        <ResponseType(GetType(Void))>
        Function PutTAB_TAREA(ByVal id As Long, ByVal tAB_TAREA As TAB_TAREA) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = tAB_TAREA.id_tarea Then
                Return BadRequest()
            End If

            db.Entry(tAB_TAREA).State = EntityState.Modified

            Try
                db.SaveChanges()
            Catch ex As DbUpdateConcurrencyException
                If Not (TAB_TAREAExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/TAB_TAREA
        <ResponseType(GetType(TAB_TAREA))>
        Function PostTAB_TAREA(ByVal tAB_TAREA As TAB_TAREA) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.TAB_TAREA.Add(tAB_TAREA)
            db.SaveChanges()

            Return CreatedAtRoute("DefaultApi", New With {.id = tAB_TAREA.id_tarea}, tAB_TAREA)
        End Function

        ' DELETE: api/TAB_TAREA/5
        <ResponseType(GetType(TAB_TAREA))>
        Function DeleteTAB_TAREA(ByVal id As Long) As IHttpActionResult
            Dim tAB_TAREA As TAB_TAREA = db.TAB_TAREA.Find(id)
            If IsNothing(tAB_TAREA) Then
                Return NotFound()
            End If

            db.TAB_TAREA.Remove(tAB_TAREA)
            db.SaveChanges()

            Return Ok(tAB_TAREA)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function TAB_TAREAExists(ByVal id As Long) As Boolean
            Return db.TAB_TAREA.Count(Function(e) e.id_tarea = id) > 0
        End Function
    End Class
End Namespace