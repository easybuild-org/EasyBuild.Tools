module EasyBuild.Tools.Nodemon

open System.IO
open SimpleExec
open BlackFox.CommandLine
open System.Threading.Tasks

type Nodemon =

    static member runAsync
        // Arguments list taken from: npx nodemon --help options
        // begin-snippet: Nodemon.runAsync
        (
            // Configuration
            ?config: FileInfo,
            ?exitCrash: bool,
            ?ignore: string list,
            ?noColors: bool,
            ?signal: string,
            ?watch: string list,
            ?noUpdateNotifier: bool,
            // Execution
            ?onChangeOnly: bool,
            ?cwd: string,
            ?extensions: string,
            ?noStdin: bool,
            ?spawn: bool,
            ?exec: string,
            ?yourArgs: string,
            // Watching
            ?delay: string,
            ?legacyWatch: bool,
            ?pollingInterval: int,
            // Information
            ?dump: bool,
            ?verbose: bool,
            ?workingDirectory: string
        )
        : Task
        // end-snippet
        =

        let appendYourArgs (cmdLine: CmdLine) =
            match yourArgs with
            | Some args -> cmdLine |> CmdLine.appendRaw "--" |> CmdLine.appendRaw args
            | None -> cmdLine

        let appendExec (cmdLine: CmdLine) =
            match exec with
            | Some exec -> cmdLine |> CmdLine.appendRaw "--exec" |> CmdLine.appendRaw exec
            | None -> cmdLine

        let config = config |> Option.map _.FullName

        Command.RunAsync(
            "npx",
            CmdLine.empty
            // Configuration
            |> CmdLine.appendRaw "nodemon"
            |> CmdLine.appendPrefixIfSome "--config" config
            |> CmdLine.apprendFlagIfSomeTrue "--exitcrash" exitCrash
            |> CmdLine.appendPrefixSeqIfSome "--ignore" ignore
            |> CmdLine.apprendFlagIfSomeTrue "--no-colors" noColors
            |> CmdLine.appendPrefixIfSome "--signal" signal
            |> CmdLine.appendPrefixSeqIfSome "--watch" watch
            |> CmdLine.apprendFlagIfSomeTrue "--no-update-notifier" noUpdateNotifier
            // Execution
            |> CmdLine.apprendFlagIfSomeTrue "--on-change-only" onChangeOnly
            |> CmdLine.appendPrefixIfSome "--cwd" cwd
            |> CmdLine.appendPrefixIfSome "--ext" extensions
            |> CmdLine.apprendFlagIfSomeTrue "--no-stdin" noStdin
            |> CmdLine.apprendFlagIfSomeTrue "--spawn" spawn
            // Watching
            |> CmdLine.appendPrefixIfSome "--delay" delay
            |> CmdLine.apprendFlagIfSomeTrue "--legacy-watch" legacyWatch
            |> CmdLine.appendPrefixIfSome
                "--polling-interval"
                (pollingInterval |> Option.map string)
            // Information
            |> CmdLine.apprendFlagIfSomeTrue "--dump" dump
            |> CmdLine.apprendFlagIfSomeTrue "--verbose" verbose
            |> appendExec
            |> appendYourArgs
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )

    static member run
        // Arguments list taken from: npx nodemon --help options
        // begin-snippet: Nodemon.run
        (
            // Configuration
            ?config: FileInfo,
            ?exitCrash: bool,
            ?ignore: string list,
            ?noColors: bool,
            ?signal: string,
            ?watch: string list,
            ?noUpdateNotifier: bool,
            // Execution
            ?onChangeOnly: bool,
            ?cwd: string,
            ?extensions: string,
            ?noStdin: bool,
            ?spawn: bool,
            ?exec: string,
            ?yourArgs: string,
            // Watching
            ?delay: string,
            ?legacyWatch: bool,
            ?pollingInterval: int,
            // Information
            ?dump: bool,
            ?verbose: bool,
            ?workingDirectory: string
        )
        : unit
        // end-snippet
        =

        Nodemon.runAsync (
            // Configuration
            ?config = config,
            ?exitCrash = exitCrash,
            ?ignore = ignore,
            ?noColors = noColors,
            ?signal = signal,
            ?watch = watch,
            ?noUpdateNotifier = noUpdateNotifier,
            // Execution
            ?onChangeOnly = onChangeOnly,
            ?cwd = cwd,
            ?extensions = extensions,
            ?noStdin = noStdin,
            ?spawn = spawn,
            ?exec = exec,
            ?yourArgs = yourArgs,
            // Watching
            ?delay = delay,
            ?legacyWatch = legacyWatch,
            ?pollingInterval = pollingInterval,
            // Information
            ?dump = dump,
            ?verbose = verbose,
            ?workingDirectory = workingDirectory
        )
        |> Task.RunSynchronously
