Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence
Imports TGGD.Processing

Friend Module FuelingStationExtensions
    <Extension>
    Sub CreateFuelingStation(bubble As ILocation)
        bubble.CreateFeature(FeatureSubtypes.FUELING_STATION, "Fueling Station", AddressOf InitializeFuelingStation)
    End Sub
    Private Sub InitializeFuelingStation(feature As IFeature)
        feature.SetDimension(Dimensions.FUEL_PRICE, 1.0 * RNG.RollDice("3d6") / 10.5)
        feature.CreateVerb(VerbSubtypes.BUY_FUEL, "Buy Fuel...")
    End Sub
End Module
