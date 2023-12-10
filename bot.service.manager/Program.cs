using bot.service.manager.IService;
using bot.service.manager.Model;
using bot.service.manager.Service;
using Bot.Service.Manager.MiddlewareServices;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IFolderDiscoveryService, FolderDiscoveryService>();
builder.Services.AddScoped<IActionService, ActionService>();
builder.Services.AddScoped<PodHelper>();
builder.Services.AddScoped<CommonService>();
builder.Services.AddScoped<EditorService>();
builder.Services.AddSingleton<YamlUtilService>();
builder.Services.AddSingleton<KubeFileConverter>();
builder.Services.AddScoped<RemoteServerConfig>();

builder.Services.Configure<RemoteServerConfig>(x => builder.Configuration.GetSection(nameof(RemoteServerConfig)).Bind(x));


var targetDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "k8-workspace"));

if (!Directory.Exists(targetDirectory))
    Directory.CreateDirectory(targetDirectory);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "K8Service Manager API");
        option.RoutePrefix = "api";
    });
}
app.UseCors();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
