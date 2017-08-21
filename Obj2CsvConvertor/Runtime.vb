Module Runtime

    Sub Main()
        Console.WriteLine(StringResource.Copyright, My.Application.Info.Version.ToString)
        Dim Input As String = Command()
        If Input.Trim = "" Then
            Dim ofd As New OpenFileDialog
            With ofd
                .Title = StringResource.OpenFileTitle
                .Filter = StringResource.OpenFileFilter
                .FileName = ""
            End With
            If ofd.ShowDialog() = DialogResult.OK Then
                Input = ofd.FileName
            Else
                End
            End If
        End If
        Dim starttime As Date = Now
        'Try
        If Not IO.File.Exists(Input) Then Throw New IO.FileNotFoundException(StringResource.ErrorInputNotFound)
        IO.Directory.SetCurrentDirectory(My.Computer.FileSystem.GetParentPath(Input))
        Dim ct As String = My.Computer.FileSystem.ReadAllText(Input, Text.Encoding.UTF8)
        Dim objfile As New Obj
        Console.WriteLine(StringResource.ReadStarted)
        objfile.Parse(ct)
        Dim rface As Integer = objfile.Faces.Count
        Console.WriteLine(StringResource.ReadFinished, rface)
        Dim csvfile As New Csv(objfile)
        Console.WriteLine(StringResource.ConvertFinished)
        Dim mywriter As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(Input & ".csv", False, Text.Encoding.UTF8)
        mywriter.WriteLine(StringResource.FileHead)
        mywriter.WriteLine()
        Dim tface As Integer = csvfile.Export(mywriter)
        mywriter.Close()
        Console.WriteLine(StringResource.ExportFinished, csvfile.Meshes.Count, tface, rface - tface)
        If rface > tface Then
            Console.WriteLine()
        End If
        'Catch ex As Exception
        'Console.WriteLine(StringResource.ErrorReport, ex.GetType.FullName, ex.Message)
        'End Try
        Console.WriteLine(StringResource.TotalTime, DateDiff(DateInterval.Second, starttime, Now))
        If Command() = "" Then Console.ReadLine()
    End Sub

End Module
