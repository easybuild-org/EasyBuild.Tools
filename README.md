# EasyBuild.Tools

[![NuGet](https://img.shields.io/nuget/v/EasyBuild.Tools.svg)](https://www.nuget.org/packages/EasyBuild.Tools)

[![Sponsors badge link](https://img.shields.io/badge/Sponsors_this_project-EA4AAA?style=for-the-badge)](https://mangelmaxime.github.io/sponsors/)

Tool for generating changelog based on Git history based on [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/). It is using [EasyBuild.CommitParser](https://github.com/easybuild-org/EasyBuild.CommitParser) to parse commit messages check their documentation for more information about configuration.

## Installation

```bash
dotnet add package EasyBuild.Tools
```

## APIs

### `Git`

<details>
<summary>
<code>Git.addAll</code>
- add all files to staging area
</summary>

#### Parameters

None

#### Returns

`unit`

#### Example

```fs
open EasyBuild.Tools.Git

Git.addAll()
```

</details>

<details>
<summary>
<code>Git.commitRelease</code>
- commit staged files with release message using conventional commit
</summary>

#### Parameters

| name         | type     | required | description        |
| ------------ | -------- | :------: | ------------------ |
| `newVersion` | `string` |    ✅    | Version to release |

#### Returns

`unit`

#### Example

```fs
open EasyBuild.Tools.Git

// Create a commit with message "chore: release 1.0.0"
Git.commitRelease "1.0.0"
```

</details>

<details>
<summary>
<code>Git.push</code>
- push changes to remote repository
</summary>

#### Parameters

| name    | type   | required | description                 |
| ------- | ------ | :------: | --------------------------- |
| `force` | `bool` |    ❌    | Force push to remote branch |

#### Returns

`unit`

#### Example

```fs
open EasyBuild.Tools.Git

Git.push()
```

</details>

### `DotNet`

<details>
<summary>
<code>DotNet.changelogGen</code>
- generate changelog using <a href="https://github.com/easybuild-org/EasyBuild.ChangelogGen">EasyBuild.ChangelogGen</a>
</code>
</summary>

#### Parameters

| name                | type          | required | default | description                                                                                                                                    |
| ------------------- | ------------- | :------: | ------- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| `changelogFile`     | `string`      |    ✅    |         |                                                                                                                                                |
| `allowDirty`        | `bool`        |    ❌    |         | Allow to run in a dirty repository                                                                                                             |
| `allowBranch`       | `string list` |    ❌    | `main`  | List of branches that are allowed to be used to generate the changelog.                                                                        |
| `tagFilter`         | `string list` |    ❌    |         | List of tags to include in the changelog                                                                                                       |
| `preRelease`        | `string`      |    ❌    |         | Indicate that the generated version is a pre-release version.                                                                                  |
| `forceVersion`      | `string`      |    ❌    |         | Force the version to be used in the changelog                                                                                                  |
| `skipInvalidCommit` | `bool`        |    ❌    |         | Skip invalid commits instead of failing                                                                                                        |
| `dryRun`            | `bool`        |    ❌    |         | Run the command without writing to the changelog file, output the result in STDOUT instead                                                     |
| `githubRepo`        | `string`      |    ❌    |         | GitHub repository name in format 'owner/repo'                                                                                                  |
| `workingDirectory`  | `string`      |    ❌    |         | Working directory path                                                                                                                         |
| `forwardArguments`  | `string list` |    ❌    |         | List of arguments to forward to the CLI tools as defined in [EasyBuild.ChangelogGen](https://github.com/easybuild-org/EasyBuild.ChangelogGen) |

#### Returns

`string` - new version generated based on the commits history

#### Example

```fs
open EasyBuild.Tools.DotNet

let newVersion = DotNet.changelogGen "CHANGELOG.md"
```

</details>

<details>
<summary>
<code>DotNet.pack</code>
- commit staged files with release message of format <code>chore: release {version}</code>
</summary>

#### Parameters

| name               | type            | required | default   | description            |
| ------------------ | --------------- | :------: | --------- | ---------------------- |
| `workingDirectory` | `string`        |    ❌    |           | Working directory path |
| `configuration`    | `Configuration` |    ❌    | `Release` | Build configuration    |

#### Returns

`FileInfo` - file descriptor to the generated `.nupkg` file

#### Example

```fs
open EasyBuild.Tools.DotNet

let nupkgFile = DotNet.pack()
```

</details>

<details>
<summary>
<code>DotNet.nugetPush</code>
- commit staged files with release message using conventional commit
</summary>

#### Parameters

| name            | type            | required | default                  | description                                                          |
| --------------- | --------------- | :------: | ------------------------ | -------------------------------------------------------------------- |
| `nupkgPath`     | `string`        |    ✅    |                          | Working directory path                                               |
| `nugetKey`      | `Configuration` |    ❌    | `NUGET_KEY` env variable | NuGet API key                                                        |
| `skipDuplicate` | `bool`          |    ❌    | `true`                   | If a package and version already exists, skip it                     |
| `source`        | `string`        |    ❌    |                          | Package source (URL, UNC/folder path or package source name) to use. |
| `forceEcho`     | `bool`          |    ❌    | `false`                  | Echo the                                                             |

#### Returns

`unit`

#### Example

```fs
open EasyBuild.Tools.DotNet

// In general, you will get the nupkg file from DotNet.pack
let nupkgFile = DotNet.pack()

DotNet.nugetPush nupkgFile

// Or you can customize it
let nugetKey = Environment.GetEnvironmentVariable "NUGET_KEY_CUSTOM"
DotNet.nugetPush (nupkgFile, nugetKey = nugetKey)
```

</details>

### `PackageJson`

<details>
<summary>
<code>PackageJson.replaceVersion</code>
- replace version in NPM <code>package.json</code> file
</code>
</summary>

#### Parameters

| name         | type       | required | default | description        |
| ------------ | ---------- | :------: | ------- | ------------------ |
| `file`       | `FileInfo` |    ✅    |         | File to update     |
| `newVersion` | `string`   |    ✅    |         | New version to set |

#### Returns

`unit`

#### Example

```fs
open EasyBuild.Tools.PackageJson

let packageJsonFile = FileInfo "package.json"
PackageJson.replaceVersion packageJsonFile "1.0.0"
```

</details>

### `Changelog`

<details>
<summary>
<code>PackageJson.tryFindLastVersion</code>
- try to find the last version in a CHANGELOG file
</code>
</summary>

#### Parameters

| name            | type       | required | default | description        |
| --------------- | ---------- | :------: | ------- | ------------------ |
| `changelogFile` | `FileInfo` |    ✅    |         | File to update     |

#### Returns

`string option` - `Some` with the last version or `None` if not found

#### Example

```fs
open EasyBuild.Tools.Changelog

let lastVersion =
    "CHANGELOG.md"
    |> FileInfo
    |> Changelog.tryFindLastVersion
```

</details>

<details>
<summary>
<code>PackageJson.findLastVersion</code>
- find the last version in a CHANGELOG file or throw an exception
</code>
</summary>

#### Parameters

| name            | type       | required | default | description        |
| --------------- | ---------- | :------: | ------- | ------------------ |
| `changelogFile` | `FileInfo` |    ✅    |         | File to update     |

#### Returns

`string` - the last version

If the version is not found, it will throw an exception of type `NoVersionFound`.

#### Example

```fs
open EasyBuild.Tools.Changelog

let lastVersion =
    "CHANGELOG.md"
    |> FileInfo
    |> Changelog.findLastVersion
```

</details>
