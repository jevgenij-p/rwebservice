using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RWebService.Logic;
using System.Linq;
using System.Reflection;

namespace RWebService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfigurationRoot Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();
            services.Configure<ScriptSettings>(Configuration.GetSection("ScriptSettings"));
            services.Configure<InterpreterSettings>(Configuration.GetSection("InterpreterSettings"));
            services.AddSingleton(provider => Configuration);
            services.AddTransient<IRouteParser, RouteParser>();
            services.AddTransient<IScriptsManager, ScriptsManager>();
            services.AddTransient<IInterpretersManager, InterpretersManager>();
            services.Configure((RazorViewEngineOptions options) =>
            {
                var previous = options.CompilationCallback;
                options.CompilationCallback = (context) =>
                {
                    previous?.Invoke(context);

                    var assembly = typeof(Startup).GetTypeInfo().Assembly;
                    var assemblies = assembly.GetReferencedAssemblies().Select(x => MetadataReference.CreateFromFile(Assembly.Load(x).Location)).ToList();
                    context.Compilation = context.Compilation.AddReferences(assemblies);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddEventLog();

            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    "catchall",
                    "{*catchall}",
                    new { controller = "Command", action = "Index" });
            });
        }
    }
}