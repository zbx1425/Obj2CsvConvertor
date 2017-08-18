Imports System.Text

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
    End Class

    Public Class Mesh
        Public Vertices As New List(Of Vertex)
        Public Material As Mtl.Material

        Public Sub New(ByVal f As Obj.Face, ByVal o As Obj)
            For i As Integer = 0 To f.VertexIndices.Length - 1
                Dim va As Obj.Vertex = If(f.VertexIndices(i) = -1, New Obj.Vertex(), o.Vertices(f.VertexIndices(i)))
                Dim na As Obj.Normal = If(f.NormalIndices(i) = -1, New Obj.Normal(), o.Normals(f.NormalIndices(i)))
                Dim ta As Obj.TextureCoordinate = If(f.TextureCoordinateIndices(i) = -1, New Obj.TextureCoordinate(), o.TextureCoordinates(f.TextureCoordinateIndices(i)))
                Vertices.Add(New Vertex(va, na, ta))
            Next
            Material = o.MaterialLib.Find(f.Material)
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
            outputbuilder.Append("AddFace")
            For i As Integer = 0 To Vertices.Count - 1
                outputbuilder.Append(", " & i)
            Next
            outputbuilder.AppendLine()
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
    End Class

    Public Meshes As New List(Of Mesh)

    Public Sub New(ByVal o As Obj)
        For Each Current As Obj.Face In o.Faces
            Meshes.Add(New Mesh(Current, o))
        Next
    End Sub

    Public Sub Export(ByRef writer As IO.StreamWriter)
        For i As Integer = 0 To Meshes.Count - 1
            writer.WriteLine(Meshes(i).Export)
            If i Mod 1000 = 0 Then writer.Flush()
        Next
        writer.Flush()
    End Sub
End Class
