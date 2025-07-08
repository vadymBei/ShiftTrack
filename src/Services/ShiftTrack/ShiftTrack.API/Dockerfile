#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/ShiftTrack/ShiftTrack.API/ShiftTrack.API.csproj", "src/Services/ShiftTrack/ShiftTrack.API/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication.Bearer/ShiftTrack.Authentication.Bearer.csproj", "src/Common/Authentication/ShiftTrack.Authentication.Bearer/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication/ShiftTrack.Authentication.csproj", "src/Common/Authentication/ShiftTrack.Authentication/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel/ShiftTrack.Kernel.csproj", "src/Common/Kernel/ShiftTrack.Kernel/"]
COPY ["src/Services/ShiftTrack/ShiftTrack.Core/ShiftTrack.Core.csproj", "src/Services/ShiftTrack/ShiftTrack.Core/"]
COPY ["src/Common/Client/ShiftTrack.Client.Http/ShiftTrack.Client.Http.csproj", "src/Common/Client/ShiftTrack.Client.Http/"]
COPY ["src/Common/Client/ShiftTrack.Client/ShiftTrack.Client.csproj", "src/Common/Client/ShiftTrack.Client/"]
COPY ["src/Common/Data/ShiftTrack.Data/ShiftTrack.Data.csproj", "src/Common/Data/ShiftTrack.Data/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel.CQRS/ShiftTrack.Kernel.CQRS.csproj", "src/Common/Kernel/ShiftTrack.Kernel.CQRS/"]
RUN dotnet restore "src/Services/ShiftTrack/ShiftTrack.API/ShiftTrack.API.csproj"
COPY . .
WORKDIR "/src/src/Services/ShiftTrack/ShiftTrack.API"
RUN dotnet build "./ShiftTrack.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShiftTrack.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShiftTrack.API.dll"]