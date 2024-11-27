module EasyBuild.Tools.PackageJson

open System.Text.Json
open System.Text.Json.Nodes
open System.IO

type PackageJson =

    static member replaceVersion(file: FileInfo, newVersion: string) =
        let rawText = File.ReadAllText(file.FullName)
        let json = JsonNode.Parse(rawText)
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
