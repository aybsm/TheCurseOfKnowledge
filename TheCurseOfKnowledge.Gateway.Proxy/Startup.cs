using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarp.ReverseProxy;
using Microsoft.AspNetCore.Cors;
using TheCurseOfKnowledge.Gateway.Proxy.Repositories;
using Serilog;

namespace TheCurseOfKnowledge.Gateway.Proxy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options
                => options.AddPolicy("AllowAll", builder
                    => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    // Penting untuk gRPC-Web: Ekspos header khusus gRPC
                    .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")));

            services.AddReverseProxy()
                .LoadFromConfig(Configuration.GetSection("ReverseProxy"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TheCurseOfKnowledge.Gateway.Proxy", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheCurseOfKnowledge.Gateway.Proxy v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints
                =>
            {
                endpoints.MapReverseProxy();
                endpoints.MapControllers();
            });
        }
    }
}
