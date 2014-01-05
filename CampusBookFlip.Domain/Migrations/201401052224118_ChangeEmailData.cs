namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEmailData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeEmailRequest",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ConfirmationToken = c.String(nullable: false),
                        NewEmail = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.CBFUser", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeEmailRequest", "UserId", "dbo.CBFUser");
            DropIndex("dbo.ChangeEmailRequest", new[] { "UserId" });
            DropTable("dbo.ChangeEmailRequest");
        }
    }
}
