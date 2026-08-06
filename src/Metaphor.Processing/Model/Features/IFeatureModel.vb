Public Interface IFeatureModel
    ReadOnly Property Name As String
    ReadOnly Property Verbs As IEnumerable(Of IVerbModel)
    ReadOnly Property Exists As Boolean
    ReadOnly Property Inventory As IInventoryModel
End Interface
