Imports Metaphor.Persistence

Friend Module BakeryInitializer
    Friend Function Initialize(
                              context As IInitializationContext,
                              blueRoom As ILocation) As LocationInitializer
        Return Sub(bakery)
                   bakery.CreateFeature(FeatureSubtypes.DOOR, "Door to the Blue Room", InitializeBlueRoomDoor(context, blueRoom))
               End Sub
    End Function

    Private Function InitializeBlueRoomDoor(
                                           context As IInitializationContext,
                                           blueRoom As ILocation) As FeatureInitializer
        Return Sub(feature)
                   feature.SetDestination(blueRoom)
                   feature.CreateEnterVerb()
               End Sub
    End Function
End Module
