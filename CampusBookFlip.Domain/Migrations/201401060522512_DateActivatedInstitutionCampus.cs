namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateActivatedInstitutionCampus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campus", "DateActivated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Institution", "DateActivated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Institution", "DateActivated");
            DropColumn("dbo.Campus", "DateActivated");
        }
    }
}
