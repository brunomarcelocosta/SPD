namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_campo_celular_paciente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SPD_AGENDA", "celular", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.SPD_AGENDA", "nm_paciente", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SPD_AGENDA", "nm_paciente", c => c.String(maxLength: 50));
            DropColumn("dbo.SPD_AGENDA", "celular");
        }
    }
}
