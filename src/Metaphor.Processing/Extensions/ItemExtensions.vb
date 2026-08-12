Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ItemExtensions
#Region "Mixing Bowl"
    ReadOnly mixingbowlCounterTable As New Dictionary(Of String, String) From
        {
            {Counters.FLOUR, "Flour"},
            {Counters.SUGAR, "Sugar"},
            {Counters.VANILLA, "Vanilla"},
            {Counters.BAKING_POWDER, "Baking Powder"},
            {Counters.SALT, "Salt"}
        }
    <Extension>
    Sub Mix(item As IItem)
        If item.EntitySubtype <> ItemSubtypes.MIXING_BOWL Then
            Return
        End If
        Dim tally As Integer = 0
        For Each counterId In mixingbowlCounterTable.Keys
            tally += item.GetCounter(counterId)
            item.MinimizeCounter(counterId)
        Next
        item.ChangeDimension(Dimensions.BATTER, tally)
    End Sub
    <Extension>
    Function IsEmpty(item As IItem) As Boolean
        If item.EntitySubtype <> ItemSubtypes.MIXING_BOWL Then
            Return False
        End If
        Return mixingbowlCounterTable.Keys.All(AddressOf item.IsCounterMinimum)
    End Function
    <Extension>
    Sub Empty(item As IItem)
        If item.EntitySubtype <> ItemSubtypes.MIXING_BOWL Then
            Return
        End If
        For Each counterId In mixingbowlCounterTable.Keys
            item.MinimizeCounter(counterId)
        Next
        item.MinimizeDimension(Dimensions.BATTER)
    End Sub
#End Region
#Region "Describe"
    Private Delegate Sub ItemDescriber(item As IItem)
    ReadOnly describeTable As New Dictionary(Of String, ItemDescriber) From
        {
            {ItemSubtypes.MIXING_BOWL, AddressOf DescribeMixingBowl}
        }
    Private Sub DescribeItem(item As IItem)
        item.AddMessage($"It is a {item.Name}.")
    End Sub
    Private Sub DescribeMixingBowl(item As IItem)
        DescribeItem(item)
        For Each entry In mixingbowlCounterTable
            Dim amount = item.GetCounter(entry.Key)
            If amount > 0 Then
                item.AddMessage($"{entry.Value}: {amount}")
            End If
        Next
        If Not item.IsDimensionMinimum(Dimensions.BATTER) Then
            Dim batter = item.GetDimension(Dimensions.BATTER)
            item.AddMessage($"Batter: {batter:f2}")
        End If
    End Sub
    <Extension>
    Sub Describe(item As IItem)
        Dim describer As ItemDescriber = Nothing
        If describeTable.TryGetValue(item.EntitySubtype, describer) Then
            describer.Invoke(item)
        Else
            DescribeItem(item)
        End If
    End Sub
#End Region
End Module
