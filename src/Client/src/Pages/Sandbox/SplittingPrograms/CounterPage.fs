namespace Client.Pages.Sandbox.SplittingPrograms.CounterPage

[<AutoOpen>]
module Types =
    type State = { count: int }
    type Msg = Increment | Decrement

module State =
    open Elmish

    let init () = { count = 0 }, Cmd.none

    let update msg state =
        match msg with
        | Increment -> { state with count = state.count + 1 }, Cmd.none
        | Decrement -> { state with count = state.count - 1 }, Cmd.none

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