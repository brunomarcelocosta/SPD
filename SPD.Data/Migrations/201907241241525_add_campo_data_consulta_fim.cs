namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_campo_data_consulta_fim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SPD_AGENDA", "dt_consulta_inicio", c => c.DateTime(nullable: false));
            AddColumn("dbo.SPD_AGENDA", "dt_consulta_fim", c => c.DateTime(nullable: false));
            DropColumn("dbo.SPD_AGENDA", "dt_consulta");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SPD_AGENDA", "dt_consulta", c => c.DateTime(nullable: false));
            DropColumn("dbo.SPD_AGENDA", "dt_consulta_fim");
            DropColumn("dbo.SPD_AGENDA", "dt_consulta_inicio");
        }
    }
}
