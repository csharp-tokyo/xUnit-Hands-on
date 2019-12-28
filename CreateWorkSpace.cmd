echo off
rmdir /S /Q WorkSpace
mkdir WorkSpace
cd WorkSpace
dotnet new sln -n HelloXUnit

dotnet new classlib -o HelloXUnit
dotnet sln HelloXUnit.sln add HelloXUnit/HelloXUnit.csproj

dotnet new xunit -o HelloXUnit.Test
dotnet sln HelloXUnit.sln add HelloXUnit.Test/HelloXUnit.Test.csproj
dotnet add HelloXUnit.Test/HelloXUnit.Test.csproj reference HelloXUnit/HelloXUnit.csproj

dotnet new xunit -o HelloXUnit.ParallelTest
dotnet sln HelloXUnit.sln add HelloXUnit.ParallelTest\HelloXUnit.ParallelTest.csproj

dotnet new xunit -o HelloXUnit.SharedContextTest
dotnet sln HelloXUnit.sln add HelloXUnit.SharedContextTest\HelloXUnit.SharedContextTest.csproj
dotnet add HelloXUnit.SharedContextTest/HelloXUnit.SharedContextTest.csproj package Docker.DotNet
