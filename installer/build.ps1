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

$publishFolder = 'publish\WindowResizer'
if (Test-Path $publishFolder) {

    Write-Host '>> publish folder not empty, delete existing files...'
    Remove-Item $publishFolder -Recurse -Force
}

dotnet restore
dotnet publish .\src\WindowResizer\ -c Release -o $publishFolder  /p:Version=$version

# nuget pack
Write-Host '>> packing...' -ForegroundColor Green
Copy-Item .\installer\AppIcon.png .\publish\WindowResizer\AppIcon.png
dotnet nuget pack .\installer\WindowResizer.nuspec -Version $version -Properties Configuration=Release -BasePath .\publish\WindowResizer -OutputDirectory  .\pack

# squirrel release
Write-Host '>> releasing...' -ForegroundColor Green
squirrel  --setupIcon .\src\WindowResizer\Resources\AppIcon.ico --shortcut-locations 'StartMenu' --releasify .\pack\WindowResizer.$version.nupkg --no-msi

# portable release
Write-Host '>> portable package releasing...' -ForegroundColor Green
$archive = "WindowResizer-portable-$version.zip"
Copy-Item .\installer\WindowResizer.config.json .\publish\WindowResizer\WindowResizer.config.json
7z a .\Releases\$archive .\publish\WindowResizer\

Start-Sleep -Seconds 3
Write-Host 'waiting for releases files...' -ForegroundColor Green
tree .\Releases /F

Write-Host '>> done.' -ForegroundColor Green
