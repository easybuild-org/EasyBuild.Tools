module EasyBuild.Tools.Femto

open System.IO
open SimpleExec
open BlackFox.CommandLine

type Femto =

    static member validate
        // begin-snippet: Femto.validate
        (?projectFile: FileInfo, ?workingDirectory: string)
        : unit
        // end-snippet
        =
        let projectFile = projectFile |> Option.map _.FullName

        Command.Run(
            "dotnet",
            CmdLine.empty
            |> CmdLine.appendRaw "femto --validate"
            |> CmdLine.appendIfSome projectFile
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )
