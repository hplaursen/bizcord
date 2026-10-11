using ChannelManagement.Infrastructure;
using ChannelManagement.Repository;
using ChannelManagement.Services;
using EasyNetQ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEasyNetQ("host=localhost;port=5673");
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
