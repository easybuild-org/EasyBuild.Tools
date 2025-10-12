module EasyBuild.Tools.FableCssModules

open System.IO
open SimpleExec
open BlackFox.CommandLine
open System.Threading.Tasks

type FableCssModules =

    static member runAsync
        // begin-snippet: FableCssModules.runAsync
        (?outFile: FileInfo, ?``internal``: bool, ?camelCase: bool, ?workingDirectory: string)
        : Task
        // end-snippet
        =
        let outFile = outFile |> Option.map _.FullName

        Command.RunAsync(
            "npx",
            CmdLine.empty
            |> CmdLine.appendRaw "fcm"
            |> CmdLine.appendPrefixIfSome "--outFile" outFile
            |> CmdLine.apprendFlagIfSomeTrue "--internal" ``internal``
            |> CmdLine.apprendFlagIfSomeTrue "--camelCase" camelCase
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )

    static member run
        // begin-snippet: FableCssModules.run
        (?outFile: FileInfo, ?``internal``: bool, ?camelCase: bool, ?workingDirectory: string)
        : unit
        // end-snippet
        =

        FableCssModules.runAsync (
            ?outFile = outFile,
            ?``internal`` = ``internal``,
            ?camelCase = camelCase,
            ?workingDirectory = workingDirectory
        )
        |> Async.AwaitTask
        |> Async.RunSynchronously
