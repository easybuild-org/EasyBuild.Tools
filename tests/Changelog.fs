module EasyBuild.Tools.Tests.Changelog

open Workspace
open Expecto
open Tests.Utils
open EasyBuild.Tools.Changelog
open System.IO

let tests =
    testList
        "Changelog"
        [
            testList
                "Changelog.tryFindLastVersion"
                [
                    test "works for Changelog generated using EasyBuild.ChangelogGen" {
                        let actual =
                            Workspace.changelogs.``easybuild_generated.md``
                            |> FileInfo
                            |> Changelog.tryFindLastVersion

                        Expect.equal actual (Some "1.1.1")
                    }

                    test "works for Changelog following the KeepAChangelog format" {
                        let actual =
                            Workspace.changelogs.``keep_a_changelog.md``
                            |> FileInfo
                            |> Changelog.tryFindLastVersion

                        Expect.equal actual (Some "1.1.1")
                    }

                    test "returns None if no version is found" {
                        let actual =
                            Workspace.changelogs.``no_version.md``
                            |> FileInfo
                            |> Changelog.tryFindLastVersion

                        Expect.equal actual None
                    }
                ]

            testList
                "Changelog.findLastVersion"
                [
                    test "throws NoVersionFound if no version is found" {
                        Expect.throwsT<NoVersionFound> (fun () ->
                            Workspace.changelogs.``no_version.md``
                            |> FileInfo
                            |> Changelog.findLastVersion
                            |> ignore
                        )
                    }

                    test "works for Changelog generated using EasyBuild.ChangelogGen" {
                        let actual =
                            Workspace.changelogs.``easybuild_generated.md``
                            |> FileInfo
                            |> Changelog.findLastVersion

                        Expect.equal actual "1.1.1"
                    }
                ]
        ]
