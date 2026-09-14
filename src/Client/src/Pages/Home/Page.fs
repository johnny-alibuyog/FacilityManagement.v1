namespace Client.Pages.Home.Page

module Api = Client.MockApi

open Client.Common.Types
type Msg = 
    | Logout
    | LoggedOut

type State = {
    session: Session option
}

module State =
    open Elmish
    open Feliz.Router

    let init session = { session = session }, Cmd.none

    let update msg state =
        match msg with
        | Logout -> 
            state, Cmd.OfAsync.perform Api.logout () (fun _ -> LoggedOut)

        | LoggedOut -> 
            state, Cmd.navigate "login"

module View =
    open Feliz
    open Feliz.Router

    let renderWelcome username dispatch =
        Html.div [
            prop.className "mt-4"
            prop.children [
                Html.p $"Welcome, {username}!"
                Html.button [
                    prop.className "mt-4 px-4 py-2 bg-blue-500 text-white rounded"
                    prop.text "Logout"
                    prop.onClick (fun _ -> dispatch Logout)
                ]
            ]
        ]

    let renderAnonymous dispatch =
        Html.div [
            prop.className "mt-4"
            prop.children [
                Html.p "You are not logged in."
                Html.a [
                    prop.className "mt-4 text-blue-500 underline"
                    prop.text "Go to Login"
                    prop.href (Router.format "login")
                ]
            ]
        ]

    let render state dispatch =
        Html.div [
            prop.className "p-6"
            prop.children [
                Html.h1 [
                    prop.className "text-4xl font-bold"
                    prop.text "Home Page"
                ]

                match state.session with
                | Some session ->
                    renderWelcome session.username dispatch

                | None -> 
                    renderAnonymous dispatch
            ]
        ]
