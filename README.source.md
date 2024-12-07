# EasyBuild.Tools

[![NuGet](https://img.shields.io/nuget/v/EasyBuild.Tools.svg)](https://www.nuget.org/packages/EasyBuild.Tools)

[![Sponsors badge link](https://img.shields.io/badge/Sponsors_this_project-EA4AAA?style=for-the-badge)](https://mangelmaxime.github.io/sponsors/)

Tool for generating changelog based on Git history based on [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/). It is using [EasyBuild.CommitParser](https://github.com/easybuild-org/EasyBuild.CommitParser) to parse commit messages check their documentation for more information about configuration.

toc

## Installation

```bash
dotnet add package EasyBuild.Tools
```

## APIs

### `Changelog`

<details>
<summary>
<code>Changelog.tryFindLastVersion</code>
- Try to find the last version in a changelog file
</summary>

snippet: Changelog.tryFindLastVersion

</details>

<details>
<summary>
<code>Changelog.findLastVersion</code>
- Find the last version in a changelog file or throw an error if not found
</summary>

snippet: Changelog.findLastVersion

</details>

### `DotNet`

<details>
<summary>
<code>DotNet.pack</code>
- create a NuGet package
</summary>

snippet: DotNet.pack

**Example**

```fs
open EasyBuild.Tools.DotNet

let nupkgFile = DotNet.pack()
```

</details>

<details>
<summary>
<code>DotNet.nugetPush</code>
- push a NuGet package to a NuGet server
</summary>

snippet: DotNet.nugetPush

If `apiKey` is not provided, `NUGET_KEY` environment variable will be used.

If `symbolApiKey` is not provided, `NUGET_SYMBOL_KEY` environment variable will be used.

</details>

generate changelog using <a href="https://github.com/easybuild-org/EasyBuild.ChangelogGen">EasyBuild.ChangelogGen</a>

<details>
<summary>
<code>DotNet.changelogGen</code>
- generate changelog using <a href="https://github.com/easybuild-org/EasyBuild.ChangelogGen">EasyBuild.ChangelogGen</a>
</summary>

snippet: DotNet.changelogGen

**Example**

```fs
open EasyBuild.Tools.DotNet

let newVersion = DotNet.changelogGen "CHANGELOG.md"
```

</details>

### `Fable`

<details>
<summary>
<code>Fable.build</code>
- run Fable compiler in build mode
</summary>

snippet: Fable.build

</details>

<details>
<summary>
<code>Fable.watch</code>
- run Fable compiler in watch mode
</summary>

snippet: Fable.watch

</details>

### `FableCssModules`

<details>
<summary>
<code>FableCssModules.runAsync</code>
- run <a href="https://www.npmjs.com/package/fable-css-modules">fable-css-modules</a> in async mode
</summary>

snippet: FableCssModules.runAsync

</details>

<details>
<summary>
<code>FableCssModules.run</code>
- run <a href="https://www.npmjs.com/package/fable-css-modules">fable-css-modules</a>
</summary>

snippet: FableCssModules.run

</details>

### `Git`

<details>
<summary>
<code>Git.addAll</code>
- add all files to staging area
</summary>

snippet: Git.addAll

</details>

<details>
<summary>
<code>Git.commitRelease</code>
- commit staged files with release message using conventional commit
</summary>

snippet: Git.commitRelease

</details>

<details>
<summary>
<code>Git.push</code>
- push to the remote repository
</summary>

snippet: Git.push

</details>

### `Nodemon`

<details>
<summary>
<code>Nodemon.runAsync</code>
- run <a href="https://www.npmjs.com/package/nodemon">nodemon</a> in async mode
</summary>

snippet: Nodemon.runAsync

</details>

<details>
<summary>
<code>Nodemon.run</code>
- run <a href="https://www.npmjs.com/package/nodemon">nodemon</a>
</summary>

snippet: Nodemon.run

</details>

### `Npm`

<details>
<summary>
<code>Npm.publish</code>
- publish a package to the npm registry
</summary>

snippet: Npm.publish

</details>

<details>
<summary>
<code>Npm.install</code>
- install npm packages
</summary>

snippet: Npm.install

</details>

### `PackageJson`

<details>
<summary>
<code>PackageJson.replaceVersion</code>
- replace the version in a <code>package.json</code> file
</summary>

snippet: PackageJson.replaceVersion

**Example**

```fs
open EasyBuild.Tools.PackageJson

let packageJsonFile = FileInfo "package.json"
PackageJson.replaceVersion packageJsonFile "1.0.0"
```

</details>

<details>
<summary>
<code>PackageJson.getName</code>
- replace the version in a <code>package.json</code> file
</summary>

snippet: PackageJson.getName

**Example**

```fs
open EasyBuild.Tools.PackageJson

let packageJsonFile = FileInfo "package.json"
let packageName = PackageJson.getName packageJsonFile
```

</details>

<details>
<summary>
<code>PackageJson.getVersion</code>
- replace the version in a <code>package.json</code> file
</summary>

snippet: PackageJson.getVersion

**Example**

```fs
open EasyBuild.Tools.PackageJson

let packageJsonFile = FileInfo "package.json"
let packageVersion = PackageJson.getVersion packageJsonFile
```

</details>

<details>
<summary>
<code>PackageJson.needPublishing</code>
- check if a package needs to be published
</summary>

snippet: PackageJson.needPublishing

**Example**

```fs
open EasyBuild.Tools.PackageJson

let packageJsonFile = FileInfo "package.json"

if PackageJson.needPublishing packageJsonFile then
    // Do something
```

</details>

### `Vercel`

<details>
<summary>
<code>Vercel.pull</code>
- Pull latest environment variables and project settings from Vercel
</summary>

snippet: Vercel.pull

</details>

<details>
<summary>
<code>Vercel.build</code>
- build the project
</summary>

snippet: Vercel.build

</details>

<details>
<summary>
<code>Vercel.deploy</code>
- deploy your project to Vercel
</summary>

snippet: Vercel.deploy

</details>

### `Vite`

<details>
<summary>
<code>Vite.build</code>
- build for production
</summary>

snippet: Vite.build

</details>

<details>
<summary>
<code>Vite.watch</code>
- start development server
</summary>

snippet: Vite.watch

</details>
