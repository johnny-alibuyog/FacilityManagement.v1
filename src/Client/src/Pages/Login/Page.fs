namespace Client.Pages.Login.Page

[<AutoOpen>]
module Types = 
    type Credentials = {
        username: string
        password: string
    }

    type Msg = 
        | Loaded
        | SetUsername of string
        | SetPassword of string
        | Submit

    type State = {
        credential: Credentials
    }

module State = 
    open Elmish

    let init () = 
        { credential = { username = "sadrfqwerr"; password = "dfdfdf" } }, Cmd.none

    let update msg state =
        match msg with
        | Loaded -> 
            state, Cmd.none
        
        | SetUsername username ->
            { state with credential = { state.credential with username = username } }, Cmd.none

        | SetPassword password ->
            { state with credential = { state.credential with password = password } }, Cmd.none

        | Submit ->
            // Handle login submission logic here
            state, Cmd.none

module View =
    open Feliz

    let render (state: State) (dispatch: Msg -> unit) =
        Html.div [
            prop.children [
                Html.h1 [
                    prop.text "Login Page"
                ]
                Html.form [
                    prop.style [ 
                        style.display.flex; 
                        style.flexDirection.column; 
                        style.gap 10 
                    ]
                    prop.children [
                        Html.input [
                            prop.type' "text"
                            prop.placeholder "Username"
                            prop.value state.credential.username
                            prop.onChange (SetUsername >> dispatch)
                        ]
                        Html.input [
                            prop.type' "password"
                            prop.placeholder "Password"
                            prop.value state.credential.password
                            prop.onChange (SetPassword >> dispatch)
                        ]
                        Html.button [
                            prop.type' "submit"
                            prop.text "Login"
                        ]
                    ]
                ]
            ]
        ]
