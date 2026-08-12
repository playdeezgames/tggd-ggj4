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
            {VerbSubtypes.ADD_FLOUR, CanAddIngredient(Counters.FLOUR, True, False)},
            {VerbSubtypes.ADD_SUGAR, CanAddIngredient(Counters.SUGAR, True, False)},
            {VerbSubtypes.ADD_VANILLA, CanAddIngredient(Counters.VANILLA, False, False)},
            {VerbSubtypes.ADD_BUTTER, CanAddIngredient(Counters.BUTTER, False, False)},
            {VerbSubtypes.ADD_EGG, CanAddIngredient(Counters.EGG, False, False)},
            {VerbSubtypes.ADD_MILK, CanAddIngredient(Counters.MILK, True, False)},
            {VerbSubtypes.ADD_BAKING_POWDER, CanAddIngredient(Counters.BAKING_POWDER, False, True)},
            {VerbSubtypes.ADD_SALT, CanAddIngredient(Counters.SALT, False, True)},
            {VerbSubtypes.EMPTY_MIXING_BOWL, AddressOf CanEmptyMixingBowl}
        }

    Private Function CanAddIngredient(counterId As String, needsMeasuringCup As Boolean, needsMeasuringSpoons As Boolean) As CanPerformHandler
        Return Function(verb As IVerb, feature As IFeature, actor As ICharacter)
                   Return Not actor.IsDead AndAlso
                        Not feature.IsCounterMinimum(counterId) AndAlso
                        (Not needsMeasuringCup OrElse actor.Inventory.HasItemOfSubtype(ItemSubtypes.MEASURING_CUP)) AndAlso
                        (Not needsMeasuringSpoons OrElse actor.Inventory.HasItemOfSubtype(ItemSubtypes.MEASURING_SPOONS)) AndAlso
                        actor.Inventory.Items.Any(Function(x) x.EntitySubtype = ItemSubtypes.MIXING_BOWL AndAlso Not x.IsCounterMaximum(counterId))
               End Function
    End Function

    Private Function CanEmptyMixingBowl(verb As IVerb, feature As IFeature, actor As ICharacter) As Boolean
        Dim mixingBowl = actor.Inventory.Items.FirstOrDefault(Function(x) x.EntitySubtype = ItemSubtypes.MIXING_BOWL)
        Return Not actor.IsDead AndAlso If(mixingBowl?.IsEmpty(), False)
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
            {VerbSubtypes.ADD_FLOUR, HandleAddIngredient(Counters.FLOUR, "flour")},
            {VerbSubtypes.ADD_SUGAR, HandleAddIngredient(Counters.SUGAR, "sugar")},
            {VerbSubtypes.ADD_BAKING_POWDER, HandleAddIngredient(Counters.BAKING_POWDER, "baking powder")},
            {VerbSubtypes.ADD_SALT, HandleAddIngredient(Counters.SALT, "salt")},
            {VerbSubtypes.ADD_BUTTER, HandleAddIngredient(Counters.BUTTER, "butter")},
            {VerbSubtypes.ADD_EGG, HandleAddIngredient(Counters.EGG, "egg")},
            {VerbSubtypes.ADD_MILK, HandleAddIngredient(Counters.MILK, "milk")},
            {VerbSubtypes.ADD_VANILLA, HandleAddIngredient(Counters.VANILLA, "vanilla")},
            {VerbSubtypes.EMPTY_MIXING_BOWL, AddressOf HandleEmptyMixingBowl}
        }

    Private Function HandleAddIngredient(counterId As String, counterName As String) As PerformHandler
        Return Sub(verb As IVerb, feature As IFeature, actor As ICharacter)
                   Dim cup = actor.Inventory.Items.First(Function(x) x.EntitySubtype = ItemSubtypes.MEASURING_CUP)
                   Dim bowl = actor.Inventory.Items.First(Function(x) x.EntitySubtype = ItemSubtypes.MIXING_BOWL)
                   actor.AddMessage($"{actor.Name} uses {cup.Name} to move 1 {counterName} from {feature.Name} to {bowl.Name}.")
                   feature.ChangeCounter(counterId, -1)
                   actor.AddMessage($"{feature.Name} now has {feature.GetCounter(counterId)} {counterName}.")
                   bowl.ChangeCounter(counterId, 1)
                   actor.AddMessage($"{bowl.Name} now has {bowl.GetCounter(counterId)} {counterName}.")
               End Sub
    End Function

    Private Sub HandleEmptyMixingBowl(verb As IVerb, feature As IFeature, actor As ICharacter)
        Dim mixingBowl = actor.Inventory.Items.Single(Function(x) x.EntitySubtype = ItemSubtypes.MIXING_BOWL)
        mixingBowl.Empty()
        actor.AddMessage($"{actor.Name} empties {mixingBowl.Name} into {feature.Name}.")
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
