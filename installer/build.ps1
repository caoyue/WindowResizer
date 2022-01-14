$ErrorActionPreference = "Stop"
New-Alias squirrel .\packages\squirrel.windows.2.0.1\tools\Squirrel.exe

$version=$args[0]

if ($version -notmatch '\d\.\d\.\d')
{
    Write-Output "Error: Version not set correctly."
    exit 1
}

function patchAssembly($patchVersion)
{
    $pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]$'
    $AssemblyFiles = Get-ChildItem . AssemblyInfo.cs -rec

    foreach ($file in $AssemblyFiles)
    {
        (Get-Content $file.PSPath) | ForEach-Object{
            if($_ -match $pattern)
            {
                '[assembly: AssemblyVersion("{0}")]' -f $patchVersion
            } else
            {
                $_
            }
        } | Set-Content $file.PSPath
    }
}

function patchFile($patchVersion)
{
    $pattern = '^\[assembly: AssemblyFileVersion\("(.*)"\)\]$'
    $AssemblyFiles = Get-ChildItem . AssemblyInfo.cs -rec

    foreach ($file in $AssemblyFiles)
    {
        (Get-Content $file.PSPath) | ForEach-Object{
            if($_ -match $pattern)
            {
                '[assembly: AssemblyFileVersion("{0}")]' -f $patchVersion
            } else
            {
                $_
            }
        } | Set-Content $file.PSPath
    }
}

Write-Output ">> current version: ", $version

# assembly info
Write-Output ">> patching assembly info..."
patchAssembly($version)
patchFile($version)

# build
Write-Output ">> building..."
nuget restore .\WindowResizer.sln
msbuild .\src\WindowResizer\WindowResizer.csproj /p:Configuration=Release

# nuget pack
Write-Output ">> packing..."
Copy-Item .\installer\AppIcon.png .\src\WindowResizer\bin\Release\AppIcon.png
Nuget pack .\installer\WindowResizer.nuspec -Version $version -Properties Configuration=Release -BasePath .\src\WindowResizer\bin\Release -OutputDirectory  .\publish

# squirrel release
Write-Output ">> releasing..."
squirrel  --setupIcon .\src\WindowResizer\Resources\AppIcon.ico --shortcut-locations 'StartMenu' --releasify .\publish\WindowResizer.$version.nupkg --no-msi

Write-Output ">> done."
