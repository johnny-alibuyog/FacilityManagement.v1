module Client.Pages.Layout

open Feliz
open Elmish

type Model = unit

type Msg = | NoOp

let init () =
    (), Cmd.none

let update msg model =
    match msg with
    | NoOp -> model, Cmd.none

let view (model: Model) dispatch =
    Html.div [
        Html.h1 "Welcome to the Layout Page"
        Html.p "This is a sample layout using Feliz and Elmish."
    ]