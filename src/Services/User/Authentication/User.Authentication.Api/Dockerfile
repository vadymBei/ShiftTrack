#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/User/Authentication/User.Authentication.Api/User.Authentication.Api.csproj", "src/Services/User/Authentication/User.Authentication.Api/"]
COPY ["src/Services/User/Authentication/User.Authentication.Core/User.Authentication.Core.csproj", "src/Services/User/Authentication/User.Authentication.Core/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication.Identity/ShiftTrack.Authentication.Identity.csproj", "src/Common/Authentication/ShiftTrack.Authentication.Identity/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication/ShiftTrack.Authentication.csproj", "src/Common/Authentication/ShiftTrack.Authentication/"]
COPY ["src/Common/Kernel/Kernel/Kernel.csproj", "src/Common/Kernel/Kernel/"]
RUN dotnet restore "./src/Services/User/Authentication/User.Authentication.Api/User.Authentication.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/User/Authentication/User.Authentication.Api"
RUN dotnet build "./User.Authentication.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User.Authentication.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Authentication.Api.dll"]