namespace Client.Pages.Sandbox.Layout

open Client.Pages.Sandbox

[<RequireQualifiedAccess>]
type Page =
    | IntegratingSideEffects of IntegratingSideEffects.Page.State
    | SplittingPrograms of SplittingPrograms.Page.State


[<RequireQualifiedAccess>]
type Msg =
    | IntegratingSideEffects of IntegratingSideEffects.Page.Msg
    | SplittingPrograms of SplittingPrograms.Page.Msg
    | Load of route: Route

type State =
    | Loading
    | Loaded of currentPage: Page 

module State =
    open Elmish

    let init route =
        Loading, Cmd.ofMsg (Msg.Load route)

    let update msg state =
        match msg, state with
        | Msg.IntegratingSideEffects msg', Loaded currentPage ->
            let state', cmd' = 
                match currentPage with
                | Page.IntegratingSideEffects page ->
                    IntegratingSideEffects.Page.State.update msg' page
                | _ ->
                    IntegratingSideEffects.Page.State.init()

            Loaded (Page.IntegratingSideEffects state'), Cmd.map Msg.IntegratingSideEffects cmd'

        | Msg.SplittingPrograms msg', Loaded currentPage ->
            let state', cmd' = 
                match currentPage with
                | Page.SplittingPrograms page ->
                    SplittingPrograms.Page.State.update msg' page
                | _ ->
                    SplittingPrograms.Page.State.init()

            Loaded (Page.SplittingPrograms state'), Cmd.map Msg.SplittingPrograms cmd'

        | Msg.Load route, _ -> 
            match route with
            | Route.IntegratingSideEffects ->
                let state', cmd' = IntegratingSideEffects.Page.State.init()
                Loaded (Page.IntegratingSideEffects state'), Cmd.map Msg.IntegratingSideEffects cmd'

            | Route.SplittingPrograms ->
                let state', cmd' = SplittingPrograms.Page.State.init()
                Loaded (Page.SplittingPrograms state'), Cmd.map Msg.SplittingPrograms cmd'
        
        | _ ->
            state, Cmd.none
        
module View =
    open Feliz

    let render state dispatch =
        match state with
        | Loading ->
            Html.div "Loading..."

        | Loaded (Page.IntegratingSideEffects subState) ->
            IntegratingSideEffects.Page.View.render subState (Msg.IntegratingSideEffects >> dispatch)

        | Loaded (Page.SplittingPrograms subState) ->
            SplittingPrograms.Page.View.render subState (Msg.SplittingPrograms >> dispatch)

