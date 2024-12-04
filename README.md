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

| name                | type          | required | default | description                                                                                                                                   |
| ------------------- | ------------- | :------: | ------- | --------------------------------------------------------------------------------------------------------------------------------------------- |
| `changelogFile`     | `string`      |    ✅    |         |                                                                                                                                               |
| `allowDirty`        | `bool`        |    ❌    |         | Allow to run in a dirty repository                                                                                                            |
| `allowBranch`       | `string list` |    ❌    | `main`  | List of branches that are allowed to be used to generate the changelog.                                                                       |
| `tagFilter`         | `string list` |    ❌    |         | List of tags to include in the changelog                                                                                                      |
| `preRelease`        | `string`      |    ❌    |         | Indicate that the generated version is a pre-release version.                                                                                 |
| `forceVersion`      | `string`      |    ❌    |         | Force the version to be used in the changelog                                                                                                 |
| `skipInvalidCommit` | `bool`        |    ❌    |         | Skip invalid commits instead of failing                                                                                                       |
| `dryRun`            | `bool`        |    ❌    |         | Run the command without writing to the changelog file, output the result in STDOUT instead                                                    |
| `githubRepo`        | `string`      |    ❌    |         | GitHub repository name in format 'owner/repo'                                                                                                 |
| `workingDirectory`  | `string`      |    ❌    |         | Working directory path                                                                                                                        |
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

<table>
   <thead>
      <tr>
         <th>name</th>
         <th>type</th>
         <th align="center">required</th>
         <th>default</th>
         <th>description</th>
      </tr>
   </thead>
   <tbody>
      <tr>
         <td><code>nupkgPath</code></td>
         <td><code>string</code></td>
         <td align="center">✅</td>
         <td></td>
         <td>Working directory path</td>
      </tr>
      <tr>
         <td><code>forceEnglishOutput</code></td>
         <td><code>bool</code></td>
         <td align="center">❌</td>
         <td><code>false</code></td>
         <td>Forces the application to run using an invariant, English-based culture</td>
      </tr>
      <tr>
         <td><code>source</code></td>
         <td><code>string</code></td>
         <td align="center">❌</td>
         <td><code>https://api.nuget.org/v3/index.json</code></td>
         <td>Package source (URL, UNC/folder path or package source name) to use.</td>
      </tr>
      <tr>
         <td><code>symbolSource</code></td>
         <td><code>string</code></td>
         <td align="center">❌</td>
         <td></td>
         <td>Symbol server URL to use</td>
      </tr>
      <tr>
         <td><code>timeout</code></td>
         <td><code>int</code></td>
         <td align="center">❌</td>
         <td><code>300</code> (5 minutes)</td>
         <td>Timeout for pushing to a server in seconds</td>
      </tr>
      <tr>
         <td><code>apiKey</code></td>
         <td><code>string</code></td>
         <td align="center">❌</td>
         <td><code>NUGET_KEY</code> env variable</td>
         <td>The API key for the server</td>
      </tr>
      <tr>
         <td><code>symbolApiKey</code></td>
         <td><code>string</code></td>
         <td align="center">❌</td>
         <td><code>NUGET_SYMBOL_KEY</code> env variable if presents otherwise we don't provide the argument</td>
         <td>The API key for the symbol server</td>
      </tr>
      <tr>
         <td><code>disableBuffering</code></td>
         <td><code>bool</code></td>
         <td align="center">❌</td>
         <td><code>false</code></td>
         <td>Disable buffering when pushing to an HTTP(S) server to decrease memory usage</td>
      </tr>
      <tr>
         <td><code>noSymbols</code></td>
         <td><code>bool</code></td>
         <td align="center">❌</td>
         <td><code>false</code></td>
         <td>If a symbols package exists, it will not be pushed to a symbols server</td>
      </tr>
      <tr>
         <td><code>interactive</code></td>
         <td><code>bool</code></td>
         <td align="center">❌</td>
         <td><code>false</code></td>
         <td>Allow the command to block and require manual action for operations like authentication</td>
      </tr>
      <tr>
         <td><code>skipDuplicate</code></td>
         <td><code>bool</code></td>
         <td align="center">❌</td>
         <td><code>false</code></td>
         <td>If a package and version already exists, skip it and continue with the next package in the push, if any</td>
      </tr>
      <tr>
         <td><code>forceEcho</code></td>
         <td><code>bool</code></td>
         <td align="center">❌</td>
         <td><code>false</code></td>
         <td>Echo the command and command output</td>
      </tr>
   </tbody>
</table>

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
<code>PackageJson.getName</code>
- get the name of the package from NPM <code>package.json</code> file
</summary>

