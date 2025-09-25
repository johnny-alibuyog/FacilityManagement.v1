namespace Client.Pages.Sandbox.SplittingPrograms.Layout

open Client.Pages.Sandbox.SplittingPrograms

[<AutoOpen>]
module Types = 
    [<RequireQualifiedAccess>]
    type Page =
        | Counter of CounterPage.Types.State
        | TextInput of InputPage.Types.State

    [<RequireQualifiedAccess>]
    type Msg = 
        | Counter of CounterPage.Types.Msg
        | TextInput of InputPage.Types.Msg
        | NavigateTo of Page

    type State = {
        currentPage: Page
        counterPage: CounterPage.Types.State
        textInputPage: InputPage.Types.State
    }

module State =
    let init () =
        let counter = CounterPage.State.init()
        let textInput = InputPage.State.init()
        { currentPage = Page.Counter counter
          counterPage = counter
          textInputPage = textInput }

    let update msg state =
        match msg, state.currentPage with
        | Msg.Counter counterMsg, Page.Counter counterState ->
            let updatedCounter = CounterPage.State.update counterMsg counterState
            { state with
                currentPage = Page.Counter updatedCounter
                counterPage = updatedCounter
            }
        
        | Msg.TextInput textInputMsg, Page.TextInput textInputState ->
            let updatedText = InputPage.State.update textInputMsg textInputState
            { state with
                currentPage = Page.TextInput updatedText
                textInputPage = updatedText
            }
                    
        | Msg.NavigateTo page, _ -> 
            match page with
            | Page.Counter s -> 
                { state with 
                    currentPage = page
                    counterPage = s
                }
            | Page.TextInput s -> 
                { state with 
                    currentPage = page 
                    textInputPage = s
                }
        
        | _, _ -> 
            state // Ignore messages that don't match the current state

module View =
    open Feliz

    let renderNavigation state dispatch =
        Html.div [
            match state.currentPage with
            | Page.Counter value ->
                Html.button [
                    prop.text "Navigate to Text Input"
                    prop.onClick (fun _ -> dispatch (Msg.NavigateTo (Page.TextInput state.textInputPage)))
                ]
            | Page.TextInput _ ->
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

            | Page.TextInput state -> 
                InputPage.View.render state (Msg.TextInput >> dispatch)
        ]