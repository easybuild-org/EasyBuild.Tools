module EasyBuild.Tools.Husky

open SimpleExec
open System

type Husky =

    /// <summary>
    /// Install Husky hooks unless it detects `ACT` env variable.
    ///
    /// This is because if the person is using `act` with worktree then Husky fails to install
    /// </summary>
    static member install
        // begin-snippet: Husky.install
        (?workingDirectory: string)
        : unit
        // end-snippet
        =

        if Environment.GetEnvironmentVariable("ACT") <> null then
            ()
        else
            Command.Run("dotnet", "husky install", ?workingDirectory = workingDirectory)
