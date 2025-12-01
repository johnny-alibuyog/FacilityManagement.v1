namespace Client.Pages.Home.Page

module Api = Client.MockApi

type Msg = 
    | Logout
    | LoggedOut

type State = {
    user: Api.User option
}

module State =
    open Elmish

    let init user = { user = Some user }, Cmd.none

    let update msg state =
        match msg with
        | Logout -> 
            { state with user = None }, Cmd.ofMsg LoggedOut

        | LoggedOut -> 
            state, Cmd.none

module View =
    open Feliz

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
                    prop.href "#/login"
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

                match state.user with
                | Some user ->
                    renderWelcome user.username dispatch
                | None -> 
                    renderAnonymous dispatch
            ]
        ]
