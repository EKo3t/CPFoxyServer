namespace TemplateAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuidFixes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Drivers", "CarId", "dbo.Cars");
            DropPrimaryKey("dbo.Cars");
            AlterColumn("dbo.Cars", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Cars", "Id");
            AddForeignKey("dbo.Drivers", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "CarId", "dbo.Cars");
            DropPrimaryKey("dbo.Cars");
            AlterColumn("dbo.Cars", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Cars", "Id");
            AddForeignKey("dbo.Drivers", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
        }
    }
}
