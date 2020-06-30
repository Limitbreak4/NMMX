Imports System.Web.Script.Serialization
Public Class grafica
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mesesPresent(0 To 11) As Integer
        Dim mesesPlaced(0 To 11) As Integer
        mesesPresent(0) = 15
        mesesPresent(1) = 15
        mesesPresent(2) = 15
        mesesPresent(3) = 15
        mesesPresent(4) = 15
        mesesPresent(5) = 15
        mesesPresent(6) = 15
        mesesPresent(7) = 15
        mesesPresent(8) = 15
        mesesPresent(9) = 15
        mesesPresent(10) = 15
        mesesPresent(11) = 15
        mesesPlaced(0) = 15
        mesesPlaced(1) = 15
        mesesPlaced(2) = 15
        mesesPlaced(3) = 15
        mesesPlaced(4) = 15
        mesesPlaced(5) = 15
        mesesPlaced(6) = 15
        mesesPlaced(7) = 15
        mesesPlaced(8) = 15
        mesesPlaced(9) = 15
        mesesPlaced(10) = 15
        mesesPlaced(11) = 15


        Dim json As String = "[
        { month: ""JAN"", Present: """ + mesesPresent(0).ToString() + """, Placed: """ + mesesPlaced(0).ToString() + """ },
        { month: ""FEB"", Present: """ + mesesPresent(1).ToString() + """, Placed: """ + mesesPlaced(1).ToString() + """ },
        { month: ""MAR"", Present: """ + mesesPresent(2).ToString() + """, Placed: """ + mesesPlaced(2).ToString() + """},
        { month: ""APR"", Present: """ + mesesPresent(3).ToString() + """, Placed: """ + mesesPlaced(3).ToString() + """},
        { month: ""MAY"", Present: """ + mesesPresent(4).ToString() + """, Placed: """ + mesesPlaced(4).ToString() + """},
        { month: ""JUN"", Present: """ + mesesPresent(5).ToString() + """, Placed: """ + mesesPlaced(5).ToString() + """},
        { month: ""JUL"", Present: """ + mesesPresent(6).ToString() + """, Placed: """ + mesesPlaced(6).ToString() + """},
        { month: ""AUG"", Present: """ + mesesPresent(7).ToString() + """, Placed: """ + mesesPlaced(7).ToString() + """},
        { month: ""SEP"", Present: """ + mesesPresent(8).ToString() + """, Placed: """ + mesesPlaced(8).ToString() + """},
        { month: ""OCT"", Present: """ + mesesPresent(9).ToString() + """, Placed: """ + mesesPlaced(9).ToString() + """},
        { month: ""NOV"", Present: """ + mesesPresent(10).ToString() + """, Placed: """ + mesesPlaced(10).ToString() + """},
        { month: ""DEC"", Present: """ + mesesPresent(11).ToString() + """, Placed: """ + mesesPlaced(11).ToString() + """},
    ]"

        mesesJSON.Value = json
    End Sub

End Class