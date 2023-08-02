# # Get base SDK Image from Microsoft
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /app

# Expose ports
EXPOSE 5002
EXPOSE 5003

# copy the CSPROJ file and restore any dependency (via NUGET)
COPY ./Project.WebApi2/Project.WebApi2.csproj ./Project.WebApi2/
WORKDIR /app/Project.WebApi2
RUN dotnet restore
WORKDIR /app
COPY . ./

# copy the projct files and build our release
WORKDIR /app/Project.WebApi2
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build-env /app/Project.WebApi2/out .
ENTRYPOINT ["dotnet" , "Project.WebApi2.dll"]