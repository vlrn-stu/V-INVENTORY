# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["V-INVENTORY.WEB.csproj", "./"]
RUN dotnet restore "V-INVENTORY.WEB.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "V-INVENTORY.WEB.csproj" -c Release -o /app/out

# Stage 2: Build the runtime image
FROM nginx:alpine
WORKDIR /var/www/web

# Remove the default nginx static resources
RUN rm -rf ./*

# Copy the build output from the build-env image
COPY --from=build-env /app/out/wwwroot .

# Copy the nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
