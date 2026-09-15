open Elmish
open Elmish.React
open Client.Pages.Layout

Program.mkProgram State.init State.update View.render
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "root"
|> Program.run

