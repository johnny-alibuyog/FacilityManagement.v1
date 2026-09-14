namespace Client.Pages.Login

type Route = | LoginPage

module Route =
    open Client.Common.Types

    let toUrl route =
        match route with
        | LoginPage -> "/login"

    let ofUrl (_: UrlSegments) =
        Some LoginPage
        // match segments with
        // | [ "login" ] -> Some LoginPage
        // | _ -> None

    let (|IsLogin|_|) (segments: UrlSegments) =
        match segments with
        | "login" :: rest -> ofUrl rest
        | _ -> None
