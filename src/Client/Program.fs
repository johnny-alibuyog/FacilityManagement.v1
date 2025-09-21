open Elmish
open Client.Pages.Layout

 #if DEBUG
open Elmish.HMR
#endif

Program.mkProgram init update view
 #if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "root"
|> Program.run