#### Parameters

<table>
   <thead>
      <tr>
         <th>name</th>
         <th>type</th>
         <th align="center">required</th>
         <th>default</th>
         <th>description</th>
      </tr>
   </thead>
   <tbody>
      <tr>
         <td><code>packageJson</code></td>
         <td><code>FileInfo</code></td>
         <td align="center">✅</td>
         <td></td>
         <td>The package.json file to get the name from</td>
      </tr>
   </tbody>
</table>

#### Returns

`string` - the name of the package

#### Example

```fs
open EasyBuild.Tools.PackageJson

let packageJsonFile = FileInfo "package.json"
let packageName = PackageJson.getName packageJsonFile
```

</details>

<details>
<summary>
<code>PackageJson.getVersion</code>
- get the version of the package from NPM <code>package.json</code> file
</summary>

#### Parameters

<table>
   <thead>
      <tr>
         <th>name</th>
         <th>type</th>
         <th align="center">required</th>
         <th>default</th>
         <th>description</th>
      </tr>
   </thead>
   <tbody>
      <tr>
         <td><code>packageJson</code></td>
         <td><code>FileInfo</code></td>
         <td align="center">✅</td>
         <td></td>
         <td>The package.json file to get the version from</td>
      </tr>
   </tbody>
</table>

#### Returns

`string` - the version of the package

#### Example

```fs
open EasyBuild.Tools.PackageJson

let packageJsonFile = FileInfo "package.json"
let packageVersion = PackageJson.getVersion packageJsonFile
```

</details>

<details>
<summary>
<code>PackageJson.needPublishing</code>
- check if the package needs publishing
</summary>

#### Parameters

<table>
   <thead>
      <tr>
         <th>name</th>
         <th>type</th>
         <th align="center">required</th>
         <th>default</th>
         <th>description</th>
      </tr>
   </thead>
   <tbody>
      <tr>
         <td><code>packageJson</code></td>
         <td><code>FileInfo</code></td>
         <td align="center">✅</td>
         <td></td>
         <td>The package.json file to check</td>
      </tr>
   </tbody>
</table>

#### Returns

`bool` - `true` if the package needs publishing, `false` otherwise

#### Example

```fs
open EasyBuild.Tools.PackageJson

let packageJsonFile = FileInfo "package.json"

if PackageJson.needPublishing packageJsonFile then
    // Do something
```

</details>

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

| name            | type       | required | default | description    |
| --------------- | ---------- | :------: | ------- | -------------- |
| `changelogFile` | `FileInfo` |    ✅    |         | File to update |

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

| name            | type       | required | default | description    |
| --------------- | ---------- | :------: | ------- | -------------- |
| `changelogFile` | `FileInfo` |    ✅    |         | File to update |

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

### `Npm`

<details>
<summary>
<code>Npm.publish</code>
- publish the package to NPM
</summary>

#### Parameters

<table>
   <thead>
      <tr>
         <th>name</th>
         <th>type</th>
         <th align="center">required</th>
         <th>default</th>
         <th>description</th>
      </tr>
   </thead>
   <tbody>
      <tr>
         <td><code>projectDirectory</code></td>
         <td><code>string</code></td>
         <td align="center">✅</td>
         <td></td>
         <td>Project directory path</td>
      </tr>
      <tr>
         <td><code>tag</code></td>
         <td><code>string</code></td>
         <td align="center">❌</td>
         <td></td>
         <td>See <a href="https://docs.npmjs.com/cli/v8/commands/npm-publish#tag">NPM documentation</a> for more information</td>
      </tr>
      <tr>
         <td><code>isRestricted</code></td>
         <td><code>bool</code></td>
         <td align="center">❌</td>
         <td><code>false</code> which default to <code>--access public</code></td>
         <td>Set the access level</td>
      </tr>
   </tbody>
</table>

#### Returns

`unit`

#### Example

```fs
open EasyBuild.Tools.Npm

Npm.publish "path/to/project"
```

</details>

<details>
<summary>
<code>Npm.install</code>
- install the package dependencies
</summary>

#### Parameters

<table>
   <thead>
      <tr>
         <th>name</th>
         <th>type</th>
         <th align="center">required</th>
         <th>default</th>
         <th>description</th>
      </tr>
   </thead>
   <tbody>
      <tr>
         <td><code>projectDirectory</code></td>
         <td><code>string</code></td>
         <td align="center">✅</td>
         <td></td>
         <td>Project directory path</td>
      </tr>
   </tbody>
</table>

#### Returns

`unit`

#### Example

```fs
open EasyBuild.Tools.Npm

Npm.install "path/to/project"
```

</details>
