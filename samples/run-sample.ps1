param ([Parameter(Mandatory=$true)][string]$projectName)

$target = "$Env:LOCALAPPDATA\vertical-templatecopy\output"

& dotnet run -p ..\src\Vertical.Tools.TemplateCopy.csproj -- `
    .\template `
    "$target" `
    --script .\Props.cs `
    -p "name=$projectName" `
    -v verbose `
    --overwrite

Write-Host "Generated output to $target"