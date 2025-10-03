namespace Client.Pages.Sandbox.IntegratingCommands.CounterWithDelay

[<AutoOpen>]
module Types =
    type ApiCall<'t> =
        | Started
        | Completed of 't

    type Model = {
        count: ApiCall<int>
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
            | Started -> 0
            | Completed value -> value

        let isLoading model =
            match model.count with
            | Started -> true
            | Completed _ -> false

        let display model =
            match model.count with
            | Started -> "..."
            | Completed value -> string value

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
            Initialized (Completed 0 |> Model.create), Cmd.none

        | RequestIncrement, Initialized model ->
            let count = Model.count model
            let cmd = Cmd.OfAsync.perform MockApi.increment count ReceivedIncrement

            Initialized { model with count = Started }, cmd

        | ReceivedIncrement newCount, Initialized model ->
            Initialized { model with count = Completed newCount }, Cmd.none

        | RequestDecrement, Initialized model ->
            let count = Model.count model
            let cmd = Cmd.OfAsync.perform MockApi.decrement count ReceivedDecrement

            Initialized { model with count = Started }, cmd

        | ReceivedDecrement newCount, Initialized model ->
            Initialized { model with count = Completed newCount }, Cmd.none

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
