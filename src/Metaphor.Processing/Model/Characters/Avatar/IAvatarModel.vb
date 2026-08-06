Public Interface IAvatarModel
    Sub ShowStatus()
    ReadOnly Property Inventory As IInventoryModel
    ReadOnly Property Verbs As IEnumerable(Of IVerbModel)
    ReadOnly Property Mode As String
End Interface
