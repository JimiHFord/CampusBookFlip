namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedateactivated : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Campus", "DateActivated");
            DropColumn("dbo.Institution", "DateActivated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Institution", "DateActivated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Campus", "DateActivated", c => c.DateTime(nullable: false));
        }
    }
}
