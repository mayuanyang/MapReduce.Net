$configFiles = Get-ChildItem . *.csproj -rec
$versionString = "<PackageVersion>" + $env:APPVEYOR_BUILD_VERSION + "-dev</PackageVersion>"
foreach ($file in $configFiles)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "<PackageVersion>0.0.1</PackageVersion>", $versionString  } |
    Set-Content $file.PSPath
}
