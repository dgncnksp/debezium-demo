# Book Review App

Book Review App is an educational project that demonstrates how a search-driven application can evolve from a simple CRUD application into a production-ready, event-driven search platform.

The application uses books as the sample domain, but the primary focus is not book management. Instead, it explores modern backend architecture patterns around **search**, **event streaming**, and **eventual consistency**.

The project starts with synchronous indexing for simplicity and gradually evolves toward an event-driven architecture powered by **Outbox Pattern**, **Debezium**, **Apache Kafka**, and independent indexing workers.

---

# Vision

The long-term goal is to build a search platform where Elasticsearch is **never updated directly by the API**.

Instead, every indexing operation will be performed asynchronously through an event pipeline.

```text
                    Client
                       │
                       ▼
               ASP.NET Core API
                       │
                       ▼
                  PostgreSQL
                       │
              Transaction Commit
                       │
                       ▼
                 Outbox Pattern
                       │
                       ▼
            Debezium (CDC Connector)
                       │
                       ▼
                 Apache Kafka
                       │
         ┌─────────────┴─────────────┐
         │                           │
         ▼                           ▼
   Go Indexing Worker        .NET Consumer
         │                           │
         └─────────────┬─────────────┘
                       ▼
                Elasticsearch
```

This architecture enables:

* Event-driven indexing
* Eventual consistency
* Loose coupling between services
* Reliable event delivery
* Independent scaling of indexing workers
* Support for multiple consumers
* Replayable event streams
* Production-ready search infrastructure

The project intentionally introduces these concepts step by step rather than implementing everything from the beginning.

---

# Current Status

At the moment, the project includes a simplified implementation:

* ASP.NET Core REST API
* Vue.js client
* PostgreSQL as the primary database
* Elasticsearch for full-text search
* Docker Compose environment
* Manual reindex endpoint
* Seed data
* Multi Match Search
* Fuzzy Search
* Custom mappings

Direct indexing from the API is currently used only to keep the early stages of the project easy to understand.

---

# Planned Architecture Evolution

The project will gradually introduce the following concepts:

## Event-Driven Architecture

* Domain Events
* Event Publishing
* Event Consumers
* Asynchronous Processing
* Eventually Consistent Read Models

## Data Synchronization

* Transactional Outbox Pattern
* Change Data Capture (CDC)
* Debezium Connector
* Apache Kafka
* Reliable Event Delivery

## Search Infrastructure

* Background indexing workers
* Go-based indexing service
* .NET consumer services
* Elasticsearch aliases
* Zero-downtime reindexing
* Index versioning
* Bulk indexing
* Incremental indexing

## Elasticsearch Features

* Custom analyzers
* Synonyms
* Autocomplete
* Search suggestions
* Highlighting
* Relevance tuning
* Sorting
* Filtering
* Aggregations

## Observability

* Health Checks
* Structured Logging
* OpenTelemetry
* Metrics
* Distributed Tracing

---

# Why This Project?

Many Elasticsearch tutorials demonstrate how to index data directly from an API.

While this approach works for small applications, production systems typically rely on asynchronous indexing pipelines.

This repository aims to bridge that gap by showing the evolution from:

```text
API
 └──► Elasticsearch
```

to

```text
API
 └──► PostgreSQL
        └──► Outbox
               └──► Debezium
                      └──► Kafka
                             └──► Worker
                                    └──► Elasticsearch
```

Each architectural step will be implemented incrementally, allowing the repository to serve both as a learning resource and as a practical reference for modern search-based systems.
