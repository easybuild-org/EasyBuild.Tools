module EasyBuild.Tools.Git

open SimpleExec
open BlackFox.CommandLine

type Git =

    static member addAll
        // begin-snippet: Git.addAll
        ()
        // end-snippet
        =
        Command.Run("git", "add --all")

    static member commitRelease
        // begin-snippet: Git.commitRelease
        (newVersion: string)
        // end-snippet
        =
        Command.Run(
            "git",
            CmdLine.empty
            |> CmdLine.appendRaw "commit"
            |> CmdLine.appendPrefix "-m" $"chore: release %s{newVersion}"
            |> CmdLine.toString
        )

    static member push
        // begin-snippet: Git.push
        (?force: bool)
        // end-snippet
        =
        Command.Run(
            "git",
            CmdLine.empty
            |> CmdLine.appendRaw "push"
            |> CmdLine.appendIf (defaultArg force false) "--force"
            |> CmdLine.toString
        )
