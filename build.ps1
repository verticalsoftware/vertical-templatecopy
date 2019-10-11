Push-Location ./src

& dotnet build -c Debug

Pop-Location

& ./cover.ps1