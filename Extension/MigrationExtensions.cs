using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Data;

namespace POS.Extension
{
    public static class MigrationExtensions
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            
            using MyDbContext context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            context.Database.Migrate();
        }
    }
}