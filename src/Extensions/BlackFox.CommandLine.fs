module BlackFox.CommandLine

open BlackFox.CommandLine

[<AutoOpen>]
module internal Extensions =

    module CmdLine =

        let appendPrefixSeqIfSome (prefix: string) (values: string list option) (cmdLine: CmdLine) =
            match values with
            | Some values -> cmdLine |> CmdLine.appendPrefixSeq prefix values
            | None -> cmdLine

        let appendSeqIfSome (values: string list option) (cmdLine: CmdLine) =
            match values with
            | Some values -> cmdLine |> CmdLine.appendSeq values
            | None -> cmdLine

        let apprendFlagIfSomeTrue (flag: string) (value: bool option) (cmdLine: CmdLine) =
            match value with
            | Some true -> cmdLine |> CmdLine.appendRaw flag
            | _ -> cmdLine

        let appendRawPrefixIfSome (prefix: string) (value: string option) (cmdLine: CmdLine) =
            match value with
            | Some value -> cmdLine |> CmdLine.appendRaw prefix |> CmdLine.appendRaw value
            | None -> cmdLine
