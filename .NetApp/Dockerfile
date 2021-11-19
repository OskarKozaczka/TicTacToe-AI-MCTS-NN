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
RUN apt -y update
RUN apt install -y wget build-essential make xz-utils zlib1g-dev libffi-dev openssl libssl-dev python3-pip
RUN wget https://www.python.org/ftp/python/3.7.4/Python-3.7.4.tar.xz
RUN tar -xf Python-3.7.4.tar.xz
RUN ./Python-3.7.4/configure --enable-shared
RUN make install
RUN chmod 777 ./libpython3.7m.so
ENV LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/local/lib/
ENV PYTHONNET_PYDLL=libpython3.7m.so
RUN python3 -m pip install keras
RUN python3 -m pip install numpy
RUN python3 -m pip install tensorflow
RUN apt install mono-devel -y
RUN dotnet publish "project.csproj" -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "project.dll"]