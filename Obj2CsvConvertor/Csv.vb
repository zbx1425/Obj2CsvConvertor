Imports System.Text
Imports Obj2CsvConvertor.Obj

Public Class Csv

    Public Class Vertex
        Public vx, vy, vz As Double
        Public nx, ny, nz As Double
        Public tu, tv As Double

        Public Sub New(ByVal v As Obj.Vertex, ByVal n As Obj.Normal, ByVal t As Obj.TextureCoordinate)
            vx = v.x : vy = v.y : vz = v.z
            nx = n.x : ny = n.y : nz = n.z
            tu = t.u : tv = t.v
        End Sub

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If obj.GetType IsNot GetType(Vertex) Then Return False
            Return obj.vx = Me.vx AndAlso obj.vy = Me.vy AndAlso obj.vz = Me.vz AndAlso _
            If(Double.IsNaN(Me.nx), Double.IsNaN(obj.nx), obj.nx = Me.nx AndAlso obj.ny = Me.ny AndAlso obj.nz = Me.nz) AndAlso _
            If(Double.IsNaN(Me.tu), Double.IsNaN(obj.tu), obj.tu = Me.tu AndAlso obj.tv = Me.tv)
        End Function
    End Class

    Public Class Face
        Public VertexIndicies As New List(Of Integer)

        Public Sub New(ByVal ls As List(Of Integer))
            VertexIndicies = ls
        End Sub
    End Class

    Public Class Mesh
        Public Vertices As New List(Of Vertex)
        Public Faces As New List(Of Face)
        Public Material As Mtl.Material

        Public Sub New(ByVal m As Mtl.Material, ByVal o As Obj)
            Dim fl As List(Of Obj.Face) = o.Find(m.Name)
            For Each Current As Obj.Face In fl
                Dim vis As New List(Of Integer)
                For i As Integer = 0 To Current.VertexIndices.Length - 1
                    Dim va As Obj.Vertex = If(Current.VertexIndices(i) = -1, New Obj.Vertex(), o.Vertices(Current.VertexIndices(i)))
                    Dim na As Obj.Normal = If(Current.NormalIndices(i) = -1, New Obj.Normal(), o.Normals(Current.NormalIndices(i)))
                    Dim ta As Obj.TextureCoordinate = If(Current.TextureCoordinateIndices(i) = -1, New Obj.TextureCoordinate(), o.TextureCoordinates(Current.TextureCoordinateIndices(i)))
                    Dim vt As Vertex = New Vertex(va, na, ta)
                    Dim idx As Integer = Vertices.IndexOf(vt)
                    If idx = -1 Then
                        idx = Vertices.Count
                        Vertices.Add(vt)
                    End If
                    vis.Add(idx)
                Next
                Faces.Add(New Face(vis))
                If Faces.Count Mod 100 = 0 Then Console.WriteLine(StringResource.MeshConverting, Faces.Count, fl.Count)
            Next
            Material = m
        End Sub

        Public Function Export() As String
            Dim outputbuilder As New StringBuilder
            outputbuilder.AppendLine("CreateMeshBuilder")
            For Each Current As Vertex In Vertices
                If Double.IsNaN(Current.nx) Then
                    outputbuilder.AppendFormat("AddVertex, {0}, {1}, {2}", Current.vx, Current.vy, Current.vz)
                Else
                    outputbuilder.AppendFormat("AddVertex, {0}, {1}, {2}, {3}, {4}, {5}", Current.vx, Current.vy, Current.vz, Current.nx, Current.ny, Current.nz)
                End If
                outputbuilder.AppendLine()
            Next
            For Each Current As Face In Faces
                outputbuilder.Append("AddFace")
                For Each i As Integer In Current.VertexIndicies
                    outputbuilder.Append(", " & i)
                Next
                outputbuilder.AppendLine()
            Next
            outputbuilder.AppendFormat("SetColor, {0}, {1}, {2}, 255", Material.OverlayColor.r, Material.OverlayColor.g, Material.OverlayColor.b)
            outputbuilder.AppendLine()
            If Material.Texture <> "" Then
                outputbuilder.AppendFormat("LoadTexture, {0}", Material.Texture)
                outputbuilder.AppendLine()
            End If
            For i As Integer = 0 To Vertices.Count - 1
                outputbuilder.AppendFormat("SetTextureCoordinates, {0}, {1}, {2}", i, Vertices(i).tu, Vertices(i).tv)
                outputbuilder.AppendLine()
            Next
            Return outputbuilder.ToString
        End Function

        Public Function Export(ByRef writer As IO.StreamWriter) As Integer
            writer.WriteLine("CreateMeshBuilder")
            For i As Integer = 0 To Vertices.Count - 1
                If Double.IsNaN(Vertices(i).nx) Then
                    writer.WriteLine("AddVertex, {0}, {1}, {2}", Vertices(i).vx, Vertices(i).vy, Vertices(i).vz)
                Else
                    writer.WriteLine("AddVertex, {0}, {1}, {2}, {3}, {4}, {5}", Vertices(i).vx, Vertices(i).vy, Vertices(i).vz, Vertices(i).nx, Vertices(i).ny, Vertices(i).nz)
                End If
                'If i Mod 1000 = 0 Then writer.Flush()
            Next
            For i As Integer = 0 To Faces.Count - 1
                writer.Write("AddFace")
                For Each j As Integer In Faces(i).VertexIndicies
                    writer.Write(", " & j)
                Next
                writer.WriteLine()
                'If i Mod 1000 = 0 Then writer.Flush()
            Next
            writer.WriteLine("SetColor, {0}, {1}, {2}, 255", Material.OverlayColor.r, Material.OverlayColor.g, Material.OverlayColor.b)
            If Material.Texture <> "" Then
                writer.WriteLine("LoadTexture, {0}", Material.Texture)
            End If
            For i As Integer = 0 To Vertices.Count - 1
                writer.WriteLine("SetTextureCoordinates, {0}, {1}, {2}", i, Vertices(i).tu, Vertices(i).tv)
                'If i Mod 1000 = 0 Then writer.Flush()
            Next
            Return Faces.Count
        End Function
    End Class

    Public Meshes As New List(Of Mesh)

    Public Sub New(ByVal o As Obj)
        For Each Current As Mtl.Material In o.MaterialLib.Materials
            Meshes.Add(New Mesh(Current, o))
            Console.WriteLine(StringResource.MeshConverted, Current.Name)
        Next
    End Sub

    Public Function Export(ByRef writer As IO.StreamWriter) As Integer
        Dim total As Integer = 0
        For i As Integer = 0 To Meshes.Count - 1
            total += Meshes(i).Export(writer)
            writer.Flush()
            Console.WriteLine(StringResource.MeshExported, Meshes(i).Material.Name, total)
        Next
        Return total
    End Function

    Public Shared Function Convert(ByVal o As Obj, ByRef writer As IO.StreamWriter) As Integer
        Dim total As Integer = 0
        For Each Current As Mtl.Material In o.MaterialLib.Materials
            Dim m As New Mesh(Current, o)
            Console.WriteLine(StringResource.MeshConverted, Current.Name)
            total += m.Export(writer)
            writer.Flush()
            Console.WriteLine(StringResource.MeshExported, m.Material.Name, total)
            m = Nothing
            GC.Collect()
        Next
        Return total
    End Function
End Class
