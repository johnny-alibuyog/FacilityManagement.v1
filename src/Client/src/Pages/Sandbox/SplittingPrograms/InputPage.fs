namespace Client.Pages.Sandbox.SplittingPrograms.InputPage

[<AutoOpen>]
module Types =
    type Case = Upper | Lower
    type State = { text: string; case: Case }
    type Msg = SetText of string | ToggleCase

    module Case =
        let toggle case =
            match case with
            | Upper -> Lower
            | Lower -> Upper

module State =
    open Elmish

    let init () = { text = ""; case = Lower }, Cmd.none

    let update msg state =
        match msg with
        | SetText text -> { state with text = text }, Cmd.none
        | ToggleCase -> { state with case = Case.toggle state.case }, Cmd.none

    let displayText state =
        match state.case with
        | Upper -> state.text.ToUpper()
        | Lower -> state.text

    let buttonText state =
        match state.case with
        | Upper -> "To Lowercase"
        | Lower -> "To Uppercase"

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
                prop.text (State.buttonText state)
                prop.onClick (fun _ -> dispatch ToggleCase)
            ]
        ]