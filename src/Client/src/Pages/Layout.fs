namespace Client.Pages.Layout

open Elmish
open Feliz
open Feliz.Router


[<AutoOpen>]
module Types =
    type State = {
        route: Client.Pages.Route
    }

    type Msg = 
        | NoOp
        | RouteChanged of Client.Pages.Route

module State =
    let init () = 
        let route = Router.currentPath() |> Client.Pages.Route.ofUrl
        { route = route }, Cmd.none

    let update msg state =
        match msg with
        | NoOp -> 
            state, Cmd.none
        | RouteChanged newRoute ->
            { state with route = newRoute }, Cmd.none // Handle route change as needed

module View =
    open Client.Pages

    let renderHome _ =
        Html.div [
            prop.className "p-6"
            prop.children [
                Html.h1 [
                    prop.className "text-4xl font-bold"
                    prop.text "Home Page"
                ]
                Html.p "Welcome to the home page!"
            ]
        ]

    let renderActivePage state dispatch =
        match state.route with
        | HomeRoute ->
            renderHome dispatch

        | LoginRoute route ->
            let s, _ = Login.Page.State.init ()
            Login.Page.View.render s (fun _ -> dispatch NoOp)

        | NotFound ->
            Html.p "Not found"

    let render state dispatch =
        React.router [
            router.pathMode
            router.onUrlChanged (Route.ofUrl >> RouteChanged >> dispatch)
            router.children [
                renderActivePage state dispatch
            ]
        ]