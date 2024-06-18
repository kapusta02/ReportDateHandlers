var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<ReportDateHandlers.Services.ReportDateService>();
app.MapGet("/", () => "gRPC service is running...");

app.Run();