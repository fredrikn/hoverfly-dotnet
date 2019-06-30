param (
    [string]$target = "All"
)

Add-Type -AssemblyName System.IO.Compression.FileSystem

function Unzip
{
    param([string]$zipfile, [string]$outpath)
    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}


IF (-NOT (Test-Path ".nuget\nuget.exe"))
{
    # Install NuGet
    MKDIR ".nuget"
    Invoke-WebRequest "https://www.nuget.org/nuget.exe" -OutFile ".nuget\nuget.exe" -UseBasicParsing 
}

IF (-NOT (Test-Path ".hoverfly\amd64\hoverfly.exe"))
{
    # Install Hoverfly v 1.0.1 64 bit
    MKDIR ".hoverfly\amd64"
    Invoke-WebRequest "https://github.com/SpectoLabs/hoverfly/releases/download/v1.0.1/hoverfly_bundle_windows_amd64.zip" -OutFile ".hoverfly\amd64\hoverfly_bundle_windows_amd64.zip" -UseBasicParsing
    Unzip (Join-Path -Path $PSScriptRoot -ChildPath ".hoverfly\amd64\hoverfly_bundle_windows_amd64.zip") ".hoverfly\amd64"
}

IF (-NOT (Test-Path ".hoverfly\386\hoverfly.exe"))
{
    # Install Hoverfly v 1.0.1 32 bit
    MKDIR ".hoverfly\386"
    Invoke-WebRequest "https://github.com/SpectoLabs/hoverfly/releases/download/v1.0.1/hoverfly_bundle_windows_386.zip" -OutFile ".hoverfly\386\hoverfly_bundle_windows_386.zip" -UseBasicParsing 
    Unzip (Join-Path -Path $PSScriptRoot -ChildPath ".hoverfly\386\hoverfly_bundle_windows_386.zip") ".hoverfly\386"
}

& ".\.nuget\nuget.exe" update -self

& ".\.nuget\NuGet.exe" install FAKE -OutputDirectory "packages" -ExcludeVersion -Version 4.64.17

& ".\.nuget/NuGet.exe" install xunit.runner.console -OutputDirectory "packages\FAKE" -ExcludeVersion -Version 2.4.1

& ".\packages\FAKE\tools\FAKE.exe" build.fsx $target