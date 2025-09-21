namespace Client.Pages

open Client.Pages
    
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