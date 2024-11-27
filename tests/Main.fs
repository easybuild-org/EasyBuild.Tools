module EasyBuild.Tools.Tests.Main

open Expecto

[<Tests>]
let allTests = testList "All Tests" [ Changelog.tests ]

[<EntryPoint>]
let main argv =
    runTestsWithCLIArgs [] Array.empty allTests
