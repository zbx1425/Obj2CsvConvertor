Module Runtime

    Sub Main()
        Language.Load(System.Globalization.CultureInfo.InstalledUICulture.Name)
        Console.WriteLine(Language.Translate("ConsoleRuntime.Copyright"), My.Application.Info.Version.ToString)
        Dim Input As String = Command().Trim
        If Input = "" Then
            Dim ofd As New OpenFileDialog
            With ofd
                .Title = Language.Translate("Interface.OpenFileTitle")
                .Filter = Language.Translate("Interface.OpenFileFilter")
                .FileName = ""
            End With
            If ofd.ShowDialog() = DialogResult.OK Then
                Input = ofd.FileName
            Else
                End
            End If
        End If
        ObjParserStatus = Language.Translate("Interface.Idle")
        MtlParserStatus = Language.Translate("Interface.Idle")
        Dim Preference As New ConvertOption
        If ConsoleReadBoolean("按下回车开始转换，或按下E设定转换设置。", ConsoleKey.E, ConsoleKey.Enter) Then
            Console.WriteLine("  数字部分--请输入数字并按下回车，输入其他或直接按下回车将采取默认值")
            ConsoleReadNumber("    顶点坐标截取位数 ：", Preference.VertexPosRound)
            ConsoleReadNumber("    材质顶点截取位数 ：", Preference.TexturePosRound)
            ConsoleReadNumber("    法线指向截取位数 ：", Preference.NormalPosRound)
            Console.WriteLine("  选择部分--输入Y表示'是'，输入N表示'否'")
            Preference.DisableVertexCombain = ConsoleReadBoolean("    不进行顶点合并   ：")
            Preference.EnableLog = ConsoleReadBoolean("    输出操作日志     ：")
        End If
        Log.Open(If(Preference.EnableLog, Input & ".csv.log", ""))
        Dim starttime As Date = Now
        Try
            If Not IO.File.Exists(Input) Then Throw New IO.FileNotFoundException(Language.Translate("Exception.ErrorInputNotFound"))
            IO.Directory.SetCurrentDirectory(My.Computer.FileSystem.GetParentPath(Input))
            Dim ct As String = Utilty.ReadAllText(Input)
            Dim objfile As New Obj
            Log.WriteLine(Language.Translate("ConsoleRuntime.ReadStarted"))
            objfile.Parse(ct, Preference)
            Dim rface As Integer = objfile.Faces.Count
            Log.WriteLine(Language.Translate("ConsoleRuntime.ReadFinished"), rface)
            If Preference.WarnNonSimpleElement Then Log.WriteLine(Language.Translate("Warning.NonSimpleElement"))
            If Preference.WarnParameterSpace Then Log.WriteLine(Language.Translate("Warning.ParameterSpace"))
            If Preference.WarnSmoothMerg Then Log.WriteLine(Language.Translate("Warning.SmoothMerg"))
            Log.Flush()

            Dim mywriter As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(Input & ".csv", False, Text.Encoding.UTF8)
            mywriter.WriteLine(My.Resources.FileHead)
            mywriter.WriteLine()
            Dim tface As Integer = Csv.Convert(objfile, mywriter, Preference)
            mywriter.Close()
            Log.WriteLine(Language.Translate("ConsoleRuntime.ExportFinished"), objfile.MaterialLib.Materials.Count, tface, rface - tface)
            If rface > tface Then
                Log.WriteLine(Language.Translate("Interface.WarningFaceNotConverted"))
            End If
        Catch ex As Exception
            Log.WriteLine(Language.Translate("ConsoleRuntime.ErrorReport"), ex.GetType.FullName, ex.Message, ObjParserStatus, MtlParserStatus)
        End Try
        Log.WriteLine(Language.Translate("ConsoleRuntime.TotalTime"), DateDiff(DateInterval.Second, starttime, Now))
        Log.Close()
        If Command() = "" Then ConsoleReadBoolean("", ConsoleKey.Enter, ConsoleKey.Enter)
    End Sub

End Module
