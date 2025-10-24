FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install dependencies for Chrome
RUN apt-get update && apt-get install -y wget gnupg ca-certificates fonts-liberation \
    libasound2 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 libexpat1 \
    libfontconfig1 libgcc1 libgdk-pixbuf2.0-0 libglib2.0-0 libgtk-3-0 libnspr4 \
    libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 \
    libxcomposite1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 \
    libxrandr2 libxrender1 libxss1 libxtst6 lsb-release xdg-utils && \
    rm -rf /var/lib/apt/lists/*

# Install Chrome Stable
RUN apt-get update && apt-get install -y chromium \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Generators/Pdf/Generators.Pdf.Api/Generators.Pdf.Api.csproj", "src/Services/Generators/Pdf/Generators.Pdf.Api/"]
COPY ["src/Services/Generators/Pdf/Generators.Pdf.Application/Generators.Pdf.Application.csproj", "src/Services/Generators/Pdf/Generators.Pdf.Application/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel.CQRS/ShiftTrack.Kernel.CQRS.csproj", "src/Common/Kernel/ShiftTrack.Kernel.CQRS/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel/ShiftTrack.Kernel.csproj", "src/Common/Kernel/ShiftTrack.Kernel/"]
COPY ["src/Services/Generators/Pdf/Generators.Pdf.Infrastructure/Generators.Pdf.Infrastructure.csproj", "src/Services/Generators/Pdf/Generators.Pdf.Infrastructure/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication.Basic/ShiftTrack.Authentication.Basic.csproj", "src/Common/Authentication/ShiftTrack.Authentication.Basic/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication/ShiftTrack.Authentication.csproj", "src/Common/Authentication/ShiftTrack.Authentication/"]
RUN dotnet restore "src/Services/Generators/Pdf/Generators.Pdf.Api/Generators.Pdf.Api.csproj"
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Generators.Pdf.Api.dll"]
