$ErrorActionPreference = 'Stop'

$version = $args[0]

if ($version -notmatch '\d\.\d\.\d') {
    Write-Output 'Error: Version not set correctly.'
    exit 1
}

Write-Host '>> current version: ', $version -ForegroundColor Green

# build
Write-Host '>> building...' -ForegroundColor Green
dotnet restore
dotnet publish .\src\WindowResizer.CLI\ -c Release -o publish\WindowResizer.CLI  /p:Version=$version

# release
$archive = "WindowResizer.CLI-$version.zip"
Write-Host ">> packing $archive..."

7z a .\Releases\$archive .\publish\WindowResizer.CLI\

Write-Host '>> done.' -ForegroundColor Green
