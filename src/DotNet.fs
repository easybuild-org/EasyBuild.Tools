module EasyBuild.Tools.DotNet

open SimpleExec
open BlackFox.CommandLine
open System.Text.RegularExpressions
open System.IO
open System.Threading.Tasks
open System.Text.Json.Nodes

[<RequireQualifiedAccess>]
type Configuration =
    | Debug
    | Release
    | Custom of string

type DotNet =

    static member restore
        // begin-snippet: DotNet.restore
        (?workingDirectory: string, ?projectFile: FileInfo)
        // end-snippet
        =
        let projectFile = projectFile |> Option.map _.FullName

        Command.Run(
            "dotnet",
            CmdLine.empty
            |> CmdLine.appendRaw "restore"
            |> CmdLine.appendIfSome projectFile
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )

    static member pack
        // begin-snippet: DotNet.pack
        (?workingDirectory: string, ?projectFile: FileInfo, ?configuration: Configuration)
        : FileInfo
        // end-snippet
        =
        let configuration =
            defaultArg configuration Configuration.Release
            |> function
                | Configuration.Debug -> "Debug"
                | Configuration.Release -> "Release"
                | Configuration.Custom c -> c

        // Because, we are invoking MSBuild task directly we need to make sure
        // the project is restored first
        //
        // We can't run `dotnet msbuild -t:Restore,Pack in one go because if some MSBuild tasks
        // are registered by one of the dependency it will not be
        DotNet.restore (?workingDirectory = workingDirectory, ?projectFile = projectFile)

        let projectFile = projectFile |> Option.map _.FullName

        let struct (standardOutput, _) =
            Command.ReadAsync(
                "dotnet",
                CmdLine.empty
                |> CmdLine.appendRaw "msbuild"
                |> CmdLine.appendIfSome projectFile
                |> CmdLine.appendRaw "-t:Pack"
                |> CmdLine.appendRaw $"-p:Configuration=%s{configuration}"
                |> CmdLine.appendRaw "-getItem:NuGetPackOutput"
                |> CmdLine.toString,
                ?workingDirectory = workingDirectory
            )
            |> Task.RunSynchronously

        let json = JsonNode.Parse(standardOutput)

        let nuGetPackOutput = json["Items"].["NuGetPackOutput"].AsArray()

        let nupkgItem =
            nuGetPackOutput
            |> Seq.tryFind (fun item -> item["Extension"].GetValue<string>() = ".nupkg")

        match nupkgItem with
        | Some item -> item["FullPath"].GetValue<string>() |> FileInfo
        | None ->
            failwithf
                $"""Failed to find nupkg path in output:
Output:
{standardOutput}"""

    static member nugetPush
        // begin-snippet: DotNet.nugetPush
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
        // end-snippet
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
