# # Get base SDK Image from Microsoft
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /app

# Expose ports
EXPOSE 5000
EXPOSE 5001

# copy the CSPROJ file and restore any dependency (via NUGET)
COPY ./Project.WebApi/Project.WebApi.csproj ./Project.WebApi/
WORKDIR /app/Project.WebApi
RUN dotnet restore
WORKDIR /app
COPY . ./

# copy the projct files and build our release
WORKDIR /app/Project.WebApi
RUN dotnet publish -c Release -o out

# ef migration
# RUN dotnet tool install --global dotnet-ef
# ENV PATH="$PATH:/root/.dotnet/tools"
# RUN dotnet ef database update -c DbContexts


# Generate runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build-env /app/Project.WebApi/out .
ENTRYPOINT ["dotnet" , "Project.WebApi.dll"]