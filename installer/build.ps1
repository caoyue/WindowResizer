$ErrorActionPreference = "Stop"
New-Alias squirrel .\packages\squirrel.windows.2.0.1\tools\Squirrel.exe

$version=$args[0]

if ($version -notmatch '\d\.\d\.\d')
{
    Write-Output "Error: Version not set correctly."
    exit 1
}

Write-Output ">> current version: ", $version

# build
Write-Output ">> building..."
dotnet restore
dotnet build
dotnet publish .\src\WindowResizer\ -o publish  /p:Version=$version

# nuget pack
Write-Output ">> packing..."
Copy-Item .\installer\AppIcon.png .\publish\AppIcon.png
Nuget pack .\installer\WindowResizer.nuspec -Version $version -Properties Configuration=Release -BasePath .\publish -OutputDirectory  .\pack

# squirrel release
Write-Output ">> releasing..."
squirrel  --setupIcon .\src\WindowResizer\Resources\AppIcon.ico --shortcut-locations 'StartMenu' --releasify .\pack\WindowResizer.$version.nupkg --no-msi

Write-Output ">> done."
