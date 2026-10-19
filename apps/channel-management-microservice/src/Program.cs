using ChannelManagement.Infrastructure;
using ChannelManagement.Repository;
using ChannelManagement.Services;
using EasyNetQ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var rabbitMqHost = builder.Configuration["RABBITMQ_HOST"] ?? "localhost";
var rabbitMqPort = builder.Configuration["RABBITMQ_PORT"] ?? "5673";
builder.Services.AddEasyNetQ($"host={rabbitMqHost};port={rabbitMqPort}");
builder.Services.AddSingleton<IMessageClient, MessageClient>();
builder.Services.AddHostedService<MessageSubscriber>();
builder.Services.AddSingleton<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<IChannelService, ChannelService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
   // app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
