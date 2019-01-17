namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SPD_PACIENTE", "bairro", c => c.String(maxLength: 50));
            AlterColumn("dbo.SPD_PACIENTE", "pais", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SPD_PACIENTE", "pais", c => c.String(maxLength: 25));
            AlterColumn("dbo.SPD_PACIENTE", "bairro", c => c.String(maxLength: 25));
        }
    }
}
