Imports Metaphor.Persistence
Imports TGGD.Processing

Friend Module ShipInitializer
    Friend Function Initialize(context As IInitializationContext) As Persistence.LocationInitializer
        Return Sub(ship)
                   context.Ship = ship
                   ship.InitializeDimension(Dimensions.HEADING, RNG.FromRange(HEADING_MINIMUM, HEADING_MAXIMUM), HEADING_MINIMUM, HEADING_MAXIMUM)
                   ship.InitializeDimension(Dimensions.SPEED, SPEED_AHEAD_FLANK / 2, SPEED_FULL_STOP, SPEED_AHEAD_FLANK)
                   ship.InitializeDimension(Dimensions.LONGITUDE, context.WorldWidth / 2, 0.0, context.WorldWidth)
                   ship.InitializeDimension(Dimensions.LATITUDE, context.WorldHeight / 2, 0.0, context.WorldHeight)
                   ship.InitializeDimension(Dimensions.DEPTH, (context.SnorkelDepth + context.WorldDepth) / 2, context.SnorkelDepth, context.WorldDepth)
                   ship.InitializeDimension(Dimensions.HYDROPLANE, 0.0, MINIMUM_HYDROPLANE, MAXIMUM_HYDROPLANE)
                   ship.InitializeDimension(Dimensions.OXYGEN, 500, 0, 1000)
                   ship.InitializeDimension(Dimensions.BATTERY, 500, 0, 1000)
                   ship.InitializeDimension(Dimensions.FUEL, 500, 0, 1000)
                   ship.InitializeDimension(Dimensions.ENGINE, 100, 0, 500)
                   ship.SetDimension(Dimensions.VISIBILITY, 100.0)
                   ship.CreateCharacter(CharacterSubtypes.N00B, context.ChosenName, InitializeAvatar(context))
               End Sub
    End Function

    Private Function InitializeAvatar(context As IInitializationContext) As CharacterInitializer
        Return Sub(character)
                   character.World.Avatar = character
                   character.InitializeCounter(Counters.HEALTH, 100, 0, 100)
                   character.InitializeCounter(Counters.SATIETY, 100, 0, 100)
                   character.InitializeCounter(Counters.STOMACH, 0, 0, 50)
#If DEBUG Then
                   character.InitializeDimension(Dimensions.JOOLS, 100.0, 0.0, Double.MaxValue)
#Else
                   character.InitializeDimension(Dimensions.JOOLS, 0.0, 0.0, Double.MaxValue)
#End If
               End Sub
    End Function
End Module
