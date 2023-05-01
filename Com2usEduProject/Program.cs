using Com2usEduProject.Middleware;
using Com2usEduProject.ModelDB;
using Com2usEduProject.Services;
using Com2usEduProject.Tools;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
var conf = configuration.GetSection(nameof(DbConnectionConfig));

builder.Services.Configure<DbConnectionConfig>(configuration.GetSection(nameof(DbConnectionConfig)));

builder.Services.AddTransient<IAccountDb, AccountDb>();
builder.Services.AddTransient<IGameDb, GameDb>();
builder.Services.AddSingleton<IMemoryDb, RedisDb>();
builder.Services.AddSingleton<IMasterDb, MasterDb>();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

LogManager.SettingLogger(builder.Logging, configuration["LogDirectory"]);

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
LogManager.SetLoggerFactory(loggerFactory, "Global");

app.UseMiddleware<CheckUserAuth>();

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


var redisDb = app.Services.GetRequiredService<IMemoryDb>();
redisDb.Init(configuration.GetSection("DbConnectionConfig")["Redis"]);

var masterDb = app.Services.GetRequiredService<IMasterDb>();
masterDb.Init(configuration.GetSection("DbConnectionConfig")["MasterDb"]);



app.Run(configuration["ServerAddress"]);