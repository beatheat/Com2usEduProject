using Com2usEduProject.ModelDB;
using Com2usEduProject.Services;
using Com2usEduProject.Tools;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
var conf = configuration.GetSection(nameof(DbConnectionConfig));

builder.Services.Configure<DbConnectionConfig>(configuration.GetSection(nameof(DbConnectionConfig)));

builder.Services.AddTransient<IAccountDb, AccountDb>();
builder.Services.AddSingleton<IMemoryDb, RedisDb>();
builder.Services.AddControllers();

LogManager.SettingLogger(builder.Logging, configuration["LogDirectory"]);

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
LogManager.SetLoggerFactory(loggerFactory, "Global");

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


var redisDb = app.Services.GetRequiredService<IMemoryDb>();
redisDb.Init(configuration.GetSection("DbConnectionConfig")["Redis"]);

// app.UseLoadRequestDataMiddlerWare();
// app.UseCheckUserSessionMiddleWare();
//


// DBManager.Init(configuration);

app.Run(configuration["ServerAddress"]);