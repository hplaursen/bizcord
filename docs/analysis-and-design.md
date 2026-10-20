# Bizcord: Analysis and Design

## System overview

Bizcord is a real-time communication platform for users, servers, and channels. The backend is divided into microservices, each responsible for one bounded context. This is an individual implementation of the **Channel Service** bounded context.

This repository contains Channel Service only; the other services shown in the course architecture are outside this individual implementation.

## Domains and ownership

The high-level architecture separates these responsibilities:

| Service / domain | Responsibility and owned data |
|---|---|
| User Service | User accounts and profiles. |
| Server Service | Servers and their configuration or structure. |
| Channel Service | Channels and channel settings within servers. This is the service implemented in this repository. |
| Messaging Service | Messages, reactions, and message delivery. |
| Notification Service | User notifications and delivery. |
| Forum / Thread Service | Forum posts and threads within servers. |
| Event / Meeting Service | Events and meetings associated with servers. |

Channel Service owns channel metadata: channel ID, name, optional description, creation time, and active state. It provides the HTTP API to create, list, retrieve, update, and delete channels. It does not own message content or user profiles.

The current `ChannelRepository` stores channels and their message counts in memory. This is enough for the course prototype, but data is lost when the service restarts.

## Service boundaries and interactions

The high-level diagram supplied for the project is [Bizcord_High_Level_Architecture_v2.drawio](Bizcord_High_Level_Architecture_v2.drawio). It shows the client communicating with backend services and the Server Service connected to channel, forum/thread, and event/meeting domains.

For the W40 message flow, the Messaging Service publishes `MessagePostedEvent` to RabbitMQ. Channel Service consumes it and publishes `ChannelMessageCountUpdatedEvent`; the result includes the original `MessageId` for traceability. This is the Track B consumer flow implemented here.

The supplied diagram versions differ in the direction of the Channel/Messaging connection. The event direction above follows the W40 assignment's explicit `MessagePostedEvent` flow.

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
- The domain boundaries above are based on the W40/W41 assignment brief and the code in this repository. Add or correct details from the Week 36 project description and workshop before final submission.
