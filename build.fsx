#r "nuget: Fun.Build, 1.1.14"
#r "./src/bin/Debug/net6.0/EasyBuild.Tools.dll"
#r "nuget: EasyBuild.FileSystemProvider, 0.3.0"

open Fun.Build
open EasyBuild.Tools.DotNet
open EasyBuild.Tools.Git
open EasyBuild.FileSystemProvider
open System.IO

type Workspace = RelativeFileSystem<".">

type VirtualWorkspace =
    VirtualFileSystem<
        "./src",
        """
bin/
obj/
"""
     >

let options =
    {| NUGET_KEY = EnvArg.Create("NUGET_KEY", description = "The NuGet key to use for publishing") |}

pipeline "release" {
    whenEnvVar options.NUGET_KEY

    stage "Clean up" {
        run (fun _ ->
            if Directory.Exists VirtualWorkspace.bin.``.`` then
                Directory.Delete(VirtualWorkspace.bin.``.``, true)
        )
    }

    stage "Pack and publish" {
        run (fun _ ->
            let newVersion = DotNet.changelogGen ()

            let nupkgPath = DotNet.pack (workingDirectory = Workspace.src.``.``)

            DotNet.nugetPush nupkgPath

            Git.addAll ()
            Git.commitRelease newVersion)
    }

    runIfOnlySpecified
}

tryPrintPipelineCommandHelp ()
