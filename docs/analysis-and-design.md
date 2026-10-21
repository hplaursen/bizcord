# Bizcord: Analysis and Design

## System overview

Bizcord is a real-time communication platform for users, servers, and channels. The backend is divided into microservices, each responsible for one bounded context. This is an individual implementation of the **Channel Service** bounded context.

This repository implements the **Channel Service**; the other services shown in the architecture are not implemented here.

## Domains and ownership

The workshop's written list has six services; its diagram also shows Event / Meeting Service. The table includes all seven services shown across the submission:

| Service / domain | Responsibility |
|---|---|
| User Service | Manages users and profiles. |
| Server Service | Manages servers and their structure. |
| Messaging Service | Handles messages and reactions. |
| Notification Service | Creates and delivers notifications. |
| Channel Service | Manages channels and channel settings. This is the service implemented in this repository. |
| Forum / Thread Service | Manages forum posts and threads. |
| Event / Meeting Service | Manages events and meetings. |

The workshop describes what each service is responsible for, but it does not specify exactly which data each service owns.

Channel Service owns channel metadata: channel ID, name, optional description, creation time, and active state. It provides the HTTP API to create, list, retrieve, update, and delete channels. It does not own message content or user profiles.

The current `ChannelRepository` stores channels and their message counts in memory. This is enough for the course prototype, but data is lost when the service restarts.

## Service boundaries and interactions

The submitted high-level diagram is [high-level diagram.pdf](<high-level diagram.pdf>). It shows the client communicating with User, Server, and Messaging services. It also shows message events from Messaging to Notification, and meeting events from Event / Meeting to Notification. It does not draw an interaction to Channel Service.

The W40 assignment adds a message flow not shown in the Week 36 diagram: Messaging Service publishes `MessagePostedEvent` to RabbitMQ, and Channel Service consumes it and publishes `ChannelMessageCountUpdatedEvent`. The result includes the original `MessageId` for traceability. This Track B consumer flow is implemented here.

Channel IDs use `Guid` in the API, domain model, repository, and shared event contract. This lets the message handler find and update the channel referenced by `MessagePostedEvent`.

## API and models

The API exposes:

| Method | Route | Purpose |
|---|---|---|
| GET | `/api/Channel` | List channels |
| GET | `/api/Channel/{id}` | Get one channel |
| POST | `/api/Channel` | Create a channel |
| PUT | `/api/Channel/{id}` | Update a channel |
| DELETE | `/api/Channel/{id}` | Delete a channel |

`Channel` is the domain model. `ChannelDto` is the model used by the API, and the controller maps between them. `MessagePostedEvent` and `ChannelMessageCountUpdatedEvent` are shared messaging models.

## Deployment and tests

The service is containerized with the repository's `Dockerfile`. `docker-compose.yml` starts the service and RabbitMQ; RabbitMQ is exposed on port `5673` for local development. The Dockerfile copies the `Shared.Contracts` project so the service can build from the monorepo context.

The tests cover the handler's result event, whether it accepts a valid event, and the RabbitMQ message flow. The integration test requires RabbitMQ to be running.

## Current prototype decisions

- Channel Service owns a per-channel message count. The handler increments it for each message and publishes the updated total.
- The count is in memory and resets when the service restarts.
- The raw-string `/Message` endpoint is a legacy messaging demo; it is separate from the typed `MessagePostedEvent` flow.
