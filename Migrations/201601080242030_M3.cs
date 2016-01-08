namespace KitchenHelperServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "MinutesForCooking", c => c.Int(nullable: false));
            DropTable("dbo.UserGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                        AppToken = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Recipes", "MinutesForCooking");
        }
    }
}
