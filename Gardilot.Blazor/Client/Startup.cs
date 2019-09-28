using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Blazor.Fluxor;

namespace Gardilot.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFluxor(options => options
                .UseDependencyInjection(typeof(Startup).Assembly)
                .AddMiddleware<Blazor.Fluxor.Routing.RoutingMiddleware>()
            );
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
