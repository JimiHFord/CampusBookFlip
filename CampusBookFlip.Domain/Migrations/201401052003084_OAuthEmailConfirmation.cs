namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OAuthEmailConfirmation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CBFUser", "OAuthConfirmEmailToken", c => c.String());
            AddColumn("dbo.CBFUser", "ConfirmedEmail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CBFUser", "ConfirmedEmail");
            DropColumn("dbo.CBFUser", "OAuthConfirmEmailToken");
        }
    }
}
