using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// telemetry
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceA
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
            services.AddControllers();

            services.AddOpenTelemetryTracing(builder => 
            {
                // sql-tracing
                builder.AddSqlClientInstrumentation(sqlConfiguration => 
                {
                    sqlConfiguration.SetStoredProcedureCommandName = true;
                    sqlConfiguration.SetTextCommandContent = true;
                    sqlConfiguration.Enrich = Enrich;
                });
                
                // external-http-client-tracing
                builder.AddHttpClientInstrumentation();

                builder.AddAspNetCoreInstrumentation(aspNetCoreConfiguration => 
                {
                    aspNetCoreConfiguration.RecordException = true;
                    aspNetCoreConfiguration.Filter = TelemetryFilterForRequestPath;
                    // aspNetCoreConfiguration.Filter = TelemetryFilterForRequestMethod;
                    aspNetCoreConfiguration.Enrich = Enrich;
                });

                builder.AddJaegerExporter(conf => 
                {
                    conf.AgentHost = Configuration.GetSection("OpenTelemetry:Jaeger:AgentHost").Get<string>();
                    conf.AgentPort = Configuration.GetSection("OpenTelemetry:Jaeger:AgentPort").Get<int>() ;
                    conf.ProcessTags = new List<KeyValuePair<string, object>>();
                    conf.ProcessTags.Append<KeyValuePair<string, object>>(new KeyValuePair<string, object>(Configuration.GetSection("OpenTelemetry:Jaeger:ProcessTagsKey").Get<string>(), Configuration.GetSection("OpenTelemetry:Jaeger:ProcessTagsValue").Get<string>()));
                });

                builder.AddZipkinExporter(zipkinConfiguration => 
                {
                    zipkinConfiguration.Endpoint = new Uri(Configuration.GetSection("OpenTelemetry:Zipkin:EndPoint").Get<string>());
                    zipkinConfiguration.MaxPayloadSizeInBytes = 2 * 1024 * 1024;
                    zipkinConfiguration.ServiceName = Configuration.GetSection("OpenTelemetry:Zipkin:ServiceName").Get<string>();
                    zipkinConfiguration.UseShortTraceIds = true;
                });
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString, e =>
            {
                e.EnableRetryOnFailure(2);
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // ----------------------------------------------------------------
        // --------------------- TELEMETRY FILTER  ------------------------
        // ----------------------------------------------------------------
        private bool TelemetryFilterForRequestPath(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var isTracingEnabled = context.Request.Path.Value.Contains("telemetry");
            return isTracingEnabled;
        }
        private bool TelemetryFilterForRequestMethod(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var isTracingEnabled = context.Request.Method.Equals("POST");
            return isTracingEnabled;
        }
        private void Enrich(System.Diagnostics.Activity activity, string eventName, object rawObject)
        {
            activity.AddTag(Configuration.GetSection("OpenTelemetry:Common:Enrich:TagKey").Get<string>(), Configuration.GetSection("OpenTelemetry:Common:Enrich:TagValue").Get<string>());
        }
    }
}
