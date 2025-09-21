namespace Client.Pages.Login.Page

[<AutoOpen>]
module Types = 
    type Credentials = {
        username: string
        password: string
    }

    type Msg = | NoOp

    type State = {
        credential: Credentials
    }

module State = 
    open Elmish

    let init () = 
        { credential = { username = ""; password = "" } }, Cmd.none

    let update msg state =
        match msg with
        | NoOp -> state, Cmd.none

module View =
    open Feliz

    let render (state: State) (dispatch: Msg -> unit) =
        Html.div [
            prop.children [
                Html.h1 [
                    prop.text "Login Page"
                ]
                Html.form [
                    prop.children [
                        Html.input [
                            prop.type' "text"
                            prop.placeholder "Username"
                            prop.value state.credential.username
                            prop.onChange (fun v -> 
                                let updatedCreds = { state.credential with username = v }
                                let updatedState = { state with credential = updatedCreds }
                                dispatch NoOp // Replace with actual dispatch if needed
                            )
                        ]
                        Html.input [
                            prop.type' "password"
                            prop.placeholder "Password"
                            prop.value state.credential.password
                            prop.onChange (fun v -> 
                                let updatedCreds = { state.credential with password = v }
                                let updatedState = { state with credential = updatedCreds }
                                dispatch NoOp // Replace with actual dispatch if needed
                            )
                        ]
                        Html.button [
                            prop.type' "submit"
                            prop.text "Login"
                        ]
                    ]
                ]
            ]
        ]
