using ReportDateHandlers.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<DataProcessingService>();
app.MapGet("/",() => "gRPC service is running...");

app.Run();