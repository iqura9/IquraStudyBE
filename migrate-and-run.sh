#!/bin/sh

# Run database migrations
echo "Running migrations..."
/root/.dotnet/tools/dotnet-ef database update --project /app/IquraStudyBE.csproj || { echo "Migration failed"; exit 1; }

# Start the application
echo "Starting the application..."
exec dotnet IquraStudyBE.dll