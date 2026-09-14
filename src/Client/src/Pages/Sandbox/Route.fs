namespace Client.Pages.Sandbox

type Route =
    | SplittingPrograms
    | IntegratingSideEffects

module Route =
    open Client.Common.Types

    let toUrl route =
        match route with
        | SplittingPrograms -> "/sandbox/splitting-programs"
        | IntegratingSideEffects -> "/sandbox/integrating-side-effects"

    let ofUrl (segments: UrlSegments) =
        match segments with
        | [ "splitting-programs" ] -> Some SplittingPrograms
        | [ "integrating-side-effects" ] -> Some IntegratingSideEffects
        | _ -> None

    let (|IsSandbox|_|) (segments: UrlSegments) =
        match segments with
        | "sandbox" :: rest -> ofUrl rest
        | _ -> None
