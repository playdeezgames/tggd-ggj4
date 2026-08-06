Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module CharacterVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, character As ICharacter, actor As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, character As ICharacter, actor As ICharacter)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
        }

    <Extension>
    Friend Function CanPerform(verb As IVerb, character As ICharacter, actor As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntitySubtype, handler) Then
            Return handler.Invoke(verb, character, actor)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
        }

    <Extension>
    Sub Perform(verb As IVerb, character As ICharacter, actor As ICharacter)
        Dim handler As PerformHandler = Nothing
        If performTable.TryGetValue(verb.EntitySubtype, handler) Then
            handler.Invoke(verb, character, actor)
            Return
        End If
    End Sub
    <Extension>
    Friend Function GetKnownBubbles(character As ICharacter) As IEnumerable(Of ILocation)
        Return character.GetYokage(Yokages.KNOWN_BUBBLES).Select(AddressOf character.World.GetLocation)
    End Function
End Module
