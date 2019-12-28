echo off
rmdir /S /Q WorkSpace
mkdir WorkSpace
cd WorkSpace
dotnet new sln -n HelloXUnit
dotnet new classlib -o HelloXUnit
dotnet new xunit -o HelloXUnit.Test
dotnet sln HelloXUnit.sln add HelloXUnit/HelloXUnit.csproj
dotnet sln HelloXUnit.sln add HelloXUnit.Test/HelloXUnit.Test.csproj
dotnet add HelloXUnit.Test/HelloXUnit.Test.csproj reference HelloXUnit/HelloXUnit.csproj