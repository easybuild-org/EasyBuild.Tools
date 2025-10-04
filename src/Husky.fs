module EasyBuild.Tools.Husky

open SimpleExec

type Husky =

    static member install
        // begin-snippet: Husky.install
        (?workingDirectory: string)
        : unit
        // end-snippet
        =
        Command.Run("dotnet", "husky install", ?workingDirectory = workingDirectory)
