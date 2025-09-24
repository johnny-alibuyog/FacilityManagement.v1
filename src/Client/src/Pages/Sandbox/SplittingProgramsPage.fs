namespace Client.Pages.Sandbox.SplittingProgramsPage

[<RequireQualifiedAccess>]
module Counter = 
    [<AutoOpen>]
    module Types =
        type State = { count: int }
        type Msg = Increment | Decrement

    module State =
        let init () = { count = 0 }

        let update msg state =
            match msg with
            | Increment -> { state with count = state.count + 1 }
            | Decrement -> { state with count = state.count - 1 }

    module View =
        open Feliz

        let render state dispatch =
            Html.div [
                Html.h2 "Counter"
                Html.p $"Count: {state.count}"
                Html.button [
                    prop.text "+"
                    prop.onClick (fun _ -> dispatch Increment)
                ]
                Html.button [
                    prop.text "-"
                    prop.onClick (fun _ -> dispatch Decrement)
                ]
            ]

[<RequireQualifiedAccess>]
module TextInput =
    [<AutoOpen>]
    module Types =
        type State = { text: string; isUpperCase: bool }
        type Msg = SetText of string | ToggleCase

    module State =
        let init () = { text = ""; isUpperCase = false }

        let update msg state =
            match msg with
            | SetText text -> { state with text = text }
            | ToggleCase -> { state with isUpperCase = not state.isUpperCase }

        let displayText state =
            match state.isUpperCase with
            | true -> state.text.ToUpper()
            | false -> state.text 

    module View =
        open Feliz

        let render state dispatch =
            Html.div [
                Html.h2 "Text Input"
                Html.input [
                    prop.type' "text"
                    prop.value state.text
                    prop.onChange (fun e -> dispatch (SetText e))
                ]
                Html.p $"You typed: {State.displayText state}"
                Html.button [
                    prop.text "Toggle Case"
                    prop.onClick (fun _ -> dispatch ToggleCase)
                ]
            ]

[<AutoOpen>]
module Types = 
    [<RequireQualifiedAccess>]
    type Page =
        | Counter of Counter.Types.State
        | TextInput of TextInput.Types.State

    [<RequireQualifiedAccess>]
    type Msg = 
        | Counter of Counter.Types.Msg
        | TextInput of TextInput.Types.Msg
        | NavigateTo of Page

    type State = {
        currentPage: Page
        counterPage: Counter.Types.State
        textInputPage: TextInput.Types.State
    }

module State =
    let init () =
        let counter = Counter.State.init()
        let textInput = TextInput.State.init()
        { currentPage = Page.Counter counter
          counterPage = counter
          textInputPage = textInput }

    let update msg state =
        match msg, state.currentPage with
        | Msg.Counter counterMsg, Page.Counter counterState ->
            let updatedCounter = Counter.State.update counterMsg counterState
            { state with
                currentPage = Page.Counter updatedCounter
                counterPage = updatedCounter
            }
        
        | Msg.TextInput textInputMsg, Page.TextInput textInputState ->
            let updatedText = TextInput.State.update textInputMsg textInputState
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
                Counter.View.render state (Msg.Counter >> dispatch)

            | Page.TextInput state -> 
                TextInput.View.render state (Msg.TextInput >> dispatch)
        ]