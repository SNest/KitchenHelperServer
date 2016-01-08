namespace KitchenHelperServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fridges", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.ProductStorages", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.Recipes", "Fridge_Id", "dbo.Fridges");
            DropForeignKey("dbo.Products", "ProductStorage_Id", "dbo.ProductStorages");
            RenameColumn(table: "dbo.Recipes", name: "Fridge_Id", newName: "Fridge_UserGroupId");
            RenameColumn(table: "dbo.Products", name: "ProductStorage_Id", newName: "ProductStorage_UserGroupId");
            RenameIndex(table: "dbo.Recipes", name: "IX_Fridge_Id", newName: "IX_Fridge_UserGroupId");
            RenameIndex(table: "dbo.Products", name: "IX_ProductStorage_Id", newName: "IX_ProductStorage_UserGroupId");
            DropPrimaryKey("dbo.Fridges");
            DropPrimaryKey("dbo.ProductStorages");
            AddPrimaryKey("dbo.Fridges", "UserGroupId");
            AddPrimaryKey("dbo.ProductStorages", "UserGroupId");
            AddForeignKey("dbo.Fridges", "UserGroupId", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.ProductStorages", "UserGroupId", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.Recipes", "Fridge_UserGroupId", "dbo.Fridges", "UserGroupId");
            AddForeignKey("dbo.Products", "ProductStorage_UserGroupId", "dbo.ProductStorages", "UserGroupId");
            DropColumn("dbo.Fridges", "Id");
            DropColumn("dbo.ProductStorages", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductStorages", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Fridges", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Products", "ProductStorage_UserGroupId", "dbo.ProductStorages");
            DropForeignKey("dbo.Recipes", "Fridge_UserGroupId", "dbo.Fridges");
            DropForeignKey("dbo.ProductStorages", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.Fridges", "UserGroupId", "dbo.UserGroups");
            DropPrimaryKey("dbo.ProductStorages");
            DropPrimaryKey("dbo.Fridges");
            AddPrimaryKey("dbo.ProductStorages", "Id");
            AddPrimaryKey("dbo.Fridges", "Id");
            RenameIndex(table: "dbo.Products", name: "IX_ProductStorage_UserGroupId", newName: "IX_ProductStorage_Id");
            RenameIndex(table: "dbo.Recipes", name: "IX_Fridge_UserGroupId", newName: "IX_Fridge_Id");
            RenameColumn(table: "dbo.Products", name: "ProductStorage_UserGroupId", newName: "ProductStorage_Id");
            RenameColumn(table: "dbo.Recipes", name: "Fridge_UserGroupId", newName: "Fridge_Id");
            AddForeignKey("dbo.Products", "ProductStorage_Id", "dbo.ProductStorages", "Id");
            AddForeignKey("dbo.Recipes", "Fridge_Id", "dbo.Fridges", "Id");
            AddForeignKey("dbo.ProductStorages", "UserGroupId", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Fridges", "UserGroupId", "dbo.UserGroups", "Id", cascadeDelete: true);
        }
    }
}
