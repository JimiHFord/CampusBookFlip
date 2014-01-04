namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CBFUser", "EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CBFUser", "EmailAddress");
        }
    }
}
