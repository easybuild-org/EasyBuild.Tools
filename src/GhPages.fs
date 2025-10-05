module EasyBuild.Tools.GhPages

open SimpleExec
open BlackFox.CommandLine

type GhPages =

    static member run
        // begin-snippet: GhPages.run
        (
            ?dist: string,
            ?src: string,
            ?branch: string,
            ?dest: string,
            ?add: bool,
            ?silent: bool,
            ?message: string,
            ?tag: string,
            ?git: string,
            ?dotfiles: bool,
            ?nojekyll: bool,
            ?cname: string,
            ?repo: string,
            ?depth: int,
            ?remote: string,
            ?user: string,
            ?remove: string,
            ?noPush: bool,
            ?noHistory: bool,
            ?beforeAdd: string,
            ?workingDirectory: string
        )
        : unit
        // end-snippet
        =
        Command.Run(
            "npx",
            CmdLine.empty
            |> CmdLine.appendRaw "gh-pages"
            |> CmdLine.appendPrefixIfSome "--dist" dist
            |> CmdLine.appendPrefixIfSome "--src" src
            |> CmdLine.appendPrefixIfSome "--branch" branch
            |> CmdLine.appendPrefixIfSome "--dest" dest
            |> CmdLine.apprendFlagIfSomeTrue "--add" add
            |> CmdLine.apprendFlagIfSomeTrue "--silent" silent
            |> CmdLine.appendPrefixIfSome "--message" message
            |> CmdLine.appendPrefixIfSome "--tag" tag
            |> CmdLine.appendPrefixIfSome "--git" git
            |> CmdLine.apprendFlagIfSomeTrue "--dotfiles" dotfiles
            |> CmdLine.apprendFlagIfSomeTrue "--nojekyll" nojekyll
            |> CmdLine.appendPrefixIfSome "--cname" cname
            |> CmdLine.appendPrefixIfSome "--repo" repo
            |> CmdLine.appendPrefixIfSome "--depth" (depth |> Option.map string)
            |> CmdLine.appendPrefixIfSome "--remote" remote
            |> CmdLine.appendPrefixIfSome "--user" user
            |> CmdLine.appendPrefixIfSome "--remove" remove
            |> CmdLine.apprendFlagIfSomeTrue "--no-push" noPush
            |> CmdLine.apprendFlagIfSomeTrue "--no-history" noHistory
            |> CmdLine.appendPrefixIfSome "--before-add" beforeAdd
            |> CmdLine.toString,
            ?workingDirectory = workingDirectory
        )
