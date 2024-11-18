namespace EasyBuild.Tools.Git

open SimpleExec
open BlackFox.CommandLine

type Git =

    static member addAll() = Command.Run("git", "add --all")

    static member commitRelease(newVersion: string) =
        Command.Run(
            "git",
            CmdLine.empty
            |> CmdLine.appendRaw "commit"
            |> CmdLine.appendPrefix "-m" $"chore: release %s{newVersion}"
            |> CmdLine.toString
        )

    static member push(?force: bool) =
        Command.Run(
            "git",
            CmdLine.empty
            |> CmdLine.appendRaw "push"
            |> CmdLine.appendIf (defaultArg force false) "--force"
            |> CmdLine.toString
        )
