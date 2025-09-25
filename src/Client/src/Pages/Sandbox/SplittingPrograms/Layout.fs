namespace Client.Pages.Sandbox.SplittingPrograms.Layout

open Client.Pages.Sandbox.SplittingPrograms

[<AutoOpen>]
module Types = 
    [<RequireQualifiedAccess>]
    type Page =
        | Counter of CounterPage.Types.State
        | Input of InputPage.Types.State

    [<RequireQualifiedAccess>]
    type Msg = 
        | Counter of CounterPage.Types.Msg
        | Input of InputPage.Types.Msg
        | NavigateTo of Page

    type State = {
        currentPage: Page
        counterPage: CounterPage.Types.State
        inputPage: InputPage.Types.State
    }

module State =
    open Elmish

    let init () =
        let counter, _ = CounterPage.State.init()
        let textInput, _ = InputPage.State.init()
        { 
            currentPage = Page.Counter counter
            counterPage = counter
            inputPage = textInput 
        },
        Cmd.none

    let update msg state =
        match msg, state.currentPage with
        | Msg.Counter msg', Page.Counter state' ->
            let updated, _ = CounterPage.State.update msg' state'
            { state with
                currentPage = Page.Counter updated
                counterPage = updated
            },
            Cmd.none
        
        | Msg.Input msg', Page.Input state' ->
            let updated, _ = InputPage.State.update msg' state'
            { state with
                currentPage = Page.Input updated
                inputPage = updated
            },
            Cmd.none
                    
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
                CounterPage.View.render state (Msg.Counter >> dispatch)

            | Page.Input state -> 
                InputPage.View.render state (Msg.Input >> dispatch)
        ]