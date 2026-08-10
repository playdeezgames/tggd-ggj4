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
    End Sub
#End Region
#Region "Measuring Cup"
    <Extension>
    Sub CreateMeasuringCup(inventory As IInventory)
        inventory.CreateItem(ItemSubtypes.MEASURING_CUP, "Measuring Cup", AddressOf InitializeMeasuringCup)
    End Sub
    Private Sub InitializeMeasuringCup(item As IItem)
        'TODO: tags and stats
    End Sub
#End Region
End Module
