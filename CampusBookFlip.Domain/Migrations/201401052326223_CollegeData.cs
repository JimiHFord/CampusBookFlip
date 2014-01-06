namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CollegeData : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserCollege", "CollegeId", "dbo.College");
            DropIndex("dbo.UserCollege", new[] { "CollegeId" });
            CreateTable(
                "dbo.Campus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstitutionId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        State = c.String(),
                        City = c.String(),
                        ZipCode = c.String(),
                        Activated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institution", t => t.InstitutionId, cascadeDelete: true)
                .Index(t => t.InstitutionId);
            
            CreateTable(
                "dbo.Institution",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Phone = c.String(),
                        WebAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserInstitution",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        InstitutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.InstitutionId })
                .ForeignKey("dbo.Institution", t => t.InstitutionId, cascadeDelete: true)
                .Index(t => t.InstitutionId);
            
            DropTable("dbo.UserCollege");
            DropTable("dbo.College");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.College",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        State = c.String(nullable: false),
                        City = c.String(nullable: false),
                        ZipCode = c.String(nullable: false, maxLength: 5),
                        Activated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCollege",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CollegeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CollegeId });
            
            DropForeignKey("dbo.UserInstitution", "InstitutionId", "dbo.Institution");
            DropForeignKey("dbo.Campus", "InstitutionId", "dbo.Institution");
            DropIndex("dbo.UserInstitution", new[] { "InstitutionId" });
            DropIndex("dbo.Campus", new[] { "InstitutionId" });
            DropTable("dbo.UserInstitution");
            DropTable("dbo.Institution");
            DropTable("dbo.Campus");
            CreateIndex("dbo.UserCollege", "CollegeId");
            AddForeignKey("dbo.UserCollege", "CollegeId", "dbo.College", "Id", cascadeDelete: true);
        }
    }
}
