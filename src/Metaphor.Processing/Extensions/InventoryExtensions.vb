Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module InventoryExtensions
#Region "Mixing Bowl"
    <Extension>
    Sub CreateMixingBowl(inventory As IInventory)
        inventory.CreateItem(ItemSubtypes.MIXING_BOWL, "Mixing Bowl", AddressOf InitializeMixingBowl)
    End Sub
    Private Sub InitializeMixingBowl(item As IItem)
        item.InitializeCounter(Counters.FLOUR, 0, 0, 5)
        item.InitializeCounter(Counters.SUGAR, 0, 0, 5)
        item.InitializeCounter(Counters.BAKING_POWDER, 0, 0, 5)
        item.InitializeCounter(Counters.SALT, 0, 0, 5)
        item.InitializeCounter(Counters.VANILLA, 0, 0, 5)
        item.CreateVerb(VerbSubtypes.MIX, "Mix")
        item.InitializeDimension(Dimensions.BATTER, 0.0, 0.0, Double.MaxValue)
    End Sub
#End Region
#Region "Measuring Cup"
    <Extension>
    Sub CreateMeasuringCup(inventory As IInventory)
        inventory.CreateItem(ItemSubtypes.MEASURING_CUP, "Measuring Cup")
    End Sub
#End Region
#Region "Wooden Spoon"
    <Extension>
    Sub CreateWoodenSpoon(inventory As IInventory)
        inventory.CreateItem(ItemSubtypes.WOODEN_SPOON, "Wooden Spoon")
    End Sub
#End Region
End Module
