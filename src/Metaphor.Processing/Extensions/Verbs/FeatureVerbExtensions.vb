Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module FeatureVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, feature As IFeature, actor As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, feature As IFeature, actor As ICharacter)
#Region "Can Perform"
    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
            {VerbSubtypes.ENTER, AddressOf CanEnter},
            {VerbSubtypes.SLEEP, AddressOf CanSleep},
            {VerbSubtypes.ADD_FLOUR, AddressOf CanAddFlour}
        }

    Private Function CanAddFlour(verb As IVerb, feature As IFeature, actor As ICharacter) As Boolean
        Return Not actor.IsDead AndAlso
            Not feature.IsCounterMinimum(Counters.FLOUR) AndAlso
            actor.Inventory.Items.Any(Function(x) x.EntitySubtype = ItemSubtypes.MEASURING_CUP) AndAlso
            actor.Inventory.Items.Any(Function(x) x.EntitySubtype = ItemSubtypes.MIXING_BOWL AndAlso Not x.IsCounterMaximum(Counters.FLOUR))
    End Function

    Private Function CanSleep(verb As IVerb, feature As IFeature, actor As ICharacter) As Boolean
        Return Not actor.IsDead AndAlso actor.GetCounter(Counters.ENERGY) < actor.GetCounterMaximum(Counters.ENERGY) \ 2
    End Function

    Private Function CanEnter(verb As IVerb, feature As IFeature, actor As ICharacter) As Boolean
        Return Not actor.IsDead
    End Function

    <Extension>
    Friend Function CanPerform(verb As IVerb, feature As IFeature, actor As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntitySubtype, handler) Then
            Return handler.Invoke(verb, feature, actor)
        End If
        Return True
    End Function
#End Region
#Region "Perform"
    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbSubtypes.ENTER, AddressOf HandleEnter},
            {VerbSubtypes.SLEEP, AddressOf HandleSleep},
            {VerbSubtypes.ADD_FLOUR, AddressOf HandleAddFlour}
        }

    Private Sub HandleAddFlour(verb As IVerb, feature As IFeature, actor As ICharacter)
        Dim cup = actor.Inventory.Items.First(Function(x) x.EntitySubtype = ItemSubtypes.MEASURING_CUP)
        Dim bowl = actor.Inventory.Items.First(Function(x) x.EntitySubtype = ItemSubtypes.MIXING_BOWL)
        actor.AddMessage($"{actor.Name} uses {cup.Name} to move 1 flour from {feature.Name} to {bowl.Name}.")
        feature.ChangeCounter(Counters.FLOUR, -1)
        actor.AddMessage($"{feature.Name} now has {feature.GetCounter(Counters.FLOUR)} flour.")
        bowl.ChangeCounter(Counters.FLOUR, 1)
        actor.AddMessage($"{bowl.Name} now has {bowl.GetCounter(Counters.FLOUR)} flour.")
    End Sub

    Private Sub HandleSleep(verb As IVerb, feature As IFeature, actor As ICharacter)
        actor.AddMessage($"{actor.Name} sleeps.")
        Dim energy = actor.GetCounterCapacity(Counters.ENERGY)
        actor.AddMessage($"{actor.Name} gains {energy} energy.")
        actor.ChangeCounter(Counters.ENERGY, energy)
        actor.AddMessage($"{actor.Name} now has {actor.GetCounterStatistic(Counters.ENERGY)} energy.")
    End Sub

    Private Sub HandleEnter(verb As IVerb, feature As IFeature, actor As ICharacter)
        actor.AddMessage($"{actor.Name} goes through {feature.Name}.")
        actor.DoBiology()
        actor.Location = feature.GetDestination()
        actor.Look()
    End Sub
    <Extension>
    Sub Perform(verb As IVerb, feature As IFeature, actor As ICharacter)
        Dim handler As PerformHandler = Nothing
        If performTable.TryGetValue(verb.EntitySubtype, handler) Then
            handler.Invoke(verb, feature, actor)
            Return
        End If
    End Sub
#End Region
End Module
