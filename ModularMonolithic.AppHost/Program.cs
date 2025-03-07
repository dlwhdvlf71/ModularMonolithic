var builder = DistributedApplication.CreateBuilder(args);


var redis = builder.AddRedis("modularmonolithic-redis")
    .WithDataVolume()
    .WithImage("redis:8.0-M03");

var postgres = builder.AddPostgres("modularmonolithic-postgres")
    .WithDataVolume()
    .WithImage("postgres:17.2")
    .WithPgAdmin();

var rabbitmq = builder.AddRabbitMQ("modularmonolithic-rabbitmq")
    .WithDataVolume()
    .WithManagementPlugin()
    .WithImage("rabbitmq:management")
    ;
    

builder.AddProject<Projects.App>("app")
    .WithReference(redis)
    .WithReference(postgres)
    .WithReference(rabbitmq)
    ;


builder.Build().Run();
