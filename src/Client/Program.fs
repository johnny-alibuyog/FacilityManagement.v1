(*

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

*)

open Elmish
open Client.Pages.Sandbox.SplittingPrograms.Layout

open Elmish.HMR

Program.mkSimple State.init State.update View.render
|> Program.withConsoleTrace
|> Program.withReactSynchronous "root"
|> Program.run

