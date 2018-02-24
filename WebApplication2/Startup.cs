using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace WebApplication2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
