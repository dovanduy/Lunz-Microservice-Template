@ECHO OFF
del /q packages\*.*
dotnet pack ../src/Lunz.Microservice.Clients/Lunz.Microservice.Clients.csproj --configuration Release --output ../../nuget/packages
pause 