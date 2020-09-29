using AutoMapper;
using BuyOrBid.Models;
using BuyOrBid.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nest;
using System;
using System.Text.Json.Serialization;

namespace BuyOrBid
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddMemoryCache();

            services.AddDbContext<MyDbContext>(options =>
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpClient<IAutoDataService, AutoDataService>();

            services.AddHttpClient<IVinDecodeService, VinDecodeService>();

            services.AddScoped<IPostService, PostService>();

            services.AddScoped<IAutoPostService, AutoPostService>();

            AddElasticsearch(services, Configuration);

            //services.AddHostedService<DatabaseSeedHostedService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:8080", "http://localhost");
            }));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                }).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<AutoPostValidator>());

            services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title = "BuyOrBid" }));
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyOrBid"));

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCaching();

            app.UseResponseCompression();

            //app.Use(async (context, next) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl =
            //        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //        {
            //            Public = true,
            //            MaxAge = TimeSpan.FromHours(1)
            //        };

            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "Accept-Encoding" };

            //    IResponseCachingFeature responseCachingFeature = context.Features.Get<IResponseCachingFeature>();

            //    if (responseCachingFeature != null)
            //    {
            //        responseCachingFeature.VaryByQueryKeys = new[] { "*" };
            //    }

            //    await next();
            //});

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public static void AddElasticsearch(IServiceCollection services, IConfiguration configuration)
        {
            string url = configuration["Elasticsearch:Url"];
            string defaultIndex = configuration["Elasticsearch:Index"];

            ConnectionSettings settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<AutoPostSearchDto>(x => x);

            ElasticClient client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
