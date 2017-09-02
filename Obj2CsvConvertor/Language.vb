Module Language
    Dim LanguageDefination As String = My.Resources.Language
    Dim Strings As New Dictionary(Of String, String)

    Public Sub Load(ByVal LangName As String)
        Strings.Clear()
        Dim NewLine As String = Utilty.GetNewLine(LanguageDefination)
        Dim Defs As New List(Of String)
        Dim Names As New List(Of String)
        Dim Name As String = "HeadSection"
        Dim Buffer As String = ""
        For Each Current As String In LanguageDefination.Split(NewLine)
            If Current.StartsWith("///") Then
                If Buffer <> "" Then
                    Defs.Add(Buffer)
                    Names.Add(Name)
                    Buffer = ""
                End If
                Name = Current.Replace("///", "")
            Else
                Buffer &= Current & NewLine
            End If
        Next
        Dim lindex As Integer = -1
        For i As Integer = 0 To Names.Count - 1
            If Names(i).Contains(LangName) Then
                lindex = i
                Exit For
            End If
        Next
        If lindex = -1 Then lindex = Names.IndexOf("")
        If lindex = -1 Then Exit Sub
        Dim Refs As String() = Defs(lindex).Split(NewLine)
        Dim StringStarted As Boolean = False
        Dim OperandName As String = ""
        For i As Integer = 0 To Refs.Count - 1
            If StringStarted Then
                Buffer &= Environment.NewLine & Refs(i).TrimEnd.Replace(""""c, "")
                If Refs(i).TrimEnd.EndsWith(""""c) Then
                    StringStarted = False
                    Buffer.Remove(Buffer.Length - NewLine.Length, NewLine.Length)
                    Strings.Add(OperandName, Buffer)
                End If
            Else
                If Refs(i).TrimEnd = "" Then Continue For
                If Refs(i).Contains(" = ") Then
                    OperandName = Split(Refs(i), " = ")(0)
                    Buffer = Split(Refs(i), " = ")(1).TrimEnd.Replace(""""c, "")
                    If Refs(i).TrimEnd.EndsWith(""""c) Then
                        Strings.Add(OperandName, Buffer)
                    Else
                        StringStarted = True
                    End If
                End If
            End If
        Next
    End Sub

    Public Function Translate(ByVal Name As String) As String
        Return Strings(Name)
    End Function
End Module
