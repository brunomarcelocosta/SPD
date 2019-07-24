namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_campo_hora_inicio_fim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SPD_AGENDA", "dt_consulta", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.SPD_AGENDA", "hora_inicio", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.SPD_AGENDA", "hora_fim", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.SPD_AGENDA", "dt_consulta_inicio");
            DropColumn("dbo.SPD_AGENDA", "dt_consulta_fim");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SPD_AGENDA", "dt_consulta_fim", c => c.DateTime(nullable: false));
            AddColumn("dbo.SPD_AGENDA", "dt_consulta_inicio", c => c.DateTime(nullable: false));
            DropColumn("dbo.SPD_AGENDA", "hora_fim");
            DropColumn("dbo.SPD_AGENDA", "hora_inicio");
            DropColumn("dbo.SPD_AGENDA", "dt_consulta");
        }
    }
}
