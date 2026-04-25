FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Location/Location.Api/Location.Api.csproj", "src/Services/Location/Location.Api/"]
COPY ["src/Services/Location/Location.Infrastructure/Location.Infrastructure.csproj", "src/Services/Location/Location.Infrastructure/"]
COPY ["src/Services/Location/Location.Application/Location.Application.csproj", "src/Services/Location/Location.Application/"]
COPY ["src/Services/Location/Location.Domain/Location.Domain.csproj", "src/Services/Location/Location.Domain/"]
COPY ["src/Common/Client/ShiftTrack.Client.Http/ShiftTrack.Client.Http.csproj", "src/Common/Client/ShiftTrack.Client.Http/"]
COPY ["src/Common/Client/ShiftTrack.Client/ShiftTrack.Client.csproj", "src/Common/Client/ShiftTrack.Client/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel/ShiftTrack.Kernel.csproj", "src/Common/Kernel/ShiftTrack.Kernel/"]
COPY ["src/Common/Data/ShiftTrack.Data/ShiftTrack.Data.csproj", "src/Common/Data/ShiftTrack.Data/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel.CQRS/ShiftTrack.Kernel.CQRS.csproj", "src/Common/Kernel/ShiftTrack.Kernel.CQRS/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication/ShiftTrack.Authentication.csproj", "src/Common/Authentication/ShiftTrack.Authentication/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication.Basic/ShiftTrack.Authentication.Basic.csproj", "src/Common/Authentication/ShiftTrack.Authentication.Basic/"]
RUN dotnet restore "src/Services/Location/Location.Api/Location.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Location/Location.Api"
RUN dotnet build "./Location.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Location.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Location.Api.dll"]
