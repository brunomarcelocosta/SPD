namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumnDataNascToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SPD_PACIENTE", "dt_nasc", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SPD_PACIENTE", "dt_nasc", c => c.DateTime(nullable: false));
        }
    }
}
