# Backend of 3D Room Arrangements System

Welcome to the repository of the backend service of the project ["3D Room Arrangements using Virtual Reality"](https://github.com/MixedRealityETHZ/3d-vr-room-arrangements). It maintains the task queue and data of rooms, models, and objects. It also exposes RESTful APIs to query and manipulate items managed by specific controllers. 

![api preview](images/apis.png)

## Build

### Build Locally

The project is built with .NET 7.0. To build the project locally, install the .NET SDK for your platform from [here](https://dotnet.microsoft.com/en-us/download). run the following commands in the `RoomArrangementsBackend` directory of the project:

```bash
dotnet restore "RoomArrangementsBackend.csproj"
dotnet build "RoomArrangementsBackend.csproj" -c Release -o build
dotnet publish "RoomArrangementsBackend.csproj" -c Release -o publish /p:UseAppHost=false
```

### Build with Docker

The backend service is designed to be container-friendly. To build the Docker image, run the following command in the root directory of the project:

```bash
docker build -t vrra-backend -f RoomArrangementsBackend/Dockerfile .
```

## Run

### Run Locally

The backend service can be run locally or in a Docker container. To run the service locally, run the following command in the `publish` directory of the project:

```bash
dotnet RoomArrangementsBackend.dll
```

### Run with Docker

To run the service in a Docker container, run the following command in the root directory of the project:

```bash
docker run -p 80:80 -v /path/to/config.json:/app/appsettings.Production.json vrra-backend
```

## Service Stack

The backend service relies on the PostgreSQL database and S3-compatible object storage service. The following sample `docker-compose.yml` file can be used to start a PostgreSQL database and MinIO object storage service:

```yaml
version: "3.9"

services:
  backend: 
    image: vrra-backend
    deploy:
      restart_policy:
        condition: on-failure
    logging:
      driver: "json-file"
      options:
        max-size: "500k"
        max-file: "10"
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost"]
      interval: 1m30s
      timeout: 10s
      retries: 3
      start_period: 10s
    configs:
      - source: config
        target: /app/appsettings.Development.json
  db:
    image: postgres:15
    deploy:
      restart_policy:
        condition: on-failure
    logging:
      driver: "json-file"
      options:
        max-size: "500k"
        max-file: "10"
    healthcheck:
      test: ["pg_isready", "-U", "vrra"]
      interval: 1m30s
      timeout: 10s
      retries: 3
      start_period: 10s
    environment:
      POSTGRES_USER: vrra
      POSTGRES_PASSWORD: passwd
      POSTGRES_DB: vrra
    volumes:
      - /data/vrra-db:/var/lib/postgresql/data
  minio:
    image: minio/minio
    command: ["server", "--console-address", ":9090"]
    deploy:
      restart_policy:
        condition: on-failure
    logging:
      driver: "json-file"
      options:
        max-size: "500k"
        max-file: "10"
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:9090"]
      interval: 1m30s
      timeout: 10s
      retries: 3
      start_period: 40s
    volumes:
      - /data/minio:/data
    environment:
      MINIO_ROOT_USER: root
      MINIO_ROOT_PASSWORD: passwd
      MINIO_VOLUMES: /data

configs: 
  config:
    file: ./appsettings.Production.json
```


## Configuration

The backend service is configured with a JSON file. The default configuration file is `appsettings.json` in the `RoomArrangementsBackend` directory of the project. Production configuration can be passed as the `appsettings.Production.json` file in the working directory.

Here is a sample `appsettings.Production.json` file:

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Warning",
            "System": "Warning",
            "Microsoft": "Warning"
        }
    },
    "ConnectionStrings": {
        "DataContext": "Host=db;Database=vrra;Username=vrra;Password=passwd"
    },
    "Minio": {
        "Uri": "https://access-key:secret-key@minio:9000",
        "Bucket": "vrra",
        "PutUrlExpiry": 3600,
        "GetUrlExpiry": 3600,
        "WithSSL": true
    }
}
```
