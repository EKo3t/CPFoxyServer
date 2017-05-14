namespace TemplateAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarDriver : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserInfo", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserInfo", new[] { "UserId" });
            CreateTable(
                "dbo.CarMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CarModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CarMarkId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarMarks", t => t.CarMarkId, cascadeDelete: true)
                .Index(t => t.CarMarkId);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Color = c.String(),
                        CarModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarModels", t => t.CarModelId, cascadeDelete: true)
                .Index(t => t.CarModelId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Color = c.String(),
                        UserId = c.String(maxLength: 128),
                        CarId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CarId);
            
            CreateTable(
                "dbo.UserToInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserInfoId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoId, cascadeDelete: true)
                .Index(t => t.UserInfoId)
                .Index(t => t.UserId);
            
            DropColumn("dbo.UserInfo", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserInfo", "UserId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.UserToInfoes", "UserInfoId", "dbo.UserInfo");
            DropForeignKey("dbo.UserToInfoes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Drivers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Drivers", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Cars", "CarModelId", "dbo.CarModels");
            DropForeignKey("dbo.CarModels", "CarMarkId", "dbo.CarMarks");
            DropIndex("dbo.UserToInfoes", new[] { "UserId" });
            DropIndex("dbo.UserToInfoes", new[] { "UserInfoId" });
            DropIndex("dbo.Drivers", new[] { "CarId" });
            DropIndex("dbo.Drivers", new[] { "UserId" });
            DropIndex("dbo.Cars", new[] { "CarModelId" });
            DropIndex("dbo.CarModels", new[] { "CarMarkId" });
            DropTable("dbo.UserToInfoes");
            DropTable("dbo.Drivers");
            DropTable("dbo.Cars");
            DropTable("dbo.CarModels");
            DropTable("dbo.CarMarks");
            CreateIndex("dbo.UserInfo", "UserId");
            AddForeignKey("dbo.UserInfo", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
