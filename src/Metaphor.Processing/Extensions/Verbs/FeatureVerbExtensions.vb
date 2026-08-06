Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module FeatureVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, feature As IFeature, actor As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, feature As IFeature, actor As ICharacter)

    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
        }

    <Extension>
    Friend Function CanPerform(verb As IVerb, feature As IFeature, actor As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntitySubtype, handler) Then
            Return handler.Invoke(verb, feature, actor)
        End If
        Return True
    End Function

    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbSubtypes.MOVE, AddressOf HandleMove},
            {VerbSubtypes.BUY_FUEL, AddressOf HandleBuyFuel}
        }

    Private Sub HandleBuyFuel(verb As IVerb, feature As IFeature, actor As ICharacter)
        Dim price = feature.GetFuelPrice()
        Dim jools = actor.GetJools()
        Dim ship = actor.GetShip()
        Dim capacity = ship.GetFuelCapacity()
        Dim maximumPurchace = Math.Min(capacity, jools / price)
        Dim bubble = feature.Location
        actor.AddMessage($"{ship.Name} has {ship.GetDimensionStatistic(Dimensions.FUEL)} fuel.")
        actor.AddMessage($"Price of fuel on {bubble.Name} is {price:f2} jools/unit.")
        actor.AddMessage($"{actor.Name} has {jools:f2} jools.")
        actor.AddMessage($"{actor.Name} can buy {maximumPurchace:f2} fuel.")
        actor.SetMode(Modes.BUYING_FUEL)
    End Sub

    Private Sub HandleMove(verb As IVerb, feature As IFeature, actor As ICharacter)
        Dim world = verb.World
        Dim avatar = world.Avatar
        avatar.Location = feature.GetDestination()
        avatar.Look()
    End Sub

    <Extension>
    Sub Perform(verb As IVerb, feature As IFeature, actor As ICharacter)
        Dim handler As PerformHandler = Nothing
        If performTable.TryGetValue(verb.EntitySubtype, handler) Then
            handler.Invoke(verb, feature, actor)
            Return
        End If
    End Sub
End Module
