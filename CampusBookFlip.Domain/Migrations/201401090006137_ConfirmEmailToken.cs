namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfirmEmailToken : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.CBFUser", "OAuthConfirmEmailToken", "ConfirmEmailToken");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.CBFUser", "ConfirmEmailToken", "OAuthConfirmEmailToken");
        }
    }
}
