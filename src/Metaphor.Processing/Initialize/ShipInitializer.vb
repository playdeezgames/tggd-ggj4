Imports Metaphor.Persistence

Friend Module ShipInitializer
    Friend Function Initialize(context As IInitializationContext) As Persistence.LocationInitializer
        Return Sub(ship)
                   ship.CreateCharacter(CharacterSubtypes.N00B, context.ChosenName, InitializeAvatar(context))
               End Sub
    End Function

    Private Function InitializeAvatar(context As IInitializationContext) As CharacterInitializer
        Return Sub(character)
                   character.World.Avatar = character
               End Sub
    End Function
End Module
