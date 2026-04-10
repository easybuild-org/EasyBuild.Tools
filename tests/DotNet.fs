module EasyBuild.Tools.Tests.DotNet

open Workspace
open Expecto
open Tests.Utils
open EasyBuild.Tools.DotNet
open System.IO
open System

let tests =
    testList
        "DotNet"
        [
            test "DotNet.pack returns the nupkg path" {
                let projectFile =
                    FileInfo Workspace.``dotnet-pack``.TestLibrary.``TestLibrary.fsproj``

                let nupkgPath = DotNet.pack (projectFile = projectFile)

                Expect.isNotNull nupkgPath
                Expect.equal nupkgPath.Extension ".nupkg"
                Expect.equal nupkgPath.Name "TestLibrary.2.42.33.nupkg"
                Expect.isTrue nupkgPath.Exists
            }

            test "DotNet.pack with Debug configuration" {
                let projectFile =
                    FileInfo Workspace.``dotnet-pack``.TestLibrary.``TestLibrary.fsproj``

                let nupkgPath =
                    DotNet.pack (projectFile = projectFile, configuration = Configuration.Debug)

                Expect.isNotNull nupkgPath
                Expect.equal nupkgPath.Extension ".nupkg"
                Expect.equal nupkgPath.Name "TestLibrary.2.42.33.nupkg"
                Expect.isTrue nupkgPath.Exists

                // Verify it's in the Debug folder
                let expectedPath =
                    Path.Combine(
                        projectFile.DirectoryName,
                        "bin",
                        "Debug",
                        "TestLibrary.2.42.33.nupkg"
                    )

                Expect.equal nupkgPath.FullName expectedPath
            }
        ]
