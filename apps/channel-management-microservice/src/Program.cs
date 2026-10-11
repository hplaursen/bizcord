using ChannelManagement;
using EasyNetQ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEasyNetQ("host=localhost");
builder.Services.AddSingleton<IMessageClient, MessageClient>();
builder.Services.AddHostedService<MessageSubscriber>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
   // app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();