Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module FeatureExtensions
#Region "Describe"
    Private Sub DescribeFeature(feature As IFeature)
        feature.AddMessage($"It is a {feature.Name}.")
    End Sub
    Private Delegate Sub FeatureDescriber(feature As IFeature)
    Private ReadOnly featureDescribers As New Dictionary(Of String, FeatureDescriber) From
        {
            {FeatureSubtypes.OVEN, AddressOf DescribeOven}
        }

    Private Sub DescribeOven(feature As IFeature)
        DescribeFeature(feature)
        If feature.HasTag(Tags.ON) Then
            feature.AddMessage($"{feature.Name} is on.")
        End If
        If feature.HasTag(Tags.OPEN) Then
            feature.AddMessage($"{feature.Name}'s door is open.")
        End If
        If feature.Inventory.HasItemOfSubtype(ItemSubtypes.CAKE_PAN) Then
            Dim item = feature.Inventory.GetItemsOfSubtype(ItemSubtypes.CAKE_PAN).Single()
            feature.AddMessage($"{feature.Name} has {item.Name} in it.")
        End If
    End Sub

    <Extension>
    Friend Sub Describe(feature As IFeature)
        Dim describer As FeatureDescriber = Nothing
        If featureDescribers.TryGetValue(feature.EntitySubtype, describer) Then
            describer.Invoke(feature)
        Else
            DescribeFeature(feature)
        End If
    End Sub
#End Region
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
