Imports System.Runtime.CompilerServices
Imports Metaphor.Persistence

Friend Module WorldInitializer
    <Extension>
    Friend Sub Initialize(world As IWorld, context As IInitializationContext)
        world.Clear()
        world.CreateLocation(LocationSubtypes.BLUE_ROOM, "The Blue Room", BlueRoomInitializer.Initialize(context))
        world.AddMessage("Welcome to Cake of SPLORR!!")
        'TODO: describe what to do
        world.Avatar.Look()
    End Sub
End Module
