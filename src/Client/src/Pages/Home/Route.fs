namespace Client.Pages.Home

type Route = | HomeRoute

module Route =
    open Client.Common.Types

    let toUrl route =
        match route with
        | HomeRoute -> "/"

    let ofUrl (_: UrlSegments) = 
        Some HomeRoute
        // match segments with
        // | [] -> Some HomeRoute
        // | _ -> None

    let (|IsHome|_|) (segments: UrlSegments) =
        match segments with
        | [] -> Some HomeRoute
        | [ "home" ] -> Some HomeRoute
        | _ -> None
