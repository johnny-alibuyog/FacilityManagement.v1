namespace Client.Pages

open Client.Pages

// TODO: Learn more about routing in Feliz.Router 
//  - https://zaid-ajaj.github.io/the-elmish-book/#/chapters/scaling/modelling-nested-routes
//  - https://github.com/Zaid-Ajaj/Feliz.Router?tab=readme-ov-file
//  - https://www.compositional-it.com/news-blog/elmish-navigation-with-feliz-router/
//  - https://philectrosophy.com/blog/path-routing-with-the-safe-stack

type Route =
    | HomeRoute
    | LoginRoute of Login.Route
    | NotFound

module Route =
    open Client.Pages.Types

    let ofUrl (segments: UrlSegments) =
        match segments with
        | [] -> HomeRoute
        | Login.Route.IsLogin route-> LoginRoute route
        | _ -> NotFound