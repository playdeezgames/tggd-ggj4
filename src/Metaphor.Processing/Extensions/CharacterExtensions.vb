Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module CharacterExtensions
    <Extension>
    Friend Sub ShowStatus(character As ICharacter)
        character.AddMessage($"TODO: Describe {character.Name}.")
    End Sub
    <Extension>
    Friend Sub Look(character As ICharacter)
        character.AddMessage($"TODO: {character.Name} looks.")
    End Sub
End Module
