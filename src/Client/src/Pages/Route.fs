namespace Client.Pages

open Client.Pages

// Route matching is centralized here to keep page modules focused on local concerns.

[<RequireQualifiedAccess>]
type Route =
    | Login of Login.Route
    | Home of Home.Route
    | Sandbox of Sandbox.Route
    | NotFound

module Route =
    open Client.Common.Types

    let ofUrl (segments: UrlSegments) =
        match segments with
        // Empty path maps to a concrete route (was: partial application of HomeRoute causing a function type)
        | Sandbox.Route.IsSandbox route -> Route.Sandbox route
        | Login.Route.IsLogin route -> Route.Login route
        | Home.Route.IsHome route -> Route.Home route
        | _ -> Route.NotFound

    let isProtected route =
        match route with
        | Route.Home _ -> true
        | _ -> false
