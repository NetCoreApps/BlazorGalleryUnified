FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

RUN apt-get update
RUN apt-get install -y ca-certificates curl gnupg
RUN mkdir -p /etc/apt/keyrings
RUN curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg
ENV NODE_MAJOR=18
RUN echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list
RUN apt-get update
RUN apt-get install -y --no-install-recommends nodejs \
 && echo "node version: $(node --version)" \
 && echo "npm version: $(npm --version)" \
 && rm -rf /var/lib/apt/lists/*

COPY ./ .
RUN dotnet restore

ARG DEPLOY_API
ARG DEPLOY_CDN

WORKDIR /app/Gallery.Unified
RUN npm run ui:build
RUN dotnet publish -c release /p:DEPLOY_API=${DEPLOY_API} /p:DEPLOY_CDN=${DEPLOY_CDN} /p:APP_TASKS=prerender -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "Gallery.Unified.dll"]