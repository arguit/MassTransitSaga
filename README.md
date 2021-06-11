# MassTransitSaga

[MassTransit - Starting with Mediator](https://www.youtube.com/watch?v=dxHNAn69x6w)<br>
[MassTransit - Moving from Mediator to RabbitMQ](https://www.youtube.com/watch?v=97PXJIrGnes)<br>
[MassTransit - State Machine Sagas using Automatonymous](https://www.youtube.com/watch?v=2bPumhSTigw)

## Docker prerequisities

```powershell
docker run --name Reids -p 5002:6379 -d redis
```

```powershell
docker run --name RabbitMQ -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
```
