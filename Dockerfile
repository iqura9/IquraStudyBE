FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["IquraStudyBE.csproj", "./"]
RUN dotnet restore "IquraStudyBE.csproj"

# Copy the entire project and build it
COPY . .
RUN dotnet build "IquraStudyBE.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "IquraStudyBE.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final
WORKDIR /app

# Install dotnet-ef globally and set up PATH
RUN dotnet tool install -g dotnet-ef --version 7
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Copy .csproj and Migrations folder if necessary
COPY ["IquraStudyBE.csproj", "./"]
COPY . .

# Add a shell script to handle migrations and start the app
COPY migrate-and-run.sh ./
RUN chmod +x /app/migrate-and-run.sh

# Execute the script as the entrypoint
ENTRYPOINT ["./migrate-and-run.sh"]