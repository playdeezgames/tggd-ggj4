Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module FeatureExtensions
#Region "Destination"
    <Extension>
    Friend Sub SetDestination(feature As IFeature, destination As ILocation)
        feature.SetYoke(Yokes.DESTINATION, destination.EntityId)
    End Sub
    <Extension>
    Friend Function GetDestination(feature As IFeature) As ILocation
        Return feature.World.GetLocation(feature.GetYoke(Yokes.DESTINATION))
    End Function
#End Region
#Region "Verbs"
    <Extension>
    Friend Sub CreateEnterVerb(feature As IFeature)
        feature.CreateVerb(VerbSubtypes.ENTER, "Enter")
    End Sub
    <Extension>
    Friend Sub CreateSleepVerb(feature As IFeature)
        feature.CreateVerb(VerbSubtypes.SLEEP, "Sleep")
    End Sub
#End Region
End Module
