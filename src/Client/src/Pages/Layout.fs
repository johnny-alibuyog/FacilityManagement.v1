namespace Client.Pages.Layout

open Client.Pages
open Client.Common.Types
open Feliz
open Feliz.Router

module Api = Client.MockApi

[<RequireQualifiedAccess>]
type Page =
    | Loading
    | Login of Login.Page.State
    | Home of Home.Page.State
    | Sandbox of Sandbox.Layout.State
    | NotFound

type State = {
    currentPage: Page
    session: Session option
    returnUrl: string option
}

[<RequireQualifiedAccess>]
type Msg = 
    | RouteChanged of Client.Pages.Route
    | LoginMsg of Login.Page.Msg
    | HomeMsg of Home.Page.Msg
    | SandboxMsg of Sandbox.Layout.Msg

module State =
    open Elmish

    let init () = 
        let route = Router.currentPath() |> Route.ofUrl
        { currentPage = Page.Loading; session = None; returnUrl = None }, Cmd.ofMsg (Msg.RouteChanged route)

    let update msg state =

        let handlRoute route =
            match route with
            | Route.Login loginRoute ->
                let loginState, loginCmd = Login.Page.State.init ()
                { state with currentPage = Page.Login loginState }, Cmd.map Msg.LoginMsg loginCmd

            | Route.Sandbox sandboxRoute ->
                let sandboxState, sandboxCmd = Sandbox.Layout.State.init sandboxRoute
                { state with currentPage = Page.Sandbox sandboxState }, Cmd.map Msg.SandboxMsg sandboxCmd

            | Route.Home homeRoute ->
                let homeState, homeCmd = Home.Page.State.init state.session
                { state with currentPage = Page.Home homeState }, Cmd.map Msg.HomeMsg homeCmd

            | Route.NotFound ->
                { state with currentPage = Page.NotFound }, Cmd.none

        match msg, state.currentPage with
        | Msg.RouteChanged route, _ ->
            if Route.isProtected route && state.session.IsNone then
                let returnUrl = Router.currentPath() |> String.concat "/" |> Some
                let loginState, loginCmd = Login.Page.State.init ()
                { state with currentPage = Page.Login loginState; returnUrl = returnUrl }, Cmd.map Msg.LoginMsg loginCmd
            else
                handlRoute route

        | Msg.LoginMsg (Login.Page.Msg.LoggedIn user), Page.Login loginState ->
            let target = state.returnUrl |> Option.defaultValue "/"
            let session = Some { username = user.username; token = "sometoken" }
            { state with session = session; returnUrl = None }, Cmd.navigate target

        | Msg.LoginMsg loginMsg, Page.Login loginState ->
            let loginState, loginCmd = Login.Page.State.update loginMsg loginState
            { state with currentPage = Page.Login loginState }, Cmd.map Msg.LoginMsg loginCmd

        | Msg.HomeMsg homeMsg, Page.Home homeState ->
            let homeState, homeCmd = Home.Page.State.update homeMsg homeState
            let session = 
                match homeMsg with
                | Home.Page.Msg.LoggedOut -> None
                | _ -> state.session
            { state with currentPage = Page.Home homeState; session = session }, Cmd.map Msg.HomeMsg homeCmd

        | Msg.SandboxMsg sandboxMsg, Page.Sandbox sandboxState ->
            let sandboxState, sandboxCmd = Sandbox.Layout.State.update sandboxMsg sandboxState
            { state with currentPage = Page.Sandbox sandboxState }, Cmd.map Msg.SandboxMsg sandboxCmd

        | _, _ ->
            state, Cmd.none // Ignore messages not relevant to the current page

module View =
    let renderActivePage state dispatch =
        match state.currentPage with
        | Page.Login loginState ->
            Login.Page.View.render loginState (Msg.LoginMsg >> dispatch)
        
        | Page.Home homeState ->
            Home.Page.View.render homeState (Msg.HomeMsg >> dispatch)

        | Page.Sandbox sandboxState ->
            Sandbox.Layout.View.render sandboxState (Msg.SandboxMsg >> dispatch)
        
        | Page.NotFound ->
            Html.p "Not found"

        | Page.Loading ->
            Html.p "Loading..."

    let render state dispatch =
        React.router [
            router.onUrlChanged (Route.ofUrl >> Msg.RouteChanged >> dispatch)
            router.children [
                renderActivePage state dispatch
            ]
        ]
