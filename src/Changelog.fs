module EasyBuild.Tools.Changelog

open System.Text.RegularExpressions
open System.IO

exception NoVersionFound

type Changelog =

    /// <summary>
    /// Try to find the last version in a changelog file
    /// </summary>
    /// <param name="changelogFile">The changelog file to read</param>
    /// <returns>
    /// <c>Some</c> with the last version found in the changelog, or <c>None</c> if no version is found.
    static member tryFindLastVersion(changelogFile: FileInfo) =
        let content = File.ReadAllText(changelogFile.FullName)
        Changelog.tryFindLastVersion (content)

    /// <summary>
    /// Try to find the last version in a changelog content.
    /// </summary>
    /// <param name="content">The content of the changelog.</param>
    /// <returns>
    /// <c>Some</c> with the last version found in the changelog, or <c>None</c> if no version is found.
    static member tryFindLastVersion(content: string) =
        let m =
            Regex.Match(
                content,
                "^##\\s\\[?v?(?<version>[\\w\\d.-]+\\.[\\w\\d.-]+[a-zA-Z0-9])\\]?(\\s-\\s(?<date>\\d{4}-\\d{2}-\\d{2}))?$",
                RegexOptions.Multiline
            )

        if m.Success then
            Some(m.Groups.["version"].Value)
        else
            None

    /// <summary>
    /// Find the last version in a changelog content.
    /// </summary>
    /// <param name="content">The content of the changelog.</param>
    /// <returns>
    /// The last version found in the changelog.
    /// </returns>
    /// <exception cref="NoVersionFound">Thrown when no version is found in the changelog.</exception>
    static member findLastVersion(content: string) =
        match Changelog.tryFindLastVersion (content) with
        | Some v -> v
        | None -> raise NoVersionFound

    /// <summary>
    /// Find the last version in a changelog file
    /// </summary>
    /// <param name="changelogFile">The changelog file to read</param>
    /// <returns>
    /// The last version found in the changelog.
    /// </returns>
    /// <exception cref="NoVersionFound">Thrown when no version is found in the changelog.</exception>
    static member findLastVersion(changelogFile: FileInfo) =
        let content = File.ReadAllText(changelogFile.FullName)
        Changelog.findLastVersion (content)
