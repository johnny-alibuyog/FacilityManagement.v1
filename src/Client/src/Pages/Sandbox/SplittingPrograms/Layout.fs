namespace Client.Pages.Sandbox.SplittingPrograms.Layout

open Client.Pages.Sandbox.SplittingPrograms

[<RequireQualifiedAccess>]
type Page =
    | Counter of CounterPage.State
    | Input of InputPage.State

[<RequireQualifiedAccess>]
type Msg = 
    | Counter of CounterPage.Msg
    | Input of InputPage.Msg
    | NavigateTo of Page

type State = {
    activePage: Page
    counterPage: CounterPage.State
    inputPage: InputPage.State
}

module State =
    open Elmish

    let init () =
        let counterState, counterCmd = CounterPage.State.init()
        let inputState, inputCmd = InputPage.State.init()
        
        let initialState = { 
            activePage = Page.Counter counterState
            counterPage = counterState
            inputPage = inputState 
        }
        
        let initialCmd = Cmd.batch [
            Cmd.map Msg.Counter counterCmd
            Cmd.map Msg.Input inputCmd
        ]

        initialState, initialCmd

    let update msg state =
        match msg, state.activePage with
        | Msg.Counter msg', Page.Counter state' ->
            let updated, cmd = CounterPage.State.update msg' state'
            { state with
                activePage = Page.Counter updated
                counterPage = updated
            },
            Cmd.map Msg.Counter cmd
        
        | Msg.Input msg', Page.Input state' ->
            let updated, cmd = InputPage.State.update msg' state'
            { state with
                activePage = Page.Input updated
                inputPage = updated
            },
            Cmd.map Msg.Input cmd
                    
        | Msg.NavigateTo page, _ -> 
            match page with
            | Page.Counter state' -> 
                { state with 
                    activePage = page
                    counterPage = state'
                },
                Cmd.none

            | Page.Input state' -> 
                { state with 
                    activePage = page 
                    inputPage = state'
                },
                Cmd.none
        
        | _, _ -> 
            state, Cmd.none // Ignore messages that don't match the current state

module View =
    open Feliz

    let renderNavigation state dispatch =
        Html.div [
            match state.activePage with
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

            match state.activePage with
            | Page.Counter state -> 
                CounterPage.View.render state (Msg.Counter >> dispatch)

            | Page.Input state -> 
                InputPage.View.render state (Msg.Input >> dispatch)
        ]