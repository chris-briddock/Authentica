Param(
    [bool]$GenerateReports = $true,
    [bool]$RunTests = $true,
    [string]$WorkingDir 
)

Function Start-Tests() 
{
    if ($runTests) 
    {
        Push-Location $WorkingDir
        dotnet test --collect:"XPlat Code Coverage" 
    }
}

Function Start-Reports()
{
    Push-Location "TestResults"
    
    if ($generateReports) 
    {
        reportgenerator -reports:"*/coverage.cobertura.xml" -targetdir:"../coveragereport" -reporttypes:Html
    }
}

Start-Tests
Start-Reports
Pop-Location
Pop-Location