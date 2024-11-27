module BlackFox.CommandLine

open BlackFox.CommandLine

[<AutoOpen>]
module internal Extensions =

    module CmdLine =

        let appendPrefixesIfSome (prefix: string) (values: string list option) (cmdLine: CmdLine) =
            match values with
            | Some values -> cmdLine |> CmdLine.appendPrefixSeq prefix values
            | None -> cmdLine

        let appendSeqIfSome (values: string list option) (cmdLine: CmdLine) =
            match values with
            | Some values -> cmdLine |> CmdLine.appendSeq values
            | None -> cmdLine
