Module Runtime

    Sub Main()
        Console.WriteLine("OBJ->CSV 转换工具")
        Console.WriteLine("编程：Zbx1425 (百度贴吧 zbx14251)")
        Console.WriteLine("--------------------------------------")
        Dim Input As String = Command()
        If Input.Trim = "" Then
            Dim ofd As New OpenFileDialog
            With ofd
                .Title = "打开OBJ输入"
                .Filter = "Wavefront OBJ|*.obj"
                .FileName = ""
            End With
            If ofd.ShowDialog() = DialogResult.OK Then
                Input = ofd.FileName
            Else
                End
            End If
        End If
        Try
            If Not IO.File.Exists(Input) Then Throw New IO.FileNotFoundException("未找到输入文件，请检查拼写错误。")
            IO.Directory.SetCurrentDirectory(My.Computer.FileSystem.GetParentPath(Input))
            Dim ct As String = My.Computer.FileSystem.ReadAllText(Input, Text.Encoding.UTF8)
            Dim objfile As New Obj
            objfile.Parse(ct)
            Dim csvfile As New Csv(objfile)
            Dim mywriter As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(Input & ".csv", False, Text.Encoding.UTF8)
            mywriter.WriteLine("; 由 OBJ->CSV 转换工具 生成")
            mywriter.WriteLine("; 编程：Zbx1425 (百度贴吧 zbx14251)")
            mywriter.WriteLine()
            csvfile.Export(mywriter)
            mywriter.Close()
            Console.WriteLine("转换完成，谢谢。")
            Console.WriteLine("该物件包含{0}个面。", csvfile.Meshes.Count)
        Catch ex As IndexOutOfRangeException
            Console.WriteLine("发生错误： " & ex.GetType.FullName)
            Console.WriteLine("输入文件中有语法错误。")
            Console.WriteLine("转换失败。")
        Catch ex As Exception
            Console.WriteLine("发生错误： " & ex.GetType.FullName)
            Console.WriteLine(ex.Message)
            Console.WriteLine("转换失败。")
        End Try
        If Command() = "" Then Console.ReadKey(True)
    End Sub

End Module
