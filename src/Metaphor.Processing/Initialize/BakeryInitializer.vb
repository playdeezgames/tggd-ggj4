Imports Metaphor.Persistence

Friend Module BakeryInitializer
    Friend Function Initialize(
                              context As IInitializationContext,
                              blueRoom As ILocation) As LocationInitializer
        Return Sub(bakery)
                   bakery.CreateFeature(FeatureSubtypes.DOOR, "Door to the Blue Room", InitializeBlueRoomDoor(context, blueRoom))
                   bakery.CreateFeature(FeatureSubtypes.CUPBOARD, "Cupboard", InitializeCupboard(context))
                   bakery.CreateFeature(FeatureSubtypes.DRY_PANTRY, "Dry Pantry", InitializeDryPantry(context))
                   bakery.CreateFeature(FeatureSubtypes.BIN, "Bin", InitializeBin(context))
               End Sub
    End Function

    Private Function InitializeBin(context As IInitializationContext) As FeatureInitializer
        Return Sub(bin)
                   bin.CreateVerb(VerbSubtypes.EMPTY_MIXING_BOWL, "Empty Mixing Bowl")
               End Sub
    End Function

    Private Function InitializeDryPantry(context As IInitializationContext) As FeatureInitializer
        Return Sub(pantry)
                   pantry.InitializeCounter(Counters.FLOUR, 50, 0, 100)
                   pantry.InitializeCounter(Counters.SUGAR, 50, 0, 100)
                   pantry.InitializeCounter(Counters.BAKING_POWDER, 25, 0, 50)
                   pantry.InitializeCounter(Counters.VANILLA, 25, 0, 50)
                   pantry.InitializeCounter(Counters.SALT, 250, 0, 500)
                   pantry.CreateVerb(VerbSubtypes.ADD_FLOUR, "Add Flour")
                   pantry.CreateVerb(VerbSubtypes.ADD_SUGAR, "Add Sugar")
                   pantry.CreateVerb(VerbSubtypes.ADD_VANILLA, "Add Vanilla")
                   pantry.CreateVerb(VerbSubtypes.ADD_SALT, "Add Salt")
                   pantry.CreateVerb(VerbSubtypes.ADD_BAKING_POWDER, "Add Baking Powder")
               End Sub
    End Function

    Private Function InitializeCupboard(context As IInitializationContext) As FeatureInitializer
        Return Sub(cupboard)
                   cupboard.Inventory.CreateMixingBowl()
                   cupboard.Inventory.CreateMeasuringCup()
                   cupboard.Inventory.CreateWoodenSpoon()
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
