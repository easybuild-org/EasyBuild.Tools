@echo off

dotnet tool restore
@REM We need the DLL in order to use the tools from the build script
dotnet build ./src/EasyBuild.Tools.fsproj
dotnet fsi build.fsx -- %*