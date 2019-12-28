echo off
mkdir GettingStarted
cd GettingStarted
dotnet new sln -n GettingStarted
dotnet new classlib -o GettingStarted
dotnet new xunit -o GettingStarted.Test
dotnet sln GettingStarted.sln add GettingStarted/GettingStarted.csproj
dotnet sln GettingStarted.sln add GettingStarted.Test/GettingStarted.Test.csproj
dotnet add GettingStarted.Test/GettingStarted.Test.csproj reference GettingStarted/GettingStarted.csproj
..