namespace TemplateAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Services : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        StartPrice = c.Double(nullable: false),
                        PriceForKilometer = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Order", "ServiceId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Order", "ServiceId");
            AddForeignKey("dbo.Order", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "ServiceId", "dbo.Services");
            DropIndex("dbo.Order", new[] { "ServiceId" });
            DropColumn("dbo.Order", "ServiceId");
            DropTable("dbo.Services");
        }
    }
}
