# ncavscreener
stock screener for net net stocks

# Code coverage
In sln root, run
dotnet test --collect:"XPlat Code Coverage"

Navigate to test result folder, then run
reportgenerator -reports:.\coverage.cobertura.xml -targetdir:"." -reporttypes:Html