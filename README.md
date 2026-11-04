# Server Statistics Collection Service

A C#/.NET 9 service that periodically collects Linux CPU and memory statistics
and publishes them as JSON to the RabbitMQ queue `server_statistics_queue`.

The [Message Processing and Anomaly Detection Service](https://github.com/none-empty/Message_Processing_and_Anomaly_Detection_Service)
consumes these messages for processing and anomaly detection. Run that service
separately and configure it to use the same RabbitMQ broker.

## Requirements

- Docker with Linux container support.
- Docker Compose 2.20.0 or newer.
- Internet access for the initial image downloads and build.

Docker builds the application, so a local .NET installation is not required.

## Run

1. Clone the repository and enter its directory:

   ```sh
   git clone https://github.com/none-empty/ServerStatisticsCollectionService.git
   cd ServerStatisticsCollectionService
   ```

2. Copy the configuration template:

   ```sh
   cp .env.example .env
   ```

   The defaults collect statistics with a five-second delay between samples.
   Optionally edit `SamplingIntervalSeconds` and `ServerIdentifier` in `.env`.

3. Start the service:

   ```sh
   docker compose up -d --build
   ```

   With the default configuration, Compose pulls RabbitMQ if needed and starts
   both containers on the same network. The statistics service waits for RabbitMQ
   to become healthy before starting.

4. Open [RabbitMQ Management](http://localhost:15672) on the Docker host and sign
   in with **guest / guest**. Under **Queues and Streams**, select virtual host
   **`/`** to find `server_statistics_queue` after the first sample is published.

## Use an existing RabbitMQ broker

Set all three connection values in `.env`, plus the port if needed:

```dotenv
RabbitMQ__HostName=mq.example.com
RabbitMQ__Port=5672
RabbitMQ__UserName=collector
RabbitMQ__Password=your-password
```

Then run:

```sh
docker compose up -d --build --remove-orphans
```

When hostname, username, and password are all provided, Compose starts only the
statistics service. Otherwise, it starts the bundled RabbitMQ broker. The external
hostname must be reachable from the container.

## Logs and shutdown

View service logs:

```sh
docker compose logs -f statistics
```

Stop the deployment, keeping RabbitMQ's stored data:

```sh
docker compose down --remove-orphans
```
