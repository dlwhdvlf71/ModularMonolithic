#!/bin/sh

echo "Applying migrations..."
dotnet ef database update --context TodoDbContext --project Modules/Todo/Todo --startup-project src/App
dotnet ef database update --context UserDbContext --project Modules/User/User --startup-project src/App

echo "Starting application..."
exec dotnet App.dll
