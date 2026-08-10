Imports Metaphor.Persistence

Friend Module BlueRoomInitializer
    Friend Function Initialize(context As IInitializationContext) As Persistence.LocationInitializer
        Return Sub(room)
                   room.CreateCharacter(CharacterSubtypes.N00B, context.ChosenName, InitializeAvatar(context))
                   room.CreateFeature(FeatureSubtypes.DOOR, "Door to Bakery", InitializeBakeryDoor(context))
                   room.CreateFeature(FeatureSubtypes.BED, "Yer Bed", InitializeBed(context))
               End Sub
    End Function
    Private Function InitializeBed(context As IInitializationContext) As FeatureInitializer
        Return Sub(bed)
                   bed.CreateSleepVerb()
               End Sub
    End Function
    Private Function InitializeBakeryDoor(context As IInitializationContext) As FeatureInitializer
        Return Sub(feature)
                   Dim bakery = feature.World.CreateLocation(
                        LocationSubtypes.BAKERY,
                        "The Aggressively Pink Bakery",
                        BakeryInitializer.Initialize(context, feature.Location))
                   feature.SetDestination(bakery)
                   feature.CreateEnterVerb()
               End Sub
    End Function

    Private Function InitializeAvatar(context As IInitializationContext) As CharacterInitializer
        Return Sub(character)
                   character.World.Avatar = character
#If DEBUG Then
                   character.InitializeCounter(Counters.ENERGY, 10, 0, 100)
                   character.InitializeCounter(Counters.HEALTH, 10, 0, 100)
#Else
                   character.InitializeCounter(Counters.ENERGY, 100, 0, 100)
                   character.InitializeCounter(Counters.HEALTH, 100, 0, 100)
#End If
               End Sub
    End Function
End Module
