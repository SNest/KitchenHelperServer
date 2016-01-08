using System;
using System.Data.Entity;
using KitchenHelperServer.Models;

namespace KitchenHelperServer.EF
{
    public class ContextInitializer : DropCreateDatabaseAlways<EfContext>
    {
        private EfContext _context = new EfContext();

        protected void Seed()
        {
            CreateIndex("Name", typeof(Product));
            CreateIndex("Name", typeof(Recipe));

            _context.SaveChanges();
        }

        private void CreateIndex(string field, Type table)
        {
            var command = string.Format("CREATE UNIQUE INDEX IX_{0} ON [{1}s]([{0}])", field, table.Name);
            _context.Database.ExecuteSqlCommand(command);
        }
    }
}