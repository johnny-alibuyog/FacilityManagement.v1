open Elmish
open Client.Pages.Layout

 #if DEBUG
open Elmish.HMR
#endif

Program.mkProgram State.init State.update View.render
 #if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "root"
|> Program.run