module EasyBuild.Tools.PackageJson

open System.Text.Json
open System.Text.Json.Nodes
open System.IO

type PackageJson =

    static member inline private parseJson(file: FileInfo) =
        let rawText = File.ReadAllText(file.FullName)
        JsonNode.Parse(rawText)

    static member replaceVersion(file: FileInfo, newVersion: string) =
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

    static member getName(file: FileInfo) =
        let json = PackageJson.parseJson file
        json["name"].GetValue<string>()

    static member getVersion(file: FileInfo) =
        let json = PackageJson.parseJson file
        json["version"].GetValue<string>()
