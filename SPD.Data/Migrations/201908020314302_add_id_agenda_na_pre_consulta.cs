namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_id_agenda_na_pre_consulta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SPD_PRE_CONSULTA", "fk_id_paciente", "dbo.SPD_PACIENTE");
            DropIndex("dbo.SPD_PRE_CONSULTA", "SPD_CONSULTA_SPD_PACIENTE_FK");
            AddColumn("dbo.SPD_PRE_CONSULTA", "fk_id_agenda", c => c.Int(nullable: false));
            CreateIndex("dbo.SPD_PRE_CONSULTA", "fk_id_agenda", name: "SPD_CONSULTA_SPD_PACIENTE_FK");
            AddForeignKey("dbo.SPD_PRE_CONSULTA", "fk_id_agenda", "dbo.SPD_AGENDA", "id_agenda");
            DropColumn("dbo.SPD_PRE_CONSULTA", "fk_id_paciente");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SPD_PRE_CONSULTA", "fk_id_paciente", c => c.Int(nullable: false));
            DropForeignKey("dbo.SPD_PRE_CONSULTA", "fk_id_agenda", "dbo.SPD_AGENDA");
            DropIndex("dbo.SPD_PRE_CONSULTA", "SPD_CONSULTA_SPD_PACIENTE_FK");
            DropColumn("dbo.SPD_PRE_CONSULTA", "fk_id_agenda");
            CreateIndex("dbo.SPD_PRE_CONSULTA", "fk_id_paciente", name: "SPD_CONSULTA_SPD_PACIENTE_FK");
            AddForeignKey("dbo.SPD_PRE_CONSULTA", "fk_id_paciente", "dbo.SPD_PACIENTE", "id_paciente");
        }
    }
}
