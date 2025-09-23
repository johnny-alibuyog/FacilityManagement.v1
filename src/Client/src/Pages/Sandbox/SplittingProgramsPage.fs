namespace Client.Pages.Sandbox.SplittingProgramsPage

[<AutoOpen>]
module Types = 
    type Page = 
        | CounterPage
        | TextInputPage

    type State = {
        count: int
        text: string
        isUpperCase: bool
        currentPage: Page
    }

    type Msg = 
        | Increment
        | Decrement
        | SetText of string
        | ToggleCase
        | NavigateTo of Page

module State =
    let init () = 
        { count = 0; text = ""; isUpperCase = false; currentPage = CounterPage }

    let update msg state =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | SetText text -> { state with text = text }
        | ToggleCase -> { state with isUpperCase = not state.isUpperCase }
        | NavigateTo page -> { state with currentPage = page }

module View =
    open Feliz

    let render state dispatch =
        let renderCounter () =
            Html.div [
                Html.nav [
                    Html.button [
                        prop.text "Navigate to Text Input"
                        prop.onClick (fun _ -> dispatch (NavigateTo TextInputPage))
                    ]
                ]
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

        let renderTextInput () =
            let displayedText = if state.isUpperCase then state.text.ToUpper() else state.text
            Html.div [
                Html.nav [
                    Html.button [
                        prop.text "Navigate to Counter"
                        prop.onClick (fun _ -> dispatch (NavigateTo CounterPage))
                    ]
                ]
                Html.h2 "Text Input"
                Html.input [
                    prop.type' "text"
                    prop.value state.text
                    prop.onChange (fun e -> dispatch (SetText e))
                ]
                Html.p $"You typed: {displayedText}"
                Html.button [
                    prop.text "Toggle Case"
                    prop.onClick (fun _ -> dispatch ToggleCase)
                ]
            ]

        Html.div [
            match state.currentPage with
            | CounterPage -> renderCounter()
            | TextInputPage -> renderTextInput()
        ]