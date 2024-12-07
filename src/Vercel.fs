module EasyBuild.Tools.Vercel

open SimpleExec
open BlackFox.CommandLine

module Advanced =

    type GlobalOptions =
        {
            Debug: bool option
            GlobalConfig: string option
            LocalConfig: string option
            NoColor: bool option
            Scope: string option
            Token: string option
        }

    let appendGlobalOptions (globalOptions: GlobalOptions) (cmdLine: CmdLine) =

        cmdLine
        |> CmdLine.apprendFlagIfSomeTrue "--debug" globalOptions.Debug
        |> CmdLine.appendPrefixIfSome "--globalConfig" globalOptions.GlobalConfig
        |> CmdLine.appendPrefixIfSome "--localConfig" globalOptions.LocalConfig
        |> CmdLine.apprendFlagIfSomeTrue "--no-color" globalOptions.NoColor
        |> CmdLine.appendPrefixIfSome "--scope" globalOptions.Scope
        |> CmdLine.appendPrefixIfSome "--token" globalOptions.Token

type Vercel =

    static member pull
        // begin-snippet: Vercel.pull
        (
            ?environment: string,
            ?gitBranch: string,
            ?yes: bool,
            ?debug: bool,
            ?globalConfig: string,
            ?localConfig: string,
            ?noColor: bool,
            ?scope: string,
            ?token: string,
            ?workingDirectory: string
        )
        // end-snippet
        =

        let globalOptions: Advanced.GlobalOptions =
            {
                Debug = debug
                GlobalConfig = globalConfig
                LocalConfig = localConfig
                NoColor = noColor
                Scope = scope
                Token = token
            }

        Command.Run(
            "npx",
            CmdLine.empty
            |> CmdLine.appendRaw "vercel"
            |> CmdLine.appendRaw "pull"
            |> CmdLine.appendPrefixIfSome "--environment" environment
            |> CmdLine.appendPrefixIfSome "--gitBranch" gitBranch
            |> CmdLine.apprendFlagIfSomeTrue "--yes" yes
            |> Advanced.appendGlobalOptions globalOptions
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )

    static member build
        // begin-snippet: Vercel.build
        (
            ?output: string,
            ?prod: bool,
            ?yes: bool,
            ?debug: bool,
            ?globalConfig: string,
            ?localConfig: string,
            ?noColor: bool,
            ?scope: string,
            ?token: string,
            ?workingDirectory: string
        )
        // end-snippet
        =

        let globalOptions: Advanced.GlobalOptions =
            {
                Debug = debug
                GlobalConfig = globalConfig
                LocalConfig = localConfig
                NoColor = noColor
                Scope = scope
                Token = token
            }

        Command.Run(
            "npx",
            CmdLine.empty
            |> CmdLine.appendRaw "vercel"
            |> CmdLine.appendRaw "build"
            |> CmdLine.appendPrefixIfSome "--output" output
            |> CmdLine.apprendFlagIfSomeTrue "--prod" prod
            |> CmdLine.apprendFlagIfSomeTrue "--yes" yes
            |> Advanced.appendGlobalOptions globalOptions
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )

    static member deploy
        // begin-snippet: Vercel.deploy
        (
            ?archive: string,
            ?buildEnv: string,
            ?env: string,
            ?force: bool,
            ?meta: string,
            ?noWait: bool,
            ?prebuilt: bool,
            ?prod: bool,
            ?``public``: bool,
            ?regions: string list,
            ?skipDomain: bool,
            ?withCache: bool,
            ?yes: bool,
            ?debug: bool,
            ?globalConfig: string,
            ?localConfig: string,
            ?noColor: bool,
            ?scope: string,
            ?token: string,
            ?workingDirectory: string
        )
        // end-snippet
        =

        let globalOptions: Advanced.GlobalOptions =
            {
                Debug = debug
                GlobalConfig = globalConfig
                LocalConfig = localConfig
                NoColor = noColor
                Scope = scope
                Token = token
            }

        Command.Run(
            "npx",
            CmdLine.empty
            |> CmdLine.appendRaw "vercel"
            |> CmdLine.appendRaw "deploy"
            |> CmdLine.appendPrefixIfSome "--archive" archive
            |> CmdLine.appendPrefixIfSome "--build-env" buildEnv
            |> CmdLine.appendPrefixIfSome "--env" env
            |> CmdLine.apprendFlagIfSomeTrue "--force" force
            |> CmdLine.appendPrefixIfSome "--meta" meta
            |> CmdLine.apprendFlagIfSomeTrue "--no-wait" noWait
            |> CmdLine.apprendFlagIfSomeTrue "--prebuilt" prebuilt
            |> CmdLine.apprendFlagIfSomeTrue "--prod" prod
            |> CmdLine.apprendFlagIfSomeTrue "--public" ``public``
            |> CmdLine.appendPrefixSeqIfSome "--regions" regions
            |> CmdLine.apprendFlagIfSomeTrue "--skip-domain" skipDomain
            |> CmdLine.apprendFlagIfSomeTrue "--with-cache" withCache
            |> CmdLine.apprendFlagIfSomeTrue "--yes" yes
            |> Advanced.appendGlobalOptions globalOptions
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )
