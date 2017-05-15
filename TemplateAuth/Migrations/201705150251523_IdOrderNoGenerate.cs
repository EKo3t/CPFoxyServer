namespace TemplateAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdOrderNoGenerate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Drivers", "OrderId", "dbo.Order");
            DropPrimaryKey("dbo.Order");
            AlterColumn("dbo.Order", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Order", "Id");
            AddForeignKey("dbo.Drivers", "OrderId", "dbo.Order", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "OrderId", "dbo.Order");
            DropPrimaryKey("dbo.Order");
            AlterColumn("dbo.Order", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Order", "Id");
            AddForeignKey("dbo.Drivers", "OrderId", "dbo.Order", "Id");
        }
    }
}
