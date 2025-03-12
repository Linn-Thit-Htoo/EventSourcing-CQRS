## Event Sourcing with CQRS

An architectural design pattern which records every changes in an append-only log instead of saving the latest state in the database. This approach is particularly useful for the applications which need a complete history of actions instead of the latest state. You can simply replay the events in order to view the initial state. There are multiple components of event sourcing.

1. Events

- In CQRS, when the state changes, the domain event which represents a particular action is published. Each event contains all the relevant information required to rebuild the system's state at moment of an event.

2. Event Store

- The event store handles the stream of events produced by the system. Simply it is the permanent storage and we can rebuild or reconstruct states later.

3. Aggregates

- For processing commands and producing events, an aggregate is a logical collection of linked domain objects handled as a single unit.

4. Command

- Simply, commands are mutable and they tend to change the system's state. In CQRS, there are commands, queries and their respective handlers. Commands are responsible for write sides and queries for read side. The respective handler handles the business logic and processes further. In event sourcing, when the product state has been updated for example, then we have to publish domain event in order to notify the event store.

5. Event Bus

- An Event Bus enables microservices to communicate asynchronously without tightly coupling to each other. Each service simply performs its specific task without needing to know the internals of others. In a simple traditional monolith, it just follows the request-response pattern. When making a third-party API call, it just waits for a response, even if a timeout or exception occurs. This blocking behavior makes the system inefficient. With asynchronous event-driven approach, things get more complex than you think (since you are always thinking about request-response :3).The biggest advantage of asynchronous communication is resilience and the huge disadvantage is that we have to handle idempotency and eventual consistency.
