using System;
using System.Data.Entity;
using KitchenHelperServer.Models;

namespace KitchenHelperServer.EF
{
    public class EfContext : DbContext
    {
        public EfContext()
            : base("DbConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = false; 
        }

        public EfContext(string connectionString)
            : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = false; 
        }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<RecipeStep> RecipeSteps { get; set; }
        
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<ProductStorage> ProductStorages { get; set; }

        public DbSet<Fridge> Fridges { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>().ToTable("Recipes");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients");
        }

        private void CreateIndex(string field, Type table)
        {
            var command = string.Format("CREATE UNIQUE INDEX IX_{0} ON [{1}s]([{0}])", field, table.Name);
            Database.ExecuteSqlCommand(command);
        }
    }
}