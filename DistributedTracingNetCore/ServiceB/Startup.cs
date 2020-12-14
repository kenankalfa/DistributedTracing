using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace ServiceB
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
                builder.AddAspNetCoreInstrumentation(aspNetCoreConfiguration =>
                {
                    aspNetCoreConfiguration.Enrich = Enrich;
                });

                builder.AddZipkinExporter(zipkinConfiguration =>
                {
                    zipkinConfiguration.Endpoint = new Uri(Configuration.GetSection("OpenTelemetry:Zipkin:EndPoint").Get<string>());
                    zipkinConfiguration.MaxPayloadSizeInBytes = 2 * 1024 * 1024;
                    zipkinConfiguration.ServiceName = Configuration.GetSection("OpenTelemetry:Zipkin:ServiceName").Get<string>();
                    zipkinConfiguration.UseShortTraceIds = true;
                });

                builder.AddJaegerExporter(conf =>
                {
                    conf.AgentHost = Configuration.GetSection("OpenTelemetry:Jaeger:AgentHost").Get<string>();
                    conf.AgentPort = Configuration.GetSection("OpenTelemetry:Jaeger:AgentPort").Get<int>();
                    conf.ProcessTags = new List<KeyValuePair<string, object>>();
                    conf.ProcessTags.Append<KeyValuePair<string, object>>(new KeyValuePair<string, object>(Configuration.GetSection("OpenTelemetry:Jaeger:ProcessTagsKey").Get<string>(), Configuration.GetSection("OpenTelemetry:Jaeger:ProcessTagsValue").Get<string>()));
                });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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

            var lifetime = app.ApplicationServices.GetService<Microsoft.Extensions.Hosting.IHostApplicationLifetime>();
            IStatistics statistics = new Statistics();

            lifetime.ApplicationStarted.Register(() =>
            {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender(Configuration.GetSection("OpenTelemetry:Zipkin:HttpZipkinSender").Get<string>(), "application/json");
                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), statistics);
                TraceManager.RegisterTracer(tracer);
                TraceManager.Start(logger);
            });
            lifetime.ApplicationStopped.Register(() => TraceManager.Stop());
            app.UseTracing(Configuration.GetSection("OpenTelemetry:Zipkin:ServiceName").Get<string>());
        }
        private void Enrich(System.Diagnostics.Activity activity, string eventName, object rawObject)
        {
            activity.AddTag(Configuration.GetSection("OpenTelemetry:Common:Enrich:TagKey").Get<string>(), Configuration.GetSection("OpenTelemetry:Common:Enrich:TagValue").Get<string>());
        }
    }
}
