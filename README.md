# MassTransitSaga

[MassTransit - Starting with Mediator](https://www.youtube.com/watch?v=dxHNAn69x6w)<br>
[MassTransit - Moving from Mediator to RabbitMQ](https://www.youtube.com/watch?v=97PXJIrGnes)<br>
[MassTransit - State Machine Sagas using Automatonymous](https://www.youtube.com/watch?v=2bPumhSTigw)

## RabbitMQ

### Docker

```powershell
docker run --name RabbitMQ -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
```

### Access 

```
http://localhost:15672/
```

## Redis

### Docker

```powershell
docker run --name Reids -p 5002:6379 -d redis
```

### Access

> Within docker shell

[Redis commands](https://redis.io/commands)

```powershell
docker ps                             # list all docker containers to obtain container id
docker exec -it <container id> sh     # run shell on the given docker
```

```shell
redis-cli     # run redis cli
scan 0        # get all stored keys
get <key>     # get all fields for the given key
```
