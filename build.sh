#!/bin/sh -x

dotnet tool restore
# We need the DLL in order to use the tools from the build script
dotnet publish ./src/EasyBuild.Tools.fsproj
dotnet fsi build.fsx -- $@
