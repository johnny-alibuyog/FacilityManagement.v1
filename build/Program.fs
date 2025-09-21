module Build.Program

open Fake.Core
open Fake.IO
// open Farmer
// open Farmer.Builders

open Build.Helpers

initializeContext ()

let getAbsolute path = Path.getFullName path

let path = {|
    root = getAbsolute "../"
    client = getAbsolute "../src/Client"
|}

Target.create "NpmInstall" (fun _ -> 
    run npm [ "ci" ] path.client
)


Target.create "Clean" (fun _ ->
    run dotnet [ "fable"; "clean"; "--yes" ] path.client 
)

Target.create "Build" (fun _ -> 
    run dotnet [ "build"; "Application.sln" ] path.root
)

Target.create "Run" (fun _ ->
    [ "client", dotnet [ "fable"; "watch"; "-o"; "output"; "-s"; "--run"; "npx"; "vite"; ] path.client ]
    |> runParallel
)

Target.create "Bundle" (fun _ ->
    run dotnet [ "fable"; "-o"; "output"; "-s"; "--run"; "npx"; "vite"; "build" ] path.client
)

open Fake.Core.TargetOperators

let dependencies = [
    "Clean" ==> "NpmInstall" ==> "Build" ==> "Run"
    "Clean" ==> "NpmInstall" ==> "Build" ==> "Bundle"
]

[<EntryPoint>]
let main args = 
    args
    |> Array.fold (fun acc current -> acc + " " + current) ""
    |> printf "Running build with args: %s"

    runOrDefault args