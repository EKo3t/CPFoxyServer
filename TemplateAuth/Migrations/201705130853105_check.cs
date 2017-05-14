namespace TemplateAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Drivers");
            AlterColumn("dbo.Drivers", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Drivers", "Id");
            DropColumn("dbo.Drivers", "Color");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Drivers", "Color", c => c.String());
            DropPrimaryKey("dbo.Drivers");
            AlterColumn("dbo.Drivers", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Drivers", "Id");
        }
    }
}
