namespace Client.Pages

open Client.Pages

// TODO: Learn more about routing in Feliz.Router 
//  - https://zaid-ajaj.github.io/the-elmish-book/#/chapters/scaling/modelling-nested-routes
//  - https://github.com/Zaid-Ajaj/Feliz.Router?tab=readme-ov-file
//  - https://www.compositional-it.com/news-blog/elmish-navigation-with-feliz-router/
//  - https://philectrosophy.com/blog/path-routing-with-the-safe-stack

[<RequireQualifiedAccess>]
type Route =
    | Login of Login.Route
    | Home of Home.Route
    | NotFound

module Route =
    open Client.Common.Types

    let ofUrl (segments: UrlSegments) =
        match segments with
        // Empty path maps to a concrete route (was: partial application of HomeRoute causing a function type)
        | Login.Route.IsLogin route -> Route.Login route
        | Home.Route.IsHome route -> Route.Home route
        | _ -> Route.NotFound

    let isProtected route =
        match route with
        | Route.Home _ -> true
        | _ -> false