Imports Metaphor.Persistence

Friend Module BakeryInitializer
    Friend Function Initialize(
                              context As IInitializationContext,
                              blueRoom As ILocation) As LocationInitializer
        Return Sub(bakery)
                   bakery.CreateFeature(FeatureSubtypes.DOOR, "Door to the Blue Room", InitializeBlueRoomDoor(context, blueRoom))
                   bakery.CreateFeature(FeatureSubtypes.CUPBOARD, "Cupboard", InitializeCupboard(context))
                   bakery.CreateFeature(FeatureSubtypes.DRY_PANTRY, "Dry Pantry", InitializeDryPantry(context))
               End Sub
    End Function

    Private Function InitializeDryPantry(context As IInitializationContext) As FeatureInitializer
        Return Sub(pantry)
                   pantry.InitializeCounter(Counters.FLOUR, 50, 0, 100)
                   pantry.CreateVerb(VerbSubtypes.ADD_FLOUR, "Add Flour")
               End Sub
    End Function

    Private Function InitializeCupboard(context As IInitializationContext) As FeatureInitializer
        Return Sub(cupboard)
                   cupboard.Inventory.CreateMixingBowl()
                   cupboard.Inventory.CreateMeasuringCup()
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
