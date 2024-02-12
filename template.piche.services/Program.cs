using Piche.Core;
using Piche.Core.Api;
using Piche.Core.Cors;
using Piche.Core.Culture;
using Piche.Core.Logging;
using Piche.Core.Metrics;
using Piche.Core.Mongo;
using Piche.Core.Mvc;
using Piche.Core.RabbitMq;
using Piche.Core.RabbitMq.Dispatchers;
using Piche.Core.Redis;
using Piche.Core.Security;
using Piche.Core.Vault;
using ServiceLocator;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseVault();


builder.Services.AddControllers();
builder.Services
    .AddAppMetrics()
    .AddSwagger()
    .AddPicheIdentity()
    .AddPicheApiVersioning()
    .AddDispatchers()
    .AddPicheCors()
    .AddServiceLocator<Program>()
    .AddMongo()
    .AddCustomMvc()
    .AddLogging()
    .AddRedis()
    .AddRabbitMq()
    .AddPicheCulture();


builder.WebHost.UseLogging();

var app = builder.Build();

app.UsePicheCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseServiceId()
    .UseUserTagging()
    .UsePicheSwagger(app)
    .UsePicheCulture();

app.UseRabbitMq();

app.MapControllers();

var initializer = app.Services.GetRequiredService<IStartupInitializer>();
await initializer.InitializeAsync();

app.Run();