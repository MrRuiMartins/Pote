using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using SimpleTodo.Api;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"];
if (!string.IsNullOrWhiteSpace(keyVaultEndpoint))
{
    var credential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultEndpoint), credential);
}

builder.Services.AddScoped<ListsRepository>();
builder.Services.AddDbContext<TodoDb>(options =>
{
    var connectionStringKey = builder.Configuration["AZURE_SQL_CONNECTION_STRING_KEY"];
    if (string.IsNullOrWhiteSpace(connectionStringKey))
    {
        throw new InvalidOperationException("AZURE_SQL_CONNECTION_STRING_KEY is not configured.");
    }

    var connectionString = builder.Configuration[connectionStringKey];
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException($"Connection string for key '{connectionStringKey}' is not configured.");
    }

    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.AddDbContext<src.DbModels.InterestingLinkContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PoteDatabase")));

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var todoDb = scope.ServiceProvider.GetRequiredService<TodoDb>();
    await todoDb.Database.EnsureCreatedAsync();

    var linkDb = scope.ServiceProvider.GetRequiredService<src.DbModels.InterestingLinkContext>();
    await linkDb.Database.EnsureCreatedAsync();
}

Console.WriteLine("app started");

app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("./openapi.yaml", "v1");
    options.RoutePrefix = "";
});

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
});

app.MapGroup("/lists")
    .MapTodoApi()
    .WithOpenApi();

app.MapControllers();

app.Run();