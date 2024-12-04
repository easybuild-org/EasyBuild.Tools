module EasyBuild.Tools.DotNet

open SimpleExec
open BlackFox.CommandLine
open System.Text.RegularExpressions
open System.IO

[<RequireQualifiedAccess>]
type Configuration =
    | Debug
    | Release
    | Custom of string

type DotNet =

    static member pack(?workingDirectory: string, ?configuration: Configuration) =
        let configuration =
            defaultArg configuration Configuration.Release
            |> function
                | Configuration.Debug -> "Debug"
                | Configuration.Release -> "Release"
                | Configuration.Custom c -> c

        let struct (standardOutput, _) =
            Command.ReadAsync(
                "dotnet",
                CmdLine.empty
                |> CmdLine.appendRaw "pack"
                |> CmdLine.appendPrefix "--configuration" configuration
                |> CmdLine.toString,
                ?workingDirectory = workingDirectory
            )
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let m =
            Regex.Match(standardOutput, "Successfully created package '(?'nupkgPath'.*\.nupkg)'")

        if m.Success then
            m.Groups.["nupkgPath"].Value |> FileInfo
        else
            failwithf
                $"""Failed to find nupkg path in output:
Output:
{standardOutput}"""

    static member nugetPush
        (
            nupkgPath: FileInfo,
            ?forceEnglishOutput: bool,
            ?source: string,
            ?symbolSource: string,
            ?timeout: int,
            ?apiKey: string,
            ?symbolApiKey: string,
            ?disableBuffering: bool,
            ?noSymbols: bool,
            ?interactive: bool,
            ?skipDuplicate: bool,
            ?forceEcho: bool
        )
        =

        let apiKey =
            match apiKey with
            | Some k -> k
            | None ->
                let key = System.Environment.GetEnvironmentVariable("NUGET_KEY")

                if isNull key then
                    failwith
                        "NUGET_KEY environment variable is not set, you can also pass it as an argument"
                else
                    key

        let symbolApiKey =
            match symbolApiKey with
            | Some k -> Some k
            | None ->
                let key = System.Environment.GetEnvironmentVariable("NUGET_SYMBOL_KEY")

                if isNull key then
                    None
                else
                    Some key

        let source = defaultArg source "https://api.nuget.org/v3/index.json"

        let timeout = timeout |> Option.map string
        let forceEnglishOutput = defaultArg forceEnglishOutput false
        let disableBuffering = defaultArg disableBuffering false
        let noSymbols = defaultArg noSymbols false
        let interactive = defaultArg interactive false
        let skipDuplicate = defaultArg skipDuplicate false

        Command.Run(
            "dotnet",
            CmdLine.empty
            |> CmdLine.appendRaw "nuget"
            |> CmdLine.appendRaw "push"
            |> CmdLine.appendRaw nupkgPath.FullName
            |> CmdLine.appendIf forceEnglishOutput "--force-english-output"
            |> CmdLine.appendPrefix "--source" source
            |> CmdLine.appendPrefixIfSome "--symbol-source" symbolSource
            |> CmdLine.appendPrefixIfSome "--timeout" timeout
            |> CmdLine.appendPrefix "--api-key" apiKey
            |> CmdLine.appendPrefixIfSome "--symbol-api-key" symbolApiKey
            |> CmdLine.appendIf disableBuffering "--disable-buffering"
            |> CmdLine.appendIf noSymbols "--no-symbols"
            |> CmdLine.appendIf interactive "--interactive"
            |> CmdLine.appendIf skipDuplicate "--skip-duplicate"
            |> CmdLine.toString,
            // Do not echo the command because it contains the NuGet key
            noEcho = defaultArg forceEcho false
        )

    static member changelogGen
        (
            changelogFile: string,
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
        =
        let (struct (newVersion, _)) =
            Command.ReadAsync(
                "dotnet",
                CmdLine.empty
                |> CmdLine.appendRaw "changelog-gen"
                |> CmdLine.appendRaw changelogFile
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
            |> Async.AwaitTask
            |> Async.RunSynchronously

        newVersion
