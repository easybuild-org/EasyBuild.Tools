module EasyBuild.Tools.Tests.PackageJson

open Workspace
open Expecto
open Tests.Utils
open EasyBuild.Tools.PackageJson
open System.IO
open SimpleExec

let tests =
    testList
        "PackageJson"
        [
            test "PackageJson.replaceVersion works when a version is present" {
                let packageJsonFile = FileInfo Workspace.``package-json``.``standard.json``

                PackageJson.replaceVersion (packageJsonFile, "1.0.0")

                let newContent = File.ReadAllText packageJsonFile.FullName

                let expected =
                    """{
    "name": "@glutinum/cli",
    "version": "1.0.0",
    "description": "TypeScript definition to F# bindings converter",
    "type": "module",
    "main": "index.js",
    "dependencies": {
        "@ts-morph/bootstrap": "^0.22.0",
        "chalk": "^5.3.0",
        "typescript": "^5.2.2"
    }
}
"""
                // Restore the original content
                Command.Run(
                    "git",
                    [
                        "checkout"
                        packageJsonFile.FullName
                    ]
                )

                Expect.equal newContent expected
            }

            test "PackageJson.replaceVersion works when a version is not present" {
                let packageJsonFile = FileInfo Workspace.``package-json``.``no-version.json``

                PackageJson.replaceVersion (packageJsonFile, "1.0.0")

                let newContent = File.ReadAllText packageJsonFile.FullName

                let expected =
                    """{
    "name": "@glutinum/cli",
    "version": "1.0.0"
}
"""
                // Restore the original content
                Command.Run(
                    "git",
                    [
                        "checkout"
                        packageJsonFile.FullName
                    ]
                )

                Expect.equal newContent expected
            }
        ]