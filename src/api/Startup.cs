using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTodo.Api;

namespace src
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Business Logic Repositories
            services.AddScoped<ListsRepository>();

            // Consolidated Connection String Extraction
            var azureSqlKey = Configuration["AZURE_SQL_CONNECTION_STRING_KEY"];
            var connectionString = !string.IsNullOrWhiteSpace(azureSqlKey) 
                ? Configuration[azureSqlKey] 
                : null;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // Local fallback target
                connectionString = Configuration.GetConnectionString("PoteDatabase");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("SQL Connection string is missing. Define 'AZURE_SQL_CONNECTION_STRING_KEY' or a local 'PoteDatabase' connection string.");
            }

            // Register the SINGLE unified database context
            services.AddDbContext<PoteDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));

            // Shared API Feature Stacks
            services.AddControllersWithViews();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors();
            services.AddApplicationInsightsTelemetry(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Database schema automation on startup (Generates all tables at once)
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var poteDb = scope.ServiceProvider.GetRequiredService<PoteDbContext>();
                poteDb.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("./openapi.yaml", "v1");
                options.RoutePrefix = ""; // Roots Swagger UI directly to http://localhost:XXXX/
            });

            app.UseStaticFiles(new StaticFileOptions { ServeUnknownFileTypes = true });
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Map the Azure template's Minimal APIs cleanly
                endpoints.MapGroup("/lists")
                         .MapTodoApi()
                         .WithOpenApi();

                // Map your hobby project's traditional controllers
                endpoints.MapControllers();
            });
        }
    }
}