#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/User/Authentication/User.Authentication.Server/User.Authentication.Server.csproj", "src/Services/User/Authentication/User.Authentication.Server/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel/ShiftTrack.Kernel.csproj", "src/Common/Kernel/ShiftTrack.Kernel/"]
COPY ["src/Common/Data/ShiftTrack.Data/ShiftTrack.Data.csproj", "src/Common/Data/ShiftTrack.Data/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication.Identity/ShiftTrack.Authentication.Identity.csproj", "src/Common/Authentication/ShiftTrack.Authentication.Identity/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication/ShiftTrack.Authentication.csproj", "src/Common/Authentication/ShiftTrack.Authentication/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel.CQRS/ShiftTrack.Kernel.CQRS.csproj", "src/Common/Kernel/ShiftTrack.Kernel.CQRS/"]
COPY ["src/Services/User/Authentication/User.Authentication.Infrastructure/User.Authentication.Infrastructure.csproj", "src/Services/User/Authentication/User.Authentication.Infrastructure/"]
COPY ["src/Services/User/Authentication/User.Authentication.Application/User.Authentication.Application.csproj", "src/Services/User/Authentication/User.Authentication.Application/"]
COPY ["src/Services/User/Authentication/User.Authentication.Domain/User.Authentication.Domain.csproj", "src/Services/User/Authentication/User.Authentication.Domain/"]
RUN dotnet restore "src/Services/User/Authentication/User.Authentication.Server/User.Authentication.Server.csproj"
COPY . .
WORKDIR "/src/src/Services/User/Authentication/User.Authentication.Server"
RUN dotnet build "./User.Authentication.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User.Authentication.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Authentication.Server.dll"]