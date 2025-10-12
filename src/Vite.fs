module EasyBuild.Tools.Vite

open System.IO
open SimpleExec
open BlackFox.CommandLine
open System.Threading.Tasks

[<RequireQualifiedAccess>]
module Vite =

    [<RequireQualifiedAccess>]
    module Build =

        [<RequireQualifiedAccess>]
        type SourceMap =
            | Bool of bool
            | Inline
            | Hidden

        [<RequireQualifiedAccess>]
        type Minifier =
            | Bool of bool
            | Terser
            | Esbuild

        [<RequireQualifiedAccess>]
        type Manifest =
            | Bool of bool
            | String of string

        [<RequireQualifiedAccess>]
        type SSRManifest =
            | Bool of bool
            | String of string

        [<RequireQualifiedAccess>]
        type LogLevel =
            | Info
            | Warn
            | Error
            | Silent
            | Custom of string

        [<RequireQualifiedAccess>]
        type Debug =
            | Bool of bool
            | Feat of string

    [<RequireQualifiedAccess>]
    module Watch =

        [<RequireQualifiedAccess>]
        type Open =
            | Bool of bool
            | String of string

        [<RequireQualifiedAccess>]
        type LogLevel =
            | Info
            | Warn
            | Error
            | Silent
            | Custom of string

        [<RequireQualifiedAccess>]
        type Debug =
            | Bool of bool
            | Feat of string

