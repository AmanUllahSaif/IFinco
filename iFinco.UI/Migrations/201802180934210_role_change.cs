namespace iFinco.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class role_change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "IsForAdmin", c => c.Boolean());
            AddColumn("dbo.Roles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "Discriminator");
            DropColumn("dbo.Roles", "IsForAdmin");
        }
    }
}
