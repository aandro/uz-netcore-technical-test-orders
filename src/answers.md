1. Resilience side-effects
	- Sending multiple requests to the same endpoint is a highly likely scenario.
		It can be due to retry policy or API abuse. 
		One way to hadle it would be adding pre-flight check to verify if entity with the same id already exists.
		In this case API can return different status code, depending on semantics (200, 409, 303, etc.)

2. From the message producer standpoint
	- It is possible, order of entities created and messages might be different. We can introduce CreatedDateTime field, add index in db and sort data. 
		In current implementation I suggest adding it to Message that is persisted. 
		Index and sorting add additional overhead.
	
	- It is possible, that message might be dublicated. Transactional outbox has at-least-once delivery guarantee.
		To counteract that, we can select other absctraction/instrument with exactly-once guarantee (stream processing, for example).
		Or handle possible duplicates on consumer's side.
		
3. From the message consumer standpoint
	- Messages are consumed from queue on FIFO basis. 
		However, with different messaging topologies there's a chance that they might be consumed in different order.
		In some event-sourcing systems there's GlobalEventIndex per aggregate (aggregate version). 
		It ensures that "past" events do not overwrite aggregate state that is a result of more recent events.
		
	- It is possible that with at-least-once guarantee there would be message duplicates.
		To avoid consuming duplicate messages we can either manually check state, based on semantics of change.
		Or if 3rd party solution is used (like EventFlow), there's built-in mechanism which skips previous events when more recent events were already applied.

4. Message semantics
	- System reacts on command and produces domain event. Each command should leave a trace in a system with respective domain event.
	- I don't think so; in the end, both approaches produce event, so nothing really changes for consumer
	- Inside OrderService itself - probably command handler and infrastructure code that will handle event persistence, aggregate version etc.
		In RabbitMQ topology - additional exchange for commands (and exchange/queue for events also stays). In case of MassTransit it is created automatically
		As a consumer - no changes needed, if event schema stays the same (no v2 events, etc.)
		