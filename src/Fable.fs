module EasyBuild.Tools.Fable

open SimpleExec
open BlackFox.CommandLine

[<RequireQualifiedAccess>]
module Fable =

    type Lang =
        | JavaScript
        | TypeScript
        | Python
        | Rust
        | PHP
        | Dart

type Fable =

    static member build
        (
            ?projFileOrDir: string,
            ?outDir: string,
            ?extension: string,
            ?sourceMaps: bool,
            ?sourceMapsRoot: string,
            ?define: string list,
            ?configuration: string,
            ?verbose: bool,
            ?silent: bool,
            ?typedArrays: bool,
            ?run: string,
            ?runFast: string,
            ?runScript: bool,
            ?noRestore: bool,
            ?noCache: bool,
            ?exclude: string list,
            ?lang: Fable.Lang,
            ?workingDirectory: string
        )
        =

        let lang =
            defaultArg lang Fable.Lang.JavaScript
            |> function
                | Fable.Lang.JavaScript -> "javascript"
                | Fable.Lang.TypeScript -> "typescript"
                | Fable.Lang.Python -> "python"
                | Fable.Lang.Rust -> "rust"
                | Fable.Lang.PHP -> "php"
                | Fable.Lang.Dart -> "dart"

        Command.Run(
            "dotnet",
            CmdLine.empty
            |> CmdLine.appendRaw "fable"
            |> CmdLine.appendIfSome projFileOrDir
            |> CmdLine.appendPrefixIfSome "--outDir" outDir
            |> CmdLine.appendPrefixIfSome "--extension" extension
            |> CmdLine.apprendFlagIfSomeTrue "--sourceMaps" sourceMaps
            |> CmdLine.appendPrefixIfSome "--sourceMapsRoot" sourceMapsRoot
            |> CmdLine.appendPrefixSeqIfSome "--define" define
            |> CmdLine.appendPrefixIfSome "--configuration" configuration
            |> CmdLine.apprendFlagIfSomeTrue "--verbose" verbose
            |> CmdLine.apprendFlagIfSomeTrue "--silent" silent
            |> CmdLine.apprendFlagIfSomeTrue "--typedArrays" typedArrays
            |> CmdLine.apprendFlagIfSomeTrue "--noRestore" noRestore
            |> CmdLine.apprendFlagIfSomeTrue "--noCache" noCache
            |> CmdLine.appendPrefixSeqIfSome "--exclude" exclude
            |> CmdLine.appendPrefix "--lang" lang
            |> CmdLine.apprendFlagIfSomeTrue "--runScript" runScript
            |> CmdLine.appendRawPrefixIfSome "--run" run
            |> CmdLine.appendRawPrefixIfSome "--runFast" runFast
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )

    static member watch
        (
            ?projFileOrDir: string,
            ?outDir: string,
            ?extension: string,
            ?sourceMaps: bool,
            ?sourceMapsRoot: string,
            ?define: string list,
            ?configuration: string,
            ?verbose: bool,
            ?silent: bool,
            ?typedArrays: bool,
            ?run: string,
            ?runFast: string,
            ?runScript: bool,
            ?runWatch: string,
            ?noRestore: bool,
            ?noCache: bool,
            ?exclude: string list,
            ?lang: Fable.Lang,
            ?workingDirectory: string
        )
        =

        let lang =
            defaultArg lang Fable.Lang.JavaScript
            |> function
                | Fable.Lang.JavaScript -> "javascript"
                | Fable.Lang.TypeScript -> "typescript"
                | Fable.Lang.Python -> "python"
                | Fable.Lang.Rust -> "rust"
                | Fable.Lang.PHP -> "php"
                | Fable.Lang.Dart -> "dart"

        Command.RunAsync(
            "dotnet",
            CmdLine.empty
            |> CmdLine.appendRaw "fable watch"
            |> CmdLine.appendIfSome projFileOrDir
            |> CmdLine.appendPrefixIfSome "--outDir" outDir
            |> CmdLine.appendPrefixIfSome "--extension" extension
            |> CmdLine.apprendFlagIfSomeTrue "--sourceMaps" sourceMaps
            |> CmdLine.appendPrefixIfSome "--sourceMapsRoot" sourceMapsRoot
            |> CmdLine.appendPrefixSeqIfSome "--define" define
            |> CmdLine.appendPrefixIfSome "--configuration" configuration
            |> CmdLine.apprendFlagIfSomeTrue "--verbose" verbose
            |> CmdLine.apprendFlagIfSomeTrue "--silent" silent
            |> CmdLine.apprendFlagIfSomeTrue "--typedArrays" typedArrays
            |> CmdLine.apprendFlagIfSomeTrue "--noRestore" noRestore
            |> CmdLine.apprendFlagIfSomeTrue "--noCache" noCache
            |> CmdLine.appendPrefixSeqIfSome "--exclude" exclude
            |> CmdLine.appendPrefix "--lang" lang
            |> CmdLine.apprendFlagIfSomeTrue "--runScript" runScript
            |> CmdLine.appendRawPrefixIfSome "--run" run
            |> CmdLine.appendRawPrefixIfSome "--runFast" runFast
            |> CmdLine.appendRawPrefixIfSome "--runWatch" runWatch
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )
