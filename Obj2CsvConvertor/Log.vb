Module Log
    Public MyWriter As IO.StreamWriter = Nothing

    Public Sub Open(ByVal FileName As String)
        If FileName = "" Then
            MyWriter = Nothing
        Else
            MyWriter = My.Computer.FileSystem.OpenTextFileWriter(FileName, False, Text.Encoding.UTF8)
        End If
    End Sub

    Public Sub WriteLine(ByVal Content As String, ByVal ParamArray Args As String())
        Console.WriteLine(Content, Args)
        If MyWriter IsNot Nothing Then MyWriter.WriteLine(String.Format(Content, Args))
    End Sub

    Public Sub Flush()
        If MyWriter IsNot Nothing Then MyWriter.Flush()
    End Sub

    Public Sub Close()
        If MyWriter IsNot Nothing Then
            MyWriter.Flush()
            MyWriter.Close()
            MyWriter = Nothing
        End If
    End Sub
End Module


Public Class ConvertOption
    Public VertexPosRound As Integer = -1
    Public TexturePosRound As Integer = -1
    Public NormalPosRound As Integer = -1

    Public DisableVertexCombain As Boolean = False
    Public EnableLog As Boolean = False

    Public WarnSmoothMerg As Boolean = False
    Public WarnParameterSpace As Boolean = False
    Public WarnNonSimpleElement As Boolean = False
End Class
