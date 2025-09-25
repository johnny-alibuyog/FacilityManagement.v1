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

module State =
    open Elmish

    let delay (ms: int) = async {
        do! Async.Sleep ms
        return ()
    }

    let init () = Initializing, Cmd.OfAsync.perform delay 1000 (fun () -> Initialize)

    let update msg state =
        let callEndpoint action = async {
            // Simulate an API call delay
            do! Async.Sleep 300
            return action()
        }

        match msg, state with
        | Initialize, _ ->
            Initialized (Completed 0 |> Model.create), Cmd.none

        | RequestIncrement, Initialized model ->
            let increment () = Model.count model +  1
            let cmd = Cmd.OfAsync.perform callEndpoint increment ReceivedIncrement

            Initialized { model with count = Started }, cmd

        | ReceivedIncrement newCount, Initialized model ->
            Initialized { model with count = Completed newCount }, Cmd.none

        | RequestDecrement, Initialized model ->
            let decrement () = Model.count model - 1
            let cmd = Cmd.OfAsync.perform callEndpoint decrement ReceivedDecrement

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