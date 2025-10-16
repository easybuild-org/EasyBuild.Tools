module EasyBuild.Tools.ChangelogGen

open System.IO
open SimpleExec
open BlackFox.CommandLine
open System.Threading.Tasks

[<RequireQualifiedAccess>]
type ChangelogGenResult =
    | NewVersion of string
    | Error of string
    | NoVersionBump

type ChangelogGen =
    static member run
        // begin-snippet: ChangelogGen.run
        (
            changelogFile: FileInfo,
            ?allowDirty: bool,
            ?allowBranch: string list,
            ?tagFilter: string list,
            ?preRelease: string,
            ?config: string,
            ?forceVersion: string,
            ?skipInvalidCommit: bool,
            ?dryRun: bool,
            ?githubRepo: string,
            ?workingDirectory: string,
            ?forwardArguments: string list
        )
        : string
        // end-snippet
        =
        let (struct (newVersion, _)) =
            Command.ReadAsync(
                "dotnet",
                CmdLine.empty
                |> CmdLine.appendRaw "changelog-gen"
                |> CmdLine.appendRaw changelogFile.FullName
                |> CmdLine.appendIf (defaultArg allowDirty false) "--allow-dirty"
                |> CmdLine.appendIf (defaultArg skipInvalidCommit false) "--skip-invalid-commit"
                |> CmdLine.appendIf (defaultArg dryRun false) "--dry-run"
                |> CmdLine.appendPrefixIfSome "--pre-release" preRelease
                |> CmdLine.appendPrefixIfSome "--config" config
                |> CmdLine.appendPrefixIfSome "--force-version" forceVersion
                |> CmdLine.appendPrefixIfSome "--github-repo" githubRepo
                |> CmdLine.appendPrefixSeqIfSome "--allow-branch" allowBranch
                |> CmdLine.appendPrefixSeqIfSome "--tag" tagFilter
                |> CmdLine.appendSeqIfSome forwardArguments
                |> CmdLine.toString,
                ?workingDirectory = workingDirectory
            )
            |> Task.RunSynchronously

        newVersion.Trim()

    static member tryRun
        // begin-snippet: ChangelogGen.tryRun
        (
            changelogFile: FileInfo,
            ?allowDirty: bool,
            ?allowBranch: string list,
            ?tagFilter: string list,
            ?preRelease: string,
            ?config: string,
            ?forceVersion: string,
            ?skipInvalidCommit: bool,
            ?dryRun: bool,
            ?githubRepo: string,
            ?workingDirectory: string,
            ?forwardArguments: string list
        )
        : ChangelogGenResult
        // end-snippet
        =
        try
            let newVersion =
                ChangelogGen.run (
                    changelogFile,
                    ?allowDirty = allowDirty,
                    ?allowBranch = allowBranch,
                    ?tagFilter = tagFilter,
                    ?preRelease = preRelease,
                    ?config = config,
                    ?forceVersion = forceVersion,
                    ?skipInvalidCommit = skipInvalidCommit,
                    ?dryRun = dryRun,
                    ?githubRepo = githubRepo,
                    ?workingDirectory = workingDirectory,
                    ?forwardArguments = forwardArguments
                )

            ChangelogGenResult.NewVersion newVersion

        with :? ExitCodeReadException as ex ->
            match ex.ExitCode with
            | 1 -> ChangelogGenResult.Error ex.StandardError
            | 101 -> ChangelogGenResult.NoVersionBump
            | _ -> reraise ()
