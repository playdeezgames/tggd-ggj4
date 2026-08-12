Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ItemVerbExtensions
    Private Delegate Function CanPerformHandler(verb As IVerb, item As IItem, actor As ICharacter) As Boolean
    Private Delegate Sub PerformHandler(verb As IVerb, item As IItem, actor As ICharacter)
#Region "Can Perform"
    Private ReadOnly canPerformTable As New Dictionary(Of String, CanPerformHandler) From
        {
            {VerbSubtypes.MIX, AddressOf CanMix},
            {VerbSubtypes.POUR_BATTER, AddressOf CanPourBatter}
        }

    Private Function CanPourBatter(verb As IVerb, item As IItem, actor As ICharacter) As Boolean
        Dim mixingBowl = actor.Inventory.GetItemsOfSubtype(ItemSubtypes.MIXING_BOWL).Single()
        Return item.EntitySubtype = ItemSubtypes.CAKE_PAN AndAlso
            mixingBowl IsNot Nothing AndAlso
            mixingBowl.HasBatter()
    End Function

    Private Function CanMix(verb As IVerb, item As IItem, actor As ICharacter) As Boolean
        Return item.EntitySubtype = ItemSubtypes.MIXING_BOWL AndAlso
            Not item.IsEmpty() AndAlso
            actor.Inventory.HasItemOfSubtype(ItemSubtypes.WOODEN_SPOON)
    End Function

    <Extension>
    Friend Function CanPerform(verb As IVerb, item As IItem, actor As ICharacter) As Boolean
        Dim handler As CanPerformHandler = Nothing
        If canPerformTable.TryGetValue(verb.EntitySubtype, handler) Then
            Return handler.Invoke(verb, item, actor)
        End If
        Return True
    End Function
#End Region
#Region "Perform"
    Private ReadOnly performTable As New Dictionary(Of String, PerformHandler) From
        {
            {VerbSubtypes.MIX, AddressOf HandleMix},
            {VerbSubtypes.POUR_BATTER, AddressOf HandlePourBatter}
        }

    Private Sub HandlePourBatter(verb As IVerb, item As IItem, actor As ICharacter)
        Dim mixingBowl = actor.Inventory.GetItemsOfSubtype(ItemSubtypes.MIXING_BOWL).Single()
        Dim batter = mixingBowl.GetDimension(Dimensions.BATTER)
        actor.AddMessage($"{actor.Name} pours {batter:f2} batter from {mixingBowl.Name} to {item.Name}.")
        mixingBowl.ChangeDimension(Dimensions.BATTER, -batter)
        item.ChangeDimension(Dimensions.BATTER, batter)
    End Sub

    Private Sub HandleMix(verb As IVerb, item As IItem, actor As ICharacter)
        actor.AddMessage($"{actor.Name} mixes the ingredients in {item.Name}.")
        item.Mix()
    End Sub

    <Extension>
    Sub Perform(verb As IVerb, item As IItem, actor As ICharacter)
        Dim handler As PerformHandler = Nothing
        If performTable.TryGetValue(verb.EntitySubtype, handler) Then
            handler.Invoke(verb, item, actor)
            Return
        End If
    End Sub
#End Region
End Module
