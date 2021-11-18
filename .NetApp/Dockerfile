#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base


WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["project.csproj", "."]
RUN dotnet restore "./project.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "project.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "project.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update
RUN apt install wget -y
RUN apt install build-essential -y
RUN apt install make -y
RUN apt install xz-utils -y
RUN apt install zlib1g-dev -y
RUN apt install libffi-dev -y
RUN wget https://www.python.org/ftp/python/3.7.4/Python-3.7.4.tar.xz
RUN tar -xf Python-3.7.4.tar.xz
RUN cd Python-3.7.4
RUN ./Python-3.7.4/configure --enable-shared --enable-optimizations --prefix=/usr/local/lib
RUN make install
RUN export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/local/lib/
ENTRYPOINT ["dotnet", "project.dll"]