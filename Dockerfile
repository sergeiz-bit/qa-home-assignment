# Use official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy project files
COPY . . 

# Restore dependencies
RUN dotnet restore

# Build the project
RUN dotnet build --no-restore

# Run tests
CMD ["dotnet", "test", "--no-build", "--configuration", "Release", "--collect:XPlat Code Coverage", "--results-directory", "TestResults"]
