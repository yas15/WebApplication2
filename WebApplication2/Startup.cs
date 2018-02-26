using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace WebApplication2
{
    public class Startup
    {
        // An instance of the Startup class is created by the WebHost class, when the Main() method in the Program
        // class is executed i.e. when we start the appliaction.
        // If the ConfigureServices() method exists it will automatically be called by the ASP.NET core runtime. 
        // By default ConfigureServices has one parameter "services" of type IServiceCollection. The IServiceCollection parameter 
        // i.e. "services" parameter is a dependency injection container and we Use this method to add services to that container.
        // Once we have added a service to the container "services" it will be available to use anywhere in our application.
        // The services we add to the container are resolved via dependency injection i.e. when we add a service to the container we 
        // specify for each interface type required by the application the concrete class the container must create an instance of.
        // The ASP.Net Core framework  automatically provides the following services to the IServiceCollection container,
        // (so we do not need to add them in the ConfigureServices() method), these include:
        // IHostingEnvironment,ILoggerFactory,ILogger<T>,IApplicationBuilderFactory,IHttpContextFactory,IOptions<T>,DiagnosticSource,         
        // DiagnosticListener, IStartupFilter,ObjectPoolProvider,IConfigureOptions<T>, IServer,IStartup,IApplicationLifetime
        // For more information see: https://go.microsoft.com/fwlink/?LinkID=398940
        // Also see: https://codingblast.com/asp-net-core-configureservices-vs-configure/
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc() add a service to the ASP.Net core web application that 
            // tells the application to use MVC to handle any http requests.
            services.AddMvc();


            // ASP.NET Core by default deserializes from and serializes to JSON. 
            // We can configure the JSON serialization settings in the ConfigureServices() method
            services.AddMvc().AddJsonOptions(o =>
            {
                if (o.SerializerSettings.ContractResolver != null)
                {
                    var castedResolver = o.SerializerSettings.ContractResolver
                        as DefaultContractResolver;
                    castedResolver.NamingStrategy = null;
                }
            });


        }

        // This method gets called by the ASP.NET core runtime after the ConfigureServices() method has been called.
        // The Configure method is used to set up the middleware components in the HTTP request pipeline, i.e. for every HTTP 
        // request message that arrives to the server, this method defines how that HTTP request is handled by our web application.
        // ASP.NET Core includes a simple built-in DI container, that already has some services registered (by service
        // we mean a concrete class type that is managed by the DI container).
        // Remember that ASP.NET core uses dependency injection, so each time an interface type is required by a method, the 
        // .Net core runtime will check the DI container to see if a concrete class has been registered for that interface
        // and then return an instance of that concrete class.

        // To the Configure() method, we have added the parameter loggerFactory of type ILoggerFactory. 
        // The ASP.NET Core built-in DI container already has a service registered for interface of type ILoggerFactory
        // so we do not need to register this service with the container and the DI container will create an instance 
        // of this type for us.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Adds a console logger  to the appliaction
            loggerFactory.AddConsole();

            //Adds a debug logger  to the appliaction
            loggerFactory.AddDebug();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }


            ////app.Run(async (context) =>
            ////{
            ////    await context.Response.WriteAsync("Hello World!");
            ////});


            // Before we can call the UseMvc() method we need to call services.AddMvc(); in the ConfigureServices() method.
            // UseMvc() Adds MVC middleware to the IApplicationBuilder request execution pipeline. we are
            // now adding controllers and actions (i.e. ASP.Net MVC classes and methods) to the Middleware pipeline.
            // Each time the application receives an http request for a particular controller, 
            // the.Net core runtime will instantiate a new instance of that controller, additionally any dependencies 
            // the controller requires will be instantiated by the DI containers in the ConfigureServices() method.
            // UseMvc() takes an Action delegate of type IRouteBuilder as a parameter. 
            // The IRouteBuilder parameter is used to configure how a HTTP request i.e. a URL the user
            // types into a web browser is routed (i.e. translated) into MVC that is which segment of the URL is the 
            // controller (i.e. a Class in MVC), which segment is the action (i.e. a method in MVC), and which section
            // is the id (i.e. an optional parameter we can input into the action).
            // The UseMvcWithDefaultRoute() method adds the MVC middleware service to the request execution pipeline.
            // However it routes all URL’s using the following default template: '{controller=Home}/{action=Index}/{id?}'.
            app.UseMvc();

        }
    }
}
