FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["CQUCloudWeb/CQUCloudWeb.csproj", "CQUCloudWeb/"]
RUN dotnet restore "CQUCloudWeb/CQUCloudWeb.csproj"
COPY . .
WORKDIR "/src/CQUCloudWeb"
RUN dotnet build "CQUCloudWeb.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CQUCloudWeb.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CQUCloudWeb.dll"]