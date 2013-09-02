'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
'* Copyright (C) 2013 Namcore Studio <https://github.com/megasus/Namcore-Studio>
'*
'* This program is free software; you can redistribute it and/or modify it
'* under the terms of the GNU General Public License as published by the
'* Free Software Foundation; either version 2 of the License, or (at your
'* option) any later version.
'*
'* This program is distributed in the hope that it will be useful, but WITHOUT
'* ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
'* FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
'* more details.
'*
'* You should have received a copy of the GNU General Public License along
'* with this program. If not, see <http://www.gnu.org/licenses/>.
'*
'* Developed by Alcanmage/megasus
'*
'* //FileInfo//
'*      /Filename:      EventLogging
'*      /Description:   Handles logging of events and exceptions
'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

Imports NCFramework.GlobalVariables
Imports System.Threading
Imports System.IO

Public Module EventLogging
    Public Delegate Sub IncomingEventDelegate(ByVal _event As String)
    Public lastprogress As Integer
    Public isbusy As Boolean = False
    Public Sub LogAppend(ByVal _event As String, ByVal location As String, Optional userOut As Boolean = False, Optional iserror As Boolean = False)
        Dim x As UInt32 = 546
        While isbusy

        End While
        isbusy = True
        userOut = True
        If iserror = False Then
            If userOut = True Then
                appendStatus(_event, location, iserror)
                eventlog = "[" & Now.TimeOfDay.ToString & "]" & _event & vbNewLine & eventlog
                eventlog_full = "[" & Date.Today.ToString & " " & Now.TimeOfDay.ToString & "]" & _event & vbNewLine & eventlog_full
            Else
                eventlog_full = "[" & Date.Today.ToString & " " & Now.TimeOfDay.ToString & "]" & _event & vbNewLine & eventlog_full
            End If
        Else
            If userOut = True Then
                appendStatus(_event, location, iserror)
                eventlog = "[" & Now.TimeOfDay.ToString & "]" & _event & vbNewLine & eventlog
                eventlog_full = "[" & Date.Today.ToString & " " & Now.TimeOfDay.ToString & "]" & _event & vbNewLine & eventlog_full
            Else
                eventlog_full = "[" & Date.Today.ToString & " " & Now.TimeOfDay.ToString & "]" & _event & vbNewLine & eventlog_full
            End If
        End If
        isbusy = False
    End Sub
    Private Sub appendStatus(ByVal _status As String, ByVal loc As String, Optional Iserror As Boolean = False)
        proccessTXT = "[" & Now.TimeOfDay.ToString & "]" & _status & vbNewLine & proccessTXT
        Dim append As String = ""
        If Iserror Then append = "[ERROR]"
        Dim fs As New StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/log.txt", FileMode.OpenOrCreate, System.Text.Encoding.Default)
        fs.WriteLine("[" & Now.TimeOfDay.ToString & "]" & append & "[" & loc & "]" & _status)
        fs.Close()
        Try
            procStatus.appendProc("[" & Now.TimeOfDay.ToString & "]" & _status)
        Catch ex As Exception

        End Try


    End Sub

    Public Sub LogClear()
        eventlog = ""
    End Sub
End Module