Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module BubbleExtensions
    Friend Sub DescribeBubble(bubble As ILocation)
        Dim world = bubble.World
        world.AddMessage($"Bubble: {bubble.Name}")
    End Sub

    <Extension>
    Function IsVisibleTo(fromLocation As ILocation, toLocation As ILocation) As Boolean
        Return fromLocation.DistanceTo(toLocation) <= Math.Min(fromLocation.GetVisibility(), toLocation.GetVisibility())
    End Function
    <Extension>
    Function DistanceTo(fromLocation As ILocation, toLocation As ILocation) As Double
        Return Utility.Distance(
            (fromLocation.GetLongitude(), fromLocation.GetLatitude()),
            (toLocation.GetLongitude(), toLocation.GetLatitude()))
    End Function
    <Extension>
    Function DepthDifference(fromLocation As ILocation, toLocation As ILocation) As Double
        Return Math.Abs(fromLocation.GetDepth() - toLocation.GetDepth())
    End Function
    <Extension>
    Function HeadingTo(fromLocation As ILocation, toLocation As ILocation) As Double
        Dim deltaX = toLocation.GetLongitude() - fromLocation.GetLongitude()
        Dim deltaY = toLocation.GetLatitude() - fromLocation.GetLatitude()
        Dim heading = Math.Atan2(deltaY, deltaX) * 360.0 / Math.PI / 2
        Return If(heading < 0.0, heading + 360.0, heading)
    End Function
    <Extension>
    Friend Function GetBubbleName(bubble As ILocation) As String
        Return If(bubble.HasTag(Tags.KNOWN), bubble.Name, "UNKNOWN BUBBLE")
    End Function
#Region "Fueling Station"
    <Extension>
    Friend Function GetFuelingStation(bubble As ILocation) As IFeature
        Return bubble.Features.SingleOrDefault(Function(x) x.EntitySubtype = FeatureSubtypes.FUELING_STATION)
    End Function
#End Region
End Module
