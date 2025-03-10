# 1️⃣ 기본 실행 환경 설정
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2️⃣ 빌드 스테이지 (서비스 프로젝트 빌드)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["App/App.csproj", "App/"]
COPY ["Todo/Todo.csproj", "Modules/Todo/"]
COPY ["Login/Login.csproj", "Modules/Login/"]
COPY ["Email/Email/Email.csproj", "Modules/Email/"]
COPY ["User/User.csproj", "Modules/User/"]
COPY ["Shared/Shared.csproj", "Shared/Shared/"]

RUN dotnet restore "./App/App.csproj"
COPY . .
WORKDIR "/src/App"
RUN dotnet build "./App.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3️⃣ 퍼블리시 스테이지 (최종 실행 파일 생성)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4️⃣ 최종 실행 환경 (마이그레이션 적용 후 애플리케이션 실행)
FROM base AS final
WORKDIR /app

# `publish` 스테이지에서 빌드한 파일 복사
COPY --from=publish /app/publish .

# ✅ `entrypoint.sh` 추가 및 실행 권한 부여
COPY App/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

USER $APP_UID
# ✅ 컨테이너 실행 시 마이그레이션 적용 후 앱 실행
ENTRYPOINT ["/bin/sh", "/app/entrypoint.sh"]
