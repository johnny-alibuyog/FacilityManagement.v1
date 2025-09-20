module Build.Program

open Fake.Core
open Fake.IO
// open Farmer
// open Farmer.Builders

open Build.Helpers

initializeContext ()

let path = {|
    solution = Path.getFullName "../Application.sln"
    client = Path.getFullName "../src/Client"
|}

Target.create "Clean" (fun _ ->
    run dotnet [ "fable"; "clean"; "--yes" ] path.client 
)

Target.create "Build" (fun _ -> 
    run dotnet [ "build"; "Application.sln" ] path.solution
)

Target.create "Run" (fun _ ->
    [ "client", dotnet [ "fable"; "watch"; ] path.client ]
    |> runParallel
)

[<EntryPoint>]
let main args = 
    args
    |> Array.fold (fun acc current -> acc + " " + current) ""
    |> printf "Running build with args: %s"

    runOrDefault args