
# Use the official microsoft dotnet image with .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-stage

# Set working directory for the build process
WORKDIR /app

# Copy the project (replace 'your-api-project' with your actual project name)
COPY apibronco.net .

# Restore dependencies
RUN dotnet restore "./apibronco.bronco.com.br.csproj"

# Publish the application in release mode (output goes to /app/publish)
RUN dotnet publish "./apibronco.bronco.com.br.csproj" -c Release -o /app/publish

# Use a slimmer runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final-stage

# Set working directory within the final image
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build-stage /app/publish .

# Expose port (replace 5000 with your desired port)
EXPOSE 5000

# Start the application using dotnet
CMD ["dotnet", "apibronco.bronco.com.br.dll"]
