﻿Imports Namcore_Studio.EventLogging
Imports Namcore_Studio.CommandHandler
Imports Namcore_Studio.GlobalVariables
Imports Namcore_Studio.Basics
Imports Namcore_Studio.Conversions
Public Class QuestCreation
    Public Shared Sub SetCharacterQuests(ByVal setId As Integer, Optional charguid As Integer = 0)
        If charguid = 0 Then charguid = characterGUID
        LogAppend("Setting quests for character: " & charguid.ToString() & " // setId is : " & setId.ToString(), "QuestCreation_SetCharacterQuests", True)
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
    Private Shared Sub createAtArcemu(ByVal characterguid As Integer, ByVal targetSetId As Integer)
        LogAppend("Creating at arcemu", "QuestCreation_createAtArcemu", False)
        Dim lastslot As Integer = TryInt(runSQLCommand_characters_string("SELECT slot FROM questlog WHERE player_guid='" & characterguid.ToString() & "' AND slot=(SELECT MAX(slot) FROM characters)", True)) + 1
        Dim character_queststatus_list As List(Of String) = ConvertStringToList(GetTemporaryCharacterInformation("@character_queststatus", targetSetId))
        If Not character_queststatus_list.Count = 0 Then
            For Each queststring As String In character_queststatus_list
                Dim explored As String = splitList(queststring, "explored")
                If explored = Nothing Then explored = ""
                Dim tmpcommand As String = "INSERT INTO questlog ( player_guid, quest_id, slot, `completed`, `explored_area1` ) VALUES ( '" & characterguid.ToString() & "', '" & splitList(queststring, "quest") & "', '" &
                                            lastslot.ToString & "', '" & splitList(queststring, "status") & "'," & " '" & explored & "')"
                runSQLCommand_characters_string(tmpcommand, True)
                lastslot += 1
            Next
        Else : LogAppend("No quests in questlog", "QuestCreation_createAtArcemu", False) : End If
        Dim finishedQuestsString As String = GetTemporaryCharacterInformation("@character_finishedQuests", targetSetId)
        If Not finishedQuestsString = "" Then runSQLCommand_characters_string("UPDATE characters SET finished_quests='" & finishedQuestsString & "' WHERE guid='" & characterguid.ToString() & "'", True)
    End Sub
    Private Shared Sub createAtTrinity(ByVal characterguid As Integer, ByVal targetSetId As Integer)
        LogAppend("Creating at Trinity", "QuestCreation_createAtTrinity", False)
        Dim character_queststatus_list As List(Of String) = ConvertStringToList(GetTemporaryCharacterInformation("@character_queststatus", targetSetId))
        If Not character_queststatus_list.Count = 0 Then
            For Each queststring As String In character_queststatus_list
                Dim queststatus As String = splitList(queststring, "status")
                If queststatus = "0" Then queststatus = "1"
                runSQLCommand_characters_string("INSERT INTO character_queststatus ( guid, quest, `status`, `explored` ) VALUES ( '" & characterguid.ToString() & "', '" & splitList(queststring, "quest") &
                                                "', '" & queststatus & "', '" & splitList(queststring, "explored") & "')", True)
            Next
        Else : LogAppend("No quests in questlog", "QuestCreation_createAtTrinity", False) : End If
        Dim finishedQuestsString As String = GetTemporaryCharacterInformation("@character_finishedQuests", targetSetId)
        If Not finishedQuestsString = "" Then
            Try
                Dim parts() As String = finishedQuestsString.Split(","c)
                Dim excounter As Integer = UBound(finishedQuestsString.Split(CChar(",")))
                Dim startcounter As Integer = 0
                Do
                    Dim questid As String = parts(startcounter)
                    runSQLCommand_characters_string("INSERT IGNORE INTO character_queststatus_rewarded ( `guid`, `quest` ) VALUES ( '" & characterguid.ToString() & "', '" & questid & "' )", True)
                    startcounter += 1
                Loop Until startcounter = excounter
            Catch : End Try
        End If
    End Sub
    Private Shared Sub createAtMangos(ByVal characterguid As Integer, ByVal targetSetId As Integer)
        LogAppend("Creating at Mangos", "QuestCreation_createAtMangos", False)
          Dim character_queststatus_list As List(Of String) = ConvertStringToList(GetTemporaryCharacterInformation("@character_queststatus", targetSetId))
        If Not character_queststatus_list.Count = 0 Then
            For Each queststring As String In character_queststatus_list
                Dim queststatus As String = splitList(queststring, "status")
                If queststatus = "0" Then queststatus = "1"
                runSQLCommand_characters_string("INSERT INTO character_queststatus ( guid, quest, `status`, `explored` ) VALUES ( '" & characterguid.ToString() & "', '" & splitList(queststring, "quest") &
                                                "', '" & queststatus & "', '" & splitList(queststring, "explored") & "')", True)
            Next
        Else : LogAppend("No quests in questlog", "QuestCreation_createAtMangos", False) : End If
        Dim finishedQuestsString As String = GetTemporaryCharacterInformation("@character_finishedQuests", targetSetId)
        If Not finishedQuestsString = "" Then
            Try
                Dim parts() As String = finishedQuestsString.Split(","c)
                Dim excounter As Integer = UBound(finishedQuestsString.Split(CChar(",")))
                Dim startcounter As Integer = 0
                Do
                    Dim questid As String = parts(startcounter)
                    runSQLCommand_characters_string("INSERT INTO character_queststatus ( guid, quest, `status`, `rewarded` ) VALUES ( '" & characterguid.ToString() & "', '" & questid & "', '1', '1')", True)
                    startcounter += 1
                Loop Until startcounter = excounter
            Catch : End Try
        End If
    End Sub
End Class