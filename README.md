# WindowResizer

![WindowResizer](https://github.com/caoyue/WindowResizer/actions/workflows/WindowsResizer.yml/badge.svg)

a simple tool gives you a way to quickly reset window size and position with a hotkey.

you can save and restore window position for different process.

## support

-   .NET Framework 4.7.2

## hotkey

you can change hotkeys in setting window

-   save window position

    default hotkey: `ctrl+alt+s`

-   restore window position

    default hotkey: `ctrl+alt+r`

-   restore all opened window position

    default hotkey: `ctrl+alt+t`

## usage

-   how to add an config entry

    Focus on the window (eg. Chrome), then press the save window hotkey (ctrl+alt+s by default), an entry will be added to the configuration file.

-   How does the window matching work?

    The process name is matched first.
    The title is not required, by default use a wildcard `*` to match all the titles for a process name.
    And if you specify the title, it will be matched first.

-   restore window position automatically

    check the checkbox on `Auto` column and `Save` configs.