type Vite =

    static member build
        // begin-snippet: Vite.build
        (
            ?target: string,
            ?outDir: string,
            ?assetsDir: string,
            ?assetsInlineLimit: int,
            ?ssr: string,
            ?sourcemap: Vite.Build.SourceMap,
            ?minify: Vite.Build.Minifier,
            ?manifest: Vite.Build.Manifest,
            ?ssrManifest: Vite.Build.SSRManifest,
            ?emptyOutDir: bool,
            ?watch: bool,
            ?config: FileInfo,
            ?``base``: string,
            ?logLevel: Vite.Build.LogLevel,
            ?clearScreen: bool,
            ?debug: Vite.Build.Debug,
            ?filter: string,
            ?mode: string,
            ?workingDirectory: string
        )
        // end-snippet
        =

        let appendSourceMap (cmdLine: CmdLine) =
            match sourcemap with
            | Some sourceMap ->
                match sourceMap with
                | Vite.Build.SourceMap.Bool true -> cmdLine |> CmdLine.append "--sourcemap"
                | Vite.Build.SourceMap.Bool false -> cmdLine
                | Vite.Build.SourceMap.Inline -> cmdLine |> CmdLine.append "--sourcemap inline"
                | Vite.Build.SourceMap.Hidden -> cmdLine |> CmdLine.append "--sourcemap hidden"
            | None -> cmdLine

        let appendMinify (cmdLine: CmdLine) =
            match minify with
            | Some minifier ->
                match minifier with
                | Vite.Build.Minifier.Bool true -> cmdLine |> CmdLine.append "--minify"
                | Vite.Build.Minifier.Bool false -> cmdLine
                | Vite.Build.Minifier.Terser -> cmdLine |> CmdLine.appendPrefix "--minify" "terser"
                | Vite.Build.Minifier.Esbuild ->
                    cmdLine |> CmdLine.appendPrefix "--minify" "esbuild"
            | None -> cmdLine

        let appendManifest (cmdLine: CmdLine) =
            match manifest with
            | Some manifest ->
                match manifest with
                | Vite.Build.Manifest.Bool true -> cmdLine |> CmdLine.append "--manifest"
                | Vite.Build.Manifest.Bool false -> cmdLine
                | Vite.Build.Manifest.String value ->
                    cmdLine |> CmdLine.appendPrefix "--manifest" value
            | None -> cmdLine

        let appendSSRManifest (cmdLine: CmdLine) =
            match ssrManifest with
            | Some ssrManifest ->
                match ssrManifest with
                | Vite.Build.SSRManifest.Bool true -> cmdLine |> CmdLine.append "--ssrManifest"
                | Vite.Build.SSRManifest.Bool false -> cmdLine
                | Vite.Build.SSRManifest.String value ->
                    cmdLine |> CmdLine.appendPrefix "--ssrManifest" value
            | None -> cmdLine

        let appendLogLevel (cmdLine: CmdLine) =
            match logLevel with
            | Some logLevel ->
                let logLevelPrefix =
                    match logLevel with
                    | Vite.Build.LogLevel.Info -> "info"
                    | Vite.Build.LogLevel.Warn -> "warn"
                    | Vite.Build.LogLevel.Error -> "error"
                    | Vite.Build.LogLevel.Silent -> "silent"
                    | Vite.Build.LogLevel.Custom value -> value

                cmdLine |> CmdLine.appendPrefix "--logLevel" logLevelPrefix

            | None -> cmdLine

        let appendDebug (cmdLine: CmdLine) =
            match debug with
            | Some debug ->
                match debug with
                | Vite.Build.Debug.Bool true -> cmdLine |> CmdLine.append "--debug"
                | Vite.Build.Debug.Bool false -> cmdLine
                | Vite.Build.Debug.Feat value -> cmdLine |> CmdLine.appendPrefix "--debug" value
            | None -> cmdLine

        let config = config |> Option.map _.FullName

        Command.Run(
            "npx",
            CmdLine.empty
            |> CmdLine.appendRaw "vite build"
            |> CmdLine.appendPrefixIfSome "-t" target
            |> CmdLine.appendPrefixIfSome "--outDir" outDir
            |> CmdLine.appendPrefixIfSome "--assetsDir" assetsDir
            |> CmdLine.appendPrefixIfSome
                "--assetsInlineLimit"
                (assetsInlineLimit |> Option.map string)
            |> CmdLine.appendPrefixIfSome "--ssr" ssr
            |> appendSourceMap
            |> appendMinify
            |> appendManifest
            |> appendSSRManifest
            |> CmdLine.apprendFlagIfSomeTrue "--emptyOutDir" emptyOutDir
            |> CmdLine.apprendFlagIfSomeTrue "--watch" watch
            |> CmdLine.appendPrefixIfSome "--config" config
            |> CmdLine.appendPrefixIfSome "--base" ``base``
            |> appendLogLevel
            |> CmdLine.apprendFlagIfSomeTrue "--clearScreen" clearScreen
            |> appendDebug
            |> CmdLine.appendPrefixIfSome "--filter" filter
            |> CmdLine.appendPrefixIfSome "--mode" mode
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )

    static member watch
        // begin-snippet: Vite.watch
        (
            ?host: string,
            ?port: int,
            ?``open``: Vite.Watch.Open,
            ?cors: bool,
            ?strictPort: bool,
            ?force: bool,
            ?config: FileInfo,
            ?``base``: string,
            ?logLevel: Vite.Watch.LogLevel,
            ?clearScreen: bool,
            ?debug: Vite.Watch.Debug,
            ?filter: string,
            ?mode: string,
            ?workingDirectory: string
        )
        : Task
        // end-snippet
        =

        let appendOpen (cmdLine: CmdLine) =
            match ``open`` with
            | Some openValue ->
                match openValue with
                | Vite.Watch.Open.Bool true -> cmdLine |> CmdLine.append "--open"
                | Vite.Watch.Open.Bool false -> cmdLine
                | Vite.Watch.Open.String value -> cmdLine |> CmdLine.appendPrefix "--open" value
            | None -> cmdLine

        let appendLogLevel (cmdLine: CmdLine) =
            match logLevel with
            | Some logLevel ->
                let logLevelPrefix =
                    match logLevel with
                    | Vite.Watch.LogLevel.Info -> "info"
                    | Vite.Watch.LogLevel.Warn -> "warn"
                    | Vite.Watch.LogLevel.Error -> "error"
                    | Vite.Watch.LogLevel.Silent -> "silent"
                    | Vite.Watch.LogLevel.Custom value -> value

                cmdLine |> CmdLine.appendPrefix "--logLevel" logLevelPrefix

            | None -> cmdLine

        let appendDebug (cmdLine: CmdLine) =
            match debug with
            | Some debug ->
                match debug with
                | Vite.Watch.Debug.Bool true -> cmdLine |> CmdLine.append "--debug"
                | Vite.Watch.Debug.Bool false -> cmdLine
                | Vite.Watch.Debug.Feat value -> cmdLine |> CmdLine.appendPrefix "--debug" value
            | None -> cmdLine

        let config = config |> Option.map _.FullName

        Command.RunAsync(
            "npx",
            CmdLine.empty
            |> CmdLine.appendRaw "vite"
            |> CmdLine.appendPrefixIfSome "--host" host
            |> CmdLine.appendPrefixIfSome "--port" (port |> Option.map string)
            |> appendOpen
            |> CmdLine.apprendFlagIfSomeTrue "--cors" cors
            |> CmdLine.apprendFlagIfSomeTrue "--strictPort" strictPort
            |> CmdLine.apprendFlagIfSomeTrue "--force" force
            |> CmdLine.appendPrefixIfSome "--config" config
            |> CmdLine.appendPrefixIfSome "--base" ``base``
            |> appendLogLevel
            |> CmdLine.apprendFlagIfSomeTrue "--clearScreen" clearScreen
            |> appendDebug
            |> CmdLine.appendPrefixIfSome "--filter" filter
            |> CmdLine.appendPrefixIfSome "--mode" mode
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )
