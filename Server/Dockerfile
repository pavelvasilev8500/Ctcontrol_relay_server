FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /udpserver
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o ./publish

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /udpserver
COPY --from=build /udpserver/publish /udpserver/
EXPOSE 5000/udp
ENV SERVER_PORT=5000
ENTRYPOINT [ "dotnet", "Server.dll" ]