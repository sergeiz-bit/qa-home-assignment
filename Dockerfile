# Use official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy project files
COPY . . 

# Restore dependencies
RUN dotnet restore

# Build in Release mode
RUN dotnet build --no-restore --configuration Release

# Run tests and collect coverage
CMD ["dotnet", "test", "--no-build", "--configuration", "Release", "--collect:XPlat Code Coverage", "--results-directory", "TestResults"]

