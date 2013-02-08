﻿Imports Namcore_Studio.EventLogging
Imports Namcore_Studio.CommandHandler
Imports Namcore_Studio.GlobalVariables
Imports Namcore_Studio.Basics
Imports Namcore_Studio.Conversions
Public Class TalentCreation
    Private Shared SDatatable As New DataTable
    Private Shared TalentRank As String
    Private Shared TalentRank2 As String
    Private Shared TalentId As String
    Private Shared
    Public Shared Sub SetCharacterTalents(ByVal setId As Integer, Optional charguid As Integer = 0)
        If charguid = 0 Then charguid = characterGUID
        LogAppend("Setting Talents for character: " & charguid.ToString() & " // setId is : " & setId.ToString(), "TalentCreation_SetCharacterTalents", True)
        Select Case sourceCore
            Case "arcemu"
                createAtArcemu(charguid, setId)
            Case "trinity"
                createAtTrinity(charguid, setId)
            Case "trinitytbc"

            Case "mangos"
                createAtMangos(charguid, setId)
            Case Else

        End Select
    End Sub
    Private Shared Function LoadTalentTable() As DataTable
        Try
            Dim dt As New DataTable()
            Dim stext As String = My.Resources.Talent
            Dim a() As String
            Dim strArray As String()
            a = Split(stext, vbNewLine)
            For i = 0 To UBound(a)
                strArray = a(i).Split(CChar(";"))
                If i = 0 Then
                    For Each value As String In strArray
                        dt.Columns.Add(value.Trim())
                    Next
                Else
                    Dim dr As DataRow = dt.NewRow()
                    dt.Rows.Add(strArray)
                End If
            Next i
            Return dt
        Catch ex As Exception
            LogAppend("Failed to load new talent datatable! -> Exception is: ###START###" & ex.ToString() & "###END###", "TalentCreation_LoadTalentTable", False, True)
            Return New DataTable
        End Try
    End Function
    Private Shared Function checkfield(ByVal lID As String) As String
        If Not executex("Rang1", lID) = "-" Then
            TalentRank = "0"
            TalentRank2 = "0"
            Return (executex("Rang1", lID))
        ElseIf Not executex("Rang2", lID) = "-" Then
            TalentRank = "1"
            TalentRank2 = "1"
            Return (executex("Rang2", lID))
        ElseIf Not executex("Rang3", lID) = "-" Then
            TalentRank = "2"
            TalentRank2 = "2"
            Return (executex("Rang3", lID))
        ElseIf Not executex("Rang4", lID) = "-" Then
            TalentRank = "3"
            TalentRank2 = "3"
            Return (executex("Rang4", lID))
        ElseIf Not executex("Rang5", lID) = "-" Then
            TalentRank = "4"
            TalentRank2 = "4"
            Return (executex("Rang5", lID))
        Else
            TalentRank = "0"
            TalentRank2 = "0"
            Return "0"
        End If
    End Function

    Private Shared Function executex(ByVal field As String, ByVal sID As String) As String
        Try
            Dim foundRows() As DataRow
            foundRows = SDatatable.Select(field & " = '" & sID & "'")
            If foundRows.Length = 0 Then
                Return "-"
            Else
                Dim i As Integer
                Dim tmpreturn As String = "-"
                For i = 0 To foundRows.GetUpperBound(0)
                    tmpreturn = (foundRows(i)(0)).ToString
                Next i
                Return tmpreturn
            End If
        Catch ex As Exception
            LogAppend("Talent " & sID & " not found! -> Exception is: ###START###" & ex.ToString() & "###END###", "CharacterTalentsHandler_executex", False, True)
            Return "-"
        End Try
    End Function
    Private Shared Sub createAtArcemu(ByVal characterguid As Integer, ByVal targetSetId As Integer)
        LogAppend("Creating at arcemu", "TalentCreation_createAtArcemu", False)
        TalentRank = Nothing
        TalentRank2 = Nothing
        SDatatable.Clear()
        SDatatable.Dispose()
        SDatatable = LoadTalentTable()
        Dim talentlist As String = Nothing
        Dim talentlist2 As String = Nothing
        Dim finaltalentstring As String = Nothing
        Dim finaltalentstring2 As String = Nothing
        Dim fullTalentList As List(Of String) = ConvertStringToList(GetTemporaryCharacterInformation("@character_talent_list", targetSetId))
        For Each talentstring As String In fullTalentList
            Dim spellid As String = splitList(talentstring, "spell")
            If spellid.Contains("clear") Then
                TalentId = spellid.Replace("clear", "")
                Dim spec As String = splitList(talentstring, "spec")
                If spec = "0" Then
                    finaltalentstring = finaltalentstring & TalentId & ",0,"
                Else
                    finaltalentstring2 = finaltalentstring2 & TalentId & ",0,"
                End If
            Else
                TalentId = checkfield(spellid)
                Dim spec As String = splitList(talentstring, "spec")
                If spec = "0" Then
                    If talentlist.Contains(TalentId) Then
                        If talentlist.Contains(TalentId & "rank5") Then

                        ElseIf talentlist.Contains(TalentId & "rank4") Then
                            If CInt(Val(TalentRank)) <= 4 Then
                            Else
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",0", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",1", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",2", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",3", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",4", (CInt(Val(TalentRank))).ToString)
                                talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                            End If
                        ElseIf talentlist.Contains(TalentId & "rank3") Then
                            If CInt(Val(TalentRank)) <= 3 Then
                            Else
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",0", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",1", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",2", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",3", (CInt(Val(TalentRank))).ToString)
                                talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                            End If
                        ElseIf talentlist.Contains(TalentId & "TalentRank2") Then
                            If CInt(Val(TalentRank)) <= 2 Then
                            Else
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",0", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",1", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",2", (CInt(Val(TalentRank)) - 1).ToString)
                                talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                            End If
                        ElseIf talentlist.Contains(TalentId & "rank1") Then
                            If CInt(Val(TalentRank)) <= 1 Then
                            Else
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",0", (CInt(Val(TalentRank))).ToString)
                                finaltalentstring = finaltalentstring.Replace(TalentId & ",1", (CInt(Val(TalentRank))).ToString)
                                talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                            End If
                        Else : End If
                    Else
                        finaltalentstring = finaltalentstring & TalentId & "," & (CInt(Val(TalentRank))).ToString & ","
                        talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                    End If
                Else
                    If talentlist2.Contains(TalentId) Then
                        If talentlist2.Contains(TalentId & "rank5") Then

                        ElseIf talentlist2.Contains(TalentId & "rank4") Then
                            If CInt(Val(TalentRank2)) <= 4 Then
                            Else
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",0", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",1", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",2", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",3", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",4", (CInt(Val(TalentRank2))).ToString)
                                talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                            End If
                        ElseIf talentlist2.Contains(TalentId & "rank3") Then
                            If CInt(Val(TalentRank2)) <= 3 Then
                            Else
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",0", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",1", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",2", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",3", (CInt(Val(TalentRank2))).ToString)
                                talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                            End If
                        ElseIf talentlist2.Contains(TalentId & "TalentRank22") Then
                            If CInt(Val(TalentRank2)) <= 2 Then
                            Else
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",0", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",1", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",2", (CInt(Val(TalentRank2)) - 1).ToString)
                                talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                            End If
                        ElseIf talentlist2.Contains(TalentId & "rank1") Then
                            If CInt(Val(TalentRank2)) <= 1 Then
                            Else
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",0", (CInt(Val(TalentRank2))).ToString)
                                finaltalentstring2 = finaltalentstring2.Replace(TalentId & ",1", (CInt(Val(TalentRank2))).ToString)
                                talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                            End If
                        Else : End If
                    Else
                        finaltalentstring2 = finaltalentstring2 & TalentId & "," & (CInt(Val(TalentRank2))).ToString & ","
                        talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                    End If
                End If
            End If
            runSQLCommand_characters_string("UPDATE characters SET talents1='" & finaltalentstring & "' WHERE guid='" & characterguid.ToString() & "'", True)
            runSQLCommand_characters_string("UPDATE characters SET talents2='" & finaltalentstring2 & "' WHERE guid='" & characterguid.ToString() & "'", True)
        Next
    End Sub
    Private Shared Sub createAtTrinity(ByVal characterguid As Integer, ByVal targetSetId As Integer)
        LogAppend("Creating at Trinity", "TalentCreation_createAtTrinity", False)
        Dim fullTalentList As List(Of String) = ConvertStringToList(GetTemporaryCharacterInformation("@character_talent_list", targetSetId))
        For Each talentstring As String In fullTalentList
            runSQLCommand_characters_string("INSERT INTO character_talent ( guid, spell, spec ) VALUES ( '" & characterguid.ToString() & "', '" & splitList(talentstring, "spell") & "', '" &
                                            splitList(talentstring, "spec") & "')", True)
        Next
    End Sub
    Private Shared Sub createAtMangos(ByVal characterguid As Integer, ByVal targetSetId As Integer)
        LogAppend("Creating at Mangos", "TalentCreation_createAtMangos", False)
        TalentRank = Nothing
        TalentRank2 = Nothing
        SDatatable.Clear()
        SDatatable.Dispose()
        SDatatable = LoadTalentTable()
        Dim talentlist As String = Nothing
        Dim talentlist2 As String = Nothing
        Dim finaltalentstring As String = Nothing
        Dim finaltalentstring2 As String = Nothing
        Dim fullTalentList As List(Of String) = ConvertStringToList(GetTemporaryCharacterInformation("@character_talent_list", targetSetId))
        For Each talentstring As String In fullTalentList
          talentid = checkfield(splitlist(talentstring, "spell"))
            Dim spec As String = splitList(talentstring, "spec")
            If spec = "0" Then
                If talentlist.Contains(TalentId) Then
                    If talentlist.Contains(TalentId & "rank5") Then
                    ElseIf talentlist.Contains(TalentId & "rank4") Then
                        If CInt(Val(TalentRank)) <= 4 Then
                        Else
                            runSQLCommand_characters_string("UPDATE character_talent SET current_rank='" & TalentRank & "' WHERE guid='" & characterguid.ToString & "' AND talent_id='" & TalentId & "' AND spec='0'", True)
                            talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                        End If
                    ElseIf talentlist.Contains(TalentId & "rank3") Then
                        If CInt(Val(TalentRank)) <= 3 Then
                        Else
                            runSQLCommand_characters_string("UPDATE character_talent SET current_rank='" & TalentRank & "' WHERE guid='" & characterguid.ToString & "' AND talent_id='" & TalentId & "' AND spec='0'", True)
                            talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                        End If
                    ElseIf talentlist.Contains(TalentId & "rank2") Then
                        If CInt(Val(TalentRank)) <= 2 Then
                        Else
                            runSQLCommand_characters_string(
                                "UPDATE character_talent SET current_rank='" & TalentRank & "' WHERE guid='" & characterguid.ToString & "' AND talent_id='" & TalentId & "' AND spec='0'", True)
                            talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                        End If
                    ElseIf talentlist.Contains(TalentId & "rank1") Then
                        If CInt(Val(TalentRank)) <= 1 Then
                        Else
                            runSQLCommand_characters_string("UPDATE character_talent SET current_rank='" & TalentRank & "' WHERE guid='" & characterguid.ToString & "' AND talent_id='" & TalentId & "' AND spec='0'", True)
                            talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                        End If
                    Else : End If
                Else
                    runSQLCommand_characters_string("INSERT INTO character_talent ( guid, talent_id, current_rank, spec ) VALUES ( '" & characterguid.ToString & "', '" & TalentId & "', '" & TalentRank & "', '0' )", True)
                    talentlist = talentlist & " " & TalentId & "rank" & TalentRank
                End If
            Else
                If talentlist2.Contains(TalentId) Then
                    If talentlist2.Contains(TalentId & "rank5") Then
                    ElseIf talentlist2.Contains(TalentId & "rank4") Then
                        If CInt(Val(TalentRank2)) <= 4 Then
                        Else
                            runSQLCommand_characters_string("UPDATE character_talent SET current_rank='" & TalentRank2 & "' WHERE guid='" & characterguid.ToString & "' AND talent_id='" & TalentId & "' AND spec='1'", True)
                            talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                        End If
                    ElseIf talentlist2.Contains(TalentId & "rank3") Then
                        If CInt(Val(TalentRank2)) <= 3 Then
                        Else
                            runSQLCommand_characters_string("UPDATE character_talent SET current_rank='" & TalentRank2 & "' WHERE guid='" & characterguid.ToString & "' AND talent_id='" & TalentId & "' AND spec='1'", True)
                            talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                        End If
                    ElseIf talentlist2.Contains(TalentId & "rank2") Then
                        If CInt(Val(TalentRank2)) <= 2 Then
                        Else
                            runSQLCommand_characters_string("UPDATE character_talent SET current_rank='" & TalentRank2 & "' WHERE guid='" & characterguid.ToString & "' AND talent_id='" & TalentId & "' AND spec='1'", True)
                            talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                        End If
                    ElseIf talentlist2.Contains(TalentId & "rank1") Then
                        If CInt(Val(TalentRank2)) <= 1 Then
                        Else
                            runSQLCommand_characters_string("UPDATE character_talent SET current_rank='" & TalentRank2 & "' WHERE guid='" & characterguid.ToString & "' AND talent_id='" & TalentId & "' AND spec='1'", True)
                            talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                        End If
                    Else : End If
                Else
                    runSQLCommand_characters_string("INSERT INTO character_talent ( guid, talent_id, current_rank, spec ) VALUES ( '" & characterguid.ToString & "', '" & TalentId & "', '" & TalentRank2 & "', '1' )", True)
                    talentlist2 = talentlist2 & " " & TalentId & "rank" & TalentRank2
                End If
            End If
        Next
    End Sub
End Class