namespace Client.Pages.Sandbox.IntegratingCommands.CounterWithDelay

open Client.Pages.Extensions

type Model = {
    count: ApiCall<unit, int>
}

type State = 
    | Initializing
    | Initialized of Model

type Msg = 
    | Initialize
    | RequestIncrement
    | ReceivedIncrement of int
    | RequestDecrement
    | ReceivedDecrement of int

module Model =
    let create count = { count = count }

    let count model =
        match model.count with
        | Start () -> 0
        | Finished value -> value

    let isLoading model =
        match model.count with
        | Start () -> true
        | Finished _ -> false

    let display model =
        match model.count with
        | Start () -> "..."
        | Finished value -> string value

module MockApi =
    let increment (current: int) = async {
        do! Async.Sleep 300
        return current + 1
    }

    let decrement (current: int) = async {
        do! Async.Sleep 300
        return current - 1
    }

module State =
    open Elmish

    let delay (ms: int) = async {
        do! Async.Sleep ms
        return ()
    }

    let init () = Initializing, Cmd.OfAsync.perform delay 1000 (fun () -> Initialize)

    let update msg state =

        match msg, state with
        | Initialize, _ ->
            Initialized (Finished 0 |> Model.create), Cmd.none

        | RequestIncrement, Initialized model ->
            let count = Model.count model
            let cmd = Cmd.OfAsync.perform MockApi.increment count ReceivedIncrement

            Initialized { model with count = Start () }, cmd

        | ReceivedIncrement newCount, Initialized model ->
            Initialized { model with count = Finished newCount }, Cmd.none

        | RequestDecrement, Initialized model ->
            let count = Model.count model
            let cmd = Cmd.OfAsync.perform MockApi.decrement count ReceivedDecrement

            Initialized { model with count = Start () }, cmd

        | ReceivedDecrement newCount, Initialized model ->
            Initialized { model with count = Finished newCount }, Cmd.none

        | _, _ ->
            state, Cmd.none

module View =
    open Feliz

    let render state dispatch =
        match state with
        | Initializing ->
            Html.div "Initializing ..."

        | Initialized model ->
            Html.div [
                Html.h2 "Counter with Delay"
                Html.p $"Count: {Model.display model}"
                Html.button [
                    prop.text "+"
                    prop.onClick (fun _ -> dispatch RequestIncrement)
                    prop.disabled (Model.isLoading model)
                ]
                Html.button [
                    prop.text "-"
                    prop.onClick (fun _ -> dispatch RequestDecrement)
                    prop.disabled (Model.isLoading model)
                ]
            ]
