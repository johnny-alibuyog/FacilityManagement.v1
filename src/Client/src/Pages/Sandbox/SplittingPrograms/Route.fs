namespace Client.Pages.Sandbox.SplittingPrograms

type Route = | CounterPage | InputPage

module Route =
    open Client.Pages.Types

    let toUrl route =
        match route with
        | CounterPage -> "sandbox/splitting-programs/counter"
        | InputPage -> "sandbox/splitting-programs/input"

    let ofUrl (segments: UrlSegments) =
        match segments with
        | [ "counter" ] -> Some CounterPage
        | [ "input"] -> Some InputPage
        | _ -> None

    let (|IsSplittingPrograms|_|) (segments: UrlSegments) =
        match segments with
        | "sandbox" :: "splitting-programs" :: rest -> ofUrl rest
        | _ -> None
