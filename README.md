# MicroServices

The services are built using :

* [.Net6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) for server side code
* [Docker](https://docs.docker.com/) for service containerization 
* [MongoDB](https://www.mongodb.com/) for database storage
* [RabbitMQ](https://www.rabbitmq.com/) and [MassTransit](https://masstransit-project.com/) for message based asynchronous communication

Use dotnet run command to run Inventory and Catalog Service.  
Use docker-compose up -d (in infra project) to run mongo Db and RabbitMQ.  
The common package is placed under packages folder. 



