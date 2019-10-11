Push-Location .\test

& Remove-Item -Path ./.coverage/* -Recurse
& dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
& New-Item -ItemType Directory -Force -Path ./.coverage
& Move-Item -Path ./coverage.cobertura.xml -Destination ./.coverage
& dotnet reportgenerator -reports:./.coverage/coverage.cobertura.xml -reporttypes:html -sourcedirs:../src -targetdir:./.coverage

Pop-Location