
IF (-NOT (Test-Path ".nuget\nuget.exe"))
{
    # Install NuGet
    MKDIR ".nuget"
    Invoke-WebRequest "https://www.nuget.org/nuget.exe" -OutFile ".nuget\nuget.exe" -UseBasicParsing 
}

& ".\.nuget\nuget.exe" update -self

& ".\.nuget\NuGet.exe" install FAKE -OutputDirectory "packages" -ExcludeVersion -Version 4.50.0

& ".\packages\FAKE\tools\FAKE.exe" build.fsx ALL