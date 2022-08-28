$ErrorActionPreference = 'Stop'
New-Alias squirrel $env:USERPROFILE\.nuget\packages\squirrel.windows\2.0.1\tools\Squirrel.exe

$version = $args[0]

if ($version -notmatch '\d\.\d\.\d') {
    Write-Output 'Error: Version not set correctly.'
    exit 1
}

Write-Host '>> current version: ', $version -ForegroundColor Green

# build
Write-Host '>> building...' -ForegroundColor Green
dotnet restore
dotnet build
dotnet publish .\src\WindowResizer\ -c Release -o publish  /p:Version=$version

# nuget pack
Write-Host '>> packing...' -ForegroundColor Green
Copy-Item .\installer\AppIcon.png .\publish\AppIcon.png
Nuget pack .\installer\WindowResizer.nuspec -Version $version -Properties Configuration=Release -BasePath .\publish -OutputDirectory  .\pack

# squirrel release
Write-Host '>> releasing...' -ForegroundColor Green
squirrel  --setupIcon .\src\WindowResizer\Resources\AppIcon.ico --shortcut-locations 'StartMenu' --releasify .\pack\WindowResizer.$version.nupkg --no-msi

Start-Sleep -Seconds 3
Write-Host 'waiting for releases files...' -ForegroundColor Green
tree .\Releases /F

Write-Host '>> done.' -ForegroundColor Green
