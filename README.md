# WindowResizer

[![WindowResizer](https://github.com/caoyue/WindowResizer/actions/workflows/WindowsResizer.yml/badge.svg)](https://github.com/caoyue/WindowResizer/actions) [![GitHub all releases](https://img.shields.io/github/downloads/caoyue/WindowResizer/total)](https://github.com/caoyue/WindowResizer/releases)  [![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/caoyue/WindowResizer?sort=semver)](https://github.com/caoyue/WindowResizer/releases/latest)

WindowResizer is a simple tool that gives you a way to use hotkeys to quickly save and restore different window positions and sizes.

## Download
- Github Release
    > <https://github.com/caoyue/WindowResizer/releases/latest>

    require:
    -   Windows 7+ (x64)
    -   .NET Framework 4.7.2+

- Microsoft Store
    > [<img src="https://raw.githubusercontent.com/caoyue/WindowResizer/package/.github/assets/microsoft-store-badge.png" width="160" title="Get WindowResizer from Microsoft Store" alt="Get WindowResizer from Microsoft Store">](https://www.microsoft.com/store/apps/9NZ07CQ6WZMB)
    
    The Windows Store version has the same features as the GitHub release version.  
    You can support development by purchasing it on the Windows Store.

## App
### hotkeys

Change hotkeys in setting window.

-   save window position

    default hotkey: `ctrl+alt+s`

-   save all opened window position

-   restore window position

    default hotkey: `ctrl+alt+r`

    <details>
        <summary>demo</summary>
        <img src="https://i.imgur.com/5TJdL44.gif" title="restore" loading="lazy" />
    </details>


-   restore all opened window position
    <details>
        <summary>demo</summary>
        <img src="https://i.imgur.com/3558lKS.gif" title="restore all" loading="lazy" />
    </details>
    

### usage

-   how to add an config entry

    Focus on the window (eg. Chrome), then press the save window hotkey (`ctrl+alt+s`by default), an entry will be added to the configuration file.

-   How does the window matching work?

    The process name is matched first.
    The title is not required, by default use a wildcard `*` to match all the titles for a process name.
    And if you specify the title, it will be matched first.

-  Option: Resize by title
    Uncheck: Resize based on process
    Checked: Resize based on Process and title

-   `Auto resize`: restore window position automatically

    check the checkbox on `Auto` column.
    <details>
        <summary>demo</summary>
        <img src="https://i.imgur.com/LeNyJQu.gif" title="auto restore" loading="lazy" />
    </details>

-  Option: Auto resize delay
    This option is used with the `Auto Resize` feature.
    When `Auto Resize` based on process titles, some titles are not immediately determined, such as Chrome web pages.
    If this option is checked, you can set a delay(in millisecond) for the Chrome process, so when a new Chrome window is created, there will be a delay before resize takes effect.

-   portable mode
    - download portable package
    - put `WindowResizer.config.json` in the same folder as the program file `WindowResizer.exe`
## CLI
run ```WindowResizer.CLI.exe resize -h```   
> The CLI can run standalone without WindowResizer App running.

```
 __        __  _               _                      ____                 _
 \ \      / / (_)  _ __     __| |   ___   __      __ |  _ \    ___   ___  (_)  ____   ___   _ __
  \ \ /\ / /  | | | '_ \   / _` |  / _ \  \ \ /\ / / | |_) |  / _ \ / __| | | |_  /  / _ \ | '__|
   \ V  V /   | | | | | | | (_| | | (_) |  \ V  V /  |  _ <  |  __/ \__ \ | |  / /  |  __/ | |
    \_/\_/    |_| |_| |_|  \__,_|  \___/    \_/\_/   |_| \_\  \___| |___/ |_| /___|  \___| |_|


Usage:
  WindowResizer.CLI resize [options]

Options:
  -c, --config <config>    Config file path, use current config file if omitted.
  -P, --profile <profile>  Profile name, use current profile if omitted.
  -p, --process <process>  Process name, use foreground process if omitted.
  -t, --title <title>      Process title, all windows of the process will be resized if not specified.
  -v, --verbose            Show more details.
  -?, -h, --help           Show help and usage information
```

e.g.,
 
```shell
# Resize all
WindowResizer.CLI.exe resize

# Specify config file and profile
WindowResizer.CLI.exe resize -c "X:\WindowResizer.config.json" -P "my-profile"

# Show verbose
WindowResizer.CLI.exe resize -v

# Filter windows by process
WindowResizer.CLI.exe resize -p "notepad.exe"

# Filter windows by title regex
WindowResizer.CLI.exe resize -t ".*.txt" 

# Combine all options
WindowResizer.CLI.exe resize -c "X:\WindowResizer.config.json" -P "my-profile" -p "notepad.exe" -t ".*.txt" -v
```


## Build
- .NET Framework 4.7.2
- Visual Studio 2019/2022 or JetBrains Rider
- Projects
  - WindowResizer: the GUI app
  - WindowResizer.CLI: the CLI app

## Stats
![Alt](https://repobeats.axiom.co/api/embed/75ddcde135edf6e28a84cbe8c5fbe2b029f73c8e.svg "Repobeats analytics image")
