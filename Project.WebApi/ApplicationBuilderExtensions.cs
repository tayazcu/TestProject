using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.WebApi.Controllers;
using Project.WebApi.Models;

namespace Project.WebApi
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder IntializeDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = scope.ServiceProvider.GetService<DbContexts>();

            dbContext.Database.Migrate();

            //var dataInitializers = scope.ServiceProvider.GetServices<IDataInitializer>();
            //foreach (var dataInitializer in dataInitializers)
            //    dataInitializer.InitializeData();

            return app;
        }
    }
}