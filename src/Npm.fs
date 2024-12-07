module EasyBuild.Tools.Npm

open SimpleExec
open System.IO
open BlackFox.CommandLine

type Npm =

    static member publish
        // begin-snippet: Npm.publish
        (projectDir: string, ?tag: string, ?isRestricted: bool)
        // end-snippet
        =
        let access =
            match isRestricted with
            | Some true -> "restricted"
            | Some false
            | None -> "public"

        Command.Run(
            "npm",
            CmdLine.empty
            |> CmdLine.appendRaw "publish"
            |> CmdLine.appendPrefixIfSome "--tag" tag
            |> CmdLine.appendPrefix "--access" access
            |> CmdLine.toString,
            workingDirectory = projectDir
        )

    static member install
        // begin-snippet: Npm.install
        (?workingDirectory: string)
        // end-snippet
        =
        Command.Run("npm", "install", ?workingDirectory = workingDirectory)
