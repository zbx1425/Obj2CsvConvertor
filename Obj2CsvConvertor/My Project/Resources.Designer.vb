﻿'------------------------------------------------------------------------------
' <auto-generated>
'     此代码由工具生成。
'     运行时版本:2.0.50727.8745
'
'     对此文件的更改可能会导致不正确的行为，并且如果
'     重新生成代码，这些更改将会丢失。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    '此类是由 StronglyTypedResourceBuilder
    '类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    '若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    '(以 /str 作为命令选项)，或重新生成 VS 项目。
    '''<summary>
    '''  强类型资源类，用于查找本地化字符串等。
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  返回此类使用的缓存 ResourceManager 实例。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Obj2CsvConvertor.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  为使用此强类型资源类的所有资源查找
        '''  重写当前线程的 CurrentUICulture 属性。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  查找类似 ; 由 OBJ-&gt;CSV 转换工具 生成
        '''; 编程：Zbx1425 (百度贴吧 zbx14251, Email: zbx1425@126.com)
        '''; Generated by Obj2Csv Conventor
        '''; By zbx1425 (zbx1422 at BVE Worldwide, Email: zbx1425@126.com) 的本地化字符串。
        '''</summary>
        Friend ReadOnly Property FileHead() As String
            Get
                Return ResourceManager.GetString("FileHead", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  查找类似 ///zh-CN, zh-TW
        '''ConsoleRuntime.Copyright = &quot;OBJ-&gt;CSV 转换工具
        '''编程：Zbx1425 (百度贴吧 zbx14251)
        '''Email: zbx1425@126.com
        '''版本 {0}
        '''--------------------------------------&quot;
        '''
        '''ConsoleRuntime.ReadStarted = &quot;正在读入Obj……&quot;
        '''ConsoleRuntime.ReadingGroup = &quot;正在读入分组 {0} ……&quot;
        '''ConsoleRuntime.ReadFinished = &quot;Obj读入完成，共 {0} 个面，开始转换。&quot;
        '''ConsoleRuntime.MeshConverting = &quot;已转换 {0} / {1} 个面。&quot;
        '''ConsoleRuntime.MeshConverted = &quot;材质 {0} 对应面顶点转换完成。&quot;
        '''ConsoleRuntime.MeshExported = &quot;材质 {0} 对应面导出完成，已导出 {1} 个面。&quot;
        '''ConsoleRuntime.ExportFinished = &quot;
        '''** 导出完成，谢谢。 **
        '''转换过 [字符串的其余部分被截断]&quot;; 的本地化字符串。
        '''</summary>
        Friend ReadOnly Property Language() As String
            Get
                Return ResourceManager.GetString("Language", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
