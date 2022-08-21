namespace iFinco.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_management : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "ImageUrl", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "ImageUrl", c => c.String(nullable: false, maxLength: 300));
        }
    }
}
