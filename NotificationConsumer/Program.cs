using NotificationConsumer.Hubs;
using NotificationConsumer.Services;
using NotificationConsumer.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add configuration for RabbitMQ
builder.Configuration.GetSection("RabbitMq").Get<RabbitMQConfiguration>();
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMq"));

// Add SignalR and register the NotificationsHub
builder.Services.AddSignalR();
builder.Services.AddHostedService<NotificationConsumerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationsHub>("/notificationsHub");
});

app.Run();