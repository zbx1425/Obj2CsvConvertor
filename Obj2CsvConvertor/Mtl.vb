Public Class Mtl
    Public Materials As New List(Of Material)

    Public Structure Color
        Dim r, g, b As Byte

        Public Sub New(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer)
            Me.r = If(r > 255, 255, r) : Me.g = If(g > 255, 255, g) : Me.b = If(b > 255, 255, b)
        End Sub
    End Structure

    Public Class Material
        Public Name As String
        Public OverlayColor As Color
        Public Texture As String = ""

        Public Sub New(ByVal Description As String)
            Dim newline As String = ""
            If Description.Contains(vbCr) Then newline &= vbCr
            If Description.Contains(vbLf) Then newline &= vbLf
            Dim lns As String() = Description.Split(newline)
            For Each Current As String In lns
                If Current.Trim() = "" Then Continue For
                If Current.StartsWith("#"c) Then Continue For
                Dim vns As String() = Current.Split(" ")
                Select Case vns(0).ToLower.Trim
                    Case "newmtl"
                        Name = vns(1)
                    Case "kd"
                        OverlayColor = New Color(vns(1) * 255, vns(2) * 255, vns(3) * 255)
                    Case "map_kd"
                        Texture = vns(1)
                        Dim Format As String = Right(Texture, Texture.Length - InStrRev(Texture, "."))
                        If Not Texture.EndsWith(".png") Then Log.WriteLine(Language.Translate("Warning.ImageFormat"), Format)
                    Case Else
                        Log.WriteLine(Language.Translate("Warning.NonSimpleMaterial"), vns(0).ToLower.Trim)
                End Select
            Next
        End Sub
    End Class

    Public Sub Parse(ByVal Description As String)
        Dim newline As String = ""
        If Description.Contains(vbCr) Then newline &= vbCr
        If Description.Contains(vbLf) Then newline &= vbLf
        Dim lns As String() = Description.Split(newline)
        Dim Buffer As String = ""
        For i As Integer = 0 To lns.Length - 1
            Dim Current As String = lns(i).Trim.Replace("  ", " ")
            MtlParserStatus = i + 1 & " -> [" & Current & "]"
            If Current.Trim() = "" Then Continue For
            If Current.StartsWith("#"c) Then Continue For
            Dim vns As String() = Current.Split(" ")
            If vns(0).ToLower = "newmtl" Then
                If Not Buffer = "" Then AddNewMaterial(New Material(Buffer))
                Buffer = ""
            End If
            Buffer &= Current & vbCrLf
        Next
        If Not Buffer = "" Then AddNewMaterial(New Material(Buffer))
        MtlParserStatus = Language.Translate("Interface.Idle")
    End Sub

    Private Sub AddNewMaterial(ByVal mtl As Material)
        Dim found As Boolean = False
        For Each Current As Material In Materials
            If Current.Name = mtl.Name Then
                found = True
                Exit For
            End If
        Next
        If Not found Then Materials.Add(mtl)
    End Sub

    Public Function Find(ByVal Name As String) As Material
        For Each Current As Material In Materials
            If Current.Name = Name Then Return Current
        Next
        Throw New Obj.ObjFormatException(Language.Translate("Exception.ErrorMatLibNotLoaded"))
    End Function
End Class
