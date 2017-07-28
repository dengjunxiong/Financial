using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Financial.DAL.SQLServer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Financial.DAL.SQLServer.FinancialDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Financial.DAL.SQLServer.FinancialDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
