namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentifyableAndUserIdChangeEmailRequest : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ChangeEmailRequest", name: "UserId", newName: "Id");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.ChangeEmailRequest", name: "Id", newName: "UserId");
        }
    }
}
