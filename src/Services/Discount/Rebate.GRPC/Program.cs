using Rebate.GRPC.Extensions;
using Rebate.GRPC.Repositories;
using Rebate.GRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddScoped<IRebateRepository, RebateRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddGrpc();

var app = builder.Build();
app.MigrateDatabase<Program>();
// Configure the HTTP request pipeline.
app.MapGrpcService<RebateService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
