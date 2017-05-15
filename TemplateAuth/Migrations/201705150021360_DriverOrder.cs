namespace TemplateAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriverOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "OrderId", c => c.Guid());
            CreateIndex("dbo.Drivers", "OrderId");
            AddForeignKey("dbo.Drivers", "OrderId", "dbo.Order", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "OrderId", "dbo.Order");
            DropIndex("dbo.Drivers", new[] { "OrderId" });
            DropColumn("dbo.Drivers", "OrderId");
        }
    }
}
