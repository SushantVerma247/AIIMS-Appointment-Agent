using AppointmentAgent.Worker;
using AppointmentAgent.Worker.Config;
using AppointmentAgent.Worker.Services;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddSingleton<TelegramService>();
builder.Services.AddSingleton<PlaywrightService>();
builder.Services.AddSingleton<OrsMonitorService>();

builder.Services.AddSingleton<NotificationCacheService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
