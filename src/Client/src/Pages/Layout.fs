// namespace Client.Pages.Layout

// open Client.Pages
// open Elmish
// open Feliz
// open Feliz.Router

// [<RequireQualifiedAccess>]
// type Page =
//     | Login of Login.Page.State
//     | Home of Home.Page.State
//     | NotFound

// type State = {
//     route: Client.Pages.Route
// }

// type Msg = 
//     | NoOp
//     | RouteChanged of Client.Pages.Route

// module State =
//     let init () = 
//         let route = Router.currentPath() |> Route.ofUrl
//         { route = route }, Cmd.none

//     let update msg state =
//         match msg with
//         | NoOp -> 
//             state, Cmd.none
//         | RouteChanged newRoute ->
//             { state with route = newRoute }, Cmd.none // Handle route change as needed

// module View =
//     let renderActivePage state dispatch =
//         match state.route with
//         | Route.Home route ->
//             let state', cmd = Home.Page.State.init ()
//             Home.Page.View.render state' (fun _ -> dispatch NoOp)

//         | Route.Login route ->
//             let state', cmd = Login.Page.State.init ()
//             Login.Page.View.render state' (fun _ -> dispatch NoOp)

//         | Route.NotFound ->
//             Html.p "Not found"

//     let render state dispatch =
//         React.router [
//             router.pathMode
//             router.onUrlChanged (Route.ofUrl >> RouteChanged >> dispatch)
//             router.children [
//                 renderActivePage state dispatch
//             ]
//         ]


namespace Client.Pages.Layout

open Client.Pages
open Elmish
open Feliz
open Client.Pages.Extensions

module Api = Client.MockApi

[<RequireQualifiedAccess>]
type Page =
    | Login of Login.Page.State
    | Home of Home.Page.State
    | NotFound

type Credential =
    | NotAuthenticated
    | Authenticated of Api.User

type State = {
    currentPage: Page
    credential: Credential
}

[<RequireQualifiedAccess>]
type Msg = 
    | Login of Login.Page.Msg
    | Home of Home.Page.Msg

module State =
    let init () = 
        let loginState, loginMsg = Login.Page.State.init ()
        { currentPage = Page.Login loginState; credential = NotAuthenticated }, Cmd.map Msg.Login loginMsg

    let update msg state =
        match msg, state.currentPage with
        | Msg.Login loginMsg, Page.Login loginState ->
            match loginMsg with
            | Login.Page.Msg.LoggedIn user->
                let homeState, homeCmd = Home.Page.State.init user
                { state with currentPage = Page.Home homeState; credential = Authenticated user }, Cmd.map Msg.Home homeCmd
            | _ ->
                let newLoginState, loginCmd = Login.Page.State.update loginMsg loginState
                { state with currentPage = Page.Login newLoginState }, Cmd.map Msg.Login loginCmd
        | Msg.Home homeMsg, Page.Home homeState ->
            match homeMsg with
            | Home.Page.Msg.Logout ->
                let loginState, loginCmd = Login.Page.State.init ()
                { state with currentPage = Page.Login loginState; credential = NotAuthenticated }, Cmd.map Msg.Login loginCmd
            | _ ->
                let newHomeState, homeCmd = Home.Page.State.update homeMsg homeState
                { state with currentPage = Page.Home newHomeState }, Cmd.map Msg.Home homeCmd
        | _, _ ->
            state, Cmd.none // Ignore messages not relevant to the current page          

module View =
    let render state dispatch =
        match state.currentPage with
        | Page.Login loginState ->
            Login.Page.View.render loginState (Msg.Login >> dispatch)
        
        | Page.Home homeState ->
            Home.Page.View.render homeState (Msg.Home >> dispatch)
        
        | Page.NotFound ->
            Html.p "Not found"
