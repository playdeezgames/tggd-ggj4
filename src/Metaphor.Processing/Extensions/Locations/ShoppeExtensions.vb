Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ShoppeExtensions
    <Extension>
    Friend Sub CreateShoppe(bubble As ILocation)
        bubble.CreateFeature(FeatureSubtypes.SHOPPE, "Shoppe", AddressOf InitializeShoppe)
    End Sub
    Private Sub InitializeShoppe(feature As IFeature)
    End Sub
End Module
