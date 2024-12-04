module EasyBuild.Tools.Npm

open SimpleExec
open System.IO
open BlackFox.CommandLine

type Npm =

    static member publish(projectDir: string, ?tag: string, ?isRestricted: bool) =
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

    static member install(projectDir: string) =
        Command.Run("npm", "install", workingDirectory = projectDir)
