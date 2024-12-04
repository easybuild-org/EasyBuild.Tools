module EasyBuild.Tools.FableCssModules

open SimpleExec
open BlackFox.CommandLine

type FableCssModules =

    static member runAsync
        (?outFile: string, ?``internal``: bool, ?camelCase: bool, ?workingDirectory: string)
        =

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
        (?outFile: string, ?``internal``: bool, ?camelCase: bool, ?workingDirectory: string)
        =

        FableCssModules.runAsync (
            ?outFile = outFile,
            ?``internal`` = ``internal``,
            ?camelCase = camelCase,
            ?workingDirectory = workingDirectory
        )
        |> Async.AwaitTask
        |> Async.RunSynchronously
