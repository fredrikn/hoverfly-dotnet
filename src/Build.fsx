#I @"packages/FAKE/tools"
#r "FakeLib.dll"

open System
open System.IO
open Fake
open Fake.FileUtils
open Fake.TaskRunnerHelper


//--------------------------------------------------------------------------------
// Information about the project for Nuget and Assembly info files
//--------------------------------------------------------------------------------

let product = "Hoverfly-DotNet"
let authors = [ "Fredrik Normen" ]
let copyright = "Copyright © 2017"
let company = "Fredrik Normen"
let description = "A .Net Library for Hoverfly"

// Read release notes and version
let release =
    File.ReadLines "RELEASE_NOTES.md"
    |> ReleaseNotesHelper.parseReleaseNotes

let envBuildNumber = System.Environment.GetEnvironmentVariable("BUILD_NUMBER")
let buildNumber = if String.IsNullOrWhiteSpace(envBuildNumber) then "0" else envBuildNumber

let version = release.AssemblyVersion + "." + buildNumber

let root = @".\"
let projectName = "Hoverfly.Core"
let solutionName = "Hoverfly.Core.sln"
let solutionPath = root @@ solutionName
let outDir = getBuildParamOrDefault "outputDir" "./out/"
let testOutput = FullName "TestResults"

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

//--------------------------------------------------------------------------------
// Generate AssemblyInfo files with the version for release notes 

open AssemblyInfoFile

Target "AssemblyInfo" <| fun _ ->
    CreateCSharpAssemblyInfoWithConfig (root @@ projectName @@ "Properties/AssemblyInfo.cs") [
        Attribute.Title product
        Attribute.Company company
        Attribute.Description description
        Attribute.Copyright copyright
        Attribute.Trademark ""
        Attribute.Version version
        Attribute.ComVisible false
        Attribute.FileVersion version ] <| AssemblyInfoFileConfig(false)

//--------------------------------------------------------------------
// Build Release

Target "Build" <| fun _ ->
    !! solutionPath
    |> MSBuildRelease "" "Rebuild"
    |> ignore

Target "BuildRelease" DoNothing

//--------------------------------------------------------------------------------
// Copy the build output to out directory

Target "CopyOutput" <| fun _ ->
    
    let copyOutput project =
        let src = root @@ project @@ @"bin/Release/"
        let dst = outDir @@ project
        CopyDir dst src allFiles
    [ projectName ]
    |> List.iter copyOutput

//--------------------------------------------------------------------------------
// Clean test output

Target "CleanTests" <| fun _ ->
    DeleteDir testOutput

//--------------------------------------------------------------------------------
// Run tests

open Fake.Testing
Target "RunTests" <| fun _ ->  
    let testAssemblies = !! (root @@ "**/bin/Release/Hoverfly.Core.Tests.dll")

    mkdir testOutput
    let xunitToolPath = findToolInSubPath "xunit.console.exe" (root @@ "packages/xunit.runner.console*/tools")

    printfn "Using XUnit runner: %s" xunitToolPath
    xUnit2
        (fun p -> { p with XmlOutputPath = Some (testOutput + @"\XUnitTestResults.xml"); HtmlOutputPath = Some (testOutput + @"\XUnitTestResults.HTML"); ToolPath = xunitToolPath; TimeOut = System.TimeSpan.FromMinutes 30.0; Parallel = ParallelMode.NoParallelization })
        testAssemblies

//------------------------------------------------------------------------------
// Create a Nuget Package

Target "CreatePackage" (fun _ ->
    let workingDir = (root @@ projectName @@ "bin/Release")
    let packages = (root @@ projectName @@ "packages.config")
    let packageDependencies = if (fileExists packages) then (getDependencies packages) else []

    let tempBuildDir = workingDir + "ForNuGet"
    ensureDirectory tempBuildDir

    CleanDir tempBuildDir

    let pack packSourceDir =
        NuGet (fun p -> 
        {p with
           Authors = authors
           Project = product
           Description = description
           OutputPath = outDir
           WorkingDir = packSourceDir
           Properties = ["Configuration", "Release"]
           ReleaseNotes = release.Notes |> String.concat "\n"
           Version = release.NugetVersion
           Dependencies = packageDependencies
           Publish = false }) 
           (root @@ projectName @@ "Hoverfly.Core.nuspec")

     // Copy dll, pdb and xml to libdir = workingDir/lib/net45/
    let tempLibDir = tempBuildDir @@ @"lib\net45\"
    ensureDirectory tempLibDir
    !! (workingDir @@ projectName + ".dll")
    ++ (workingDir @@ projectName + ".pdb")
    ++ (workingDir @@ projectName + ".xml")
    |> CopyFiles tempLibDir

    pack tempBuildDir
)

//--------------------------------------------------------------------
// Target Help

Target "Help" (fun _ ->
   trace "build [target]"
)

//--------------------------------------------------------------------
// Target dependencies

// build dependencies
"Clean" ==> "RestorePackages" ==> "AssemblyInfo" ==> "Build" ==> "CopyOutput" ==> "BuildRelease"

// tests dependencies
"CleanTests" ==> "RunTests"

// nuget dependencies
"BuildRelease" ==> "CreatePackage"

Target "All" DoNothing
"BuildRelease" ==> "All"
"RunTests" ==> "All"
"CreatePackage" ==> "All"

// start build
RunTargetOrDefault "Help"