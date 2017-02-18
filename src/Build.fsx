#I @"packages/FAKE/tools"
#r "FakeLib.dll"

open Fake

// Properties
let root = @".\"
let solutionName = "Hoverfly.Core.sln"
let solutionPath = root @@ solutionName
let outDir = getBuildParamOrDefault "outputDir" "./out/"

//--------------------------------------------------------------------
// Restore all packages

open Fake.RestorePackageHelper
Target "RestorePackages" (fun _ -> 
     solutionPath
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             OutputPath = root + "packages"
             Retries = 4 })
 )

//--------------------------------------------------------------------
// Clean build folder.

Target "Clean" (fun _ ->
  CleanDir outDir
)

//--------------------------------------------------------------------
// Build Release

Target "Build" <| fun _ ->
    !! solutionPath
    |> MSBuildRelease "" "Rebuild"
    |> ignore

Target "BuildRelease" DoNothing

//--------------------------------------------------------------------------------
// Copy the build output to bin directory

Target "CopyOutput" <| fun _ ->
    
    let copyOutput project =
        let src = root @@ project @@ @"bin/Release/"
        let dst = outDir @@ project
        CopyDir dst src allFiles
    [ "Hoverfly.Core" ]
    |> List.iter copyOutput

//--------------------------------------------------------------------
// Target Help

Target "Help" (fun _ ->
   trace "build [target]"
)

//--------------------------------------------------------------------
// Target dependencies

// build dependencies
"Clean" ==> "RestorePackages" ==> "Build" ==> "CopyOutput" ==> "BuildRelease"

Target "All" DoNothing
"BuildRelease" ==> "All"

// start build
RunTargetOrDefault "Help"