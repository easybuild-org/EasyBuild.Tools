module EasyBuild.Tools.PackageJson

open System.Text.Json
open System.Text.Json.Nodes
open System.IO
open SimpleExec
open System.Threading.Tasks

type PackageJson =

    static member inline private parseJson(file: FileInfo) =
        let rawText = File.ReadAllText(file.FullName)
        JsonNode.Parse(rawText)

    static member replaceVersion
        // begin-snippet: PackageJson.replaceVersion
        (file: FileInfo, newVersion: string)
        // end-snippet
        =
        let json = PackageJson.parseJson file
        json["version"] <- newVersion

        let options =
            JsonSerializerOptions(
                WriteIndented = true,
                NewLine = "\n",
                IndentSize = 4,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            )

        let newContent = JsonSerializer.Serialize(json, options)

        File.WriteAllText(file.FullName, newContent + "\n")

    static member getName
        // begin-snippet: PackageJson.getName
        (file: FileInfo)
        // end-snippet
        =
        let json = PackageJson.parseJson file
        json["name"].GetValue<string>()

    static member getVersion
        // begin-snippet: PackageJson.getVersion
        (file: FileInfo)
        // end-snippet
        =
        let json = PackageJson.parseJson file
        json["version"].GetValue<string>()

    /// <summary>
    /// Checks if the package needs to be published
    /// </summary>
    /// <param name="packageJson">The package.json file</param>
    /// <returns><c>true</c> if the package needs to be published, <c>false</c> otherwise</returns>
    static member needPublishing
        // begin-snippet: PackageJson.needPublishing
        (packageJson: FileInfo)
        : bool
        // end-snippet
        =
        let packageName = PackageJson.getName packageJson
        let packageVersion = PackageJson.getVersion packageJson

        let packageSpec = packageName + "@" + packageVersion

        task {

            try
                let! _ = Command.ReadAsync("npm", $"view {packageSpec} --json")

                return false

            with :? ExitCodeReadException as e ->
                let json = JsonNode.Parse(e.StandardOutput)

                let errorCode = (json["error"]["code"]).GetValue<string>()

                if errorCode = "E404" then
                    return true
                else
                    return
                        failwithf
                            """An error occurred while checking if the package %s is published:

%s"""
                            packageSpec
                            e.Message
        }
        |> Task.RunSynchronously
