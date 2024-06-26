# Use the official .NET 8.0 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src/product-api

# Copy everything and restore as distinct layers
COPY . ./
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Use the official .NET 8.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /src/product-api

# Copy the published src/product-api
COPY --from=build-env /src/product-api/out .

# Set the environment variable to make the src/product-api listen on port 80 from all network interfaces
ENV ASPNETCORE_URLS=http://+:80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Dapr.API.Product.dll"]