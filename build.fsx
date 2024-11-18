#r "nuget: Fun.Build, 1.1.14"
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
    |}

pipeline "release" {
    whenEnvVar options.NUGET_KEY

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
