Public Class Obj

    Public Class ObjFormatException
        Inherits Exception

        Public Sub New(ByVal Description As String)
            MyBase.New(Description)
        End Sub
    End Class

    Public Class Vertex
        Public x, y, z As Double

        Public Sub New(ByVal Description As String)
            Dim vns As String() = Description.Split(" ")
            If vns(0) <> "v" Then Throw New ObjFormatException("指令格式不正确。")
            x = vns(1)
            y = vns(2)
            z = vns(3)
        End Sub

        Public Sub New()
            x = Double.NaN
            y = Double.NaN
            z = Double.NaN
        End Sub
    End Class

    Public Class TextureCoordinate
        Public u, v As Double

        Public Sub New(ByVal Description As String)
            Dim vns As String() = Description.Split(" ")
            If vns(0) <> "vt" Then Throw New ObjFormatException("指令格式不正确。")
            u = vns(1)
            v = vns(2)
        End Sub

        Public Sub New()
            u = Double.NaN
            v = Double.NaN
        End Sub
    End Class

    Public Class Normal
        Public x, y, z As Double

        Public Sub New(ByVal Description As String)
            Dim vns As String() = Description.Split(" ")
            If vns(0) <> "v" Then Throw New ObjFormatException("指令格式不正确。")
            x = vns(1)
            y = vns(2)
            z = vns(3)
        End Sub

        Public Sub New()
            x = Double.NaN
            y = Double.NaN
            z = Double.NaN
        End Sub
    End Class

    Public Class Face
        Public VertexIndices() As Integer
        Public TextureCoordinateIndices() As Integer
        Public NormalIndices() As Integer
        Public Material As String

        Public Sub New(ByVal Description As String, ByVal mtl As String)
            Dim vns As String() = Description.Split(" ")
            If vns(0) <> "f" Then Throw New ObjFormatException("指令格式不正确。")
            ReDim VertexIndices(vns.Length - 2), TextureCoordinateIndices(vns.Length - 2), NormalIndices(vns.Length - 2)
            For i As Integer = 1 To vns.Length - 1
                Dim dfs As String() = vns(i).Split("/")
                VertexIndices(i - 1) = -1
                TextureCoordinateIndices(i - 1) = -1
                NormalIndices(i - 1) = -1
                Select Case dfs.Length
                    Case 0
                        Throw New ObjFormatException("面定义非法，请检查空格。")
                    Case 1
                        VertexIndices(i - 1) = dfs(0) - 1
                    Case 2
                        VertexIndices(i - 1) = dfs(0) - 1
                        TextureCoordinateIndices(i - 1) = dfs(1) - 1
                    Case 3
                        VertexIndices(i - 1) = dfs(0) - 1
                        If dfs(1) <> "" Then TextureCoordinateIndices(i - 1) = dfs(1) - 1
                        NormalIndices(i - 1) = dfs(2) - 1
                End Select
            Next
            Material = mtl
        End Sub
    End Class

    Public Vertices As New List(Of Vertex)
    Public TextureCoordinates As New List(Of TextureCoordinate)
    Public Normals As New List(Of Normal)
    Public Faces As New List(Of Face)
    Public MaterialLib As New Mtl

    Public Sub Parse(ByVal Description As String)
        Dim newline As String = ""
        If Description.Contains(vbCr) Then newline &= vbCr
        If Description.Contains(vbLf) Then newline &= vbLf
        Dim lns As String() = Description.Split(newline)
        Dim Currentmtl As String = ""
        For Each Current As String In lns
            If Current.Trim() = "" Then Continue For
            If Current.StartsWith("#"c) Then Continue For
            Dim vns As String() = Current.Split(" ")
            Select Case vns(0).ToLower.Trim
                Case "v"
                    Vertices.Add(New Vertex(Current))
                Case "vt"
                    TextureCoordinates.Add(New TextureCoordinate(Current))
                Case "vn"
                    Normals.Add(New Normal(Current))
                Case "vp"
                    Throw New ObjFormatException("本转换器只支持简单面。请在导出时选择""不使用曲面""")
                Case "f"
                    Faces.Add(New Face(Current, Currentmtl))
                Case "mtllib"
                    If Not IO.File.Exists(vns(1)) Then Throw New IO.FileNotFoundException("没有找到MTL文件。请重新导出。")
                    MaterialLib.Parse(My.Computer.FileSystem.ReadAllText(vns(1), System.Text.Encoding.UTF8))
                Case "usemtl"
                    Currentmtl = vns(1)
                Case "o"

                Case "g"

                Case Else
                    Throw New ObjFormatException("不支持或非法的指令：" & Current)
            End Select
        Next
    End Sub

End Class
