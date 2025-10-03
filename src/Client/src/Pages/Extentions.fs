namespace Client.Pages.Extentions

open Elmish

type ApiData<'t> =
    | NotStarted
    | Loading of 't option
    | Loaded of 't

type ApiCall<'TStart, 'TFinished> =
    | Start of 'TStart
    | Finished of 'TFinished

module Cmd =
    let fromAsync (operation: Async<'msg>): Cmd<'msg> =
        let delayedCmd (dispatch: 'msg -> unit): unit =
            let delayedDispatch = async {
                match! Async.Catch operation with
                | Choice1Of2 msg -> dispatch msg
                | Choice2Of2 ex -> printfn "Error in async operation: %s" ex.Message
            }

            Async.StartImmediate delayedDispatch

        Cmd.ofEffect delayedCmd
    
module Async =
    let map f asyncOp = async {
        let! result = asyncOp
        return f result
    }