#r "nuget: Fun.Build, 1.1.15"
#r "./src/bin/Debug/net6.0/publish/EasyBuild.Tools.dll"
#r "./src/bin/Debug/net6.0/publish/BlackFox.CommandLine.dll"
#r "./src/bin/Debug/net6.0/publish/SimpleExec.dll"
#r "nuget: EasyBuild.FileSystemProvider, 0.3.0"

open Fun.Build
open EasyBuild.Tools.DotNet
open EasyBuild.Tools.Git
open EasyBuild.FileSystemProvider

type Workspace = RelativeFileSystem<".">

let options =
    {|
        NUGET_KEY = EnvArg.Create("NUGET_KEY", description = "The NuGet key to use for publishing")
        WATCH =
            CmdArg.Create("--watch", "-w", description = "Watch for changes and re-run the process")
    |}

module Stages =

    let test =
        stage "Run tests" {
            stage "Watch" {
                whenCmdArg options.WATCH

                run "dotnet watch test --project tests/EasyBuild.Tools.Tests.fsproj"
            }

            stage "Run tests" {
                whenNot { cmdArg options.WATCH }
                run "dotnet test"
            }
        }

pipeline "Install Git hooks" {
    stage "Husky" { run "dotnet husky install" }
    runImmediate // Always run this pipeline
}

pipeline "test" {
    Stages.test

    runIfOnlySpecified
}

pipeline "update-docs" {
    stage "Update README.md" {
        run "dotnet mdsnippets --read-only true"
        run "git add README.md"
        run "git add README.source.md"
        run "git commit -m \"chore: update README.md\""
        run "git push"
    }

    runIfOnlySpecified
}

pipeline "release" {
    whenEnvVar options.NUGET_KEY

    Stages.test

    stage "Update README.md" { run "dotnet mdsnippets --read-only true" }

    stage "Pack and publish" {
        run (fun _ ->
            let newVersion = DotNet.changelogGen Workspace.``CHANGELOG.md``

            let nupkgPath = DotNet.pack (workingDirectory = Workspace.src.``.``)

            DotNet.nugetPush nupkgPath

            Git.addAll ()
            Git.commitRelease newVersion
            Git.push ()
        )
    }

    runIfOnlySpecified
}

tryPrintPipelineCommandHelp ()
