module Tests.Utils

open Expecto

module Expect =

    let equal actual expected = Expect.equal actual expected ""

    let notEqual actual expected = Expect.notEqual actual expected ""

    let isNotEmpty actual = Expect.isNotEmpty actual ""

    let isOk actual = Expect.isOk actual ""

    let isError actual = Expect.isError actual ""

    let throws actual = Expect.throws actual ""

    let throwsT<'texn when 'texn :> exn> actual = Expect.throwsT<'texn> actual ""
