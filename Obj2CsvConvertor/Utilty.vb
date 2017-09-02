Module Utilty
    Public ObjParserStatus As String
    Public MtlParserStatus As String

    Public Function GetNewLine(ByVal Source As String) As String
        Dim newline As String = ""
        If Source.Contains(vbCr) Then newline &= vbCr
        If Source.Contains(vbLf) Then newline &= vbLf
        Return newline
    End Function

    Public Function ReadAllText(ByVal FileName As String) As String
        Dim fs As IO.FileStream = New IO.FileStream(FileName, IO.FileMode.Open, IO.FileAccess.Read)
        Dim Unicode As Byte() = {&HFF, &HFE, &H41}
        Dim UnicodeBig As Byte() = {&HFE, &HFF, &H0}
        Dim UTF8BOM As Byte() = {&HEF, &HBB, &HBF}
        Dim enc As Text.Encoding = Text.Encoding.Default
        Dim br As New IO.BinaryReader(fs, enc)
        Dim ss As Byte() = br.ReadBytes(fs.Length)
        If (JudgeUTF8NBOM(ss) Or JudgeBOM(ss, UTF8BOM)) Then
            enc = Text.Encoding.UTF8
        ElseIf JudgeBOM(ss, UnicodeBig) Then
            enc = Text.Encoding.BigEndianUnicode
        ElseIf JudgeBOM(ss, Unicode) Then
            enc = Text.Encoding.Unicode
        Else
            enc = Text.Encoding.Default
        End If
        fs.Seek(0, IO.SeekOrigin.Begin)
        Dim tr As New IO.StreamReader(fs, enc)
        Dim rs As String = tr.ReadToEnd
        tr.Close()
        Return rs
    End Function

    Public Function JudgeBOM(ByVal File As Byte(), ByVal BOM As Byte()) As Boolean
        For i As Integer = 0 To BOM.Length - 1
            If File(i) <> BOM(i) Then Return False
        Next
        Return True
    End Function

    Public Function JudgeUTF8NBOM(ByVal File As Byte())
        Dim charByteCounter As Integer = 1
        Dim curByte As Byte
        For i = 0 To File.Length - 1
            curByte = File(i)
            If charByteCounter = 1 Then
                If curByte >= &H80 Then
                    Do
                        curByte <<= 1
                        charByteCounter += 1
                    Loop Until curByte And &H80 = 0 Or charByteCounter > 6
                    If charByteCounter = 1 Or charByteCounter > 6 Then
                        Return False
                    End If
                End If
            Else
                If curByte And &HC0 <> &H80 Then
                    Return False
                End If
                charByteCounter -= 1
            End If
        Next
        If charByteCounter > 1 Then
            Return False
        End If
        Return True
    End Function

    Public Function ConsoleReadNumber(ByVal Prompt As String, ByRef Target As Object) As Boolean
        Console.Write(Prompt)
        Dim res As String = Console.ReadLine
        If IsNumeric(res) Then
            Target = res
            Return True
        End If
        Return False
    End Function

    Public Function ConsoleReadBoolean(ByVal Prompt As String, Optional ByVal TrueKey As ConsoleKey = ConsoleKey.Y, Optional ByVal FalseKey As ConsoleKey = ConsoleKey.N) As Boolean
        Dim InputKey As ConsoleKey = ConsoleKey.NoName
        Console.Write(Prompt)
        Do
            InputKey = Console.ReadKey(True).Key
        Loop Until InputKey = TrueKey Or InputKey = FalseKey
        If InputKey <> ConsoleKey.Enter Then Console.WriteLine(InputKey.ToString)
        Return InputKey = TrueKey
    End Function
End Module
