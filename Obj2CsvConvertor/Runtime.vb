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
            Console.WriteLine("正在转换……")
            objfile.Parse(ct)
            Console.WriteLine("Obj读入完成，开始转换。")
            Dim csvfile As New Csv(objfile)
            Console.WriteLine("顶点指令转换完成。开始导出。")
            Dim mywriter As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(Input & ".csv", False, Text.Encoding.UTF8)
            mywriter.WriteLine("; 由 OBJ->CSV 转换工具 生成")
            mywriter.WriteLine("; 编程：Zbx1425 (百度贴吧 zbx14251)")
            mywriter.WriteLine()
            Dim rface As Integer = objfile.Faces.Count
            Dim tface As Integer = csvfile.Export(mywriter)
            mywriter.Close()
            Console.WriteLine("导出完成，谢谢。")
            Console.WriteLine("转换过程中载入了{0}个材质。", csvfile.Meshes.Count)
            Console.WriteLine("读入了 {0} 个面，导出了 {1} 个面， {2} 个面转换失败。", rface, tface, rface - tface)
            If rface > tface Then
                Console.WriteLine("没有绑定材质库(MTL)的面没有被转换。请检查是否有物体没有绑定材质，或导出的材质库是否齐全。")
            End If
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
