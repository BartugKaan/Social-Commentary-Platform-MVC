namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig_heading_status_fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Headings", "HeadingStatus", c => c.Boolean(nullable: false));
            DropColumn("dbo.Headings", "HeadingContent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Headings", "HeadingContent", c => c.Boolean(nullable: false));
            DropColumn("dbo.Headings", "HeadingStatus");
        }
    }
}
