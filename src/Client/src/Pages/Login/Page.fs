namespace Client.Pages.Login.Page

open Client.Common.Extensions
module Api = Client.MockApi

type Msg = 
    | SetUsername of string
    | SetPassword of string
    | Login of ApiCall<Api.LoginRequest, Api.LoginResponse>
    | LoggedIn of Api.User

type State = {
    credentials: Api.LoginRequest
    loginStatus: ApiData<Api.LoginResponse>
}

module State = 
    open Elmish
    open Feliz.Router

    let init () = 
        { 
            credentials = {
                username = "admin"
                password = "password"
            }
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
            | Starting credentials-> 
                let cmd = Cmd.OfAsync.perform Api.login credentials Finished
                { state with loginStatus = Loading None }, Cmd.map Login cmd

            | Finished (Api.Authenticated response) ->
                { state with loginStatus = Loaded (Api.Authenticated response) }, Cmd.ofMsg (LoggedIn response)

            | Finished Api.IncorrectCredentials ->
                { state with loginStatus = Loaded Api.IncorrectCredentials }, Cmd.none

        | LoggedIn _ ->
            state, Cmd.navigate "home"

    let isLoading state =
        match state.loginStatus with
        | Loading _ -> true
        |  _ -> false

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
                                    prop.onClick (fun e -> 
                                        e.preventDefault() // prevent form submission
                                        dispatch (Login (Starting state.credentials))
                                    )
                                ]
                            ]
                        ]

                        renderLoginStatus state.loginStatus
                    ]
                ]
            ]
        ]
