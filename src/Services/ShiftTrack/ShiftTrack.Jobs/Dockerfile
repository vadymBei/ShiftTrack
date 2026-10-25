FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/ShiftTrack/ShiftTrack.Jobs/ShiftTrack.Jobs.csproj", "src/Services/ShiftTrack/ShiftTrack.Jobs/"]
COPY ["src/Common/Scheduling/ShiftTrack.Scheduling/ShiftTrack.Scheduling.csproj", "src/Common/Scheduling/ShiftTrack.Scheduling/"]
COPY ["src/Services/ShiftTrack/ShiftTrack.Application/ShiftTrack.Application.csproj", "src/Services/ShiftTrack/ShiftTrack.Application/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication/ShiftTrack.Authentication.csproj", "src/Common/Authentication/ShiftTrack.Authentication/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel/ShiftTrack.Kernel.csproj", "src/Common/Kernel/ShiftTrack.Kernel/"]
COPY ["src/Common/Data/ShiftTrack.Data/ShiftTrack.Data.csproj", "src/Common/Data/ShiftTrack.Data/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel.CQRS/ShiftTrack.Kernel.CQRS.csproj", "src/Common/Kernel/ShiftTrack.Kernel.CQRS/"]
COPY ["src/Services/ShiftTrack/ShiftTrack.Domain/ShiftTrack.Domain.csproj", "src/Services/ShiftTrack/ShiftTrack.Domain/"]
COPY ["src/Services/ShiftTrack/ShiftTrack.Infrastructure/ShiftTrack.Infrastructure.csproj", "src/Services/ShiftTrack/ShiftTrack.Infrastructure/"]
COPY ["src/Common/Client/ShiftTrack.Client.Http/ShiftTrack.Client.Http.csproj", "src/Common/Client/ShiftTrack.Client.Http/"]
COPY ["src/Common/Client/ShiftTrack.Client/ShiftTrack.Client.csproj", "src/Common/Client/ShiftTrack.Client/"]
RUN dotnet restore "src/Services/ShiftTrack/ShiftTrack.Jobs/ShiftTrack.Jobs.csproj"
COPY . .
WORKDIR "/src/src/Services/ShiftTrack/ShiftTrack.Jobs"
RUN dotnet build "./ShiftTrack.Jobs.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShiftTrack.Jobs.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShiftTrack.Jobs.dll"]
