module EasyBuild.Tools.Femto

open SimpleExec
open BlackFox.CommandLine

type Femto =

    static member validate
        // begin-snippet: Femto.validate
        (?project: string, ?workingDirectory: string)
        : unit
        // end-snippet
        =
        Command.Run(
            "dotnet",
            CmdLine.empty
            |> CmdLine.appendRaw "femto --validate"
            |> CmdLine.appendIfSome project
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )
