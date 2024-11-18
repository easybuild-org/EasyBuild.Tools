namespace EasyBuild.Tools.Git

open SimpleExec
open BlackFox.CommandLine

type Git =

    static member addAll() =
        Command.Run("git", "add --all")

    static member commitRelease(newVersion: string) =
        Command.Run(
            "git",
            CmdLine.empty
            |> CmdLine.appendRaw "commit"
            |> CmdLine.appendPrefix "-m" $"chore: release %s{newVersion}"
            |> CmdLine.toString
        )
