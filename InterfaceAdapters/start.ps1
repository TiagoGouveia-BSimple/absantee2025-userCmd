# parameter definition
param (
    [int]$DbPort = 5432,
    [int]$MicroservicePort = 5074,
    [string]$BaseDatabaseName = "absanteeCmd",
    [string]$DbUser = "postgres",
    [string]$DbPassword = "postgres",
    [string]$DbHost = "localhost",
    [string]$MicroservicePath = "."
)

# database creation
$Guid = [guid]::NewGuid().ToString().Substring(0, 8)
$NewDatabaseName = "${BaseDatabaseName}_$Guid"

Write-Host "creating database $NewDatabaseName"

# set environment variable to password so we aren't prompted
$env:PGPASSWORD = $DbPassword

$createDbCommand = "CREATE DATABASE `"$NewDatabaseName`";"
psql -h $DbHost -U $DbUser -p $DbPort -d postgres -c $createDbCommand

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to create database. Exiting."
    exit 1
}

Write-Host "database $NewDatabaseName created successfully"

# clear environment variable post usage
Remove-Item Env:PGPASSWORD

$newConnectionString = "Host=$DbHost;Database=$NewDatabaseName;Username=$DbUser;Password=$DbPassword"

Write-Host "Launching microservice with connection string: $newConnectionString"

# define the connectionstring and port for the microservice
$env:ConnectionStrings__DefaultConnection = $newConnectionString
$env:ASPNETCORE_URLS = "http://localhost:$MicroservicePort"

# launch the microservice
dotnet run --project . --no-launch-profile

# clear environment variables post usage
Remove-Item Env:ConnectionStrings__DefaultConnection
Remove-Item Env:ASPNETCORE_URLS
