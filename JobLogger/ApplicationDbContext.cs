﻿using System.Data.Entity;
using EntityFramework.CodeFirst.Entities;
using EntityFramework.CodeFirst.Migrations;

namespace EntityFramework.CodeFirst
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            //Important performance code
            Configuration.AutoDetectChangesEnabled = false; 
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, ApplicationDbInitializer>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<LogValue> LogValues { get; set; } 
    }
}
