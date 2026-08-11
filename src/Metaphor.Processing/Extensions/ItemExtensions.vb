Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module ItemExtensions
#Region "Describe"
    Private Delegate Sub ItemDescriber(item As IItem)
    ReadOnly describeTable As New Dictionary(Of String, ItemDescriber) From
        {
            {ItemSubtypes.MEASURING_CUP, AddressOf DescribeMeasuringCup},
            {ItemSubtypes.MIXING_BOWL, AddressOf DescribeMixingBowl}
        }
    Private Sub DescribeItem(item As IItem)
        item.AddMessage($"It is a {item.Name}.")
    End Sub
    Private Sub DescribeMixingBowl(item As IItem)
        DescribeItem(item)
        Dim flour = item.GetCounter(Counters.FLOUR)
        If flour > 0 Then
            item.AddMessage($"Flour: {flour}")
        End If
        Dim sugar = item.GetCounter(Counters.SUGAR)
        If sugar > 0 Then
            item.AddMessage($"Sugar: {sugar}")
        End If
    End Sub
    Private Sub DescribeMeasuringCup(item As IItem)
        DescribeItem(item)
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
