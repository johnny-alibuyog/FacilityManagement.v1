namespace Client.Pages.Login.Page

open Client.Pages.Extentions
module Api = Client.Pages.Login.Api

[<AutoOpen>]
module Types = 

    type Credentials = Api.LoginRequest

    module Credentials = 
        let init (): Credentials = { username = ""; password = "" }

    type Authenticated = Api.Authenticated

    type Msg = 
        | SetUsername of string
        | SetPassword of string
        | Login of ApiCall<Api.LoginRequest, Api.LoginResponse>

    type State = {
        credentials: Credentials
        loginStatus: ApiData<Api.LoginResponse>
    }

module State = 
    open Elmish

    let init () = 
        { 
            credentials = Credentials.init ()
            loginStatus = NotStarted
        }, 
        Cmd.none

    let update msg state =
        match msg with
        | SetUsername username ->
            { state with credentials = { state.credentials with username = username } }, Cmd.none

        | SetPassword password ->
            { state with credentials = { state.credentials with password = password } }, Cmd.none

        | Login call ->
            match call with
            | Start request -> 
                let cmd = Cmd.OfAsync.perform Api.login request Finished
                { state with loginStatus = Loading None }, Cmd.map Login cmd

            | Finished response -> 
                { state with loginStatus = Loaded response }, Cmd.none

    let isLoading state =
        match state.loginStatus with
        | Loading _ -> true
        | NotStarted | Loaded _ -> false

module View =
    open Feliz

    let renderLoginStatus loginStatus =
        match loginStatus with
        | NotStarted -> 
            Html.none

        | Loading _ -> 
            Html.paragraph [
                prop.style [ style.color.blue ]
                prop.text "Logging in..."
            ]

        | Loaded response ->
            match response with
            | Api.IncorrectCredentials -> 
                Html.paragraph [
                    prop.style [ style.color.crimson ]
                    prop.text "Username or password is incorrect"
                ]
            
            | Api.Authenticated auth -> 
                Html.paragraph [
                    prop.style [ style.color.green ]
                    prop.text (sprintf "User has successfully logged in")
                ]

    let render (state: State) (dispatch: Msg -> unit) =
        Html.form [
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
                        Html.fieldSet [
                            prop.disabled (State.isLoading state)
                            prop.style [ 
                                style.display.flex
                                style.flexDirection.column
                                style.gap 10
                                style.padding 15
                                style.border (1, borderStyle.solid, color.lightGray)
                            ]
                            prop.children [
                                Html.legend "Credentials"
                                Html.input [
                                    prop.type' "text"
                                    prop.placeholder "Username"
                                    prop.value state.credentials.username
                                    prop.onChange (SetUsername >> dispatch)
                                ]
                                Html.input [
                                    prop.type' "password"
                                    prop.placeholder "Password"
                                    prop.value state.credentials.password
                                    prop.onChange (SetPassword >> dispatch)
                                ]
                                Html.button [
                                    prop.type' "submit"
                                    prop.text "Login"
                                    prop.onClick (fun ev -> 
                                        ev.preventDefault() // prevent form submission
                                        dispatch (Login (Start state.credentials))
                                    )
                                ]
                            ]
                        ]

                        renderLoginStatus state.loginStatus
                    ]
                ]
            ]
        ]
