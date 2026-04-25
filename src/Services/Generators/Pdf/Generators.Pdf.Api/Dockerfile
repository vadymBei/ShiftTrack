# =========================
# 1. Base runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

EXPOSE 8080

# Install Google Chrome and its dependencies
RUN apt-get update && apt-get install -y \
    wget \
    gnupg \
    && wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | gpg --dearmor > /usr/share/keyrings/google-archive-keyring.gpg \
    && echo "deb [arch=amd64 signed-by=/usr/share/keyrings/google-archive-keyring.gpg] http://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google.list \
    && apt-get update && apt-get install -y \
    google-chrome-stable \
    fonts-liberation \
    fonts-dejavu-core \
    fonts-freefont-ttf \
    libasound2t64 \
    libatk-bridge2.0-0t64 \
    libatk1.0-0t64 \
    libcups2t64 \
    libdrm2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxrandr2 \
    libgbm1 \
    libgtk-3-0t64 \
    libnss3 \
    libxss1 \
    libxtst6 \
    xdg-utils \
    ca-certificates \
    --no-install-recommends \
    && rm -rf /var/lib/apt/lists/*

ENV PUPPETEER_EXECUTABLE_PATH=/usr/bin/google-chrome-stable
ENV ASPNETCORE_URLS=http://+:8080

# =========================
# 2. Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# copy csproj (cache optimization)
COPY ["src/Services/Generators/Pdf/Generators.Pdf.Api/Generators.Pdf.Api.csproj", "src/Services/Generators/Pdf/Generators.Pdf.Api/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication.Basic/ShiftTrack.Authentication.Basic.csproj", "src/Common/Authentication/ShiftTrack.Authentication.Basic/"]
COPY ["src/Common/Authentication/ShiftTrack.Authentication/ShiftTrack.Authentication.csproj", "src/Common/Authentication/ShiftTrack.Authentication/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel/ShiftTrack.Kernel.csproj", "src/Common/Kernel/ShiftTrack.Kernel/"]
COPY ["src/Common/Kernel/ShiftTrack.Kernel.CQRS/ShiftTrack.Kernel.CQRS.csproj", "src/Common/Kernel/ShiftTrack.Kernel.CQRS/"]
COPY ["src/Services/Generators/Pdf/Generators.Pdf.Application/Generators.Pdf.Application.csproj", "src/Services/Generators/Pdf/Generators.Pdf.Application/"]
COPY ["src/Services/Generators/Pdf/Generators.Pdf.Infrastructure/Generators.Pdf.Infrastructure.csproj", "src/Services/Generators/Pdf/Generators.Pdf.Infrastructure/"]

RUN dotnet restore "src/Services/Generators/Pdf/Generators.Pdf.Api/Generators.Pdf.Api.csproj"

# copy everything else
COPY . .

WORKDIR "/src/src/Services/Generators/Pdf/Generators.Pdf.Api"
RUN dotnet publish "Generators.Pdf.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# 3. Final image
# =========================
FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Generators.Pdf.Api.dll"]