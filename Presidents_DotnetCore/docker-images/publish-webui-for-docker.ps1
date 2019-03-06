$userProfilePath = $env:USERPROFILE
$nugetPackagePath = "$userProfilePath\.nuget\packages"
$efDllPath = "$nugetPackagePath\microsoft.entityframeworkcore.tools.dotnet\1.0.1\tools\netcoreapp1.0\ef.dll"

if (-Not(Test-Path $efDllPath))
{
	Write-Output "Could not locate ef.dll at $efDllPath"
	throw "Could not locate ef.dll"
}

Write-Output "Path to ef.dll: $efDllPath"

$publishPath = "$PWD\webui\published-for-docker"

if (Test-Path $publishPath)
{
    Remove-Item $publishPath -Force -Recurse
}

dotnet restore ..\src\Benday.Presidents.WebUi
dotnet build ..\src\Benday.Presidents.WebUi
dotnet publish ..\src\Benday.Presidents.WebUi -o $publishPath

Copy-Item $efDllPath $publishPath
