#!/bin/sh

# Run database migrations
echo "Running migrations..."
if ! /root/.dotnet/tools/dotnet-ef database update --project /app/IquraStudyBE.csproj; then
  echo "Migration failed"
  exit 1
fi

# Start the application
echo "Starting the application..."
if [ "$ENVIRONMENT" = "PRODUCTION" ]; then
  echo "Running in PRODUCTION mode..."
  exec dotnet /app/IquraStudyBE.dll
else
  echo "Running in DEVELOPMENT mode..."
  exec dotnet watch run --project /app/IquraStudyBE.csproj --urls "http://0.0.0.0:8080"
fi
