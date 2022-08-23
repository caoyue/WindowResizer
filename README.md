# WindowResizer

[![WindowResizer](https://github.com/caoyue/WindowResizer/actions/workflows/WindowsResizer.yml/badge.svg)](https://github.com/caoyue/WindowResizer/actions) [![GitHub all releases](https://img.shields.io/github/downloads/caoyue/WindowResizer/total)](https://github.com/caoyue/WindowResizer/releases)  [![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/caoyue/WindowResizer?sort=semver)](https://github.com/caoyue/WindowResizer/releases/latest)

A simple tool gives you a way to quickly reset window size and position with a hotkey.

You can save and restore window position for different process.

## require

-   Windows 7+ (x64)
-   .NET Framework 4.7.2+

## hotkeys

Change hotkeys in setting window.

-   save window position

    default hotkey: `ctrl+alt+s`

-   save all opened window position

-   restore window position

    default hotkey: `ctrl+alt+r`

    ![restore](https://i.imgur.com/5TJdL44.gif)

-   restore all opened window position

    ![restore-all.gif](https://i.imgur.com/3558lKS.gif)

## usage

-   how to add an config entry

    Focus on the window (eg. Chrome), then press the save window hotkey (`ctrl+alt+s`by default), an entry will be added to the configuration file.

-   How does the window matching work?

    The process name is matched first.
    The title is not required, by default use a wildcard `*` to match all the titles for a process name.
    And if you specify the title, it will be matched first.

-   restore window position automatically

    check the checkbox on `Auto` column.

    ![auto-restore](https://i.imgur.com/LeNyJQu.gif)

## Stats
![Alt](https://repobeats.axiom.co/api/embed/75ddcde135edf6e28a84cbe8c5fbe2b029f73c8e.svg "Repobeats analytics image")
