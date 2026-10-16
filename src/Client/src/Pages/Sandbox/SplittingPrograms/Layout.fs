namespace Client.Pages.Sandbox.SplittingPrograms.Page

open Client.Pages.Sandbox.SplittingPrograms

[<RequireQualifiedAccess>]
type Page =
    | Counter of Counter.State
    | Input of Input.State

[<RequireQualifiedAccess>]
type Msg = 
    | Counter of Counter.Msg
    | Input of Input.Msg
    | NavigateTo of Page

type State = {
    currentPage: Page
    counterPage: Counter.State
    inputPage: Input.State
}

module State =
    open Elmish

    let init () =
        let counterState, counterCmd = Counter.State.init()
        let inputState, inputCmd = Input.State.init()
        
        let initialState = { 
            currentPage = Page.Counter counterState
            counterPage = counterState
            inputPage = inputState 
        }
        
        let initialCmd = Cmd.batch [
            Cmd.map Msg.Counter counterCmd
            Cmd.map Msg.Input inputCmd
        ]

        initialState, initialCmd

    let update msg state =
        match msg, state.currentPage with
        | Msg.Counter msg', Page.Counter state' ->
            let updated, cmd = Counter.State.update msg' state'
            { state with
                currentPage = Page.Counter updated
                counterPage = updated
            },
            Cmd.map Msg.Counter cmd
        
        | Msg.Input msg', Page.Input state' ->
            let updated, cmd = Input.State.update msg' state'
            { state with
                currentPage = Page.Input updated
                inputPage = updated
            },
            Cmd.map Msg.Input cmd
                    
        | Msg.NavigateTo page, _ -> 
            match page with
            | Page.Counter state' -> 
                { state with 
                    currentPage = page
                    counterPage = state'
                },
                Cmd.none

            | Page.Input state' -> 
                { state with 
                    currentPage = page 
                    inputPage = state'
                },
                Cmd.none
        
        | _, _ -> 
            state, Cmd.none // Ignore messages that don't match the current state

module View =
    open Feliz

    let renderNavigation state dispatch =
        Html.div [
            match state.currentPage with
            | Page.Counter value ->
                Html.button [
                    prop.text "Navigate to Text Input"
                    prop.onClick (fun _ -> dispatch (Msg.NavigateTo (Page.Input state.inputPage)))
                ]
            | Page.Input _ ->
                Html.button [
                    prop.text "Navigate to Counter"
                    prop.onClick (fun _ -> dispatch (Msg.NavigateTo (Page.Counter state.counterPage)))
                ]
        ]

    let render state dispatch =
        Html.div [
            renderNavigation state dispatch

            match state.currentPage with
            | Page.Counter state -> 
                Counter.View.render state (Msg.Counter >> dispatch)

            | Page.Input state -> 
                Input.View.render state (Msg.Input >> dispatch)
        ]